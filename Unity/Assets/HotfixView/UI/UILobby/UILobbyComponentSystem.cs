using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Managers;

namespace ET
{
    public class UILobbyComponentAwakeSystem : AwakeSystem<UILobbyComponent>
    {
        public override void Awake(UILobbyComponent self)
        {
            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			
            self.content = rc.Get<GameObject>("Content");
            self.refreshButton = rc.Get<GameObject>("RefreshButton").GetComponent<Button>();
            self.refreshButton.onClick.AddListener(self.Refresh);
            self.createRoomButton= rc.Get<GameObject>("CreateButton").GetComponent<Button>();
            self.createRoomButton.onClick.AddListener(self.CreateRoom);
            self.roomlist = EntityFactory.CreateWithParent<ListUIComponent<RoomEntryComponent>>(self);
            self.roomlist.Awake(self.content);
        }


    }
    public class UILobbyComponentStartSystem : StartSystem<UILobbyComponent>
    {
        public override void Start(UILobbyComponent self)
        {
            //self.Refresh();
        }
    }
    public static class UILobbyComponentSystem
    {
        public static void EnterMap(this UILobbyComponent self)
        {
            MapHelper.EnterMapAsync(self.ZoneScene(), "Map").Coroutine();
        }

        public static async ETVoid RefreshAsync(this UILobbyComponent self)
        {
            var resp = (G2C_RoomList)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(new C2G_RoomList());
            var bud=await NewResourcesComponent.Instance.LoadAssetAsync<GameObject>(ABPathUtilities.GetUIPath(UIType.RoomEntry));
            self.roomlist.RefreshList(UIType.RoomEntry,resp.rooms, bud);
        }

        public static void Refresh(this UILobbyComponent self)
        {
            self.RefreshAsync().Coroutine();
        }
        public static void CreateRoom(this UILobbyComponent self)
        {
            //ResourceManager.Instance.LoadSettings(out var gameSetting);
            //MahjoneHelper.CreateRoomAsync(self.DomainScene(), "2人战", gameSetting).Coroutine();
            Game.EventSystem.Publish(new EventType.CreateRoomPanel() { Scene = self.ZoneScene() }).Coroutine();

        }
    }
}
