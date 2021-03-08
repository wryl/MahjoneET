using System.Collections.Generic;
using System.Linq;
using GamePlay.Server.Model;
using Mahjong.Logic;
using Mahjong.Model;
namespace ET 
{
	public class PlayerDiscardTileState : ServerState
	{
		public Tile DiscardTile;
		public bool IsRichiing;
		public bool DiscardLastDraw;
		public int BonusTurnTime;
		public MahjongSet MahjongSet => GetParent<MahjoneBehaviourComponent>().mahjongSet;
		public bool TurnDoraAfterDiscard;
		private HashSet<long> responds;
		private OutTurnOperation[] operations;
		private long timerId;
		public void Init(Tile discardTile, bool isRichiing, bool discardLastDraw, bool turnDoraAfterDiscard, int bonusTurnTime) {
			DiscardTile = discardTile;
			IsRichiing = isRichiing;
			DiscardLastDraw = discardLastDraw;
			TurnDoraAfterDiscard = turnDoraAfterDiscard;
			BonusTurnTime = bonusTurnTime;
			responds = new HashSet<long>();
			operations = new OutTurnOperation[players.Count];
		}
		public override void OnServerStateEnter()
		{
			UpdateRoundStatus();
			for (int i = 0; i < players.Count; i++)
			{
				var info = new M2C_DiscardTileInfo
				{
					PlayerIndex = CurrentRoundStatus.CurrentPlayerIndex,
					DiscardingLastDraw = DiscardLastDraw,
					IsRichiing = IsRichiing,
					Tile = DiscardTile,
					BonusTurnTime = CurrentRoundStatus.GetBonusTurnTime(i),
					Zhenting = CurrentRoundStatus.IsZhenting(i),
					Operations = GetOperations(i),
					HandTiles = CurrentRoundStatus.HandTiles(i),
					Rivers = CurrentRoundStatus.Rivers
				};
				Game.EventSystem.Publish(new EventType.ActorMessage() { actorId = players[i], actorMessage = info }).Coroutine();
			}
				
			Log.Debug($"[Server] CurrentRoundStatus: {CurrentRoundStatus}");
			timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + gameSettings.BaseTurnTime * 1000 + CurrentRoundStatus.MaxBonusTurnTime * 1000
													  + ServerConstants.ServerTimeBuffer, TimeoutFunc);
		}

        public void Destroy()
        {
			OnServerStateExit();
		}

        private void UpdateRoundStatus()
		{
			CurrentRoundStatus.SetBonusTurnTime(CurrentRoundStatus.CurrentPlayerIndex, BonusTurnTime);
			var lastDraw = CurrentRoundStatus.LastDraw;
			CurrentRoundStatus.LastDraw = null;
			if (!DiscardLastDraw)
			{
				CurrentRoundStatus.RemoveTile(CurrentRoundStatus.CurrentPlayerIndex, DiscardTile);
				if (lastDraw != null)
					CurrentRoundStatus.AddTile(CurrentRoundStatus.CurrentPlayerIndex, (Tile)lastDraw);
			}

			CurrentRoundStatus.AddToRiver(CurrentRoundStatus.CurrentPlayerIndex, DiscardTile, IsRichiing);
			CurrentRoundStatus.SortHandTiles();
			CurrentRoundStatus.UpdateDiscardZhenting();
		}
        #region »ñÈ¡²Ù×÷Âß¼­
        private List<OutTurnOperation> GetOperations(int playerIndex)
        {
            if (playerIndex == CurrentRoundStatus.CurrentPlayerIndex)
                return new List<OutTurnOperation>
                {
                    new OutTurnOperation {Type = OutTurnOperationType.Skip}
                };
            // other players' operations
            var operations = new List<OutTurnOperation>
            {
                new OutTurnOperation {Type = OutTurnOperationType.Skip}
            };
            // test rong
            TestRong(playerIndex, DiscardTile, operations);
            if (!CurrentRoundStatus.RichiStatus(playerIndex))
            {
                // get side
                var side = GetSide(playerIndex, CurrentRoundStatus.CurrentPlayerIndex, CurrentRoundStatus.TotalPlayers);
                var handTiles = CurrentRoundStatus.HandTiles(playerIndex);
                // test kong
                TestKongs(handTiles, DiscardTile, side, operations);
                // test pong
                TestPongs(handTiles, DiscardTile, side, operations);
                // test chow
                TestChows(handTiles, DiscardTile, side, operations);
            }

            return operations;
        }

        private void TestRong(int playerIndex, Tile discardTile, IList<OutTurnOperation> operations)
		{
			var baseHandStatus = HandStatus.Nothing;
			// test haidi
			if (MahjongSet.Data.TilesDrawn == gameSettings.MountainReservedTiles)
				baseHandStatus |= HandStatus.Haidi;
			// test lingshang -- not gonna happen
			var allTiles = MahjongSet.AllTiles;
			var doraTiles = MahjongSet.DoraIndicators.Select(
				indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
			var uraDoraTiles = MahjongSet.UraDoraIndicators.Select(
				indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
			var beiDora = CurrentRoundStatus.GetBeiDora(playerIndex);
			var point = ServerMahjongLogic.GetPointInfo(
				playerIndex, CurrentRoundStatus, discardTile, baseHandStatus,
				doraTiles, uraDoraTiles, beiDora, gameSettings);
			// test if enough
			if (gameSettings.CheckConstraint(point))
			{
				operations.Add(new OutTurnOperation
				{
					Type = OutTurnOperationType.Rong,
					Tile = discardTile,
					HandData = CurrentRoundStatus.HandData(playerIndex)
				});
			}
		}

		private MeldSide GetSide(int playerIndex, int discardPlayerIndex, int totalPlayer)
		{
			int diff = discardPlayerIndex - playerIndex;
			if (diff < 0) diff += totalPlayer;
			switch (totalPlayer)
			{
				case 2: return GetSide2();
				case 3: return GetSide3(diff);
				case 4: return GetSide4(diff);
				default:
					Log.Error($"TotalPlayer = {totalPlayer}, this should not happen");
					return MeldSide.Left;
			}
		}

		private MeldSide GetSide2()
		{
			return MeldSide.Left;
		}

		private MeldSide GetSide3(int diff)
		{
			switch (diff)
			{
				case 1: return MeldSide.Right;
				case 2: return MeldSide.Left;
				default:
					Log.Error($"Diff = {diff}, this should not happen");
					return MeldSide.Left;
			}
		}

		private MeldSide GetSide4(int diff)
		{
			switch (diff)
			{
				case 1: return MeldSide.Right;
				case 2: return MeldSide.Opposite;
				case 3: return MeldSide.Left;
				default:
					Log.Error($"Diff = {diff}, this should not happen");
					return MeldSide.Left;
			}
		}

		private void TestKongs(IList<Tile> handTiles, Tile discardTile, MeldSide side,
			IList<OutTurnOperation> operations)
		{
			if (!gameSettings.AllowPongs) return;
			var kongs = MahjongLogic.GetKongs(handTiles, discardTile, side);
			if (kongs.Any())
			{
				foreach (var kong in kongs)
				{
					operations.Add(new OutTurnOperation
					{
						Type = OutTurnOperationType.Kong,
						Tile = discardTile,
						Meld = kong,
						ForbiddenTiles = gameSettings.AllowDiscardSameAfterOpen
							? null
							: kong.GetForbiddenTiles(discardTile)
					});
				}
			}
		}

		private void TestPongs(IList<Tile> handTiles, Tile discardTile, MeldSide side,
			IList<OutTurnOperation> operations)
		{
			if (!gameSettings.AllowPongs) return;
			var pongs = MahjongLogic.GetPongs(handTiles, discardTile, side);
			if (pongs.Any())
			{
				foreach (var pong in pongs)
				{
					operations.Add(new OutTurnOperation
					{
						Type = OutTurnOperationType.Pong,
						Tile = discardTile,
						Meld = pong,
						ForbiddenTiles = gameSettings.AllowDiscardSameAfterOpen
							? null
							: pong.GetForbiddenTiles(discardTile)
					});
				}
			}
		}

		private void TestChows(IList<Tile> handTiles, Tile discardTile, MeldSide side,
			IList<OutTurnOperation> operations)
		{
			if (!gameSettings.AllowChows) return;
			if (side != MeldSide.Left) return;
			var chows = MahjongLogic.GetChows(handTiles, discardTile, side);
			if (chows.Any())
			{
				foreach (var chow in chows)
				{
					operations.Add(new OutTurnOperation
					{
						Type = OutTurnOperationType.Chow,
						Tile = discardTile,
						Meld = chow,
						ForbiddenTiles = gameSettings.AllowDiscardSameAfterOpen
							? null
							: chow.GetForbiddenTiles(discardTile)
					});
				}
			}
		}
        #endregion
        public void TimeoutFunc()
		{
				// Time out, entering next state
				for (int i = 0; i <players.Count; i++)
				{
					if (responds.Contains(players[i])) continue;
					CurrentRoundStatus.SetBonusTurnTime(i, 0);
					operations[i] = new OutTurnOperation {Type = OutTurnOperationType.Skip};
				}
				TurnEnd();
		}

		private void TurnEnd()
		{
			ParentBehaviour.TurnEnd(CurrentRoundStatus.CurrentPlayerIndex, DiscardTile, IsRichiing, operations,
				false, TurnDoraAfterDiscard);
		}

		public void OnOutTurnOperationEvent(int index, OutTurnOperation operation, int bonusTime)
		{
			if (responds.Contains(players[index])) return;
			responds.Add(players[index]);
			operations[index]= operation;
			CurrentRoundStatus.SetBonusTurnTime(index, bonusTime);
            if (responds.Count==players.Count)
            {
				TurnEnd();
			}
		}

		public override void OnServerStateExit()
		{
			TimerComponent.Instance.Remove(timerId);
			timerId = 0;
			//operations = new OutTurnOperation[players.Count]; 
			//responds = new HashSet<long>();
		}
	}
}