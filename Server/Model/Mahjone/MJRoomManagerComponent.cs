//此文件格式由工具自动生成

using System.Collections.Generic;

namespace ET
{
    #region System

    public class MJRoomManagerComponentAwakeSystem : AwakeSystem<MJRoomManagerComponent>
    {
        public override void Awake(MJRoomManagerComponent self)
        {
            self.Awake();
        }
    }

    public class MJRoomManagerComponentDestroySystem : DestroySystem<MJRoomManagerComponent>
    {
        public override void Destroy(MJRoomManagerComponent self)
        {
            self.Destroy();
        }
    }



    #endregion


    /// <summary>
    /// username :d
    /// </summary>
    public class MJRoomManagerComponent : Entity
    {
        #region 私有成员

        private readonly Dictionary<long, MJRoomPlayerComponent> idPlayers = new Dictionary<long, MJRoomPlayerComponent>();


        #endregion
        //静态成员
        #region 公有成员
        public Dictionary<long, MJRoomComponent> AllRoom = new Dictionary<long, MJRoomComponent>();

        #endregion

        #region 生命周期函数

        public void Awake()
        {
            //此处填写Awake逻辑
        }
        public void AddPlayer(MJRoomPlayerComponent mJRoomPlayer)
        {
            idPlayers.Add(mJRoomPlayer.InstanceId, mJRoomPlayer);
        }
        public MJRoomComponent AddRoom(Mahjong.Model.GameSetting setting,string roomname)
        {
            var room=EntityFactory.CreateWithParentAndId<MJRoomComponent, Mahjong.Model.GameSetting>(this,IdGenerater.Instance.GenerateId(), setting);
            room.RoomName = roomname;
            AllRoom.Add(room.InstanceId, room);
            return room;
        }
        public void RemoveRoom(long insid)
        {
            if (AllRoom.TryGetValue(insid, out var room))
            {
                AllRoom.Remove(insid);
                room.Dispose();
            }
        }

        public void Destroy()
        {
            //此处填写Destroy逻辑
        }

        #endregion

    }
}