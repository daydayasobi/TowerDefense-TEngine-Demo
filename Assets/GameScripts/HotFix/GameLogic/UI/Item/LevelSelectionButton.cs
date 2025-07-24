using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using GameConfig;
using TEngine;
using UnityEngine;
using UnityEngine.UI;
using YooAsset;

namespace GameLogic
{
    public class LevelSelectionButton : UIWidget
    {
        private bool isResLoaded = false;

        #region 脚本工具生成的代码

        private Button m_btnLevelSelectionButton;
        private Image m_imgMask;
        private Image m_imgProgress;
        private Text m_textLevelName;
        private Text m_textDownloadText;
        private Text m_textDescription;
        private Image m_imgStar1;
        private Image m_imgStar2;
        private Image m_imgStar3;

        protected override void ScriptGenerator()
        {
            m_btnLevelSelectionButton = FindChildComponent<Button>("m_btnLevelSelectionButton");
            m_imgMask = FindChildComponent<Image>("m_btnLevelSelectionButton/m_imgMask");
            m_imgProgress = FindChildComponent<Image>("m_btnLevelSelectionButton/m_imgProgress");
            m_textLevelName = FindChildComponent<Text>("m_btnLevelSelectionButton/m_textLevelName");
            m_textDownloadText = FindChildComponent<Text>("m_btnLevelSelectionButton/m_textDownloadText");
            m_textDescription = FindChildComponent<Text>("m_btnLevelSelectionButton/Content/m_textDescription");
            m_imgStar1 = FindChildComponent<Image>("m_btnLevelSelectionButton/Content/Score Panel/1/m_imgStar1");
            m_imgStar2 = FindChildComponent<Image>("m_btnLevelSelectionButton/Content/Score Panel/2/m_imgStar2");
            m_imgStar3 = FindChildComponent<Image>("m_btnLevelSelectionButton/Content/Score Panel/3/m_imgStar3");
            m_btnLevelSelectionButton.onClick.AddListener(OnClickLevelSelectionButtonBtn);

            GameEvent.AddEventListener<LevelDownloadProgress>(ChangeSceneEvent.LevelDownloadProgress, OnLevelDownloadProgress);
        }

        #endregion

        #region 事件

        protected override void OnDestroy()
        {
            base.OnDestroy();
            GameEvent.RemoveEventListener<LevelDownloadProgress>(ChangeSceneEvent.LevelDownloadProgress, OnLevelDownloadProgress);
        }

        private void OnLevelDownloadProgress(LevelDownloadProgress data)
        {
            if (data.LevelId == levelConfig.Id)
            {
                Log.Debug("关卡下载进度: " + data.LevelId + " - " + data.Progress);
            }
            // m_imgProgress.fillAmount = progress;
            m_textDownloadText.gameObject.SetActive(true);
            m_textDownloadText.text = $"{(int)(data.Progress * 100)}%";
            // m_imgMask.gameObject.SetActive(progress < 1f);
        }

        private void OnClickLevelSelectionButtonBtn()
        {
            GameEvent.Send(ChangeSceneEvent.LevelSelect, levelConfig);
        }

        #endregion

        LevelDataBase levelConfig;
        public Image[] stars;

        protected override void OnCreate()
        {
            base.OnCreate();
            stars = new Image[3] { m_imgStar1, m_imgStar2, m_imgStar3 };
        }

        public void SetLevelData(LevelDataBase levelConfig)
        {
            m_textLevelName.text = levelConfig.Name;
            m_textDescription.text = levelConfig.Description;
            this.levelConfig = levelConfig;

            int currentStarCount = GameModule.Save.GetInt(string.Format(LevelKey.LevelStarRecord, levelConfig.Id), 0);
            for (int i = 0; i < stars.Length; i++)
            {
                stars[i].gameObject.SetActive(i < currentStarCount);
            }
        }
    }
}