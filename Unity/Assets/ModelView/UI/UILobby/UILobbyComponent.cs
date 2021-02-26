using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 
namespace ET
{
    public class UILobbyComponent : Entity
	{
		public GameObject content;
		public TextMeshProUGUI text;
		public Button refreshButton;
		public Button createRoomButton;
		public List<UI> UIRommList = new List<UI>();
		public ListUIComponent<RoomEntryComponent> roomlist;
	}
}
