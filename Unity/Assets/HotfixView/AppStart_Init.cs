namespace ET
{
    public class AppStart_Init: AEvent<EventType.AppStart>
    {
        public override async ETTask Run(EventType.AppStart args)
        {
            MongoHelper.Init();
            Game.Scene.AddComponent<TimerComponent>();


            // 下载ab包
            //await BundleHelper.DownloadBundle("http://192.168.0.167:8080/PC/");

            // 加载配置
            var downloader=Game.Scene.AddComponent<NewBundleDownloaderComponent>();
            await downloader.StartUpdate();
            Game.Scene.RemoveComponent<NewBundleDownloaderComponent>();
            Game.Scene.AddComponent<NewResourcesComponent>();
            
            //ResourcesComponent.Instance.LoadBundle("config.unity3d");
            //Game.Scene.AddComponent<ConfigComponent>();
            //ResourcesComponent.Instance.UnloadBundle("config.unity3d");
            
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            Game.Scene.AddComponent<UIEventComponent>();

            //ResourcesComponent.Instance.LoadBundle("unit.unity3d");

            Scene zoneScene = await SceneFactory.CreateZoneScene(1, 1, "Game");

            await Game.EventSystem.Publish(new EventType.AppStartInitFinish() { ZoneScene = zoneScene });
        }
    }
}