using System;


namespace ET
{
    [MessageHandler]
    public class C2G_JoinRoomHandler : AMRpcHandler<C2G_JoinRoom, G2C_JoinRoom>
    {
        protected override async ETTask Run(Session session, C2G_JoinRoom request, G2C_JoinRoom response, Action reply)
        {
            PlayerInRoom player = new PlayerInRoom
            {
                IsMaster = false,
                IsReady = false,
                UserId = session.GetComponent<SessionPlayerComponent>().Player.Id,
                NickName = session.GetComponent<SessionPlayerComponent>().Player.Account,
                GateSessionId=session.InstanceId
            };
            M2G_JoinRoom JoinRoom = (M2G_JoinRoom)await ActorMessageSenderComponent.Instance.Call(
                request.RoomId, new G2M_JoinRoom() { Player = player, GateSessionId = session.InstanceId });
            response.RoomInfo = JoinRoom.RoomInfo;
            response.myplayer = player;
            session.GetComponent<SessionPlayerComponent>().Player.UnitId = JoinRoom.UnitId;
            reply();
            await ETTask.CompletedTask;
        }
    }
    [ActorMessageHandler]
    public class G2M_JoinRoomHandler : AMActorRpcHandler<MJRoomComponent, G2M_JoinRoom, M2G_JoinRoom>
    {
        protected override async ETTask Run(MJRoomComponent room, G2M_JoinRoom request, M2G_JoinRoom response, Action reply)
        {
            var player = room.AddPlayer(request.Player, request.GateSessionId);
            response.UnitId = player.Id;
            response.RoomInfo = new MJ_RoomInfo();
            response.RoomInfo.RoomId = room.InstanceId;
            response.RoomInfo.Setting = room.GameSettings;
            response.RoomInfo.PlayerInRooms = room.AllPlayer;
            reply();
            MahjongHelper.BroadcastWithOutPlayer(room, player, new G2C_RoomPlayerChange() { player = player.PlayerInfo, ChangeState = Mahjong.Model.PlayerChangeState.Add });
            await ETTask.CompletedTask;
        }
    }
}