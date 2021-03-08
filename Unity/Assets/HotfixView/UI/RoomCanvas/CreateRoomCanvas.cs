

using UnityEngine;

namespace ET
{
    public class CreateRoomCanvasUI : AEvent<EventType.CreateRoomCanvas>
    {
        protected override async ETTask Run(EventType.CreateRoomCanvas args)
        {
            var ui=await UIHelper.Create(args.Scene, UIType.RoomCanvas);
            ui.GetComponent<RoomCanvasComponent>().InitRoom(args.MJ_RoomInfo);
            ui.GetComponent<RoomCanvasComponent>().MasterInit();
            var bundleGameObject = await NewResourcesComponent.Instance.LoadAssetAsync<GameObject>(ABPathUtilities.GetUIPath(UIType.SlotPanel));
            ui.GetComponent<RoomCanvasComponent>().SlotPanelList.RefreshList(UIType.SlotPanel,args.MJ_RoomInfo.PlayerInRooms, bundleGameObject);
            await UIHelper.Remove(args.Scene, UIType.CreateRoomPanel);
        }
    }
    public class JoinRoomCanvasUI : AEvent<EventType.JoinRoomCanvas>
    {
        protected override async ETTask Run(EventType.JoinRoomCanvas args)
        {
            var ui = await UIHelper.Create(args.Scene, UIType.RoomCanvas);
            ui.GetComponent<RoomCanvasComponent>().InitRoom(args.MJ_RoomInfo);
            var bundleGameObject = await NewResourcesComponent.Instance.LoadAssetAsync<GameObject>(ABPathUtilities.GetUIPath(UIType.SlotPanel));

            ui.GetComponent<RoomCanvasComponent>().SlotPanelList.RefreshList(UIType.SlotPanel, args.MJ_RoomInfo.PlayerInRooms, bundleGameObject);
            await UIHelper.Remove(args.Scene, UIType.UILobby);
        }
    }
    public class StartGameCanvasUI : AEvent<EventType.StartGame>
    {
        protected override async ETTask Run(EventType.StartGame args)
        {
            await Game.Scene.GetComponent<NewResourcesComponent>().LoadSceneAsync(ABPathUtilities.GetScenePath("PUN_Mahjong"));
            //var sceneAsset = Assets.LoadSceneAsync(assetPath, true);
            // 切换到map场景
            //using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
            //{
            //    await sceneChangeComponent.ChangeSceneAsync("PUN_Mahjong");
            //}
            Log.Debug("new StartGame");
            args.Scene.Domain.AddComponent<GamePlay.Client.Controller.ClientBehaviour>();
            await UIHelper.Remove(args.Scene, UIType.RoomCanvas);
        }
    }
    public class CloseGameCanvasUI : AEvent<EventType.GameEnd>
    {
        protected override async ETTask Run(EventType.GameEnd args)
        {
            await Game.Scene.GetComponent<NewResourcesComponent>().LoadSceneAsync("Assets/Scenes/Empty.unity");
            Game.Scene.GetComponent<NewResourcesComponent>().UnLoadScene("Assets/Scenes/PUN_Mahjong.unity");
            // 切换到map场景
            //ET.Log.Debug(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
            //if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name=="Init")
            //{
            //    return;
            //}
            //UnityEngine.SceneManagement.SceneManager.LoadScene("Empty");
            //using (SceneChangeComponent sceneChangeComponent = Game.Scene.AddComponent<SceneChangeComponent>())
            //{
            //    await sceneChangeComponent.ChangeSceneAsync("Init");
            //}
            Game.Scene.Get(1).RemoveComponent<GamePlay.Client.Controller.ClientBehaviour>();
            Game.Scene.Get(1).RemoveComponent<MJRoomPlayerComponent>();
            await UIHelper.Create(Game.Scene.Get(1), UIType.UILobby);
        }
    }
}
