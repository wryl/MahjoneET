//此文件格式由工具自动生成

namespace ET
{




    /// <summary>
    /// username :d
    /// </summary>
    public class MJRoomPlayerComponent : Entity
    {
        #region 私有成员



        #endregion

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
            //此处填写Destroy逻辑
            PlayerInfo = null;
        }

        #endregion

    }
}