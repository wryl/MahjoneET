//此文件格式由工具自动生成

using Mahjong.Model;
using System.Collections.Generic;
using System.Linq;

namespace ET
{

    #region System

    public class MJRoomComponentDestroySystem : DestroySystem<MJRoomComponent>
    {
        public override void Destroy(MJRoomComponent self)
        {
            self.Destroy();
        }
    }



    #endregion


    /// <summary>
    /// username :d
    /// </summary>
    public class MJRoomComponent : Entity
    {
        #region 私有成员

        private Dictionary<long, MJRoomPlayerComponent> idPlayers;

        #endregion
        #region 公有成员
        public GameSetting GameSettings;
        public string RoomName;
        #endregion

        #region 生命周期函数

        public void Awake(Mahjong.Model.GameSetting settings)
        {
            idPlayers = new Dictionary<long, MJRoomPlayerComponent>();
            GameSettings = settings;
        }
        public MJRoomPlayerComponent AddPlayer(PlayerInRoom player,long GateSessionId)
        {
            var playercomponent=EntityFactory.CreateWithParentAndId<MJRoomPlayerComponent, PlayerInRoom,long>(this,IdGenerater.GenerateId(), player, GateSessionId);
            playercomponent.PlayerInfo.InsId = playercomponent.InstanceId;
            idPlayers.Add(playercomponent.InstanceId, playercomponent);
            return playercomponent;
        }
        public void RemovePlayer(long insid)
        {
            if (idPlayers.TryGetValue(insid, out var player))
            {
                idPlayers.Remove(insid);
                player.Dispose();
            }
        }


        public MJRoomPlayerComponent GetPlayer(long insid)
        {
            idPlayers.TryGetValue(insid, out var player);
            return player;

        }
        public List<PlayerInRoom> AllPlayer => (from player in idPlayers.Values
                                                select player.PlayerInfo).ToList();
        public List<long> AllPlayerActorids => (from player in idPlayers.Values
                                                select player.GetComponent<UnitGateComponent>().GateSessionActorId).ToList();
        public int PlayerCount => idPlayers.Count;
        public void Destroy()
        {
            //此处填写Destroy逻辑
            foreach (var item in idPlayers.Keys)
            {
                idPlayers[item].Dispose();
            }
            idPlayers = null;
            GameSettings = null;
        }

        #endregion

    }
}