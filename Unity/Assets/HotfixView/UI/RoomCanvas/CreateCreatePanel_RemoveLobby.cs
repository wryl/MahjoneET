

namespace ET
{
    public class LeaveCreateRoomPanelUI : AEvent<EventType.LeaveCreateRoomPanel>
    {
        public override async ETTask Run(EventType.LeaveCreateRoomPanel args)
        {
            await UIHelper.Create(args.Scene, UIType.UILobby);
            await UIHelper.Remove(args.Scene, UIType.CreateRoomPanel);
        }
    }
}
