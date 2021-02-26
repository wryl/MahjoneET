namespace ET
{
    public class LoadingFinishEvent_RemoveLoadingUI : AEvent<EventType.LoadingFinish>
    {
        public override async ETTask Run(EventType.LoadingFinish args)
        {
            await UIHelper.Remove(args.Scene, UIType.UILoading);
        }
    }
}
