using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace GameLogic
{
    [Window(UILayer.UI)]
    public class UILevelSelectForm : UIWindow
    {
        #region 脚本工具生成的代码

        private GameObject m_gorScroll;
        private Transform m_tfLayout;
        private Button m_btnBack;
        private MouseScroll mouseScroll;

        protected override void ScriptGenerator()
        {
            m_gorScroll = FindChild("Levels/m_gorScroll").gameObject;
            m_tfLayout = FindChild("Levels/m_gorScroll/m_tfLayout");
            m_btnBack = FindChildComponent<Button>("m_btnBack");
            mouseScroll = FindChild("Levels/m_gorScroll").GetComponent<MouseScroll>();
            m_btnBack.onClick.AddListener(OnClickBackBtn);
        }

        #endregion

        #region 事件

        private void OnClickBackBtn()
        {
            GameEvent.Send(ChangeSceneEvent.MenuSelect);
        }

        #endregion

        protected override void OnCreate()
        {
            ShowLevelSelectionButtonItems();
            // mouseScroll.Init();
        }

        private async UniTask ShowLevelSelectionButtonItems()
        {
            int index = 0;
            List<UniTask> downloadTasks = new List<UniTask>();

            foreach (var itemdata in LevelDataControl.Instance.GetAllLevelData())
            {
                LevelSelectionButton item = await CreateWidgetByPathAsync<LevelSelectionButton>(this.transform, "LevelSelectionButton", true);

                if(itemdata.Id > 3)
                    return;
                
                item.transform.SetParent(m_tfLayout, false);
                item.transform.localScale = Vector3.one;
                item.transform.eulerAngles = Vector3.zero;
                item.transform.localPosition = new Vector3(index * item.rectTransform.rect.width, 0, 0);
                item.SetLevelData(itemdata);

                string assetName = $"level{itemdata.Id}";
                string packageName = itemdata.PackageName;

                // 判断是否需要下载（包含资源包初始化 + 本地资源 + 最新版本检查）
                // bool initialized = await EnsurePackageInitialized(packageName);

                if (YooAssets.GetPackage(packageName) == null)
                {
                    // 注册package
                    YooAssets.CreatePackage(packageName);
                    var task = HandleLevelDownloadAsync(itemdata.PackageName, itemdata.Id);
                    downloadTasks.Add(task);
                }
                else
                {
                    // 已经有资源就直接标记完成
                    GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(itemdata.Id, 1f));
                }

                index++;
            }

            // 等待所有下载任务完成
            await UniTask.WhenAll(downloadTasks);
        }
        
        // /// <summary>
        // /// 检查资源是否在本地，并检测是否需要更新
        // /// </summary>
        // private async UniTask<bool> ShouldDownloadAssetAsync(string assetName, string packageName)
        // {
        //     var package = await EnsurePackageInitialized(packageName);
        //
        //     // 本地存在？
        //     var hasAssetResult = GameModule.Resource.HasAsset(assetName);
        //     if (hasAssetResult != HasAssetResult.AssetOnDisk)
        //         return false;
        //     else 
        //         return true;
        // }
        
        /// <summary>
        /// 确保资源包已注册，并初始化完成
        /// </summary>
        private async UniTask<bool> EnsurePackageInitialized(string packageName)
        {
            var package = YooAssets.TryGetPackage(packageName);
            if (package == null)
            {
                Log.Warning($"资源包 {packageName} 未注册，开始注册并初始化...");
                // package = YooAssets.CreatePackage(packageName);
                await  GameModule.Resource.InitPackage(packageName);
                return false;
            }

            return true;
        }
        
        // /// <summary>
        // /// 确保资源包已注册，并初始化完成
        // /// </summary>
        // private async UniTask<ResourcePackage> EnsurePackageInitialized(string packageName)
        // {
        //     var package = YooAssets.TryGetPackage(packageName);
        //     if (package == null)
        //     {
        //         Log.Warning($"资源包 {packageName} 未注册，开始注册并初始化...");
        //         package = YooAssets.CreatePackage(packageName);
        //         await  GameModule.Resource.InitPackage(packageName);
        //         return YooAssets.TryGetPackage(packageName);
        //     }
        //
        //     return package;
        // }


        /// <summary>
        /// 下载level的函数
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="levelId"></param>
        private async UniTask HandleLevelDownloadAsync(string packageName, int levelId)
        {
            await GameModule.Resource.InitPackage(packageName);
            // // 获取并更新远端版本
            // var versionOp = GameModule.Resource.RequestPackageVersionAsync(false, 60, packageName);
            // await versionOp.ToUniTask();
            //
            // await GameModule.Resource.UpdatePackageManifestAsync(versionOp.PackageVersion, 60, packageName);
            // var downloader = GameModule.Resource.CreateResourceDownloader(packageName); 
            //
            // if (downloader.TotalDownloadCount > 0)
            // {
            //     downloader.DownloadUpdateCallback = (totalBytes) =>
            //     {
            //         Log.Debug("download update {0}",totalBytes);
            //         
            //         string currentSizeMb = (totalBytes.CurrentDownloadBytes / 1048576f).ToString("f1");
            //         string totalSizeMb = (totalBytes.TotalDownloadBytes / 1048576f).ToString("f1");
            //         float progress = totalBytes.TotalDownloadBytes > 0 ? totalBytes.CurrentDownloadBytes / totalBytes.TotalDownloadBytes : 0f;
            //         GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(levelId, progress));
            //     };
            //
            //     downloader.DownloadErrorCallback = (error) =>
            //     {
            //         Log.Error($"下载出错 [{packageName}] : {error}");
            //         GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(levelId, -1f));
            //     };
            //
            //     await downloader.ToUniTask();
            //
            //     if (downloader.IsDone)
            //     {
            //         GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(levelId, 1f));
            //     }
            //     else
            //     {
            //         Log.Error($"资源包 {packageName} 下载失败！");
            //         GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(levelId, -1f));
            //     }
            // }
        }
    }
}