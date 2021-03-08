using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mahjong.Model
{
	[ProtoContract]
	[Serializable]
	public struct WaitingData
	{
		[ProtoMember(1)]
		public Tile[] HandTiles;
		[ProtoMember(2)]
		public Tile[] WaitingTiles;

		public override string ToString()
		{
			return $"HandTiles: {string.Join("", HandTiles)}, "
			       + $"WaitingTiles: {string.Join("", WaitingTiles)}";
		}
	}
	[ProtoContract]
	[Serializable]
	public struct PlayerHandData
	{
		[ProtoMember(1)]
		public int HandTilesCount;
		[ProtoMember(2)]
		public List<Tile> HandTiles;
		[ProtoMember(3)]
		public OpenMeld[] OpenMelds;

		public override string ToString()
		{
			var hands = HandTiles == null ? "Confidential" : string.Join("", HandTiles);
			return $"HandTiles: {hands}, "
			       + $"OpenMelds: {string.Join(",", OpenMelds)}";
		}

		public Meld[] Melds => OpenMelds.Select(openMeld => openMeld.Meld).ToArray();
	}
}