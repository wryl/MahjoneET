
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
            //如果是杠则进入摸牌状态,否则进入待打牌状态
            if (Operation.Type == OutTurnOperationType.Kong)
            { 
                ParentBehaviour.DrawTile(CurrentPlayerIndex, true, turnDoraAfterDiscard);
            }
            else
            {
                var tiles = CurrentRoundStatus.HandTiles(CurrentPlayerIndex);
                ParentBehaviour.WaitingDiscardTile(CurrentPlayerIndex, tiles[tiles.Count - 1], turnDoraAfterDiscard);
            }
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


        public override void OnServerStateExit()
        {

        }
    }
}