using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Mahjong.Model;
using GamePlay.Client.View.Elements;
using System.Linq;
using GamePlay.Client.Controller;

namespace GamePlay.Client.View
{
	public class SelectTilesPanelManager : MonoBehaviour
	{
		public SelectTile[] SelectTiles;
		public Transform SelectTilesParent;
		public Transform ReadyButton;

		private void Awake()
        {
			SelectTiles = new SelectTile[SelectTilesParent.childCount];
			for (int i = 0; i < SelectTilesParent.childCount; i++)
			{
				var t = SelectTilesParent.GetChild(i);
				SelectTiles[i] = t.GetComponent<SelectTile>();
			}
			ReadyButton.GetComponent<Button>().onClick.AddListener(()=> {
				ClientBehaviour.Instance.ClientSelectTilesReady(SelectTiles.Where(s => s.IsSelect).Select(s => s.Tile).ToList());
				this.Close();
			});
			//if (PlayerPrefs.HasKey("SelectTile"))
			//{
			//	var conf = PlayerPrefs.GetString("SelectTile");
			//	var conflist = conf.Split(',');
   //             for (int i = 0; i < conflist.Count(); i++)
   //             {

			//		Tile tile = new Tile();
   //             }
			//}
		}
        public void SetTiles(Tile[] allTiles)
		{
			this.enabled = true;
			for (int i = 0; i < allTiles.Count(); i++)
			{
				SelectTiles[i].SetTile(allTiles[i]);
			}
		}



		private const float AnimationDuration = 0.5f;

		public void Close()
		{
			this.gameObject.SetActive(false);
			//PlayerPrefs.SetString("SelectTile", string.Join(", ", SelectTiles.Select(s => s.Tile.ToStringIgnoreColor())));
		}

        public void Selectone()
        {
			ReadyButton.GetComponent<Button>().interactable = SelectTiles.Count(s => s.IsSelect) == 28;
        }
    }
}