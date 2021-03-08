using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{

    /// <summary>
    /// 用于列表的管理.这种类型不可以加awake函数,需要手动调用初始化
    /// </summary>
    public class ListUIComponent<TE> : Entity where TE : Entity, new()
    {
        private GameObject parentGameObject;
        private List<UI> uilists;
        private List<long> UIIds;
        public List<TE> ItemComponents = new List<TE>();
        /// <summary>
        /// 根据指定uitype创建列表节点
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="uIType"></param>
        /// <param name="datalists"></param>
		public void RefreshList<T>(string uIType, List<T> datalists,GameObject bundleGameObject) where T : IDataWithInsId
        {
            //var bundleGameObject = NewResourcesComponent.Instance.LoadAsset<GameObject>(ABPathUtilities.GetUIPath(uIType));
            foreach (var ui in uilists)
            {
                ui.Dispose();
            }
            uilists.Clear();
            UIIds.Clear();
            for (int i = 0; i < datalists.Count; i++)
            {
                GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, parentGameObject.transform);
                UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(this, uIType, gameObject);
                ItemComponents.Add(ui.AddComponent<TE, T>(datalists[i]));
                uilists.Add(ui);
                UIIds.Add(datalists[i].InsId);
            }
        }
        public void AddItem<T>(string uIType, T item, GameObject bundleGameObject) where T : IDataWithInsId
        {
            //var bundleGameObject = NewResourcesComponent.Instance.LoadAsset<GameObject>(ABPathUtilities.GetUIPath(uIType));
            GameObject gameObject = UnityEngine.Object.Instantiate(bundleGameObject, parentGameObject.transform);
            UI ui = EntityFactory.CreateWithParent<UI, string, GameObject>(this, uIType, gameObject);
            ItemComponents.Add(ui.AddComponent<TE, T>(item));
            uilists.Add(ui);
            UIIds.Add(item.InsId);
        }
        public void RemoveItem(long insId)
        {
            if (UIIds.Contains(insId))
            {
                int index = UIIds.IndexOf(insId);
                uilists[index].Dispose();
                uilists.RemoveAt(index);
                UIIds.RemoveAt(index);
                ItemComponents.RemoveAt(index);
            }
        }
        public TE GetEntityById(long insId)
        {
            return uilists[UIIds.IndexOf(insId)].GetComponent<TE>();
        }

        public bool IsExist(long insId) => UIIds.Contains(insId);

        public int ItemCount => UIIds.Count;
        public void Awake(GameObject parentObject)
        {
            uilists = new List<UI>();
            UIIds = new List<long>();
            parentGameObject = parentObject;
        }
    }
}