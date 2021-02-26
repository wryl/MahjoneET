//此文件格式由工具自动生成

namespace ET
{
    public class MJRoomComponentAwakeSystem : AwakeSystem<MJRoomComponent, Mahjong.Model.GameSetting>
    {
        public override void Awake(MJRoomComponent self, Mahjong.Model.GameSetting settings)
        {
            self.Awake(settings);
            self.AddComponent<MailBoxComponent>();
        }
    }
    public static class MJRoomComponentEx
    {
        public static void StartGame(this MJRoomComponent self)
        {
            if (self.CanStartGame())
            {
                self.AddComponent<MahjoneBehaviourComponent, Mahjong.Model.GameSetting>(self.GameSettings);
            }
        }
        public static bool CanStartGame(this MJRoomComponent self)
        {
            return (self.GameSettings.MaxPlayer == self.PlayerCount)&&self.GetComponent<MahjoneBehaviourComponent>()==null;
        }
    }
}