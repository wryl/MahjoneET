//此文件格式由工具自动生成

namespace ET
{

    public class MJRoomPlayerComponentAwakeSystem : AwakeSystem<MJRoomPlayerComponent, PlayerInRoom,long>
    {
        public override void Awake(MJRoomPlayerComponent self, PlayerInRoom player,long gateSessionId)
        {
            self.Awake(player);
            self.AddComponent<MailBoxComponent>();
            self.AddComponent<UnitGateComponent, long>(gateSessionId);
            self.AddLocation().Coroutine();
        }
    }

    public class MJRoomPlayerComponentDestroySystem : DestroySystem<MJRoomPlayerComponent>
    {
        public override void Destroy(MJRoomPlayerComponent self)
        {
            self.Destroy();
            self.RemoveLocation().Coroutine();
        }
    }
    public static class MJRoomPlayerComponentEx
    {
        public static void PlayerQuit(this MJRoomPlayerComponent self)
        {
            var isMaster = self.PlayerInfo.IsMaster;
            var room = self.GetParent<MJRoomComponent>();
            room.RemovePlayer(self.InstanceId);
            //如果是房主.在自己退出后关闭房间.这样保证自己不会收到多余的房间关闭消息
            if (isMaster)
            {
                MahjongHelper.Broadcast(room,new M2C_RoomClose());
                room.GetParent<MJRoomManagerComponent>().RemoveRoom(room.InstanceId);
            }
        }
    }
}