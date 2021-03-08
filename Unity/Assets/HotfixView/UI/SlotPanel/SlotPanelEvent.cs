using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.SlotPanel)]
    public class SlotPanelEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            GameObject bundleGameObject = await NewResourcesComponent.Instance.LoadAssetAsync<GameObject>(ABPathUtilities.GetUIPath(UIType.SlotPanel));
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.SlotPanel, gameObject);
            ui.AddComponent<SlotPanelComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            NewResourcesComponent.Instance.UnLoadAsset(ABPathUtilities.GetUIPath(UIType.SlotPanel));
        }
    }
}