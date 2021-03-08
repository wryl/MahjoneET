

using ET.EventType;

namespace ET
{
    public class LeaveCreateRoomPanelUI : AEvent<EventType.LeaveCreateRoomPanel>
    {
        protected override async ETTask Run(EventType.LeaveCreateRoomPanel args)
        {
            await UIHelper.Create(args.Scene, UIType.UILobby);
            await UIHelper.Remove(args.Scene, UIType.CreateRoomPanel);
        }
    }
}
