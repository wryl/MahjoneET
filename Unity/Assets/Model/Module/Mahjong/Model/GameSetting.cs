using System;
using Mahjong.Logic;
using ProtoBuf;
namespace Mahjong.Model
{
	[ProtoContract]
	[Serializable]
	public class GameSetting
	{
		// Basic settings
		[ProtoMember(1)]
		public GameMode GameMode;
		[ProtoMember(2)]
		public GamePlayers GamePlayers;
		[ProtoMember(3)]
		public RoundCount RoundCount;
		[ProtoMember(4)]
		public MinimumFanConstraintType MinimumFanConstraintType;
		[ProtoMember(5)]
		public PointsToGameEnd PointsToGameEnd;
		[ProtoMember(6)]
		public bool GameEndsWhenAllLastTop;
		[ProtoMember(7)]
		public int InitialPoints;
		[ProtoMember(8)]
		public int FirstPlacePoints;

		[ProtoMember(9)]
		public bool AllowHint;

		// Advance settings
		[ProtoMember(10)]
		public InitialDoraCount InitialDoraCount;
		[ProtoMember(11)]
		public bool AllowRichiWhenPointsLow;
		[ProtoMember(12)]
		public bool AllowRichiWhenNotReady;
		[ProtoMember(13)]
		public bool AllowDiscardSameAfterOpen;
		[ProtoMember(14)]
		public int RichiMortgagePoints;
		[ProtoMember(15)]
		public int ExtraRoundBonusPerPlayer;
		[ProtoMember(16)]
		public int NotReadyPunishPerPlayer;
		[ProtoMember(17)]
		public int FalseRichiPunishPerPlayer;
		[ProtoMember(18)]
		public bool AllowMultipleRong;
		[ProtoMember(19)]
		public bool Allow3RongDraw;
		[ProtoMember(20)]
		public bool Allow4RichiDraw;
		[ProtoMember(21)]
		public bool Allow4KongDraw;
		[ProtoMember(22)]
		public bool Allow4WindDraw;

		[ProtoMember(23)]
		public bool Allow9OrphanDraw;

		// Time settings
		[ProtoMember(24)]
		public int BaseTurnTime = 5;

		[ProtoMember(25)]
		public int BonusTurnTime = 20;

		// hidden entries in setting panel
		[ProtoMember(26)]
		public bool AllowChows;
		[ProtoMember(27)]
		public bool AllowPongs;
		[ProtoMember(28)]
		public bool AllowBeiDora;
		[ProtoMember(29)]
		public bool AllowBeiAsYaku;
		[ProtoMember(30)]
		public bool AllowBeiDoraRongAsRobbKong;
		[ProtoMember(31)]
		public bool AllowBeiDoraTsumoAsLingShang;
		[ProtoMember(32)]
		public int DiceMin = 2;
		[ProtoMember(33)]
		public int DiceMax = 12;
		[ProtoMember(34)]
		public int MountainReservedTiles = 14;

		public int InitialDora
		{
			get
			{
				switch (InitialDoraCount)
				{
					case InitialDoraCount.One:
						return 1;
					case InitialDoraCount.Two:
						return 2;
					case InitialDoraCount.Three:
						return 3;
					case InitialDoraCount.Four:
						return 4;
					case InitialDoraCount.Five:
						return 5;
					default:
						ET.Log.Error($"Unknown InitialDoraCount {InitialDoraCount}");
						return 1;
				}
			}
		}

		[ProtoMember(35)]
		public int MaxDora = 5;
		public int LingshangTilesCount => AllowBeiDora ? 8 : 4;

		// Yaku settings
		[ProtoMember(36)]
		public bool OpenDuanYao = true;
		[ProtoMember(37)]
		public bool HasOneShot = true;
		[ProtoMember(38)]
		public bool 连风对子额外加符 = true; // this field do not have setting entry
		[ProtoMember(39)]
		public bool AllowGswsRobConcealedKong = true;
		[ProtoMember(40)]
		public YakumanLevel SiAnKe = YakumanLevel.Two;
		[ProtoMember(41)]
		public YakumanLevel GuoShi = YakumanLevel.Two;
		[ProtoMember(42)]
		public YakumanLevel JiuLian = YakumanLevel.Two;
		[ProtoMember(43)]
		public YakumanLevel LvYiSe = YakumanLevel.One;
		public int 四暗刻单骑 => YakumanLevelToInt(SiAnKe);
		public int 国士无双十三面 => YakumanLevelToInt(GuoShi);
		public int 纯正九连宝灯 => YakumanLevelToInt(JiuLian);
		public int 纯绿一色 => YakumanLevelToInt(LvYiSe);

		private static int YakumanLevelToInt(YakumanLevel level)
		{
			switch (level)
			{
				case YakumanLevel.One:
					return 1;
				case YakumanLevel.Two:
					return 2;
				default:
					throw new System.ArgumentException($"Unknown level {level}");
			}
		}
		[ProtoMember(44)]
		public Tile[] redTiles = new Tile[]
		{
			new Tile(Suit.M, 5, true),
			new Tile(Suit.P, 5, true),
			new Tile(Suit.S, 5, true)
		};

		public Tile[] GetAllTiles()
		{
			switch (GamePlayers)
			{
				case GamePlayers.Two:
					return MahjongConstants.TwoPlayerTiles.ToArray();
				case GamePlayers.Three:
					return MahjongConstants.ThreePlayerTiles.ToArray();
				case GamePlayers.Four:
					return MahjongConstants.FullTiles.ToArray();
				default:
					ET.Log.Error($"This should not happen, GamePlayers: {GamePlayers}");
					return null;
			}
		}

		public int MaxPlayer => GetPlayerCount(GamePlayers);

		public static int GetPlayerCount(GamePlayers playerSetting)
		{
			switch (playerSetting)
			{
				case GamePlayers.Two:
					return 2;
				case GamePlayers.Three:
					return 3;
				case GamePlayers.Four:
					return 4;
				default:
					ET.Log.Error($"Unknown GamePlayers option: {playerSetting}");
					return 4;
			}
		}

		public bool CheckConstraint(PointInfo point)
		{
			int baseFan = point.FanWithoutDora;
			int fan = point.TotalFan;
			int basePoint = point.BasePoint;
			switch (MinimumFanConstraintType)
			{
				case MinimumFanConstraintType.One:
					return baseFan >= 1;
				case MinimumFanConstraintType.Two:
					return baseFan >= 1 && fan >= 2;
				case MinimumFanConstraintType.Three:
					return baseFan >= 1 && fan >= 3;
				case MinimumFanConstraintType.Four:
					return baseFan >= 1 && fan >= 4;
				case MinimumFanConstraintType.Mangan:
					return baseFan >= 1 && basePoint >= MahjongConstants.Mangan;
				case MinimumFanConstraintType.Haneman:
					return baseFan >= 1 && basePoint >= MahjongConstants.Haneman;
				case MinimumFanConstraintType.Baiman:
					return baseFan >= 1 && basePoint >= MahjongConstants.Baiman;
				case MinimumFanConstraintType.Yakuman:
					return baseFan >= 1 && basePoint >= MahjongConstants.Yakuman;
				default:
					ET.Log.Error($"Unknown type {MinimumFanConstraintType}");
					return false;
			}
		}

		public int GetMultiplier(bool isDealer, int totalPlayers)
		{
			return isDealer ? 6 : 4; // this is for 4-player mahjong -- todo
		}

		public bool IsAllLast(int oyaIndex, int field, int totalPlayers)
		{
			return (oyaIndex == totalPlayers - 1 && field == FieldThreshold - 1) || field >= FieldThreshold;
		}

		public bool GameForceEnd(int oyaIndex, int field, int totalPlayers)
		{
			return oyaIndex == totalPlayers - 1 && field == MaxField - 1;
		}

		private int FieldThreshold
		{
			get
			{
				switch (RoundCount)
				{
					case RoundCount.E:
						return 1;
					case RoundCount.ES:
						return 2;
					case RoundCount.FULL:
						return 4;
					default:
						ET.Log.Error($"Unknown type {RoundCount}");
						return 2;
				}
			}
		}

		private int MaxField
		{
			get
			{
				switch (RoundCount)
				{
					case RoundCount.E:
						return 2;
					case RoundCount.ES:
						return 3;
					case RoundCount.FULL:
						return 4;
					default:
						ET.Log.Error($"Unknown type {RoundCount}");
						return 2;
				}
			}
		}


	}

	public enum GameMode
	{
		Normal,
		QTJ
	}

	public enum GamePlayers
	{
		Two,
		Three,
		Four
	}

	public enum RoundCount
	{
		E,
		ES,
		FULL
	}

	public enum MinimumFanConstraintType
	{
		One,
		Two,
		Three,
		Four,
		Mangan,
		Haneman,
		Baiman,
		Yakuman
	}

	public enum PointsToGameEnd
	{
		Negative,
		Zero,
		Never
	}

	public enum InitialDoraCount
	{
		One,
		Two,
		Three,
		Four,
		Five
	}

	public enum YakumanLevel
	{
		One,
		Two
	}
	public enum PlayerChangeState
	{ 
		Add,
		Modify,
		Exit
	}
}