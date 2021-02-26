using ProtoBuf;
using System;

namespace Mahjong.Model
{
	[ProtoContract]
	[Serializable]
	public struct OpenMeld
	{
		[ProtoMember(1)]
		public Meld Meld;
		[ProtoMember(2)]
		public Tile Tile;
		[ProtoMember(3)]
		public MeldSide Side;
		[ProtoMember(4)]
		public Tile Extra;
		[ProtoMember(5)]
		public bool IsAdded;

		public MeldType Type => Meld.Type;
		public Tile First => Meld.First;
		public bool IsKong => Meld.IsKong;
		public bool Revealed => Meld.Revealed;
		public Tile[] Tiles => Meld.Tiles;

		public Tile[] GetForbiddenTiles(Tile tile)
		{
			return Meld.GetForbiddenTiles(tile);
		}

		public OpenMeld AddToKong(Tile extra)
		{
			return new OpenMeld
			{
				Meld = Meld.AddToKong(extra),
				Tile = Tile,
				Side = Side,
				Extra = extra,
				IsAdded = true
			};
		}

		public override string ToString()
		{
			return $"{Meld}/{Tile}/{Side}";
		}
	}

	public enum MeldSide
	{
		Left,
		Opposite,
		Right,
		Self
	}
}