using GamePlay.Server.Model;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class PlayerRongState : ServerState
    {
        public int CurrentPlayerIndex;
        public int[] RongPlayerIndices;
        public Tile WinningTile;
        public MahjongSet MahjongSet=>ParentBehaviour.mahjongSet;
        public PointInfo[] RongPointInfos;
        private List<PointTransfer> transfers;
        private HashSet<long> responds;
        private bool next;
        private const int ServerMaxTimeOut = 10000;
        private long timerId;
        public void Init(int currentPlayerIndex, Tile winningTile, int[] rongPlayerIndices, PointInfo[] rongPointInfos)
        {
            CurrentPlayerIndex = currentPlayerIndex;
            RongPlayerIndices = rongPlayerIndices;
            WinningTile = winningTile;
            RongPointInfos = rongPointInfos;
        }
        public override void OnServerStateEnter()
        {
            responds = new HashSet<long>();
            var playerNames = RongPlayerIndices.Select(
                playerIndex => CurrentRoundStatus.GetPlayerName(playerIndex)
            ).ToList();
            var handData = RongPlayerIndices.Select(
                playerIndex => CurrentRoundStatus.HandData(playerIndex)
            ).ToList();
            var richiStatus = RongPlayerIndices.Select(
                playerIndex => CurrentRoundStatus.RichiStatus(playerIndex)
            ).ToList();
            var multipliers = RongPlayerIndices.Select(
                playerIndex => gameSettings.GetMultiplier(CurrentRoundStatus.IsDealer(playerIndex), players.Count)
            ).ToList();
            var totalPoints = RongPointInfos.Select((info, i) => info.BasePoint * multipliers[i]).ToList();
            var netInfos = RongPointInfos.Select(info => new NetworkPointInfo
            {
                Fu = info.Fu,
                YakuValues = info.YakuList.ToArray(),
                Dora = info.Dora,
                UraDora = info.UraDora,
                RedDora = info.RedDora,
                BeiDora = info.BeiDora,
                IsQTJ = info.IsQTJ
            }).ToList();
            Log.Debug($"The following players are claiming rong: {string.Join(",", RongPlayerIndices)}, "
                      + $"PlayerNames: {string.Join(",", playerNames)}");
            var rongInfo = new M2C_RongInfo
            {
                RongPlayerIndices = RongPlayerIndices.ToList(),
                RongPlayerNames = playerNames,
                HandData = handData,
                WinningTile = WinningTile,
                DoraIndicators = MahjongSet.DoraIndicators.ToList(),
                UraDoraIndicators = MahjongSet.UraDoraIndicators.ToList(),
                RongPlayerRichiStatus = richiStatus,
                RongPointInfos = netInfos,
                TotalPoints = totalPoints
            };
            Game.EventSystem.Publish(new EventType.MessageBroadCast() { actorIds = players, actorMessage = rongInfo }).Coroutine();
            // get point transfers
            transfers = new List<PointTransfer>();
            for (int i = 0; i < RongPlayerIndices.Length; i++)
            {
                var rongPlayerIndex = RongPlayerIndices[i];
                var point = RongPointInfos[i];
                var multiplier = multipliers[i];
                int pointValue = point.BasePoint * multiplier;
                int extraPoints = i == 0 ? CurrentRoundStatus.ExtraPoints * (players.Count - 1) : 0;
                transfers.Add(new PointTransfer
                {
                    From = CurrentPlayerIndex,
                    To = rongPlayerIndex,
                    Amount = pointValue + extraPoints
                });
            }

            // richi-sticks-points
            transfers.Add(new PointTransfer
            {
                From = -1,
                To = RongPlayerIndices[0],
                Amount = CurrentRoundStatus.RichiSticksPoints
            });
            next = !RongPlayerIndices.Contains(CurrentRoundStatus.OyaPlayerIndex);
            // determine server time out
            timerId =TimerComponent.Instance.NewOnceTimer(ServerMaxTimeOut * RongPointInfos.Length + ServerConstants.ServerTimeBuffer,TimeoutFunc);
        }

        public override void OnServerStateExit()
        {
            responds.Clear();
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }

        public void TimeoutFunc(bool b)
        {
            PointTransfer();
        }

        private void PointTransfer()
        {
            ParentBehaviour.PointTransfer(transfers, next, !next, false);
        }

        public void OnEvent(long userid)
        {
            responds.Add(userid);
            if (responds.Count>= totalPlayers)
            {
                PointTransfer();
            }
        }
    }
}