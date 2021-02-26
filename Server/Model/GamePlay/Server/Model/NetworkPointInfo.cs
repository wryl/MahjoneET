using System;
using Mahjong.Logic;
using ProtoBuf;
namespace GamePlay.Server.Model
{
	[ProtoContract]
	[Serializable]
	public struct NetworkPointInfo
	{
		[ProtoMember(1)]
		public int Fu;
		[ProtoMember(2)]
		public YakuValue[] YakuValues;
		public int Dora;
		[ProtoMember(3)]
		public int UraDora;
		[ProtoMember(4)]
		public int RedDora;
		[ProtoMember(5)]
		public int BeiDora;
		[ProtoMember(6)]
		public bool IsQTJ;

		public override string ToString()
		{
			return $"Fu: {Fu}, YakuValues: {string.Join(",", YakuValues)}, "
			       + $"Dora: {Dora}, UraDora: {UraDora}, RedDora: {RedDora}, BeiDora: {BeiDora}, IsQTJ: {IsQTJ}";
		}
	}
}