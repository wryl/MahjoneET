using GamePlay.Server.Model;
using Mahjong.Logic;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;
namespace ET
{
    /// <summary>
    /// This turn is used to end a player's turn, complete richi declaration, etc.
    /// Transfers to PlayerDiscardTileState (when a opening is claimed), PlayerDrawTileState (when kong is claimed or nothing claimed),
    /// RoundEndState (when a rong is claimed or when there are no more tiles to draw)
    /// </summary>
    public class TurnEndState : ServerState
    {
        public int CurrentPlayerIndex => CurrentRoundStatus.CurrentPlayerIndex;
        public Tile DiscardingTile;
        public bool IsRichiing;
        public OutTurnOperation[] Operations;
        public MahjongSet MahjongSet => ParentBehaviour.mahjongSet;
        public bool IsRobKong;
        public bool TurnDoraAfterDiscard;
        public OutTurnOperationType operationChosen;
        private long timerId;
        public void Init(Tile tile, OutTurnOperation[] operations, bool isRichiing, bool isRobKong, bool turnDoraAfterDiscard)
        {
            DiscardingTile = tile;
            Operations = operations;
            IsRichiing = isRichiing;
            IsRobKong = isRobKong;
            TurnDoraAfterDiscard = turnDoraAfterDiscard;
        }

        public void Destroy()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }

        private void TimeOutFunc()
        {
            TurnEndTimeOut();
        }

        public override void OnServerStateEnter()
        {
            // determines the operation to take when turn ends
            operationChosen = ChooseOperations();
            Log.Debug(
                $"The operation chosen by this round is {operationChosen}, operation after choosing: {string.Join(",", Operations)}");
            // if operation is not rong or round-draw, perform richi and test zhenting
            if (operationChosen != OutTurnOperationType.Rong && operationChosen != OutTurnOperationType.RoundDraw)
            {
                CurrentRoundStatus.TryRichi(CurrentPlayerIndex, IsRichiing);
                CurrentRoundStatus.UpdateTempZhenting(CurrentPlayerIndex, DiscardingTile);
                CurrentRoundStatus.UpdateDiscardZhenting();
                CurrentRoundStatus.UpdateRichiZhenting(DiscardingTile);
            }

            if (operationChosen == OutTurnOperationType.Chow
                || operationChosen == OutTurnOperationType.Pong
                || operationChosen == OutTurnOperationType.Kong)
                CurrentRoundStatus.BreakOneShotsAndFirstTurn();
            timerId = TimerComponent.Instance.NewOnceTimer(operationChosen == OutTurnOperationType.Rong || operationChosen == OutTurnOperationType.RoundDraw
                    ? TimeHelper.ServerNow() + ServerConstants.ServerTurnEndTimeOutExtra
                    : TimeHelper.ServerNow() + ServerConstants.ServerTurnEndTimeOut, TimeOutFunc);
            for (int i = 0; i < players.Count; i++)
            {
                var info = new M2C_TurnEndInfo
                {
                    PlayerIndex = i,
                    ChosenOperationType = operationChosen,
                    Operations = Operations.ToList(),
                    RichiStatus = CurrentRoundStatus.RichiStatusArray.ToList(),
                    RichiSticks = CurrentRoundStatus.RichiSticks,
                    Points = CurrentRoundStatus.Points,
                    MahjongSetData = MahjongSet.Data,
                    Zhenting= CurrentRoundStatus.IsZhenting(i),
                };
                Game.EventSystem.Publish(new EventType.ActorMessage() { actorId = players[i], actorMessage = info }).Coroutine();
            }
        }

        private OutTurnOperationType ChooseOperations()
        {
            Log.Debug($"[Server] Operation before choosing: {string.Join(",", Operations)}");
            // test every circumstances by priority
            // test for rong
            if (Operations.Any(op => op.Type == OutTurnOperationType.Rong))
            {
                // check if 3 rong
                if (CurrentRoundStatus.CheckThreeRongs(Operations))
                {
                    for (int i = 0; i < Operations.Length; i++)
                    {
                        Operations[i] = new OutTurnOperation
                        {
                            Type = OutTurnOperationType.RoundDraw,
                            RoundDrawType = RoundDrawType.ThreeRong
                        };
                    }

                    return OutTurnOperationType.RoundDraw;
                }

                for (int i = 0; i < Operations.Length; i++)
                {
                    if (Operations[i].Type != OutTurnOperationType.Rong)
                        Operations[i] = new OutTurnOperation { Type = OutTurnOperationType.Skip };
                }

                return OutTurnOperationType.Rong;
            }

            // check if 4 winds
            if (CurrentRoundStatus.CheckFourWinds())
            {
                Log.Debug($"[Server] Round draw -- Four winds");
                for (int i = 0; i < Operations.Length; i++)
                {
                    Operations[i] = new OutTurnOperation
                    {
                        Type = OutTurnOperationType.RoundDraw,
                        RoundDrawType = RoundDrawType.FourWinds
                    };
                }

                return OutTurnOperationType.RoundDraw;
            }

            // check if 4 richis
            if (CurrentRoundStatus.CheckFourRichis())
            {
                Log.Debug($"[Server] Round draw -- Four richis");
                for (int i = 0; i < Operations.Length; i++)
                {
                    Operations[i] = new OutTurnOperation
                    {
                        Type = OutTurnOperationType.RoundDraw,
                        RoundDrawType = RoundDrawType.FourRichis
                    };
                }

                return OutTurnOperationType.RoundDraw;
            }

            // check if 4 kongs
            if (CurrentRoundStatus.CheckFourKongs())
            {
                Log.Debug($"[Server] Round draw -- Four kongs");
                for (int i = 0; i < Operations.Length; i++)
                {
                    Operations[i] = new OutTurnOperation
                    {
                        Type = OutTurnOperationType.RoundDraw,
                        RoundDrawType = RoundDrawType.FourKongs
                    };
                }

                return OutTurnOperationType.RoundDraw;
            }

            // check if run out of tiles -- leads to a normal round draw
            if (MahjongSet.TilesRemain <= gameSettings.MountainReservedTiles)
            {
                // no more tiles to draw and no one choose a rong operation.
                Log.Debug("No more tiles to draw and nobody claims a rong, the round has drawn.");
                for (int i = 0; i < Operations.Length; i++)
                    Operations[i] = new OutTurnOperation
                    {
                        Type = OutTurnOperationType.RoundDraw,
                        RoundDrawType = RoundDrawType.RoundDraw
                    };
                return OutTurnOperationType.RoundDraw;
            }

            // check if some one claimed kong
            if (Operations.Any(op => op.Type == OutTurnOperationType.Kong))
            {
                for (int i = 0; i < Operations.Length; i++)
                {
                    if (Operations[i].Type != OutTurnOperationType.Kong)
                        Operations[i] = new OutTurnOperation { Type = OutTurnOperationType.Skip };
                }

                return OutTurnOperationType.Kong;
            }

            // check if some one claimed pong
            if (Operations.Any(op => op.Type == OutTurnOperationType.Pong))
            {
                for (int i = 0; i < Operations.Length; i++)
                {
                    if (Operations[i].Type != OutTurnOperationType.Pong)
                        Operations[i] = new OutTurnOperation { Type = OutTurnOperationType.Skip };
                }

                return OutTurnOperationType.Pong;
            }

            // check if some one claimed chow
            if (Operations.Any(op => op.Type == OutTurnOperationType.Chow))
            {
                for (int i = 0; i < Operations.Length; i++)
                {
                    if (Operations[i].Type != OutTurnOperationType.Chow)
                        Operations[i] = new OutTurnOperation { Type = OutTurnOperationType.Skip };
                }

                return OutTurnOperationType.Chow;
            }

            // no particular operations -- skip
            return OutTurnOperationType.Skip;
        }

        private void TurnEndTimeOut()
        {
            // determines which state the server should transfer to by operationChosen.
            switch (operationChosen)
            {
                case OutTurnOperationType.Rong:
                    HandleRong();
                    break;
                case OutTurnOperationType.RoundDraw:
                    ParentBehaviour.BattleRoundDraw((CurrentPlayerIndex + 1) % players.Count);
                    break;
                case OutTurnOperationType.Kong:
                case OutTurnOperationType.Pong:
                case OutTurnOperationType.Chow:
                    int index = System.Array.FindIndex(Operations, op => op.Type != OutTurnOperationType.Skip);
                    ParentBehaviour.PerformOutTurnOperation(index, Operations[index]);
                    break;
                case OutTurnOperationType.Skip:
                    int nextPlayer = CurrentPlayerIndex + 1;
                    if (nextPlayer >= players.Count) nextPlayer -= players.Count;
                    Log.Debug($"[Server] Next turn player index: {nextPlayer}");
                    ParentBehaviour.PreDrawTile(nextPlayer);
                    break;
                default:
                    Log.Error($"Unknown type of out turn operation: {operationChosen}");
                    break;
            }
        }

        private void HandleRong()
        {
            var rongPlayerIndexList = new List<int>();
            for (int i = 0; i < Operations.Length; i++)
            {
                if (Operations[i].Type == OutTurnOperationType.Rong)
                    rongPlayerIndexList.Add(i);
            }

            // sort this array
            rongPlayerIndexList.Sort(new RongPlayerIndexComparer(CurrentPlayerIndex, players.Count));
            var rongPlayerIndices = rongPlayerIndexList.ToArray();
            var rongPointInfos = new PointInfo[rongPlayerIndices.Length];
            for (int i = 0; i < rongPlayerIndices.Length; i++)
            {
                int playerIndex = rongPlayerIndices[i];
                rongPointInfos[i] = GetRongInfo(playerIndex, DiscardingTile);
            }

            Log.Debug($"[Server] Players who claimed rong: {string.Join(", ", rongPlayerIndices)}, "
                      + $"corresponding pointInfos: {string.Join(";", rongPointInfos)}");
            ParentBehaviour.Rong(CurrentPlayerIndex, DiscardingTile, rongPlayerIndices, rongPointInfos);
        }

        private PointInfo GetRongInfo(int playerIndex, Tile discard)
        {
            var baseHandStatus = HandStatus.Nothing;
            // test haidi
            if (MahjongSet.TilesRemain == gameSettings.MountainReservedTiles)
                baseHandStatus |= HandStatus.Haidi;
            // test rob kong
            if (IsRobKong)
                baseHandStatus |= HandStatus.RobKong;
            // test lingshang -- not gonna happen
            var allTiles = MahjongSet.AllTiles;
            var doraTiles = MahjongSet.DoraIndicators.Select(
                indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
            var uraDoraTiles = MahjongSet.UraDoraIndicators.Select(
                indicator => MahjongLogic.GetDoraTile(indicator, allTiles)).ToArray();
            var beiDora = CurrentRoundStatus.GetBeiDora(playerIndex);
            var point = ServerMahjongLogic.GetPointInfo(
                playerIndex, CurrentRoundStatus, discard, baseHandStatus,
                doraTiles, uraDoraTiles, beiDora, gameSettings);
            Log.Debug($"TurnEndState: pointInfo: {point}");
            return point;
        }

        public override void OnServerStateExit()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
            if (operationChosen != OutTurnOperationType.Rong && TurnDoraAfterDiscard)
                MahjongSet.TurnDora();
        }

        private struct RongPlayerIndexComparer : IComparer<int>
        {
            private int current;
            private int total;

            public RongPlayerIndexComparer(int current, int total)
            {
                this.current = current;
                this.total = total;
            }

            public int Compare(int x, int y)
            {
                int dx = x - current;
                int dy = y - current;
                if (dx < 0) dx += total;
                if (dy < 0) dy += total;
                return dx - dy;
            }
        }
    }
}