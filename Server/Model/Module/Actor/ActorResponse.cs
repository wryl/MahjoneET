using ProtoBuf;
namespace ET
{
	/// <summary>
	/// actor RPC消息响应
	/// </summary>
	[Message(Opcode.ActorResponse)]
	[ProtoContract]
	public class ActorResponse : IActorLocationResponse
	{
		[ProtoMember(1)]
		public int RpcId { get; set; }
		[ProtoMember(2)]
		public int Error { get; set; }
		[ProtoMember(3)]
		public string Message { get; set; }
	}
}
