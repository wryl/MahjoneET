using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	//public class SlotPanelComponentAwakeSystem : AwakeSystem<SlotPanelComponent>
	//{
	//	public override void Awake(SlotPanelComponent self)
	//	{
	//		self.Awake();
	//	}
	//}
	public class SlotPanelComponentAwakeSystem2 : AwakeSystem<SlotPanelComponent, PlayerInRoom>
	{
		public override void Awake(SlotPanelComponent self, PlayerInRoom player)
		{
			self.Awake();
			self.SetPlayerInfo(player);
			self.Id = player.UserId;
			
		}
	}

	public static class SlotPanelComponentSystem
	{
		internal static void SetPlayerInfo(this SlotPanelComponent self, PlayerInRoom player)
		{
			self.MPlayerName.text = player.NickName;
			self.MMaster.SetActive(player.IsMaster);
			self.MReady.SetActive(player.IsReady);
			self.player = player;
		}
		/// <summary>
		/// 增加房主功能
		/// </summary>
		/// <param name="self"></param>
		public static void SetAdminControll(this SlotPanelComponent self)
		{
			self.MKickButton.gameObject.SetActive(true);
			self.MKickButton.onClick.AddListener(self.KickPlayer);
		}
		public static void KickPlayer(this SlotPanelComponent self)
		{
			self.DomainScene().GetComponent<SessionComponent>().Session.Send(new MJ_KickPlayer() { KickId= self.player.InsId });
		}
	}
}
