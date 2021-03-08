using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using libx;
using UnityEngine;

namespace ET
{
    public class UiBundleDownloaderComponentAwakeSystem: AwakeSystem<NewBundleDownloaderComponent>
    {
        public override void Awake(NewBundleDownloaderComponent self)
        {
            self.Updater = GameObject.FindObjectOfType<Updater>();
        }
    }
    
    public class UiBundleDownloaderComponentSystem: UpdateSystem<NewBundleDownloaderComponent>
    {
        public override void Update(NewBundleDownloaderComponent self)
        {
            if (self.Updater.Step == Step.Completed)
            {
                self.Tcs.SetResult();
            }
        }
    }

    /// <summary>
    /// 封装XAsset Updater
    /// </summary>
    public class NewBundleDownloaderComponent: Entity
    {
        public Updater Updater;

        public ETTaskCompletionSource Tcs;

        public ETTask StartUpdate()
        {
            Tcs = new ETTaskCompletionSource();
            Updater.ResPreparedCompleted = () =>
            {
                Tcs.SetResult();
            };
            Updater.StartUpdate();
            
            return Tcs.Task;
        }
    }
}