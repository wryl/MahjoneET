﻿

namespace ET
{
    public class CreateRoomPanelUI : AEvent<EventType.CreateRoomPanel>
    {
        protected override async ETTask Run(EventType.CreateRoomPanel args)
        {
            await UIHelper.Create(args.Scene, UIType.CreateRoomPanel);
            await UIHelper.Remove(args.Scene, UIType.UILobby);
        }
    }
}
