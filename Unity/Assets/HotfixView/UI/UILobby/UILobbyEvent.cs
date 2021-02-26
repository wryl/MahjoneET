using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.UILobby)]
    public class UILobbyEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            GameObject bundleGameObject = await NewResourcesComponent.Instance.LoadAssetAsync<GameObject>(ABPathUtilities.GetUIPath(UIType.UILobby));
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.UILobby, gameObject);
            ui.AddComponent<UILobbyComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            NewResourcesComponent.Instance.UnLoadAsset(ABPathUtilities.GetUIPath(UIType.UILobby));
        }
    }
}