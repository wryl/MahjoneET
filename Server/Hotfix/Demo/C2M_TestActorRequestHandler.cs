using System;


namespace ET
{
	[ActorMessageHandler]
	public class C2M_TestActorRequestHandler : AMActorLocationRpcHandler<Unit, C2M_TestActorRequest, M2C_TestActorResponse>
	{
		protected override async ETTask Run(Unit unit, C2M_TestActorRequest message, M2C_TestActorResponse response, Action reply)
		{
			response.Info = "actor rpc response";
			reply();
			await ETTask.CompletedTask;
		}
	}
 //   [MessageHandler]
 //   public class Xxhandler : AMHandler<TestInfo>
 //   {
 //       protected override async ETTask Run(Session session, TestInfo message)
 //       {
	//		Log.Debug(JsonHelper.ToJson(message));
	//		await ETTask.CompletedTask;
	//	}
	//}
}