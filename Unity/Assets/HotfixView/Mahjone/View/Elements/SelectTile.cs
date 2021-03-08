using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;
using Mahjong.Model;
using GamePlay.Client.Model;
using GamePlay.Client.View.SubManagers;
using GamePlay.Client.Controller;
using Common.Interfaces;
using Managers;

namespace GamePlay.Client.View.Elements
{
	/// <summary>
	/// 开局选卡用
	/// </summary>
	[RequireComponent(typeof(Image))]
	public class SelectTile : MonoBehaviour,
		IPointerClickHandler,
		IObserver<ClientRoundStatus>
	{
		public bool IsSelect=false;
		public Tile Tile => tile;
		private Image image;
		private RectTransform rect;
		private Tile tile;
		private bool locked = false;
		private IDictionary<Tile, IList<Tile>> waitingTiles;

		private void Awake()
		{
			rect = GetComponent<RectTransform>();
			image = GetComponent<Image>();
			image.DOColor(Color.gray, AnimationDuration);
		}

		private void OnEnable()
		{
			rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, 0);
		}

		public void SetTile(Tile tile)
		{
			gameObject.SetActive(true);
			this.tile = tile;
			var sprite = ResourceManager.Instance.GetTileSprite(tile);
			if (image == null) image = GetComponent<Image>();
			image.sprite = sprite;
		}

		public void TurnOff()
		{
			if (image != null)
			{
				image.DOColor(Color.gray, AnimationDuration);
			}
		}

		public void TurnOn()
		{
			if (image != null)
			{
				image.DOColor(Color.white, AnimationDuration);
			}
		}

		public void SetLock(bool locked)
		{
			this.locked = locked;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			IsSelect = !IsSelect;
			GetComponentInParent<SelectTilesPanelManager>().Selectone();
            if (IsSelect)
            {
				TurnOn();
			}
            else
            {
				TurnOff();
            }
		}


		public void UpdateStatus(ClientRoundStatus subject)
		{
			waitingTiles = subject.PossibleWaitingTiles;
		}

		private const float AnimationDuration = 0.5f;
	}
}