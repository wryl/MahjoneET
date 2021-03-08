using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	public class RulePanelComponentAwakeSystem : AwakeSystem<RulePanelComponent>
	{
		public override void Awake(RulePanelComponent self)
		{
			self.Awake();
		}
	}
	
	public static class RulePanelComponentSystem
	{
		public static void ExtendMethod(this RulePanelComponent self)
		{
			//dosomething
		}
	}
}
