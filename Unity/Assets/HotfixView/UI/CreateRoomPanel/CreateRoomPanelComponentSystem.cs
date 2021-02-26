using Managers;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class CreateRoomPanelComponentAwakeSystem : AwakeSystem<CreateRoomPanelComponent>
    {
        public override void Awake(CreateRoomPanelComponent self)
        {
            self.Awake();
            self.MBackButton.onClick.AddListener(self.MBackButtonClick);
            self.MYakuSettingButton.onClick.AddListener(self.MYakuSettingButtonClick);
            self.MReturnBasicSettingButton.onClick.AddListener(self.MReturnBasicSettingButtonClick);
            self.MCreateButton.onClick.AddListener(self.MCreateButtonClick);
        }
    }

    public static class CreateRoomPanelComponentSystem
    {
        public static void MBackButtonClick(this CreateRoomPanelComponent self)
        {
            //触发返回房间列表事件
            Game.EventSystem.Publish(new EventType.LeaveCreateRoomPanel() { Scene = self.DomainScene() }).Coroutine();
        }
        public static void MYakuSettingButtonClick(this CreateRoomPanelComponent self)
        {
            //触发详细设置
            self.MYakuPanel.SetActive(true);
            self.MSettingPanel.SetActive(false);
        }
        public static void MReturnBasicSettingButtonClick(this CreateRoomPanelComponent self)
        {
            //关闭详细设置
            self.MYakuPanel.SetActive(false);
            self.MSettingPanel.SetActive(true);
        }
        public static void MCreateButtonClick(this CreateRoomPanelComponent self)
        {
            //创建
            Mahjong.Model.GameSetting setting = new Mahjong.Model.GameSetting();
            setting.GameMode = (Mahjong.Model.GameMode)self.MGameMode.GroupValue;
            setting.GamePlayers = (Mahjong.Model.GamePlayers)self.MGamePlayer.GroupValue;
            setting.RoundCount = (Mahjong.Model.RoundCount)self.MRoundCount.GroupValue;
            setting.MinimumFanConstraintType = (Mahjong.Model.MinimumFanConstraintType)self.MMinimumFanConstraintType.GroupValue;
            setting.PointsToGameEnd = (Mahjong.Model.PointsToGameEnd)self.MPointsToGameEnd.GroupValue;
            setting.GameEndsWhenAllLastTop = self.MGameEndsWhenAllLastTop.GroupValue == 0 ? true : false;
            setting.InitialPoints = int.Parse(self.MInitialPoints.text);
            setting.FirstPlacePoints = int.Parse(self.MFirstPlacePoints.text);
            setting.AllowHint = self.MAllowHint.GroupValue == 0 ? true : false;
            setting.AllowDiscardSameAfterOpen = self.MAllowDiscardSameAfterOpen.GroupValue == 0 ? true : false;
            setting.AllowRichiWhenPointsLow = self.MAllowRichiWhenPointsLow.GroupValue == 0 ? true : false;
            setting.AllowRichiWhenNotReady = false;
            setting.RichiMortgagePoints = int.Parse(self.MRichiMortgagePoints.text);
            setting.ExtraRoundBonusPerPlayer = int.Parse(self.MExtraRoundBonusPerPlayer.text);
            setting.NotReadyPunishPerPlayer = int.Parse(self.MNotReadyPunishPerPlayer.text);
            setting.FalseRichiPunishPerPlayer = int.Parse(self.MFalseRichiPunishPerPlayer.text);
            setting.AllowMultipleRong = self.MAllowMultipleRong.GroupValue == 0 ? true : false;
            setting.Allow3RongDraw = self.MAllow3RongDraw.GroupValue == 0 ? true : false;
            setting.Allow4RichiDraw = self.MAllow4RichiDraw.GroupValue == 0 ? true : false;
            setting.Allow4KongDraw = self.MAllow4KongDraw.GroupValue == 0 ? true : false;
            setting.Allow4WindDraw = self.MAllow4WindDraw.GroupValue == 0 ? true : false;
            setting.Allow9OrphanDraw = self.MAllow9OrphanDraw.GroupValue == 0 ? true : false;
            setting.InitialDoraCount = (Mahjong.Model.InitialDoraCount)self.MInitialDoraCount.GroupValue;
            setting.AllowChows = true;
            setting.AllowPongs = true;
            setting.AllowBeiDora = false;
            setting.AllowBeiDoraRongAsRobbKong = false;
            setting.AllowBeiDoraTsumoAsLingShang = false;
            setting.AllowBeiAsYaku = false;
            setting.OpenDuanYao = self.MOpenDuanYao.GroupValue == 0 ? true : false;
            setting.连风对子额外加符 = true;
            setting.SiAnKe = (Mahjong.Model.YakumanLevel)self.MSiAnKe.GroupValue;
            setting.GuoShi = (Mahjong.Model.YakumanLevel)self.MGuoShi.GroupValue;
            setting.JiuLian = (Mahjong.Model.YakumanLevel)self.MJiuLian.GroupValue;
            setting.LvYiSe = (Mahjong.Model.YakumanLevel)self.MLvYiSe.GroupValue;
            MahjoneHelper.CreateRoomAsync(self.DomainScene(), self.MRoomNameInputField.text, setting).Coroutine();
        }
    }
}
