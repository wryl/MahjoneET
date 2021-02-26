
using GamePlay.Server.Model;
using Mahjong.Model;
using System.Linq;
namespace ET
{
    public class OperationPerformState : ServerState
    {
        public int CurrentPlayerIndex;
        public int DiscardPlayerIndex;
        public OutTurnOperation Operation;
        public MahjongSet MahjongSet=>ParentBehaviour.mahjongSet;
        private bool turnDoraAfterDiscard;
        private long timerId;
        public void Init(int currIndex,int discardIndex, OutTurnOperation opration)
        {
            CurrentPlayerIndex = currIndex;
            DiscardPlayerIndex = discardIndex;
            Operation = opration;
        }
        public override void OnServerStateEnter()
        {
            // update hand data
            UpdateRoundStatus();
            // send messages
            var currPlayerMsg = GetCurrInfo();
            Game.EventSystem.Publish(new EventType.ActorMessage() { actorId = players[CurrentPlayerIndex], actorMessage = currPlayerMsg }).Coroutine();
            var notcurrmsg = GetNotCurrInfo();
            Game.EventSystem.Publish(new EventType.MessageBroadCast() { actorIds = players.Where(p => p != players[CurrentPlayerIndex]).ToList(), actorMessage = currPlayerMsg }).Coroutine();
            KongOperation();
            timerId=TimerComponent.Instance.NewOnceTimer(gameSettings.BaseTurnTime*1000 + CurrentRoundStatus.MaxBonusTurnTime*1000 +
                            ServerConstants.ServerTimeBuffer,TimeOutFunc);
        }
        private M2C_OperationPerformInfo GetCurrInfo()
        {
            return new M2C_OperationPerformInfo
            {
                OperationPlayerIndex = CurrentPlayerIndex,
                Operation = Operation,
                HandData = CurrentRoundStatus.HandData(CurrentPlayerIndex),
                BonusTurnTime = CurrentRoundStatus.GetBonusTurnTime(CurrentPlayerIndex),
                Rivers = CurrentRoundStatus.Rivers,
                MahjongSetData = MahjongSet.Data
            };
        }
        private M2C_OperationPerformInfo GetNotCurrInfo()
        {
            return new M2C_OperationPerformInfo
            {
                OperationPlayerIndex = CurrentPlayerIndex,
                Operation = Operation,
                HandData = new PlayerHandData
                {
                    HandTilesCount = CurrentRoundStatus.HandTiles(CurrentPlayerIndex).Count,
                    OpenMelds = CurrentRoundStatus.OpenMelds(CurrentPlayerIndex)
                },
                Rivers = CurrentRoundStatus.Rivers,
                MahjongSetData = MahjongSet.Data
            };
        }

        private void UpdateRoundStatus()
        {
            CurrentRoundStatus.CurrentPlayerIndex = CurrentPlayerIndex;
            // update hand tiles and open melds
            CurrentRoundStatus.RemoveFromRiver(DiscardPlayerIndex);
            CurrentRoundStatus.AddMeld(CurrentPlayerIndex, Operation.Meld);
            CurrentRoundStatus.RemoveTile(CurrentPlayerIndex, Operation.Meld);
            turnDoraAfterDiscard = Operation.Type == OutTurnOperationType.Kong;
        }

        private void KongOperation()
        {
            if (Operation.Type != OutTurnOperationType.Kong) return;
            ParentBehaviour.DrawTile(CurrentPlayerIndex, true, turnDoraAfterDiscard);
        }
        public void OnDiscardTileEvent(Event_DiscardTileInfo info)
        {
            if (info.PlayerIndex != CurrentRoundStatus.CurrentPlayerIndex)
            {
                Log.Debug(
                    $"[Server] It is not player {info.PlayerIndex}'s turn to discard a tile, ignoring this message");
                return;
            }

            // Change to discardTileState
            GetParent<MahjoneBehaviourComponent>().DiscardTile(
                info.PlayerIndex, info.Tile, info.IsRichiing,
                info.DiscardingLastDraw, info.BonusTurnTime, turnDoraAfterDiscard);
        }
    
        public void TimeOutFunc(bool b)
        {
            // force auto discard
            var tiles = CurrentRoundStatus.HandTiles(CurrentPlayerIndex);
            ParentBehaviour.DiscardTile(CurrentPlayerIndex, tiles[tiles.Count - 1], false, false, 0,
                turnDoraAfterDiscard);
        }

        public override void OnServerStateExit()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }
    }
}