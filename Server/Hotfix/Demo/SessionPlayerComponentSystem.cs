﻿

namespace ET
{
	public class SessionPlayerComponentDestroySystem : DestroySystem<SessionPlayerComponent>
	{
		public override void Destroy(SessionPlayerComponent self)
		{
            // 发送断线消息
            if (self.Player.UnitId!=0)
            {
				ActorLocationSenderComponent.Instance.Send(self.Player.UnitId, new G2M_SessionDisconnect());
			}
			Game.Scene.GetComponent<PlayerComponent>()?.Remove(self.Player.Id);
		}
	}
}