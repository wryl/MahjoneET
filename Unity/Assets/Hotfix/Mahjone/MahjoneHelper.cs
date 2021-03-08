using System;


namespace ET
{
    public static class MahjoneHelper
    {
        public static async ETVoid CreateRoomAsync(Scene domainScene, string roomname, Mahjong.Model.GameSetting settingstr)
        {
            try
            {
                Session gateSession = domainScene.GetComponent<SessionComponent>().Session;
                Log.Debug($"准备请求创建房间:{roomname}");
                var resp = (G2C_CreateRoom)await gateSession.Call(new C2G_CreateRoom() { RoomName=roomname,Setting=settingstr });
                if (resp.Error == 0)
                {
                    domainScene.AddComponent<MJRoomPlayerComponent, PlayerInRoom>(resp.myplayer);
                    Game.EventSystem.Publish(new EventType.CreateRoomCanvas() { MJ_RoomInfo = resp.RoomInfo, Scene = domainScene, myplayer = resp.myplayer }).Coroutine();
                }
            }
            catch (Exception e)
            {
                Log.Error(e);
            }
        }
        /// <summary>
        /// 加入
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETVoid JoinRoomAsync(Scene domainScene, long roomid)
        {
            Session gateSession = domainScene.GetComponent<SessionComponent>().Session;
            var resp = (G2C_JoinRoom)await gateSession.Call(new C2G_JoinRoom() { RoomId = roomid });
            if (resp.Error == 0)
            {
                domainScene.AddComponent<MJRoomPlayerComponent, PlayerInRoom>(resp.myplayer);
                Game.EventSystem.Publish(new EventType.JoinRoomCanvas() { MJ_RoomInfo = resp.RoomInfo, Scene = domainScene, myplayer = resp.myplayer }).Coroutine();
            }
        }

        public static async ETVoid StartGameAsync(Scene domainScene)
        {
            Session gateSession = domainScene.GetComponent<SessionComponent>().Session;
            var response = await gateSession.Call(new MJ_StartGameRequest());
            //如果没有报错则等待服务器推送加载游戏
            if (response.Error==0)
            {
                //Game.EventSystem.Publish(new EventType.StartGame() { Scene = domainScene }).Coroutine();
            }
        }
        public static async ETVoid EnterMahjoneGameAsync(Scene domainScene)
        {
            Session gateSession = domainScene.GetComponent<SessionComponent>().Session;
            await Game.EventSystem.Publish(new EventType.StartGame() { Scene = domainScene });
            gateSession.Send(new MJ_LoadComplete());
        }
        /// <summary>
        /// 退出房间
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static async ETVoid LeaveRoomAsync(Scene domainScene)
        {
            Session gateSession = domainScene.GetComponent<SessionComponent>().Session;
            var resp = (MJ_QuitRoomResponse)await gateSession.Call(new MJ_QuitRoomRequest() { });
            if (resp.Error == 0)
            {
                domainScene.RemoveComponent<MJRoomPlayerComponent>();
                Game.EventSystem.Publish(new EventType.RemoveRoomCanvas() { Scene = domainScene }).Coroutine();
            }
        }

        public static void OnDiscardTile(Scene domainScene, Event_DiscardTileInfo info)
        {
            Session gateSession = domainScene.GetComponent<SessionComponent>().Session;
            gateSession.Send(info);
        }

        public static void OnPreDrawTile(Scene domainScene, Event_SelectTileInfo info)
        {
            Session gateSession = domainScene.GetComponent<SessionComponent>().Session;
            gateSession.Send(info);
        }
        public static void OnChangeTileButton( bool isChange)
        {
            Session gateSession = Game.Scene.Get(1).GetComponent<SessionComponent>().Session;
            gateSession.Send(new ClientChangeTileConf() { IsChangeTileAfterRong=isChange});
        }
    }
}