using System;
using System.Collections.Generic;
using Mahjong.Model.Exceptions;
using ProtoBuf;
using Utils;
namespace Mahjong.Model
{
	[Serializable]
	public class MahjongSet
	{
		private GameSetting setting;
		private List<Tile> allTiles;
		private int tilesDrawn = 0;
		private int lingShangDrawn = 0;
		private int doraTurned = 0;

		public MahjongSet(GameSetting setting, IEnumerable<Tile> tiles)
		{
			this.setting = setting;
			allTiles = new List<Tile>(tiles);
			ET.Log.Debug($"In current settings, total count of all tiles is {allTiles.Count}");
			for (int i = 0; i < setting.redTiles.Length; i++)
			{
				var red = setting.redTiles[i];
				var index = allTiles.FindIndex(t => t.EqualsIgnoreColor(red) && !t.IsRed);
				if (index < 0)
				{
					ET.Log.Warning($"Not enough tile of {red}");
					continue;
				}

				allTiles[index] = new Tile(red.Suit, red.Rank, true);
			}
		}

		/// <summary>
		/// Shuffle and reset the mahjong set to its original state, then turn dora tiles of the initial count and return an array containing the initial doras.
		/// </summary>
		/// <returns>Array of initial dora tiles</returns>
		public Tile[] Reset()
		{
			allTiles.Shuffle();
			tilesDrawn = 0;
			lingShangDrawn = 0;
			doraTurned = 0;
			var doraList = new List<Tile>();
			for (int i = 0; i < setting.InitialDora; i++)
			{
				doraList.Add(TurnDora());
			}

			return doraList.ToArray();
		}
		public Tile[] ResetByTiles(List<Tile> CanShuffleTiles)
		{
			allTiles = CanShuffleTiles;
			allTiles.Shuffle();
			tilesDrawn = 0;
			lingShangDrawn = 0;
			doraTurned = 0;
			var doraList = new List<Tile>();
			for (int i = 0; i < setting.InitialDora; i++)
			{
				doraList.Add(TurnDora());
			}

			return doraList.ToArray();
		}
		/// <summary>
		/// Draw a new tile on the wall, throw a exception when there are none
		/// </summary>
		/// <returns>The next tile</returns>
		public Tile DrawTile()
		{
			if (TilesRemain <= 0) throw new NoMoreTilesException("There are no more tiles to drawn!");
			return allTiles[tilesDrawn++];
		}

		/// <summary>
		/// Peek the next tile on the wall without drawing it, throw a exception when there are none
		/// </summary>
		/// <returns>The next tile</returns>
		public Tile PeekTile()
		{
			if (TilesRemain <= 0) throw new NoMoreTilesException("There are no more tiles to drawn!");
			return allTiles[tilesDrawn];
		}
		/// <summary>
		/// Peek tiles on the wall without drawing it, throw a exception when there are none
		/// </summary>
		/// <returns>The next tile</returns>
		public List<Tile> PeekTiles(int peeknum)
		{
			if (TilesRemain <= 0) throw new NoMoreTilesException("There are no more tiles to drawn!");
			return allTiles.GetRange(tilesDrawn, Math.Min(peeknum,TilesRemain)) ;
		}
		/// <summary>
		/// Draw a lingshang tile from the other side of the wall, throw a exception when there are none
		/// </summary>
		/// <returns>The lingshang tile</returns>
		public Tile DrawLingShang()
		{
			if (LingShangDrawn >= setting.LingshangTilesCount)
				throw new NoMoreTilesException("There are no more LingShang tiles to drawn!");
			var tile = allTiles[allTiles.Count - lingShangDrawn - 1];
			lingShangDrawn++;
			return tile;
		}

		/// <summary>
		/// Turn a new dora indicator, throw a exception when there are none
		/// </summary>
		/// <returns>The new dora indicator</returns>
		public Tile TurnDora()
		{
			if (doraTurned >= setting.MaxDora)
				throw new NoMoreTilesException("There are no more dora indicators to turn.");
			var firstDora = setting.LingshangTilesCount; // index from tail of list
			var currentDora = firstDora + DoraTurned * 2;
			doraTurned++;
			return allTiles[allTiles.Count - 1 - currentDora];
		}

		/// <summary>
		/// Returns current turned dora indicators
		/// </summary>
		/// <value>Array of dora indicators</value>
		public Tile[] DoraIndicators
		{
			get
			{
				var firstDora = setting.LingshangTilesCount;
				var doraTiles = new Tile[DoraTurned];
				for (int i = 0; i < doraTiles.Length; i++)
				{
					var currentDora = firstDora + i * 2;
					doraTiles[i] = allTiles[allTiles.Count - 1 - currentDora];
				}

				return doraTiles;
			}
		}

		/// <summary>
		/// Returns current turned ura-dora indicators
		/// </summary>
		/// <value>Array of ura-dora indicators</value>
		public Tile[] UraDoraIndicators
		{
			get
			{
				var firstUraDora = setting.LingshangTilesCount + 1;
				var uraDoraTiles = new Tile[DoraTurned];
				for (int i = 0; i < uraDoraTiles.Length; i++)
				{
					var currentUraDora = firstUraDora + i * 2;
					uraDoraTiles[i] = allTiles[allTiles.Count - 1 - currentUraDora];
				}

				return uraDoraTiles;
			}
		}

		private void SetTiles(IList<Tile> tiles)
		{
			ET.Log.Debug("Cheating...");
			for (int i = 0; i < tiles.Count; i++)
			{
				allTiles[tilesDrawn + i] = tiles[i];
			}
		}

		private void SetTilesReverse(IList<Tile> tiles)
		{
			ET.Log.Debug("Cheating...");
			for (int i = 0; i < tiles.Count; i++)
			{
				allTiles[allTiles.Count - lingShangDrawn - 1 - i] = tiles[i];
			}
		}
		/// <summary>
		/// Ñ¡ÅÆºóÅÆÐò¸ü»».»»µ½Ä©Î²
		/// </summary>
		/// <param name="selectIndex"></param>
		public void TransTiles(int selectIndex)
		{
			var indexA = tilesDrawn + 2;
			var indexB = tilesDrawn + selectIndex;
			var tmp = allTiles[indexA];
			allTiles[indexA] = allTiles[indexB];
			allTiles[indexB] = tmp;
		}
		public IList<Tile> AllTiles => allTiles.AsReadOnly();
		public int TilesDrawn => tilesDrawn;
		public int DoraTurned => doraTurned;
		public int LingShangDrawn => lingShangDrawn;
		public int TilesRemain => allTiles.Count - tilesDrawn - lingShangDrawn;

		public MahjongSetData Data
		{
			get
			{
				return new MahjongSetData
				{
					TilesDrawn = TilesDrawn,
					DoraTurned = DoraTurned,
					LingShangDrawn = LingShangDrawn,
					TilesRemain = TilesRemain,
					DoraIndicators = DoraIndicators,
					TotalTiles = allTiles.Count
				};
			}
		}
	}
	[ProtoContract]
	[Serializable]
	public struct MahjongSetData
	{
		[ProtoMember(1)]
		public int TilesDrawn;
		[ProtoMember(2)]
		public int DoraTurned;
		[ProtoMember(3)]
		public int LingShangDrawn;
		[ProtoMember(4)]
		public int TilesRemain;
		[ProtoMember(5)]
		public Tile[] DoraIndicators;
		[ProtoMember(6)]
		public int TotalTiles;

		public override string ToString()
		{
			return $"TilesDrawn: {TilesDrawn}\n"
			       + $"DoraTurned: {DoraTurned}\n"
			       + $"LingShangDrawn: {LingShangDrawn}\n"
			       + $"TilesRemain: {TilesRemain}\n"
			       + $"DoraIndicators: {string.Join("", DoraIndicators)}\n"
			       + $"TotalTiles: {TotalTiles}";
		}
	}
}