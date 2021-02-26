using UnityEngine;
using UnityEngine.UI;
using TMPro;
namespace ET
{
    public class RoomCanvasComponentAwakeSystem : AwakeSystem<RoomCanvasComponent>
    {
        public override void Awake(RoomCanvasComponent self)
        {
            self.Awake();
            self.MLeaveRoomButton.onClick.AddListener(self.LeaveRoom);
            self.MStartGameButton.onClick.AddListener(self.StartGame);
            self.MReadyButton.onClick.AddListener(self.SetReady);
            self.SlotPanelList = EntityFactory.CreateWithParent<ListUIComponent<SlotPanelComponent>>(self);
            self.SlotPanelList.Awake(self.MPlayerSlotsPanel);
        }
    }

    public static class RoomCanvasComponentSystem
    {
        /// <summary>
        /// ��Ⲣ�޸Ŀ�ʼ��ť״̬
        /// </summary>
        /// <param name="self"></param>
        public static void CheckStartButton(this RoomCanvasComponent self)
        {
            self.MStartGameButton.interactable = self.CanStartGame();
        }
        /// <summary>
        /// ����Ƿ���Կ�ʼ��Ϸ
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static bool CanStartGame(this RoomCanvasComponent self)
        {
            if (Mahjong.Model.GameSetting.GetPlayerCount(self.Settings.GamePlayers) != self.SlotPanelList.ItemCount)
            {
                return false;
            }
            foreach (var item in self.SlotPanelList.ItemComponents)
            {
                if (!item.player.IsReady)
                {
                    return false;
                }
            }
            return true;
        }
        //��ʼ��Ϸ
        public static void StartGame(this RoomCanvasComponent self)
        {
            MahjoneHelper.StartGameAsync(self.DomainScene()).Coroutine();
        }

        public static void LeaveRoom(this RoomCanvasComponent self)
        {
            MahjoneHelper.LeaveRoomAsync(self.DomainScene()).Coroutine();
        }
        public static void MasterInit(this RoomCanvasComponent self)
        {
            self.MStartGameButton.gameObject.SetActive(true);
        }
        public static void InitRoom(this RoomCanvasComponent self, MJ_RoomInfo settings)
        {
            self.Settings = settings.Setting;
            self.MRoomTitle.text = settings.RoomName;
            //var resp = (G2C_RoomList)await self.DomainScene().GetComponent<SessionComponent>().Session.Call(new C2G_RoomList());

        }
        public static void SetReady(this RoomCanvasComponent self)
        {
            var player = self.DomainScene().GetComponent<MJRoomPlayerComponent>().PlayerInfo;
            //��ǰ��׼��.��Ϊ׼��
            if (!player.IsReady)
            {
                player.IsReady = true;
                self.MReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "ȡ��׼��";
            }
            else
            {
                player.IsReady = false;
                self.MReadyButton.GetComponentInChildren<TextMeshProUGUI>().text = "׼��";
            }
            self.DomainScene().GetComponent<SessionComponent>().Session.Send(new MJ_RoomPlayerChangeRequest() { player = player });
        }
    }
}

