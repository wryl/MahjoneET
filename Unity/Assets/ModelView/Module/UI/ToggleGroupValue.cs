using UnityEngine;
using UnityEngine.UI;
using System;

namespace ET
{
	[RequireComponent(typeof(ToggleGroup))]
	public class ToggleGroupValue : MonoBehaviour
	{
		private Toggle[] toggles;
        public int GroupValue
        {
            get
            {
                if (toggles!=null)
                {
					return Array.FindIndex(toggles, t => t.isOn);
				}
				return 0;
            }
        }

        private void Awake()
		{
			toggles = new Toggle[transform.childCount];
			for (int i = 0, count = 0; i < transform.childCount; i++)
			{
				if (count >= toggles.Length) break;
				var t = transform.GetChild(i).GetComponent<Toggle>();
				if (t != null) toggles[count++] = t;
			}
		}

	}
}