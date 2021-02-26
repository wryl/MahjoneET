
namespace ET
{
    public class MJRoomPlayerComponentAwakeSystem : AwakeSystem<MJRoomPlayerComponent, PlayerInRoom>
    {
        public override void Awake(MJRoomPlayerComponent self, PlayerInRoom player)
        {
            self.Awake(player);
        }
    }

    public class MJRoomPlayerComponent : Entity
    {

        #region 公有成员
        public PlayerInRoom PlayerInfo;
        #endregion

        #region 生命周期函数

        public void Awake(PlayerInRoom player)
        {
            //此处填写Awake逻辑
            PlayerInfo = player;
        }


        public void Destroy()
        {
            PlayerInfo = null;
            Log.Debug("MJRoomPlayerComponent destroy");
        }

        #endregion

    }
}
