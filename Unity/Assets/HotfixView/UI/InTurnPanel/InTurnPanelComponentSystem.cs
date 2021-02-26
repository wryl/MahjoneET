using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public class InTurnPanelComponentAwakeSystem : AwakeSystem<InTurnPanelComponent>
	{
		public override void Awake(InTurnPanelComponent self)
		{
			self.Awake();
		}
	}
	
	public static class InTurnPanelComponentSystem
	{
		public static void ExtendMethod(this InTurnPanelComponent self)
		{
			//dosomething
		}
	}
}
