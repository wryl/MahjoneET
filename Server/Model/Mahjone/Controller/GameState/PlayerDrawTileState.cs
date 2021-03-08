using GamePlay.Server.Model;
using Mahjong.Logic;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class PlayerDrawTileStateDestroySystem : DestroySystem<PlayerDrawTileState>
    {
        public override void Destroy(PlayerDrawTileState self)
        {
            self.Destroy();
        }
    }
    public class PlayerDrawTileState : ServerState
    {
        public int CurrentPlayerIndex;
        public MahjongSet MahjongSet => GetParent<MahjoneBehaviourComponent>().mahjongSet;
        public bool IsLingShang;
        public bool TurnDoraAfterDiscard;
        public Tile justDraw;
        private PointInfo tsumoPointInfo;
        private long timerId;
        public void Init(int index, bool isLingShang, bool turnDoraAfterDiscard)
        {
            CurrentPlayerIndex = index;
            IsLingShang = isLingShang;
            TurnDoraAfterDiscard = turnDoraAfterDiscard;
        }

        public void Destroy()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }

        public override void OnServerStateEnter()
        {
            if (IsLingShang)
                justDraw = MahjongSet.DrawLingShang();
            else
                justDraw = MahjongSet.DrawTile();
            CurrentRoundStatus.CurrentPlayerIndex = CurrentPlayerIndex;
            CurrentRoundStatus.LastDraw = justDraw;
            CurrentRoundStatus.CheckFirstTurn(CurrentPlayerIndex);
            CurrentRoundStatus.BreakTempZhenting(CurrentPlayerIndex);
            Log.Debug($"[Server] Distribute a tile {justDraw} to current turn player {CurrentPlayerIndex}, "
                      + $"first turn: {CurrentRoundStatus.FirstTurn}.");
            for (int index = 0; index < players.Count; index++)
            {
                var info = new M2C_DrawTileInfo
                {
                    DrawPlayerIndex = CurrentPlayerIndex,
                    MahjongSetData = MahjongSet.Data
                };
                if (index == CurrentPlayerIndex)
                {
                    info.Tile = justDraw;
                    info.BonusTurnTime = CurrentRoundStatus.GetBonusTurnTime(CurrentPlayerIndex);
                    info.Zhenting = CurrentRoundStatus.IsZhenting(CurrentPlayerIndex);
                    info.Operations = GetOperations(CurrentPlayerIndex);
                }
                Game.EventSystem.Publish(new EventType.ActorMessage() {actorId= players[index],actorMessage=info }).Coroutine();
            }

            timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + gameSettings.BaseTurnTime*1000
                            + CurrentRoundStatus.GetBonusTurnTime(CurrentPlayerIndex)*1000
                            + ServerConstants.ServerTimeBuffer, TimeOutFunc);
        }

        private List<InTurnOperation> GetOperations(int playerIndex)
        {
            var operations = new List<InTurnOperation> { new InTurnOperation { Type = InTurnOperationType.Discard } };
            // test tsumo
            TestTsumo(playerIndex, justDraw, operations);
            var handTiles = CurrentRoundStatus.HandTiles(playerIndex);
            var openMelds = CurrentRoundStatus.Melds(playerIndex);
            // test round draw
            Test9Orphans(handTiles, operations);
            // test richi
            TestRichi(playerIndex, handTiles, openMelds, operations);
            // test kongs
            TestKongs(playerIndex, handTiles, operations);
            // test bei
            TestBei(playerIndex, handTiles, operations);
            return operations;
        }

        private void TestTsumo(int playerIndex, Tile tile, List<InTurnOperation> operations)
        {
            var baseHandStatus = HandStatus.Tsumo;
            // test haidi
            if (MahjongSet.TilesRemain == gameSettings.MountainReservedTiles)
                baseHandStatus |= HandStatus.Haidi;
            // test lingshang
            if (IsLingShang) baseHandStatus |= HandStatus.Lingshang;
            var allTiles = MahjongSet.AllTiles;
            var doraTiles = MahjongSet.DoraIndicators.Select(
                indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
            var uraDoraTiles = MahjongSet.UraDoraIndicators.Select(
                indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
            var beiDora = CurrentRoundStatus.GetBeiDora(playerIndex);
            tsumoPointInfo = ServerMahjongLogic.GetPointInfo(
                playerIndex, CurrentRoundStatus, tile, baseHandStatus,
                doraTiles, uraDoraTiles, beiDora, gameSettings);
            // test if enough
            if (gameSettings.CheckConstraint(tsumoPointInfo))
            {
                operations.Add(new InTurnOperation
                {
                    Type = InTurnOperationType.Tsumo,
                    Tile = justDraw
                });
            }
        }

        private void Test9Orphans(List<Tile> handTiles, IList<InTurnOperation> operations)
        {
            if (!gameSettings.Allow9OrphanDraw) return;
            if (!CurrentRoundStatus.FirstTurn) return;
            if (MahjongLogic.Test9KindsOfOrphans(handTiles, justDraw))
            {
                operations.Add(new InTurnOperation
                {
                    Type = InTurnOperationType.RoundDraw
                });
            }
        }

        private void TestRichi(int playerIndex, List<Tile> handTiles, Meld[] openMelds, IList<InTurnOperation> operations)
        {
            var alreadyRichied = CurrentRoundStatus.RichiStatus(playerIndex);
            if (alreadyRichied) return;
            var availability = gameSettings.AllowRichiWhenPointsLow ||
                               CurrentRoundStatus.GetPoints(playerIndex) >= gameSettings.RichiMortgagePoints;
            if (!availability) return;
            IList<Tile> availableTiles;
            if (MahjongLogic.TestRichi(handTiles, openMelds, justDraw, gameSettings.AllowRichiWhenNotReady,
                out availableTiles))
            {
                operations.Add(new InTurnOperation
                {
                    Type = InTurnOperationType.Richi,
                    RichiAvailableTiles = availableTiles.ToArray()
                });
            }
        }

        private void TestKongs(int playerIndex, List<Tile> handTiles, IList<InTurnOperation> operations)
        {
            if (CurrentRoundStatus.KongClaimed == MahjongConstants.MaxKongs)
                return; // no more kong can be claimed after 4 kongs claimed
            var alreadyRichied = CurrentRoundStatus.RichiStatus(playerIndex);
            if (alreadyRichied)
            {
                // test kongs in richied player hand
                var richiKongs = MahjongLogic.GetRichiKongs(handTiles, justDraw);
                if (richiKongs.Any())
                {
                    foreach (var kong in richiKongs)
                    {
                        operations.Add(new InTurnOperation
                        {
                            Type = InTurnOperationType.Kong,
                            Meld = kong
                        });
                    }
                }
            }
            else
            {
                // 1. test self kongs, aka four same tiles in hand and lastdraw
                var selfKongs = MahjongLogic.GetSelfKongs(handTiles, justDraw);
                if (selfKongs.Any())
                {
                    foreach (var kong in selfKongs)
                    {
                        operations.Add(new InTurnOperation
                        {
                            Type = InTurnOperationType.Kong,
                            Meld = kong
                        });
                    }
                }

                // 2. test add kongs, aka whether a single tile in hand and lastdraw is identical to a pong in open melds
                var addKongs = MahjongLogic.GetAddKongs(
                    CurrentRoundStatus.HandTiles(playerIndex), CurrentRoundStatus.OpenMelds(playerIndex), justDraw);
                if (addKongs.Any())
                {
                    foreach (var kong in addKongs)
                    {
                        operations.Add(new InTurnOperation
                        {
                            Type = InTurnOperationType.Kong,
                            Meld = kong
                        });
                    }
                }
            }
        }

        public void TestBei(int playerIndex, List<Tile> handTiles, IList<InTurnOperation> operations)
        {
            if (!gameSettings.AllowBeiDora) return;
            var beiTile = new Tile(Suit.Z, 4);
            int bei = handTiles.Count(tile => tile.EqualsIgnoreColor(beiTile));
            if (bei > 0)
            {
                operations.Add(new InTurnOperation
                {
                    Type = InTurnOperationType.Bei
                });
            }
            else if (justDraw.EqualsIgnoreColor(beiTile))
            {
                operations.Add(new InTurnOperation
                {
                    Type = InTurnOperationType.Bei
                });
            }
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
                info.DiscardingLastDraw, info.BonusTurnTime, TurnDoraAfterDiscard);
        }

        public void OnInTurnOperationEvent(Event_InTurnOperationInfo info)
        {
            if (info.PlayerIndex != CurrentRoundStatus.CurrentPlayerIndex)
            {
                Log.Debug(
                    $"[Server] It is not player {info.PlayerIndex}'s turn to perform a in turn operation, ignoring this message");
                return;
            }

            // handle message according to its type
            var operation = info.Operation;
            switch (operation.Type)
            {
                case InTurnOperationType.Tsumo:
                    HandleTsumo(operation);
                    break;
                case InTurnOperationType.Kong:
                    HandleKong(operation);
                    break;
                case InTurnOperationType.RoundDraw:
                    HandleRoundDraw(operation);
                    break;
                case InTurnOperationType.Bei:
                    HandleBei(operation);
                    break;
                default:
                    Log.Error($"[Server] This type of in turn operation should not be sent to server.");
                    break;
            }
        }
        private bool GetTsumoPoints(int playerIndex, Tile tile)
        {
            var baseHandStatus = HandStatus.Tsumo;
            // test haidi
            if (MahjongSet.TilesRemain == gameSettings.MountainReservedTiles)
                baseHandStatus |= HandStatus.Haidi;
            // test lingshang
            if (IsLingShang) baseHandStatus |= HandStatus.Lingshang;
            var allTiles = MahjongSet.AllTiles;
            var doraTiles = MahjongSet.DoraIndicators.Select(
                indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
            var uraDoraTiles = MahjongSet.UraDoraIndicators.Select(
                indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
            var beiDora = CurrentRoundStatus.GetBeiDora(playerIndex);
            tsumoPointInfo = ServerMahjongLogic.GetPointInfo(
                playerIndex, CurrentRoundStatus, tile, baseHandStatus,
                doraTiles, uraDoraTiles, beiDora, gameSettings);
            // test if enough
            if (gameSettings.CheckConstraint(tsumoPointInfo))
            {
                return true;
            }
            return false;
        }
        private void HandleTsumo(InTurnOperation operation)
        {
            int playerIndex = CurrentRoundStatus.CurrentPlayerIndex;
            GetTsumoPoints(playerIndex, justDraw);
            GetParent<MahjoneBehaviourComponent>().Tsumo(playerIndex, operation.Tile, tsumoPointInfo);
        }

        private void HandleKong(InTurnOperation operation)
        {
            int playerIndex = CurrentRoundStatus.CurrentPlayerIndex;
            var kong = operation.Meld;
            Log.Debug($"Server is handling the operation of player {playerIndex} of claiming kong {kong}");
            GetParent<MahjoneBehaviourComponent>().Kong(playerIndex, kong);
        }

        private void HandleRoundDraw(InTurnOperation operation)
        {
            int playerIndex = CurrentRoundStatus.CurrentPlayerIndex;
            Log.Debug($"Player {playerIndex} has claimed 9-orphans");
            GetParent<MahjoneBehaviourComponent>().RoundDraw(RoundDrawType.NineOrphans);
        }

        private void HandleBei(InTurnOperation operation)
        {
            int playerIndex = CurrentRoundStatus.CurrentPlayerIndex;
            Log.Debug($"Player {playerIndex} has claimed a bei-dora");
            GetParent<MahjoneBehaviourComponent>().BeiDora(playerIndex);
        }

        public void TimeOutFunc()
        {
            // force auto discard
            GetParent<MahjoneBehaviourComponent>().DiscardTile(CurrentPlayerIndex, (Tile)CurrentRoundStatus.LastDraw, false, true,
                0, TurnDoraAfterDiscard);
        }

        public override void OnServerStateExit()
        {
            CurrentRoundStatus.CheckOneShot(CurrentPlayerIndex);
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }
    }
}