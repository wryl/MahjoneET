using System;

namespace ET
{
    [MessageHandler]
    public class C2G_CreateRoomHandler : AMRpcHandler<C2G_CreateRoom, G2C_CreateRoom>
    {
        protected override async ETTask Run(Session session, C2G_CreateRoom request, G2C_CreateRoom response, Action reply)
        {

            PlayerInRoom player = new PlayerInRoom
            {
                IsMaster = true,
                IsReady = false,
                UserId = session.GetComponent<SessionPlayerComponent>().Player.Id,
                NickName = session.GetComponent<SessionPlayerComponent>().Player.Account,
                GateSessionId = session.InstanceId
            };
            long mjInstanceId = MahjongHelper.GetScence(session.DomainZone());
            M2G_CreateRoom createRoom = (M2G_CreateRoom)await ActorMessageSenderComponent.Instance.Call(
                mjInstanceId, new G2M_CreateRoom() { Player = player, Setting = request.Setting, GateSessionId = session.InstanceId, RoomName = request.RoomName });
            response.myplayer = player;
            response.RoomInfo = createRoom.RoomInfo;
            session.GetComponent<SessionPlayerComponent>().Player.UnitId = createRoom.UnitId;
            reply();
            await ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler]
    public class G2M_CreateRoomHandler : AMActorRpcHandler<Scene, G2M_CreateRoom, M2G_CreateRoom>
    {
        protected override async ETTask Run(Scene scene, G2M_CreateRoom request, M2G_CreateRoom response, Action reply)
        {
            //创建房间

            var room = scene.GetComponent<MJRoomManagerComponent>().AddRoom(request.Setting,request.RoomName);
            var player = room.AddPlayer(request.Player, request.GateSessionId);
            //加入房主
            response.UnitId = player.Id;
            response.RoomInfo = new MJ_RoomInfo();
            response.RoomInfo.RoomName = room.RoomName;
            response.RoomInfo.RoomId = room.InstanceId;
            response.RoomInfo.Setting =room.GameSettings;
            response.RoomInfo.PlayerInRooms = room.AllPlayer;
            reply();
            await ETTask.CompletedTask;

        }
    }
}