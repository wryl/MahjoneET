using System;


namespace ET
{
    [ActorMessageHandler]
    public class MJ_KickPlayerHandler : AMActorLocationHandler<MJRoomPlayerComponent, MJ_KickPlayer>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, MJ_KickPlayer message)
        {
            if (entity.PlayerInfo.IsMaster)
            {
                if (message.KickId == entity.PlayerInfo.InsId)
                {
                    return;
                }
                var player=entity.GetParent<MJRoomComponent>().GetPlayer(message.KickId);
                if (player==default)
                {
                    return;
                }
                MahjongHelper.Broadcast(entity.GetParent<MJRoomComponent>(),new G2C_RoomPlayerChange() { player = player.PlayerInfo, ChangeState = Mahjong.Model.PlayerChangeState.Exit });
                player.PlayerQuit();
            }
        }
    }
}