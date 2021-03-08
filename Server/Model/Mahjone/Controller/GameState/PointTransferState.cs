using GamePlay.Server.Model;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;


namespace ET
{
    public class PointTransferState : ServerState
    {
        public List<PointTransfer> PointTransferList;
        public bool NextRound;
        public bool ExtraRound;
        public bool KeepSticks;
        private HashSet<long> responds;
        private long timerId;
        public void Init(List<PointTransfer> transfers, bool next, bool extra, bool keepSticks)
        {
            NextRound = next;
            ExtraRound = extra;
            KeepSticks = keepSticks;
            PointTransferList = transfers;
        }
        public override void OnServerStateEnter()
        {
            Log.Debug($"[Server] Transfers: {string.Join(", ", PointTransferList)}");
            // var names = players.Select(player => player.PlayerName).ToArray();
            var names = CurrentRoundStatus.PlayerNames;
            responds = new HashSet<long>();
            // update points of each player
            foreach (var transfer in PointTransferList)
            {
                ChangePoints(transfer);
            }

            var info = new M2C_PointTransferInfo
            {
                PlayerNames = names,
                Points = CurrentRoundStatus.Points,
                PointTransfers = PointTransferList
            };
            Game.EventSystem.Publish(new EventType.MessageBroadCast() { actorIds = players, actorMessage = info }).Coroutine();
            timerId =TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + ServerConstants.ServerPointTransferTimeOut,TimeOutFunc);
        }

        public override void OnServerStateExit()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }

        public void TimeOutFunc()
        {
            // time out
            Log.Debug("[Server] Server time out, start next round");
            StartNewRound();
        }

        private void StartNewRound()
        {
            if (CheckIfGameEnds())
                ParentBehaviour.GameEnd();
            else
                //ParentBehaviour.RoundStart(NextRound, ExtraRound, KeepSticks);
                ParentBehaviour.InitHandAfterRong(CurrentRoundStatus.CurrentPlayerIndex);
        }

        /// <summary>
        /// Check if game ends. When the game ends, return true, otherwise return false
        /// </summary>
        /// <returns></returns>
        private bool CheckIfGameEnds()
        {
            // check if allow zero or negative points
            int lowestPoint = CurrentRoundStatus.Points.Min();
            switch (CurrentRoundStatus.GameSettings.PointsToGameEnd)
            {
                case PointsToGameEnd.Zero:
                    if (lowestPoint <= 0) return true;
                    break;
                case PointsToGameEnd.Negative:
                    if (lowestPoint < 0) return true;
                    break;
            }

            if (CurrentRoundStatus.GameForceEnd) return true;
            var isAllLast = CurrentRoundStatus.IsAllLast;
            if (!isAllLast) return false;
            // is all last
            var maxPoint = CurrentRoundStatus.Points.Max();
            if (NextRound) // if next round
            {
                return maxPoint >= CurrentRoundStatus.GameSettings.FirstPlacePoints;
            }
            else // if not next -- same oya
            {
                if (maxPoint < CurrentRoundStatus.GameSettings.FirstPlacePoints)
                {
                    return false;
                }

                int playerIndex = CurrentRoundStatus.Points.IndexOf(maxPoint);
                if (playerIndex == CurrentRoundStatus.OyaPlayerIndex) // last oya is top
                {
                    return CurrentRoundStatus.GameSettings.GameEndsWhenAllLastTop;
                }

                return false;
            }
        }

        private void ChangePoints(PointTransfer transfer)
        {
            CurrentRoundStatus.ChangePoints(transfer.To, transfer.Amount);
            if (transfer.From >= 0)
                CurrentRoundStatus.ChangePoints(transfer.From, -transfer.Amount);
        }

        public void OnNextRoundEvent(long playerid)
        {
            responds.Add(playerid);
            if (responds.Count==totalPlayers)
            {
                Log.Debug("[Server] All players has responded, start next round");
                StartNewRound();
                return;
            }
        }
    }
}