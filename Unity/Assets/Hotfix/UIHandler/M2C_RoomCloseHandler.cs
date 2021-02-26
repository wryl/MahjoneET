namespace ET
{
    /// <summary>
    /// 推送消息.表示房间关闭
    /// </summary>
    [MessageHandler]
    public class M2C_RoomCloseHandler : AMHandler<M2C_RoomClose>
    {
        protected override async ETTask Run(Session session, M2C_RoomClose message)
        {
            session.Domain.RemoveComponent<MJRoomPlayerComponent>();
            Game.EventSystem.Publish(new EventType.RemoveRoomCanvas() { Scene = (Scene)session.Domain }).Coroutine();
            await ETTask.CompletedTask;
        }
    }
}
