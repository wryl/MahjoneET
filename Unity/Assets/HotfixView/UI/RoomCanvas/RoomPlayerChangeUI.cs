

using UnityEngine;

namespace ET
{
    //public class RoomPlayerUpdateUI : AEvent<EventType.RoomPlayerUpdate>
    //{
    //    public override async ETTask Run(EventType.RoomPlayerUpdate args)
    //    {
    //        var x=Game.Scene.GetComponent<UIComponent>().Get(UIType.RoomCanvas);
    //        x.GetComponent<RoomCanvasComponent>().SlotPanelList.RefreshList(UIType.SlotPanel,args.PlayerChange.players);
    //    }
    //}
    public class RoomPlayerChangeUI : AEvent<EventType.RoomPlayerChange>
    {
        protected override async ETTask Run(EventType.RoomPlayerChange args)
        {
            var RoomUI = args.Scene.GetComponent<UIComponent>().Get(UIType.RoomCanvas);
            switch (args.ChangeState)
            {
                case Mahjong.Model.PlayerChangeState.Add:
                    var bundleGameObject =await NewResourcesComponent.Instance.LoadAssetAsync<GameObject>(ABPathUtilities.GetUIPath(UIType.SlotPanel));
                    RoomUI.GetComponent<RoomCanvasComponent>().SlotPanelList.AddItem(UIType.SlotPanel, args.PlayerInfo, bundleGameObject);
                    if (args.IsMaster)
                    {
                        RoomUI.GetComponent<RoomCanvasComponent>().SlotPanelList.GetEntityById(args.PlayerInfo.InsId).SetAdminControll();
                    }
                    break;
                case Mahjong.Model.PlayerChangeState.Modify:
                    RoomUI.GetComponent<RoomCanvasComponent>().SlotPanelList.GetEntityById(args.PlayerInfo.InsId).SetPlayerInfo(args.PlayerInfo);
                    break;
                case Mahjong.Model.PlayerChangeState.Exit:
                    RoomUI.GetComponent<RoomCanvasComponent>().SlotPanelList.RemoveItem(args.PlayerInfo.InsId);
                    break;
            }
          
            RoomUI.GetComponent<RoomCanvasComponent>().CheckStartButton();
        }
    }
}
