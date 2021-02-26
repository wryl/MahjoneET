using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
    public class InTurnPanelComponent : Entity
    {
        public Button MTsumoButton;
        public Button MDrawButton;
        public Button MRichiButton;
        public Button MKongButton;
        public Button MBeiButton;
        public Button MSkipButton;
        public Button MBackButton;
        public void Awake()
        {
            ReferenceCollector rc = GetParent<UI>().GameObject.GetComponent<ReferenceCollector>();
            MTsumoButton = rc.Get<GameObject>("TsumoButton").GetComponent<Button>();
            MDrawButton = rc.Get<GameObject>("DrawButton").GetComponent<Button>();
            MRichiButton = rc.Get<GameObject>("RichiButton").GetComponent<Button>();
            MKongButton = rc.Get<GameObject>("KongButton").GetComponent<Button>();
            MBeiButton = rc.Get<GameObject>("BeiButton").GetComponent<Button>();
            MSkipButton = rc.Get<GameObject>("SkipButton").GetComponent<Button>();
            MBackButton = rc.Get<GameObject>("BackButton").GetComponent<Button>();
        }
    }
}

