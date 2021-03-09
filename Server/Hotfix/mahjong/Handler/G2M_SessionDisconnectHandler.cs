
namespace ET
{
	[ActorMessageHandler]
	public class G2M_SessionDisconnectHandler : AMActorLocationHandler<MJRoomPlayerComponent, G2M_SessionDisconnect>
	{
		protected override async ETTask Run(MJRoomPlayerComponent unit, G2M_SessionDisconnect message)
		{
			unit.PlayerQuit();
			await ETTask.CompletedTask;
		}
	}
}