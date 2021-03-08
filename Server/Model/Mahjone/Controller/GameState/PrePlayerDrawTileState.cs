using GamePlay.Server.Model;
using Mahjong.Logic;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class PrePlayerDrawTileStateDestroySystem : DestroySystem<PrePlayerDrawTileState>
    {
        public override void Destroy(PrePlayerDrawTileState self)
        {
            self.Destroy();
        }
    }
    /// <summary>
    /// 玩家选牌 暂定3选1
    /// </summary>
    public class PrePlayerDrawTileState : ServerState
    {
        public int CurrentPlayerIndex;
        public MahjongSet MahjongSet => GetParent<MahjoneBehaviourComponent>().mahjongSet;
        public bool IsLingShang;
        public bool TurnDoraAfterDiscard;
        public Tile justDraw;
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

            var justDraw = MahjongSet.PeekTiles(3);
            CurrentRoundStatus.CurrentPlayerIndex = CurrentPlayerIndex;
            for (int index = 0; index < players.Count; index++)
            {

                if (index == CurrentPlayerIndex)
                {
                    var info = new M2C_PreDrawTileInfo
                    {
                        DrawPlayerIndex = CurrentPlayerIndex,
                    };
                    info.Tiles = justDraw;
                    info.BonusTurnTime = CurrentRoundStatus.GetBonusTurnTime(CurrentPlayerIndex);
                    Game.EventSystem.Publish(new EventType.ActorMessage() { actorId = players[index], actorMessage = info }).Coroutine();
                }
            }

            timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + gameSettings.BaseTurnTime*1000
                            + CurrentRoundStatus.GetBonusTurnTime(CurrentPlayerIndex)*1000
                            + ServerConstants.ServerTimeBuffer, TimeOutFunc);
        }
        public void OnSelectTileEvent(Event_SelectTileInfo info)
        {
            if (info.PlayerIndex != CurrentRoundStatus.CurrentPlayerIndex)
            {
                Log.Debug(
                    $"[Server] It is not player {info.PlayerIndex}'s turn to discard a tile, ignoring this message");
                return;
            }
            //开始调整次序

            MahjongSet.TransTiles(info.SelectIndex);
            CurrentRoundStatus.SetBonusTurnTime(info.PlayerIndex, info.BonusTurnTime);
            CurrentRoundStatus.AddToRiver(info.PlayerIndex, MahjongSet.DrawTile());
            CurrentRoundStatus.AddToRiver(info.PlayerIndex, MahjongSet.DrawTile());

            // Change to DrawTileState
            GetParent<MahjoneBehaviourComponent>().DrawTile(
                info.PlayerIndex,IsLingShang, TurnDoraAfterDiscard);
        }

      
     

        public void TimeOutFunc()
        {
            // force auto draw
            GetParent<MahjoneBehaviourComponent>().DrawTile(CurrentPlayerIndex, IsLingShang, TurnDoraAfterDiscard);
        }

        public override void OnServerStateExit()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }
    }
}