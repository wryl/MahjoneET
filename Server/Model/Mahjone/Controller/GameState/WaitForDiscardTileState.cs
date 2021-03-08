using GamePlay.Server.Model;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 等待固定用户打出牌,此状态跟随于操作表现之后
    /// </summary>
    public class WaitForDiscardTileState : ServerState
    {
        private long timerId;
        //是否在打出牌后翻宝牌
        private bool turnDoraAfterDiscard;
        private int needDiscardPlayerIndex;
        private Mahjong.Model.Tile lastTile;
        public void Init(int playerindex,Mahjong.Model.Tile tile,bool turnDora)
        {
            needDiscardPlayerIndex = playerindex;
            lastTile = tile;
            turnDoraAfterDiscard = turnDora;
        }
        public void Destory()
        {
            OnServerStateExit();
        }
        public override void OnServerStateEnter()
        {
            Log.Debug($"WaitForDiscardTileState needDiscardPlayerIndex:{needDiscardPlayerIndex}");
            timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + gameSettings.BaseTurnTime * 1000
                            + CurrentRoundStatus.GetBonusTurnTime(needDiscardPlayerIndex) * 1000
                            + ServerConstants.ServerTimeBuffer, TimeOutAutoDiscard);
        }

        public override void OnServerStateExit()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }
        private void TimeOutAutoDiscard()
        {
            GetParent<MahjoneBehaviourComponent>().DiscardTile(
               needDiscardPlayerIndex, lastTile, false,
               true, 0, turnDoraAfterDiscard);
        }

        public void OnDiscardTileEvent(Event_DiscardTileInfo info)
        {
            if (info.PlayerIndex != needDiscardPlayerIndex)
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
    }
}