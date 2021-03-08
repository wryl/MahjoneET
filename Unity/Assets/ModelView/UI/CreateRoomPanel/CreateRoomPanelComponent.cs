using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class CreateRoomPanelComponent : Entity
    {
        public InputField MRoomNameInputField;
        public Button MCreateButton;
        public Button MResetButton;
        public Button MBackButton;
        public ToggleGroupValue MGamePlayer;
        public ToggleGroupValue MGameMode;
        public ToggleGroupValue MRoundCount;
        public ToggleGroupValue MGameEndsWhenAllLastTop;
        public ToggleGroupValue MMinimumFanConstraintType;
        public ToggleGroupValue MPointsToGameEnd;
        public InputField MInitialPoints;
        public InputField MFirstPlacePoints;
        public ToggleGroupValue MAllowHint;
        public ToggleGroupValue MInitialDoraCount;
        public ToggleGroupValue MAllowRichiWhenPointsLow;
        public ToggleGroupValue MAllowDiscardSameAfterOpen;
        public InputField MRichiMortgagePoints;
        public InputField MExtraRoundBonusPerPlayer;
        public InputField MNotReadyPunishPerPlayer;
        public InputField MFalseRichiPunishPerPlayer;
        public ToggleGroupValue MAllowMultipleRong;
        public ToggleGroupValue MAllow4WindDraw;
        public ToggleGroupValue MAllow3RongDraw;
        public ToggleGroupValue MAllow4RichiDraw;
        public ToggleGroupValue MAllow4KongDraw;
        public ToggleGroupValue MAllow9OrphanDraw;
        public InputField MBaseTurnTimeInputField;
        public InputField MBonusTurnTimeInputField;
        public Button MYakuSettingButton;
        public GameObject MYakuPanel;
        public GameObject MSettingPanel;
        public Button MReturnBasicSettingButton;
        public ToggleGroupValue MAllowGswsRobConcealedKong;
        public ToggleGroupValue MOpenDuanYao;
        public ToggleGroupValue MHasOneShot;
        public ToggleGroupValue MSiAnKe;
        public ToggleGroupValue MGuoShi;
        public ToggleGroupValue MJiuLian;
        public ToggleGroupValue MLvYiSe;
        public void Awake()
        {
            ReferenceCollector rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            MRoomNameInputField = rc.Get<GameObject>("RoomNameInputField").GetComponent<InputField>();
            MCreateButton = rc.Get<GameObject>("CreateButton").GetComponent<Button>();
            MResetButton = rc.Get<GameObject>("ResetButton").GetComponent<Button>();
            MBackButton = rc.Get<GameObject>("BackButton").GetComponent<Button>();
            MGamePlayer = rc.Get<GameObject>("GamePlayer").GetComponent<ToggleGroupValue>();
            MGameMode = rc.Get<GameObject>("GameMode").GetComponent<ToggleGroupValue>();
            MRoundCount = rc.Get<GameObject>("RoundCount").GetComponent<ToggleGroupValue>();
            MGameEndsWhenAllLastTop = rc.Get<GameObject>("GameEndsWhenAllLastTop").GetComponent<ToggleGroupValue>();
            MMinimumFanConstraintType = rc.Get<GameObject>("MinimumFanConstraintType").GetComponent<ToggleGroupValue>();
            MPointsToGameEnd = rc.Get<GameObject>("PointsToGameEnd").GetComponent<ToggleGroupValue>();
            MInitialPoints = rc.Get<GameObject>("InitialPoints").GetComponentInChildren<InputField>();
            MFirstPlacePoints = rc.Get<GameObject>("FirstPlacePoints").GetComponentInChildren<InputField>();
            MAllowHint = rc.Get<GameObject>("AllowHint").GetComponent<ToggleGroupValue>();
            MInitialDoraCount = rc.Get<GameObject>("InitialDoraCount").GetComponent<ToggleGroupValue>();
            MAllowRichiWhenPointsLow = rc.Get<GameObject>("AllowRichiWhenPointsLow").GetComponent<ToggleGroupValue>();
            MAllowDiscardSameAfterOpen = rc.Get<GameObject>("AllowDiscardSameAfterOpen").GetComponent<ToggleGroupValue>();
            MRichiMortgagePoints = rc.Get<GameObject>("RichiMortgagePoints").GetComponentInChildren<InputField>();
            MExtraRoundBonusPerPlayer = rc.Get<GameObject>("ExtraRoundBonusPerPlayer").GetComponentInChildren<InputField>();
            MNotReadyPunishPerPlayer = rc.Get<GameObject>("NotReadyPunishPerPlayer").GetComponentInChildren<InputField>();
            MFalseRichiPunishPerPlayer = rc.Get<GameObject>("FalseRichiPunishPerPlayer").GetComponentInChildren<InputField>();
            MAllowMultipleRong = rc.Get<GameObject>("AllowMultipleRong").GetComponent<ToggleGroupValue>();
            MAllow4WindDraw = rc.Get<GameObject>("Allow4WindDraw").GetComponent<ToggleGroupValue>();
            MAllow3RongDraw = rc.Get<GameObject>("Allow3RongDraw").GetComponent<ToggleGroupValue>();
            MAllow4RichiDraw = rc.Get<GameObject>("Allow4RichiDraw").GetComponent<ToggleGroupValue>();
            MAllow4KongDraw = rc.Get<GameObject>("Allow4KongDraw").GetComponent<ToggleGroupValue>();
            MAllow9OrphanDraw = rc.Get<GameObject>("Allow9OrphanDraw").GetComponent<ToggleGroupValue>();
            MBaseTurnTimeInputField = rc.Get<GameObject>("BaseTurnTimeInputField").GetComponentInChildren<InputField>();
            MBonusTurnTimeInputField = rc.Get<GameObject>("BonusTurnTimeInputField").GetComponentInChildren<InputField>();
            MYakuSettingButton = rc.Get<GameObject>("YakuSettingButton").GetComponent<Button>();
            MYakuPanel = rc.Get<GameObject>("YakuPanel");
            MSettingPanel = rc.Get<GameObject>("SettingPanel");
            MReturnBasicSettingButton = rc.Get<GameObject>("ReturnBasicSettingButton").GetComponent<Button>();
            MAllowGswsRobConcealedKong = rc.Get<GameObject>("AllowGswsRobConcealedKong").GetComponent<ToggleGroupValue>();
            MOpenDuanYao = rc.Get<GameObject>("OpenDuanYao").GetComponent<ToggleGroupValue>();
            MHasOneShot = rc.Get<GameObject>("HasOneShot").GetComponent<ToggleGroupValue>();
            MSiAnKe = rc.Get<GameObject>("SiAnKe").GetComponent<ToggleGroupValue>();
            MGuoShi = rc.Get<GameObject>("GuoShi").GetComponent<ToggleGroupValue>();
            MJiuLian = rc.Get<GameObject>("JiuLian").GetComponent<ToggleGroupValue>();
            MLvYiSe = rc.Get<GameObject>("LvYiSe").GetComponent<ToggleGroupValue>();
        }
    }
}

