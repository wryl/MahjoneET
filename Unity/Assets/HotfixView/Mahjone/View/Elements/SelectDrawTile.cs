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
	[RequireComponent(typeof(Image))]
	public class SelectDrawTile : MonoBehaviour,
		IPointerClickHandler,
		IPointerEnterHandler,
		IPointerExitHandler,
		IObserver<ClientRoundStatus>
	{
		public int SelectIndex;
		public bool interactable = true;
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
			interactable = false;
			rect.DOAnchorPosY(0, AnimationDuration);
			if (image != null)
			{
				image.DOColor(Color.gray, AnimationDuration);
			}
		}

		public void TurnOn()
		{
			interactable = true;
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
			if (!interactable || locked) return;
			if (ClientBehaviour.Instance.CurrentRoundStatus.CurrentPlaceIndex == 0)
				ClientBehaviour.Instance.OnPreDrawTile(SelectIndex);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			if (!interactable) return;
			rect.DOAnchorPosY(20, AnimationDuration);

		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (!interactable) return;
			rect.DOAnchorPosY(0, AnimationDuration);
		}

		public void UpdateStatus(ClientRoundStatus subject)
		{
			waitingTiles = subject.PossibleWaitingTiles;
		}

		private const float AnimationDuration = 0.5f;
	}
}