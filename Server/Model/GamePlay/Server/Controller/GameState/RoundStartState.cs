using GamePlay.Server.Model;
using Mahjong.Logic;
using Mahjong.Model;
using System.Collections.Generic;

namespace ET
{
    public class RoundStartStateDestroySystem : DestroySystem<RoundStartState>
	{
		public override void Destroy(RoundStartState self)
		{
			self.Destroy();
		}
	}
	/// <summary>
	/// When the server is in this state, the server will distribute initial tiles for every player, 
	/// and will determine the initial dora indicator(s) according to the settings.
	/// All the data such as initial tiles, initial dora indicators, and mahjongSetData.
	/// Transfers to PlayerDrawTileState. The state transfer will be done regardless whether enough client responds received.
	/// </summary>
	public class RoundStartState : ServerState
	{
		private long timerId;
		public MahjongSet MahjongSet=>GetParent<MahjoneBehaviourComponent>().mahjongSet;
		public bool NextRound;
		public bool ExtraRound;
		public bool KeepSticks;
		private HashSet<long> responds;
		public void Init(bool next, bool extra, bool keepSticks)
		{
			NextRound = next;
			ExtraRound = extra;
			KeepSticks = keepSticks;
		}

		public void Destroy()
		{
			TimerComponent.Instance.Remove(timerId);
			timerId = 0;
			responds = null;
		}
		public override void OnServerStateEnter()
		{
			MahjongSet.Reset();
			// throwing dice
			var dice = RandomHelper.RandomNumber(CurrentRoundStatus.GameSettings.DiceMin,
				CurrentRoundStatus.GameSettings.DiceMax + 1);
			CurrentRoundStatus.NextRound(dice, NextRound, ExtraRound, KeepSticks);
			// draw initial tiles
			DrawInitial();
			Log.Debug("[Server] Initial tiles distribution done");
			CurrentRoundStatus.SortHandTiles();
			CurrentRoundStatus.SetBonusTurnTime(gameSettings.BonusTurnTime);
			responds = new HashSet<long>();
			timerId = TimerComponent.Instance.NewOnceTimer(ServerConstants.ServerTimeOut, TimeOutFunc);
			for (int index = 0; index < players.Count; index++)
			{
				var tiles = CurrentRoundStatus.HandTiles(index);
				Log.Debug($"[Server] Hand tiles of player {index}: {string.Join("", tiles)}");
				var info = new M2C_RoundStartInfo
				{
					PlayerIndex = index,
					Field = CurrentRoundStatus.Field,
					Dice = CurrentRoundStatus.Dice,
					Extra = CurrentRoundStatus.Extra,
					RichiSticks = CurrentRoundStatus.RichiSticks,
					OyaPlayerIndex = CurrentRoundStatus.OyaPlayerIndex,
					Points = CurrentRoundStatus.Points,
					InitialHandTiles = tiles,
					MahjongSetData = MahjongSet.Data
				};
				Game.EventSystem.Publish(new EventType.ActorMessage() { actorId = players[index], actorMessage = info }).Coroutine();
			}
		}
		public void OnEvent(long actorid)
		{
			responds.Add(actorid);
            if (responds.Count==totalPlayers)
            {
				Log.Debug("[Server] RoundStart MaxPlayer");
				ServerNextState();
			}
		}
		public void TimeOutFunc(bool timeout)
		{
			Log.Debug("[Server] RoundStart Timeout");
			ServerNextState();
		}

		private void ServerNextState()
		{
			GetParent<MahjoneBehaviourComponent>().DrawTile(CurrentRoundStatus.OyaPlayerIndex);
		}

		public override void OnServerStateExit()
		{
			responds.Clear();
			TimerComponent.Instance.Remove(timerId);
			timerId = 0;
		}

		private void DrawInitial()
		{
			for (int round = 0; round < MahjongConstants.InitialDrawRound; round++)
			{
				// Draw 4 tiles for each player
				for (int index = 0; index < players.Count; index++)
				{
					for (int i = 0; i < MahjongConstants.TilesEveryRound; i++)
					{
						var tile = MahjongSet.DrawTile();
						CurrentRoundStatus.AddTile(index, tile);
					}
				}
			}

			// Last round, 1 tile for each player
			for (int index = 0; index < players.Count; index++)
			{
				for (int i = 0; i < MahjongConstants.TilesLastRound; i++)
				{
					var tile = MahjongSet.DrawTile();
					CurrentRoundStatus.AddTile(index, tile);
				}
			}
		}
	}
}