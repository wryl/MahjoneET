

namespace ET
{
    public static class SceneFactory
    {
        public static async ETTask<Scene> Create(Entity parent, string name, SceneType sceneType)
        {
            long id = IdGenerater.GenerateId();
            return await Create(parent, id, parent.DomainZone(), name, sceneType);
        }
        
        public static async ETTask<Scene> Create(Entity parent, long id, int zone, string name, SceneType sceneType, StartSceneConfig startSceneConfig = null)
        {
            Scene scene = EntitySceneFactory.CreateScene(id, zone, sceneType, name);
            scene.Parent = parent;

            scene.AddComponent<MailBoxComponent, MailboxType>(MailboxType.UnOrderMessageDispatcher);

            switch (scene.SceneType)
            {
                case SceneType.Realm:
                    scene.AddComponent<NetOuterComponent, string>($"0.0.0.0:{startSceneConfig.OuterPort}");
                    break;
                case SceneType.Gate:
                    scene.AddComponent<NetOuterComponent, string>($"0.0.0.0:{startSceneConfig.OuterPort}");
                    scene.AddComponent<PlayerComponent>();
                    scene.AddComponent<GateSessionKeyComponent>();

                    break;
                case SceneType.Map:
                    scene.AddComponent<UnitComponent>();
                    break;
                case SceneType.Location:
                    scene.AddComponent<LocationComponent>();
                    break;
                case SceneType.MahjongCenter:
                    scene.AddComponent<MJRoomManagerComponent>();
                    break;
            }

            return scene;
        }
    }
}