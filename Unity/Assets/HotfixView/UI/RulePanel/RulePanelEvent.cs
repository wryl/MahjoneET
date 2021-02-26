using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.RulePanel)]
    public class RulePanelEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            await ResourcesComponent.Instance.LoadBundleAsync(UIType.RulePanel.StringToAB());
            GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset(UIType.RulePanel.StringToAB(), UIType.RulePanel);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.RulePanel, gameObject);
            ui.AddComponent<RulePanelComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            ResourcesComponent.Instance.UnloadBundle(UIType.RulePanel.StringToAB());
        }
    }
}