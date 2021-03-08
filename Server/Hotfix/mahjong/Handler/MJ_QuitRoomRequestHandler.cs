using System;


namespace ET
{
    [ActorMessageHandler]
    public class MJ_QuitRoomRequestHandler : AMActorLocationRpcHandler<MJRoomPlayerComponent, MJ_QuitRoomRequest, MJ_QuitRoomResponse>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, MJ_QuitRoomRequest request, MJ_QuitRoomResponse response, Action reply)
        {
            //todo: 做一些检测.比如游戏中是否可退
            MahjongHelper.BroadcastWithOutPlayer(entity.GetParent<MJRoomComponent>(), entity, new G2C_RoomPlayerChange() { player = entity.PlayerInfo, ChangeState = Mahjong.Model.PlayerChangeState.Exit });
            //是房主的话关闭房间
            entity.PlayerQuit();
            reply();
            await ETTask.CompletedTask;
        }


    }
}