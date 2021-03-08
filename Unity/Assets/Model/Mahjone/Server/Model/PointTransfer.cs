using System;
using ProtoBuf;
namespace GamePlay.Server.Model
{
	[ProtoContract]
	[Serializable]
	public struct PointTransfer
	{
		[ProtoMember(1)]
		public int From;
		[ProtoMember(2)]
		public int To;
		[ProtoMember(3)]
		public int Amount;

		public override string ToString()
		{
			return $"PointTransfer from player {From} to player {To} with amount of {Amount}";
		}
	}
}