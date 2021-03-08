using GamePlay.Server.Model;
using Mahjong.Logic;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;
namespace ET
{
    public class PlayerKongState : ServerState
    {
        public int CurrentPlayerIndex;
        public MahjongSet MahjongSet => ParentBehaviour.mahjongSet;
        public OpenMeld Kong;
        private bool[] responds;
        private OutTurnOperation[] outTurnOperations;
        private long timerId;
        public void Init(int currindex, OpenMeld kong)
        {
            CurrentPlayerIndex = currindex;
            Kong = kong;
        }
        public override void OnServerStateEnter()
        {
            // update hand tiles and open melds
            UpdateRoundStatus();
            // send messages
            for (int i = 0; i < players.Count; i++)
            {
                var info = GetInfo(i);
                Game.EventSystem.Publish(new EventType.ActorMessage() { actorId = players[i], actorMessage = info }).Coroutine();
            }
            
            responds = new bool[players.Count];
            outTurnOperations = new OutTurnOperation[players.Count];
            timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + CurrentRoundStatus.MaxBonusTurnTime*1000 + gameSettings.BaseTurnTime*1000 +
                            ServerConstants.ServerTimeBuffer, TimeOutFunc);
        }

        private void UpdateRoundStatus()
        {
            var lastDraw = (Tile)CurrentRoundStatus.LastDraw;
            CurrentRoundStatus.LastDraw = null;
            CurrentRoundStatus.AddTile(CurrentPlayerIndex, lastDraw);
            if (Kong.IsAdded) // add kong
            {
                CurrentRoundStatus.AddKong(CurrentPlayerIndex, Kong);
                CurrentRoundStatus.RemoveTile(CurrentPlayerIndex, Kong.Extra);
            }
            else // self kong
            {
                CurrentRoundStatus.AddMeld(CurrentPlayerIndex, Kong);
                CurrentRoundStatus.RemoveTile(CurrentPlayerIndex, Kong);
            }

            CurrentRoundStatus.SortHandTiles();
            // turn dora if this is a self kong
            if (Kong.Side == MeldSide.Self)
                MahjongSet.TurnDora();
        }

        private M2C_KongInfo GetInfo(int index)
        {
            if (index == CurrentPlayerIndex)
            {
                return new M2C_KongInfo
                {
                    KongPlayerIndex = CurrentPlayerIndex,
                    HandData = CurrentRoundStatus.HandData(CurrentPlayerIndex),
                    BonusTurnTime = CurrentRoundStatus.GetBonusTurnTime(CurrentPlayerIndex),
                    Operations = GetKongOperations(CurrentPlayerIndex),
                    MahjongSetData = MahjongSet.Data
                };
            }
            else
            {
                return new M2C_KongInfo
                {
                    KongPlayerIndex = CurrentPlayerIndex,
                    HandData = new PlayerHandData
                    {
                        HandTilesCount = CurrentRoundStatus.HandTiles(CurrentPlayerIndex).Count,
                        OpenMelds = CurrentRoundStatus.OpenMelds(CurrentPlayerIndex)
                    },
                    BonusTurnTime = CurrentRoundStatus.GetBonusTurnTime(CurrentPlayerIndex),
                    Operations = GetKongOperations(index),
                    MahjongSetData = MahjongSet.Data
                };
            }
        }

        private List<OutTurnOperation> GetKongOperations(int playerIndex)
        {
            var operations = new List<OutTurnOperation>();
            operations.Add(new OutTurnOperation
            {
                Type = OutTurnOperationType.Skip
            });
            if (playerIndex == CurrentPlayerIndex) return operations;
            // rob kong test
            TestRobKong(playerIndex, operations);
            return operations;
        }

        private Tile GetTileFromKong()
        {
            if (Kong.Side == MeldSide.Self) return Kong.First;
            return Kong.Tile;
        }

        private void TestRobKong(int playerIndex, IList<OutTurnOperation> operations)
        {
            var tile = GetTileFromKong();
            var point = GetRongInfo(playerIndex, tile);
            if (!gameSettings.CheckConstraint(point)) return;
            if (Kong.Side == MeldSide.Self)
            {
                // handle self kong
                if (gameSettings.AllowGswsRobConcealedKong &&
                    point.YakuList.Any(yaku => yaku.Name.StartsWith("国士无双")))
                {
                    operations.Add(new OutTurnOperation
                    {
                        Type = OutTurnOperationType.Rong,
                        Tile = tile,
                        HandData = CurrentRoundStatus.HandData(playerIndex)
                    });
                }
            }
            else
            {
                // handle added kong
                operations.Add(new OutTurnOperation
                {
                    Type = OutTurnOperationType.Rong,
                    Tile = tile,
                    HandData = CurrentRoundStatus.HandData(playerIndex)
                });
            }
        }

        private PointInfo GetRongInfo(int playerIndex, Tile tile)
        {
            var baseHandStatus = HandStatus.RobKong;
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

        public void TimeOutFunc()
        {
            // check operations: if all operations are skip, let the current player to draw his lingshang
            // if some one claimed rong, transfer to TurnEndState handling rong operations
            for (int i = 0; i < responds.Length; i++)
            {
                if (responds[i]) continue;
                // players[i].BonusTurnTime = 0;
                outTurnOperations[i] = new OutTurnOperation { Type = OutTurnOperationType.Skip };
                NextState();
                return;
            }
        }

        private void NextState()
        {
            if (outTurnOperations.All(op => op.Type == OutTurnOperationType.Skip))
            {
                // no one claimed a rob kong
                var turnDoraAfterDiscard = Kong.Side != MeldSide.Self;
                CurrentRoundStatus.BreakOneShotsAndFirstTurn();
                ParentBehaviour.DrawTile(CurrentPlayerIndex, true, turnDoraAfterDiscard);
                return;
            }

            if (outTurnOperations.Any(op => op.Type == OutTurnOperationType.Rong))
            {
                var discardingTile = GetTileFromKong();
                ParentBehaviour.TurnEnd(CurrentPlayerIndex, discardingTile, false, outTurnOperations, true,
                    false);
                return;
            }

            Log.Error(
                $"[Server] Logically cannot reach here, operations are {string.Join("|", outTurnOperations)}");
        }

        public void OnOutTurnOperationEvent(Event_OutTurnOperationInfo info)
        {
            var index = info.PlayerIndex;
            if (responds[index]) return;
            responds[index] = true;
            outTurnOperations[index] = info.Operation;
            CurrentRoundStatus.SetBonusTurnTime(index, info.BonusTurnTime);
            if (responds.All(r => r))
            {
                Log.Debug("[Server] Server received all operation response, handling results.");
                NextState();
            }
        }
    }
}