using System;
using Mahjong.Model;
using ProtoBuf;
using UnityEngine;

namespace GamePlay.Server.Model
{
	public enum InTurnOperationType
	{
		Discard,
		Richi,
		Tsumo,
		Bei,
		Kong,
		RoundDraw
	}
	[ProtoContract]
	[Serializable]
	public struct InTurnOperation
	{
		[ProtoMember(1)]
		public InTurnOperationType Type;
		[ProtoMember(2)]
		public Tile Tile;
		[ProtoMember(3)]
		public OpenMeld Meld;
		[ProtoMember(4)]
		public Tile[] RichiAvailableTiles;

		public override string ToString()
		{
			switch (Type)
			{
				case InTurnOperationType.Discard:
				case InTurnOperationType.Tsumo:
					return $"Type: {Type}, Tile: {Tile}";
				case InTurnOperationType.Richi:
					return $"Type: {Type}, Tile: {Tile}, RichiAvailableTiles: {string.Join("", RichiAvailableTiles)}";
				case InTurnOperationType.RoundDraw:
				case InTurnOperationType.Bei:
					return $"Type: {Type}";
				case InTurnOperationType.Kong:
					return $"Type: {Type}, Meld: {Meld}";
				default:
					ET.Log.Warning($"Unknown type: {Type}");
					throw new Exception("This will never happen");
			}
		}
	}

	public enum OutTurnOperationType
	{
		Skip,
		Chow,
		Pong,
		Kong,
		Rong,
		RoundDraw,
	}
	[ProtoContract]
	[Serializable]
	public struct OutTurnOperation
	{
		[ProtoMember(1)]
		public OutTurnOperationType Type;
		[ProtoMember(2)]
		public Tile Tile;
		[ProtoMember(3)]
		public OpenMeld Meld;
		[ProtoMember(4)]
		public Tile[] ForbiddenTiles;
		[ProtoMember(5)]
		public PlayerHandData HandData;
		[ProtoMember(6)]
		public RoundDrawType RoundDrawType;

		public override string ToString()
		{
			switch (Type)
			{
				case OutTurnOperationType.Skip:
					return $"Type: {Type}";
				case OutTurnOperationType.RoundDraw:
					return $"Type: {Type}, RoundDrawType: {RoundDrawType}";
				case OutTurnOperationType.Chow:
				case OutTurnOperationType.Pong:
				case OutTurnOperationType.Kong:
					return
						$"Type: {Type}, Tile: {Tile}, Meld: {Meld}, ForbiddenTiles: { (ForbiddenTiles == null ? "" : string.Join(",", ForbiddenTiles))}";
				case OutTurnOperationType.Rong:
					return $"Type: {Type}, Tile: {Tile}";
				default:
					ET.Log.Warning($"Unknown type: {Type}");
					throw new Exception("This will never happen");
			}
		}
	}
}