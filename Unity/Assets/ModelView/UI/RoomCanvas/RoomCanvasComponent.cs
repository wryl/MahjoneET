using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Mahjong.Model;

namespace ET
{
    public class RoomCanvasComponent : Entity
    {
        public Button MCheckRuleButton;
        public Button MReadyButton;
        public TextMeshProUGUI MRoomTitle;
        public GameObject MPlayerSlotsPanel;
        public Button MStartGameButton;
        public Button MLeaveRoomButton;
        public GameObject MWarningPanel;
        public ListUIComponent<SlotPanelComponent> SlotPanelList;
        public GameSetting Settings;
        public void Awake()
        {
            ReferenceCollector rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            MCheckRuleButton = rc.Get<GameObject>("CheckRuleButton").GetComponent<Button>();
            MReadyButton = rc.Get<GameObject>("ReadyButton").GetComponent<Button>();
            MRoomTitle = rc.Get<GameObject>("RoomTitle").GetComponent<TextMeshProUGUI>();
            MPlayerSlotsPanel = rc.Get<GameObject>("PlayerSlotsPanel");
            MStartGameButton = rc.Get<GameObject>("StartGameButton").GetComponent<Button>();
            MLeaveRoomButton = rc.Get<GameObject>("LeaveRoomButton").GetComponent<Button>();
            MWarningPanel = rc.Get<GameObject>("WarningPanel");
        }
    }
}

