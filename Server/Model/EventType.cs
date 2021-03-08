using System.Collections.Generic;

namespace ET
{
	namespace EventType
	{
		public struct AppStart
		{
		}
		public struct GameEnd
		{
			public List<long> actorIds;
			public M2C_GameEndInfo msg;
		}
		public struct MessageBroadCast
		{
			public List<long> actorIds;
			public IActorMessage actorMessage;
		}
		public struct ActorMessage
		{
			public long actorId;
			public IActorMessage actorMessage;
		}
	}
}