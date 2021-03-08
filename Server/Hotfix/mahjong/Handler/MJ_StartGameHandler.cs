using System;


namespace ET
{
    [ActorMessageHandler]
    public class MJ_StartGameRequestHandler : AMActorLocationRpcHandler<MJRoomPlayerComponent, MJ_StartGameRequest, MJ_StartGameResponse>
    {

        protected override async ETTask Run(MJRoomPlayerComponent entity, MJ_StartGameRequest message, MJ_StartGameResponse response, Action reply)
        {
            if (entity.PlayerInfo.IsMaster)
            {
                entity.GetParent<MJRoomComponent>().StartGame();
            }
            else
            {
                response.Error = ErrorCode.ERR_NOTMASTER;
            }
            reply();
            await ETTask.CompletedTask;
        }
    }
}