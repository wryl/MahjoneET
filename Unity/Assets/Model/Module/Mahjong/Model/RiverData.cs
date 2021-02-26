using System;
using System.Text;
using ProtoBuf;
namespace Mahjong.Model
{
	[ProtoContract]
	[Serializable]
	public struct RiverTile
	{
		[ProtoMember(1)]
		public Tile Tile;
		[ProtoMember(2)]
		public bool IsRichi;
		[ProtoMember(3)]
		public bool IsGone;

		public override string ToString()
		{
			var builder = new StringBuilder(Tile.ToString());
			if (IsRichi)
				builder.Append("R");
			if (IsGone)
				builder.Append("G");
			return builder.ToString();
		}
	}
	[ProtoContract]
	[Serializable]
	public struct RiverData
	{
		[ProtoMember(1)]
		public RiverTile[] River;

		public override string ToString()
		{
			return string.Join("", River);
		}
	}
}