
namespace ET
{
    public class MahjongEventActorMessage : AEvent<EventType.ActorMessage>
    {
        protected override async ETTask Run(EventType.ActorMessage arg)
        {
            ActorMessageSenderComponent.Instance.Send(arg.actorId, arg.actorMessage);
            await ETTask.CompletedTask;
        }
    }
    public class MahjongEventMessageBroadCast : AEvent<EventType.MessageBroadCast>
    {
        protected override async ETTask Run(EventType.MessageBroadCast arg)
        {
            foreach (long actorid in arg.actorIds)
            {
                ActorMessageSenderComponent.Instance.Send(actorid, arg.actorMessage);
            }
            await ETTask.CompletedTask;
        }
    }
    /// <summary>
    /// 处理游戏结束后的清理
    /// </summary>
    public class MahjongEventGameEnd : AEvent<EventType.GameEnd>
    {
        protected override async ETTask Run(EventType.GameEnd arg)
        {
            foreach (long actorid in arg.actorIds)
            {
                ActorMessageSenderComponent.Instance.Send(actorid, arg.msg);
            }
            await ETTask.CompletedTask;
        }
    }

}
