//using UnityEngine;
//using UnityEngine.UI;

//namespace ET
//{
//    public class UIRoomListComponentAwakeSystem : AwakeSystem<UIRoomListComponent>
//    {
//        public override void Awake(UIRoomListComponent self)
//        {
//            ReferenceCollector rc = self.GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
			
//            self.roomlistPanle = rc.Get<GameObject>("RoomlistPanle");
//            self.enterMap.GetComponent<Button>().onClick.AddListener(self.EnterMap);
//            self.text = rc.Get<GameObject>("Text").GetComponent<Text>();
            
//        }
//    }
//    public class UIRoomListComponentStartSystem : StartSystem<UIRoomListComponent>
//    {
//        public override void Start(UIRoomListComponent self)
//        {
//            self.start().Coroutine();
//        }
//    }
//    public static class UIRoomListComponentSystem
//    {
//        public static void EnterMap(this UIRoomListComponent self)
//        {
//            MapHelper.EnterMapAsync(self.ZoneScene(), "Map").Coroutine();
//        }
//        public static async ETVoid start(this UIRoomListComponent self)
//        {
//            self.DomainScene().GetComponent<SessionComponent>().Session.Send(new TestInfo() { 
//                Setting=new MJ_Setting() { InitialDoraCount=Mahjong.Model.InitialDoraCount.Five},
//                Maps=new System.Collections.Generic.Dictionary<long, MJ_NormalSetting>() { { 5,new MJ_NormalSetting() { testid=5} } }
//            });
//            var resp=(G2C_RoomList)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(new C2G_RoomList());
//            self.text.text = resp.rooms.Count.ToString();
//        }
//    }
//}
