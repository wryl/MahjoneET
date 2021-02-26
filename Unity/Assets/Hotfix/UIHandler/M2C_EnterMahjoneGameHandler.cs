namespace ET
{
    /// <summary>
    /// 推送消息.表示开始游戏
    /// </summary>
    [MessageHandler]
    public class M2C_EnterMahjoneGameHandler : AMHandler<M2C_EnterMahjoneGame>
    {
        protected override async ETTask Run(Session session, M2C_EnterMahjoneGame message)
        {
            MahjoneHelper.EnterMahjoneGameAsync((Scene)session.Domain).Coroutine();
        }
    }
}
