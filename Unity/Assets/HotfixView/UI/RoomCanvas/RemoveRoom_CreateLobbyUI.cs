

using GamePlay.Client.Controller;

namespace ET
{
    public class RemoveRoomCanvasUI : AEvent<EventType.RemoveRoomCanvas>
    {
        public override async ETTask Run(EventType.RemoveRoomCanvas args)
        {
            await UIHelper.Create(args.Scene, UIType.UILobby);
            await UIHelper.Remove(args.Scene, UIType.RoomCanvas);

        }
    }
}
