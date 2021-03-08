using GamePlay.Server.Model;
using Mahjong.Logic;
using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    public class InitHandAfterRongStateDestroySystem : DestroySystem<InitHandAfterRongState>
    {
        public override void Destroy(InitHandAfterRongState self)
        {
            self.Destroy();
        }
    }
    /// <summary>
    /// 和牌后的状态
    /// </summary>
    public class InitHandAfterRongState : ServerState
    {
        private long timerId;
        public MahjongSet MahjongSet => GetParent<MahjoneBehaviourComponent>().mahjongSet;
        private HashSet<long> responds;
        private int playerIndex;
        public void Init(int playerIndex)
        {
            this.playerIndex = playerIndex;
        }

        public void Destroy()
        {
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
            responds = null;
        }
        public override void OnServerStateEnter()
        {
            responds = new HashSet<long>();
            var needDrawCount=13-CurrentRoundStatus.ClearOnePlayer(playerIndex, CurrentRoundStatus.ChangeHandAfterRong[playerIndex]);
            //和牌后不足以重新摸.则需要洗牌
            if (MahjongSet.TilesRemain - CurrentRoundStatus.GameSettings.MountainReservedTiles < needDrawCount)
            {
                Log.Debug("重新洗牌了:"+ MahjongSet.TilesRemain.ToString());
                List<Tile> newTiles = CurrentRoundStatus.GetTileFromRiver();
                if (MahjongSet.TilesRemain > 0) newTiles.AddRange(MahjongSet.PeekTiles(MahjongSet.TilesRemain));
                MahjongSet.ResetByTiles(newTiles);
            }
            for (int i = 0; i < needDrawCount; i++)
            {
                CurrentRoundStatus.AddTile(playerIndex, MahjongSet.DrawTile());
            }
            CurrentRoundStatus.SortHandTiles(playerIndex);
            //responds = new HashSet<long>();
            timerId = TimerComponent.Instance.NewOnceTimer(TimeHelper.ServerNow() + ServerConstants.ServerTimeOut, TimeOutFunc);
            var tiles = CurrentRoundStatus.HandTiles(playerIndex);
            Log.Debug($"[Server] Hand tiles of player {playerIndex}: {string.Join("", tiles)}");
            var info = new M2C_InitHandAfterRong
            {
                PlayerIndex = playerIndex,
                InitialHandTiles = tiles,
                MahjongSetData = MahjongSet.Data,
                RichiStatus= CurrentRoundStatus.RichiStatusArray.ToList()
            };
            Game.EventSystem.Publish(new EventType.MessageBroadCast() { actorIds = players, actorMessage = info }).Coroutine();
        }
        public void OnEvent(long actorid)
        {
            responds.Add(actorid);
            if (responds.Count == totalPlayers)
            {
                Log.Debug("[Server] RoundStart MaxPlayer");
                ServerNextState();
            }
        }
        public void TimeOutFunc()
        {
            Log.Debug("[Server] InitHandAfterRongState Timeout");
            ServerNextState();
        }

        private void ServerNextState()
        {
            GetParent<MahjoneBehaviourComponent>().PreDrawTile(playerIndex);
        }

        public override void OnServerStateExit()
        {
            responds.Clear();
            TimerComponent.Instance.Remove(timerId);
            timerId = 0;
        }
    }
}