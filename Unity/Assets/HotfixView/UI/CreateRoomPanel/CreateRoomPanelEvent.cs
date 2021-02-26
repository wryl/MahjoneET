using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.CreateRoomPanel)]
    public class CreateRoomPanelEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            GameObject bundleGameObject = await NewResourcesComponent.Instance.LoadAssetAsync<GameObject>(ABPathUtilities.GetUIPath(UIType.CreateRoomPanel));
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.CreateRoomPanel, gameObject);
            ui.AddComponent<CreateRoomPanelComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            NewResourcesComponent.Instance.UnLoadAsset(ABPathUtilities.GetUIPath(UIType.CreateRoomPanel));
        }
    }
}