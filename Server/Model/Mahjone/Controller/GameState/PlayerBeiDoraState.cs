using GamePlay.Server.Model;
using Mahjong.Logic;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;


namespace ET
{
    public class PlayerBeiDoraState : ServerState
    {
        public int CurrentPlayerIndex;
        public MahjongSet MahjongSet => ParentBehaviour.mahjongSet;
        private bool[] responds;
        private OutTurnOperation[] outTurnOperations;
        private long timerId;
        public void Init(int playerIndex)
        {
            CurrentPlayerIndex = playerIndex;
        }
        public void Destroy()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
            responds = null;
        }
        public override void OnServerStateEnter()
        {
            // update hand tiles and bei doras
            UpdateRoundStatus();
            // send messages
            for (int i = 0; i < players.Count; i++)
            {

                Game.EventSystem.Publish(new EventType.ActorMessage() { actorId = players[i], actorMessage = GetInfo(i) }).Coroutine();
            }

            responds = new bool[players.Count];
            outTurnOperations = new OutTurnOperation[players.Count];
            timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + CurrentRoundStatus.MaxBonusTurnTime + gameSettings.BaseTurnTime +
                            ServerConstants.ServerTimeBuffer, TimeoutFunc);
        }

        private void TimeoutFunc()
        {
            for (int i = 0; i < responds.Length; i++)
            {
                if (responds[i]) continue;
                // players[i].BonusTurnTime = 0;
                outTurnOperations[i] = new OutTurnOperation { Type = OutTurnOperationType.Skip };
            }
            NextState();
        }

        private void UpdateRoundStatus()
        {
            var lastDraw = (Tile)CurrentRoundStatus.LastDraw;
            CurrentRoundStatus.LastDraw = null;
            CurrentRoundStatus.AddTile(CurrentPlayerIndex, lastDraw);
            CurrentRoundStatus.RemoveTile(CurrentPlayerIndex, new Tile(Suit.Z, 4));
            CurrentRoundStatus.AddBeiDoras(CurrentPlayerIndex);
            CurrentRoundStatus.SortHandTiles();
        }

        private M2C_BeiDoraInfo GetInfo(int index)
        {
            if (index == CurrentPlayerIndex)
            {
                return new M2C_BeiDoraInfo
                {
                    BeiDoraPlayerIndex = CurrentPlayerIndex,
                    BeiDoras = CurrentRoundStatus.GetBeiDoras().ToList(),
                    HandData = CurrentRoundStatus.HandData(CurrentPlayerIndex),
                    BonusTurnTime = CurrentRoundStatus.GetBonusTurnTime(CurrentPlayerIndex),
                    Operations = GetBeiDoraOperations(CurrentPlayerIndex)
                };
            }
            else
            {
                return new M2C_BeiDoraInfo
                {
                    BeiDoraPlayerIndex = CurrentPlayerIndex,
                    BeiDoras = CurrentRoundStatus.GetBeiDoras().ToList(),
                    HandData = new PlayerHandData
                    {
                        HandTilesCount = CurrentRoundStatus.HandTiles(CurrentPlayerIndex).Count,
                        OpenMelds = CurrentRoundStatus.OpenMelds(CurrentPlayerIndex)
                    },
                    BonusTurnTime = CurrentRoundStatus.GetBonusTurnTime(index),
                    Operations = GetBeiDoraOperations(index),
                    MahjongSetData = MahjongSet.Data
                };
            }
        }

        private List<OutTurnOperation> GetBeiDoraOperations(int playerIndex)
        {
            var operations = new List<OutTurnOperation>();
            operations.Add(new OutTurnOperation
            {
                Type = OutTurnOperationType.Skip
            });
            if (playerIndex == CurrentPlayerIndex) return operations;
            // rong test
            TestBeiDoraRong(playerIndex, operations);
            return operations;
        }

        private void TestBeiDoraRong(int playerIndex, IList<OutTurnOperation> operations)
        {
            var tile = new Tile(Suit.Z, 4);
            var point = GetRongInfo(playerIndex, tile);
            if (!gameSettings.CheckConstraint(point)) return;
            operations.Add(new OutTurnOperation
            {
                Type = OutTurnOperationType.Rong,
                Tile = tile,
                HandData = CurrentRoundStatus.HandData(playerIndex)
            });
        }

        private PointInfo GetRongInfo(int playerIndex, Tile tile)
        {
            var baseHandStatus = HandStatus.Nothing;
            if (gameSettings.AllowBeiDoraRongAsRobbKong) baseHandStatus |= HandStatus.RobKong;
            var allTiles = MahjongSet.AllTiles;
            var doraTiles = MahjongSet.DoraIndicators.Select(
                indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
            var uraDoraTiles = MahjongSet.UraDoraIndicators.Select(
                indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
            var beiDora = CurrentRoundStatus.GetBeiDora(playerIndex);
            var point = ServerMahjongLogic.GetPointInfo(
                playerIndex, CurrentRoundStatus, tile, baseHandStatus,
                doraTiles, uraDoraTiles, beiDora, gameSettings);
            return point;
        }

        public override void OnServerStateExit()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }



        private void NextState()
        {
            if (outTurnOperations.All(op => op.Type == OutTurnOperationType.Skip))
            {
                // no one claimed rong
                var turnDoraAfterDiscard = false;
                var isLingShang = gameSettings.AllowBeiDoraTsumoAsLingShang;
                CurrentRoundStatus.BreakOneShotsAndFirstTurn();
                ParentBehaviour.DrawTile(CurrentPlayerIndex, isLingShang, turnDoraAfterDiscard);
                return;
            }

            if (outTurnOperations.Any(op => op.Type == OutTurnOperationType.Rong))
            {
                var tile = new Tile(Suit.Z, 4);
                var turnDoraAfterDiscard = false;
                var isRobKong = gameSettings.AllowBeiDoraRongAsRobbKong;
                ParentBehaviour.TurnEnd(CurrentPlayerIndex, tile, false, outTurnOperations, isRobKong,
                    turnDoraAfterDiscard);
                return;
            }

            Log.Error(
                $"[Server] Logically cannot reach here, operations are {string.Join("|", outTurnOperations)}");
        }

        public void OnOutTurnOperationEvent(int index, OutTurnOperation operation,int bonusTime)
        {
            if (responds[index]) return;
            responds[index] = true;
            outTurnOperations[index] = operation;
            CurrentRoundStatus.SetBonusTurnTime(index, bonusTime);
            if (responds.All(r => r))
            {
                Log.Debug("[Server] Server received all operation response, handling results.");
                NextState();
            }
        }

  
    }
}