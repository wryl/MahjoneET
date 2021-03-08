using System;
using UnityEngine;

namespace ET
{
    [UIEvent(UIType.RoomCanvas)]
    public class RoomCanvasEvent: AUIEvent
    {
        public override async ETTask<UI> OnCreate(UIComponent uiComponent)
        {
            GameObject bundleGameObject = await NewResourcesComponent.Instance.LoadAssetAsync<GameObject>(ABPathUtilities.GetUIPath(UIType.RoomCanvas));
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject);
            gameObject.layer = LayerMask.NameToLayer(LayerNames.UI);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(uiComponent, UIType.RoomCanvas, gameObject);
            ui.AddComponent<RoomCanvasComponent>();
            return ui;
        }

        public override void OnRemove(UIComponent uiComponent)
        {
            NewResourcesComponent.Instance.UnLoadAsset(ABPathUtilities.GetUIPath(UIType.RoomCanvas));
        }
    }
}