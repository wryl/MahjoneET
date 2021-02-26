using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.InTurnPanel)]
    public class InTurnPanelEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            await ResourcesComponent.Instance.LoadBundleAsync(UIType.InTurnPanel.StringToAB());
            GameObject bundleGameObject = (GameObject) ResourcesComponent.Instance.GetAsset(UIType.InTurnPanel.StringToAB(), UIType.InTurnPanel);
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.InTurnPanel, gameObject);
            ui.AddComponent<InTurnPanelComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            ResourcesComponent.Instance.UnloadBundle(UIType.InTurnPanel.StringToAB());
        }
    }
}