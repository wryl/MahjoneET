using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public class RoomEntryComponentAwakeSystem : AwakeSystem<RoomEntryComponent, MJ_RoomInfoInList>
	{
		public override void Awake(RoomEntryComponent self, MJ_RoomInfoInList mJ_RoomInfoIn)
		{
			self.Awake(mJ_RoomInfoIn);
			self.MJoinButton.onClick.AddListener(self.JoinRoom);
		}
	}
	
	public static class RoomEntryComponentSystem
	{
		public static void JoinRoom(this RoomEntryComponent self)
		{
			MahjoneHelper.JoinRoomAsync(self.DomainScene(), self.Roomid).Coroutine();
		}
	}
}
