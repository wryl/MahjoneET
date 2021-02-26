
namespace ET
{

    public static class MahjongHelper
    {
        /// <summary>
        /// 获取麻将进程场景id
        /// </summary>
        /// <param name="zoneid"></param>
        /// <returns></returns>
        public static long GetScence(int zoneid)
        {
            return StartSceneConfigCategory.Instance.GetBySceneName(zoneid, "Mahjong").SceneId;
        }
        public static void Broadcast<T>(MJRoomComponent room, T message) where T : IActorMessage
        {
            foreach (long actorid in room.AllPlayerActorids)
            {
                if (actorid == 0)
                {
                    continue;
                }
                ActorMessageSenderComponent.Instance.Send(actorid, message);
            }
        }

        public static void BroadcastWithOutPlayer(MJRoomComponent room, MJRoomPlayerComponent player, IActorMessage message)
        {
            foreach (long actorid in room.AllPlayerActorids)
            {
                if (actorid == 0 || actorid == player.GetComponent<UnitGateComponent>().GateSessionActorId)
                {
                    continue;
                }
                ActorMessageSenderComponent.Instance.Send(actorid, message);
            }
        }

    }
}
