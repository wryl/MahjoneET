using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UILogin)]
    public class UILoginEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            GameObject bundleGameObject = await NewResourcesComponent.Instance.LoadAssetAsync<GameObject>(ABPathUtilities.GetUIPath(UIType.UILogin));
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.UILogin, gameObject);
            ui.AddComponent<UILoginComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            NewResourcesComponent.Instance.UnLoadAsset(ABPathUtilities.GetUIPath(UIType.UILogin));
        }
    }

}