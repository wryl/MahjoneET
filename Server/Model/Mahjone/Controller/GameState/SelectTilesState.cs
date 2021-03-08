using Mahjong.Logic;
using Mahjong.Model;
using System.Collections.Generic;

namespace ET
{
    /// <summary>
    /// 在游戏准备后.先进行选牌
    /// </summary>
    public class SelectTilesState : ServerState
    {
        private HashSet<long> responds;
        private List<Tile> SelectTiles;
        private long timerId;
        public void Init()
        {
            responds = new HashSet<long>();
            SelectTiles = new List<Tile>();
        }
        public void Destroy()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
            responds = null;
        }
        public override void OnServerStateEnter()
        {
            //这里不做超时处理
            //timerId = TimerComponent.Instance.NewOnceTimer(1000*60, TimeOutRoundStart);
            Log.Debug("MahjongConstants.FullTiles cout:"+ MahjongConstants.FullTiles.Count);
            var info = new M2C_SelectTiles
            {
                AllTiles = MahjongConstants.FullTiles
            };
            Game.EventSystem.Publish(new EventType.MessageBroadCast() { actorIds = players, actorMessage = info }).Coroutine();
        }
        public override void OnServerStateExit()
        {
            responds.Clear();
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }

        public void TimeOutRoundStart(bool timeout)
        {
            Log.Debug("[Server] Prepare state time out");
            GetParent<MahjoneBehaviourComponent>().RoundStart(true, false, false);
        }

        public void OnEvent(long playerid,List<Tile> tiles)
        {
            if (responds.Add(playerid))
            {
                SelectTiles.AddRange(tiles);
            }
            if (responds.Count == totalPlayers)
            {
                
                int maxnum = MahjongConstants.FullTiles.Count;
                for (int i = 0; i < 5; i++)
                {
                    var ranint=RandomHelper.RandomNumber(0, maxnum);
                    for (int j = 0; j < 4; j++)
                    {
                        SelectTiles.Add(MahjongConstants.FullTiles[ranint]);
                    }
                }
                Log.Debug("[Server] Prepare state tileCount:"+ SelectTiles.Count);
                var comp = GetParent<MahjoneBehaviourComponent>();
                comp.mahjongSet = new MahjongSet(comp.GameSettings, SelectTiles);
                GetParent<MahjoneBehaviourComponent>().RoundStart(true, false, false);
            }
        }

    }
}