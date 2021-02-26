using System;
using System.Linq;
using System.Reflection;
using System.Threading;
using UnityEngine;
namespace ET
{
	public class Init : MonoBehaviour
	{
		private void Start()
		{
			try
			{
				SynchronizationContext.SetSynchronizationContext(OneThreadSynchronizationContext.Instance);
				
				DontDestroyOnLoad(gameObject);

				string[] assemblyNames = { "Unity.Model", "Unity.Hotfix", "Unity.ModelView", "Unity.HotfixView" };
				
				foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
				{
					//Log.Debug(assembly.FullName);
					string assemblyName = assembly.FullName;
					if (!assemblyNames.Contains(assemblyName.Split(',')[0]))
					{
						continue;
					}
					Game.EventSystem.Add(assembly);	
				}
				
				Game.EventSystem.Publish(new EventType.AppStart());
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			OneThreadSynchronizationContext.Instance.Update();
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Close();
		}
	}
}