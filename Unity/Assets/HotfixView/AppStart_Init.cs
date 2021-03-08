namespace ET
{
    public class AppStart_Init: AEvent<EventType.AppStart>
    {
        protected override async ETTask Run(EventType.AppStart args)
        {
            Game.Scene.AddComponent<TimerComponent>();
            Game.Scene.AddComponent<CoroutineLockComponent>();

            // 加载配置
            //Game.Scene.AddComponent<ResourcesComponent>();
            //ResourcesComponent.Instance.LoadBundle("config.unity3d");
            //Game.Scene.AddComponent<ConfigComponent>();
            //ConfigComponent.GetAllConfigBytes = LoadConfigHelper.LoadAllConfigBytes;
            //await ConfigComponent.Instance.LoadAsync();
            //ResourcesComponent.Instance.UnloadBundle("config.unity3d");

            var downloader = Game.Scene.AddComponent<NewBundleDownloaderComponent>();
            await downloader.StartUpdate();
            Game.Scene.RemoveComponent<NewBundleDownloaderComponent>();
            Game.Scene.AddComponent<NewResourcesComponent>();
            Game.Scene.AddComponent<OpcodeTypeComponent>();
            Game.Scene.AddComponent<MessageDispatcherComponent>();
            
            Game.Scene.AddComponent<NetThreadComponent>();


            Scene zoneScene = await SceneFactory.CreateZoneScene(1, 1, "Game");

            await Game.EventSystem.Publish(new EventType.AppStartInitFinish() { ZoneScene = zoneScene });
        }
    }
}
