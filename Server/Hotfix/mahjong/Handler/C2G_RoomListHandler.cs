using System;


namespace ET
{
    [MessageHandler]
    public class C2G_RoomListHandler : AMRpcHandler<C2G_RoomList, G2C_RoomList>
    {
        protected override async ETTask Run(Session session, C2G_RoomList request, G2C_RoomList response, Action reply)
        {
            response.rooms = ((M2G_RoomList)await ActorMessageSenderComponent.Instance.Call(
                MahjongHelper.GetScence(session.DomainZone()), new G2M_RoomList() { })).rooms;
            reply();
            await ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler]
    public class G2M_RoomListHandler : AMActorRpcHandler<Scene, G2M_RoomList, M2G_RoomList>
    {
        protected override async ETTask Run(Scene scene, G2M_RoomList request, M2G_RoomList response, Action reply)
        {
            response.rooms = new System.Collections.Generic.List<MJ_RoomInfoInList>() { };
            foreach (MJRoomComponent room in scene.GetComponent<MJRoomManagerComponent>().AllRoom.Values)
            {
                response.rooms.Add(new MJ_RoomInfoInList()
                {
                    InsId = room.InstanceId,
                    RoomId = room.InstanceId,
                    RoomName = room.RoomName,
                    GameMode = room.GameSettings.GameMode,
                    GamePlayers = room.GameSettings.GamePlayers,
                    NowPlayerNum = room.PlayerCount
                });
            }
            reply();
            await ETTask.CompletedTask;
        }
    }
}