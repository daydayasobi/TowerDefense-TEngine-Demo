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

                item.transform.SetParent(m_tfLayout, false);
                item.transform.localScale = Vector3.one;
                item.transform.eulerAngles = Vector3.zero;
                item.transform.localPosition = new Vector3(index * item.rectTransform.rect.width, 0, 0);
                item.SetLevelData(itemdata);

                var task = HandleLevelDownloadAsync(itemdata.GroupName, itemdata.PackageName, itemdata.Id);
                downloadTasks.Add(task);

                index++;
            }

            // 等待所有下载任务完成
            await UniTask.WhenAll(downloadTasks);
        }


        /// <summary>
        /// 下载level的函数
        /// </summary>
        /// <param name="packageName"></param>
        /// <param name="levelId"></param>
        private async UniTask HandleLevelDownloadAsync(string tag, string packageName, int levelId)
        {
            // 获取并更新远端版本
            var versionOp = GameModule.Resource.RequestPackageVersionAsync(false, 60, packageName);
            await versionOp.ToUniTask();

            await GameModule.Resource.UpdatePackageManifestAsync(versionOp.PackageVersion, 60, packageName);
            var downloader = GameModule.Resource.CreateResourceDownloader(new string[] { tag }, packageName);

            if (downloader.TotalDownloadCount > 0)
            {
                Log.Debug("开始下载关卡资源包: {0} - {1}", packageName, levelId);
                downloader.DownloadUpdateCallback = (downloadUpdateData) =>
                {
                    Log.Debug("download update {0}", downloader.Progress);
                    GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(levelId, downloader.Progress));
                };

                downloader.DownloadErrorCallback = (error) =>
                {
                    Log.Error($"下载出错 [{packageName}] : {error}");
                    GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(levelId, -1f));
                };

                downloader.BeginDownload();
                await downloader.ToUniTask();

                if (downloader.IsDone)
                {
                    GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(levelId, 1f));
                }
                else
                {
                    Log.Error($"资源包 {packageName} 下载失败！");
                    GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(levelId, -1f));
                }
            }
            else
            {
                // 已经有资源就直接标记完成
                GameEvent.Send(ChangeSceneEvent.LevelDownloadProgress, new LevelDownloadProgress(levelId, 1f));
            }
        }
    }
}