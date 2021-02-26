namespace ET
{
    //[MessageHandler]
    //public class UpdateMemberHandler : AMHandler<MJ_RoomPlayerUpdate>
    //{
    //    protected override async ETTask Run(Session session, MJ_RoomPlayerUpdate message)
    //    {
    //        await Game.EventSystem.Publish(new EventType.RoomPlayerUpdate() { PlayerChange = message });
    //        await ETTask.CompletedTask;
    //    }
    //}
    [MessageHandler]
    public class RoomPlayerChangeHandler : AMHandler<G2C_RoomPlayerChange>
    {
        protected override async ETTask Run(Session session, G2C_RoomPlayerChange message)
        {
            //说明是自己.则更新自己的组件
            if (message.player.UserId == session.Domain.GetComponent<MJRoomPlayerComponent>().PlayerInfo.UserId)
            {
                if (message.ChangeState != Mahjong.Model.PlayerChangeState.Exit)
                {
                    session.Domain.GetComponent<MJRoomPlayerComponent>().PlayerInfo = message.player;
                }
                //这个逻辑说明被踢了,就不用再更新自己的view了
                else
                {
                    Log.Debug("被踢了!!");
                    session.Domain.RemoveComponent<MJRoomPlayerComponent>();
                    Game.EventSystem.Publish(new EventType.RemoveRoomCanvas() { Scene = (Scene)session.Domain }).Coroutine();
                    return;
                }
            }
            await Game.EventSystem.Publish(new EventType.RoomPlayerChange() {
                Scene=(Scene)session.Domain, 
                PlayerInfo = message.player,ChangeState=message.ChangeState,
                IsMaster= session.Domain.GetComponent<MJRoomPlayerComponent>().PlayerInfo.IsMaster
            });
            await ETTask.CompletedTask;
        }
    }
}
