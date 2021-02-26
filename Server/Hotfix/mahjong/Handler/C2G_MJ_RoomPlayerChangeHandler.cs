using System;


namespace ET
{
    [ActorMessageHandler]
    public class MJ_RoomPlayerChangeHandler : AMActorLocationHandler<MJRoomPlayerComponent, MJ_RoomPlayerChangeRequest>
    {
        protected override async ETTask Run(MJRoomPlayerComponent entity, MJ_RoomPlayerChangeRequest message)
        {
            entity.PlayerInfo.Icon = message.player.Icon;
            entity.PlayerInfo.IsReady = message.player.IsReady;
            MahjongHelper.Broadcast(entity.GetParent<MJRoomComponent>(), new G2C_RoomPlayerChange() { player=entity.PlayerInfo,ChangeState=Mahjong.Model.PlayerChangeState.Modify});
        }
    }

}