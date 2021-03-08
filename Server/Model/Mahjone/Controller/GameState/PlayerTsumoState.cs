using GamePlay.Server.Model;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class PlayerTsumoState : ServerState
	{
		public int TsumoPlayerIndex;
		public Tile WinningTile;
		public MahjongSet MahjongSet=>ParentBehaviour.mahjongSet;
		public PointInfo TsumoPointInfo;
		private List<PointTransfer> transfers;
		private HashSet<long> responds;
		private const int ServerMaxTimeOut = 10000;
		private long timerId;
		public void Init(int currentPlayerIndex, Tile winningTile, PointInfo pointInfo)
		{
			TsumoPlayerIndex = currentPlayerIndex;
			WinningTile = winningTile;
			TsumoPointInfo = pointInfo;
		}
		public override void OnServerStateEnter()
		{
			int multiplier = gameSettings.GetMultiplier(CurrentRoundStatus.IsDealer(TsumoPlayerIndex), players.Count);
			var netInfo = new NetworkPointInfo
			{
				Fu = TsumoPointInfo.Fu,
				YakuValues = TsumoPointInfo.YakuList.ToArray(),
				Dora = TsumoPointInfo.Dora,
				UraDora = TsumoPointInfo.UraDora,
				RedDora = TsumoPointInfo.RedDora,
				IsQTJ = TsumoPointInfo.IsQTJ
			};
			var info = new M2C_TsumoInfo
			{
				TsumoPlayerIndex = TsumoPlayerIndex,
				TsumoPlayerName = CurrentRoundStatus.GetPlayerName(TsumoPlayerIndex),
				TsumoHandData = CurrentRoundStatus.HandData(TsumoPlayerIndex),
				WinningTile = WinningTile,
				DoraIndicators = MahjongSet.DoraIndicators.ToList(),
				UraDoraIndicators = MahjongSet.UraDoraIndicators.ToList(),
				IsRichi = CurrentRoundStatus.RichiStatus(TsumoPlayerIndex),
				TsumoPointInfo = netInfo,
				TotalPoints = TsumoPointInfo.BasePoint * multiplier
			};
			Game.EventSystem.Publish(new EventType.MessageBroadCast() { actorIds = players, actorMessage = info }).Coroutine();
			// 改动.直接全额计算
			transfers = new List<PointTransfer>();
			for (int playerIndex = 0; playerIndex < players.Count; playerIndex++)
			{
				if (playerIndex == TsumoPlayerIndex) continue;
				int extraPoints = CurrentRoundStatus.ExtraPoints;
				transfers.Add(new PointTransfer
				{
					From = playerIndex,
					To = TsumoPlayerIndex,
					Amount = info.TotalPoints + extraPoints
				});
			}

			// richi-sticks-points
			transfers.Add(new PointTransfer
			{
				From = -1,
				To = TsumoPlayerIndex,
				Amount = CurrentRoundStatus.RichiSticksPoints
			});
			responds = new HashSet<long>();
			// determine server time out
			timerId =TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + ServerMaxTimeOut + ServerConstants.ServerTimeBuffer,TimeOutFunc);
		}

		public override void OnServerStateExit()
		{
			responds.Clear();
			TimerComponent.Instance.Remove(timerId);
			timerId = 0;
		}

		public void TimeOutFunc()
		{
			PointTransfer();
		}

		private void PointTransfer()
		{
			var next = CurrentRoundStatus.OyaPlayerIndex != TsumoPlayerIndex;
			ParentBehaviour.PointTransfer(transfers, next, !next, false);
		}

		public void OnEvent(long userid)
		{
			if (responds.Add(userid))
			{
				if (responds.Count == totalPlayers)
				{
					PointTransfer();
				}
			}
		}
	}
}