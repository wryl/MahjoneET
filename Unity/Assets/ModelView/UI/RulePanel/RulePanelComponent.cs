using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
public class RulePanelComponent:Entity
{
	public ToggleGroup MAllow3RongDraw;
	public ToggleGroup MAllow4KongDraw;
	public ToggleGroup MAllow4RichiDraw;
	public ToggleGroup MAllow4WindDraw;
	public ToggleGroup MAllow9OrphanDraw;
	public ToggleGroup MAllowDiscardSameAfterOpen;
	public ToggleGroup MAllowGswsRobConcealedKong;
	public ToggleGroup MAllowHint;
	public ToggleGroup MAllowMultipleRong;
	public ToggleGroup MAllowRichiWhenNotReady;
	public ToggleGroup MAllowRichiWhenPointsLow;
	public Button MBackButton;
	public InputField MBaseTurnTimeInputField;
	public InputField MBonusTurnTimeInputField;
	public Text MExtraRoundBonusPerPlayer;
	public Text MFalseRichiPunishPerPlayer;
	public Text MFirstPlacePoints;
	public ToggleGroup MGameEndsWhenAllLastTop;
	public ToggleGroup MGameMode;
	public ToggleGroup MGamePlayer;
	public ToggleGroup MGuoShi;
	public ToggleGroup MHasOneShot;
	public ToggleGroup MInitialDoraCount;
	public Text MInitialPoints;
	public ToggleGroup MJiuLian;
	public ToggleGroup MLvYiSe;
	public ToggleGroup MMinimumFanConstraintType;
	public Text MNotReadyPunishPerPlayer;
	public ToggleGroup MOpenDuanYao;
	public ToggleGroup MPointsToGameEnd;
	public Button MReturnBasicSettingButton;
	public Text MRichiMortgagePoints;
	public ToggleGroup MRoundCount;
	public GameObject MSettingPanel;
	public ToggleGroup MSiAnKe;
	public GameObject MYakuPanel;
	public Button MYakuSettingButton;
public void Awake()
{
	ReferenceCollector rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
	MAllow3RongDraw=rc.Get<GameObject>("Allow3RongDraw").GetComponent<ToggleGroup>();
	MAllow4KongDraw=rc.Get<GameObject>("Allow4KongDraw").GetComponent<ToggleGroup>();
	MAllow4RichiDraw=rc.Get<GameObject>("Allow4RichiDraw").GetComponent<ToggleGroup>();
	MAllow4WindDraw=rc.Get<GameObject>("Allow4WindDraw").GetComponent<ToggleGroup>();
	MAllow9OrphanDraw=rc.Get<GameObject>("Allow9OrphanDraw").GetComponent<ToggleGroup>();
	MAllowDiscardSameAfterOpen=rc.Get<GameObject>("AllowDiscardSameAfterOpen").GetComponent<ToggleGroup>();
	MAllowGswsRobConcealedKong=rc.Get<GameObject>("AllowGswsRobConcealedKong").GetComponent<ToggleGroup>();
	MAllowHint=rc.Get<GameObject>("AllowHint").GetComponent<ToggleGroup>();
	MAllowMultipleRong=rc.Get<GameObject>("AllowMultipleRong").GetComponent<ToggleGroup>();
	MAllowRichiWhenNotReady=rc.Get<GameObject>("AllowRichiWhenNotReady").GetComponent<ToggleGroup>();
	MAllowRichiWhenPointsLow=rc.Get<GameObject>("AllowRichiWhenPointsLow").GetComponent<ToggleGroup>();
	MBackButton=rc.Get<GameObject>("BackButton").GetComponent<Button>();
	MBaseTurnTimeInputField=rc.Get<GameObject>("BaseTurnTimeInputField").GetComponent<InputField>();
	MBonusTurnTimeInputField=rc.Get<GameObject>("BonusTurnTimeInputField").GetComponent<InputField>();
	MExtraRoundBonusPerPlayer=rc.Get<GameObject>("ExtraRoundBonusPerPlayer").GetComponent<Text>();
	MFalseRichiPunishPerPlayer=rc.Get<GameObject>("FalseRichiPunishPerPlayer").GetComponent<Text>();
	MFirstPlacePoints=rc.Get<GameObject>("FirstPlacePoints").GetComponent<Text>();
	MGameEndsWhenAllLastTop=rc.Get<GameObject>("GameEndsWhenAllLastTop").GetComponent<ToggleGroup>();
	MGameMode=rc.Get<GameObject>("GameMode").GetComponent<ToggleGroup>();
	MGamePlayer=rc.Get<GameObject>("GamePlayer").GetComponent<ToggleGroup>();
	MGuoShi=rc.Get<GameObject>("GuoShi").GetComponent<ToggleGroup>();
	MHasOneShot=rc.Get<GameObject>("HasOneShot").GetComponent<ToggleGroup>();
	MInitialDoraCount=rc.Get<GameObject>("InitialDoraCount").GetComponent<ToggleGroup>();
	MInitialPoints=rc.Get<GameObject>("InitialPoints").GetComponent<Text>();
	MJiuLian=rc.Get<GameObject>("JiuLian").GetComponent<ToggleGroup>();
	MLvYiSe=rc.Get<GameObject>("LvYiSe").GetComponent<ToggleGroup>();
	MMinimumFanConstraintType=rc.Get<GameObject>("MinimumFanConstraintType").GetComponent<ToggleGroup>();
	MNotReadyPunishPerPlayer=rc.Get<GameObject>("NotReadyPunishPerPlayer").GetComponent<Text>();
	MOpenDuanYao=rc.Get<GameObject>("OpenDuanYao").GetComponent<ToggleGroup>();
	MPointsToGameEnd=rc.Get<GameObject>("PointsToGameEnd").GetComponent<ToggleGroup>();
	MReturnBasicSettingButton=rc.Get<GameObject>("ReturnBasicSettingButton").GetComponent<Button>();
	MRichiMortgagePoints=rc.Get<GameObject>("RichiMortgagePoints").GetComponent<Text>();
	MRoundCount=rc.Get<GameObject>("RoundCount").GetComponent<ToggleGroup>();
	MSettingPanel=rc.Get<GameObject>("SettingPanel");
	MSiAnKe=rc.Get<GameObject>("SiAnKe").GetComponent<ToggleGroup>();
	MYakuPanel=rc.Get<GameObject>("YakuPanel");
	MYakuSettingButton=rc.Get<GameObject>("YakuSettingButton").GetComponent<Button>();
}
}
}

