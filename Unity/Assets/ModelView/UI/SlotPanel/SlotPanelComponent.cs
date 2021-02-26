using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace ET
{
    public class SlotPanelComponent : Entity
    {

        public PlayerInRoom player;
        public Image MBackground;
        public Image MCharaImage;
        public GameObject MMaster;
        public TextMeshProUGUI MPlayerName;
        public Image MPlayerNamePanel;
        public GameObject MReady;
        public Button MKickButton;
        public void Awake()
        {
            ReferenceCollector rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            MBackground = rc.Get<GameObject>("Background").GetComponent<Image>();
            MCharaImage = rc.Get<GameObject>("CharaImage").GetComponent<Image>();
            MMaster = rc.Get<GameObject>("Master");
            MPlayerName = rc.Get<GameObject>("PlayerName").GetComponent<TextMeshProUGUI>();
            MPlayerNamePanel = rc.Get<GameObject>("PlayerNamePanel").GetComponent<Image>();
            MReady = rc.Get<GameObject>("Ready");
            MKickButton = rc.Get<GameObject>("KickButton").GetComponent<Button>();
        }
    }
}

