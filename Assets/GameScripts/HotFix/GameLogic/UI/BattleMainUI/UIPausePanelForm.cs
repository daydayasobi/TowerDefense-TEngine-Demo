using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.UI;
using TEngine;
using GameConfig;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UIPausePanelForm : UIWindow
    {
        #region 脚本工具生成的代码
        private Text m_textLevelTitleText;
        private Text m_textLevelDescriptionText;
        private Button m_btnClose;
        private Button m_btnMainMenu;
        private Button m_btnRestart;
        protected override void ScriptGenerator()
        {
            m_textLevelTitleText = FindChildComponent<Text>("PauseMenu/Container/Top/m_textLevelTitleText");
            m_textLevelDescriptionText = FindChildComponent<Text>("PauseMenu/Container/Top/m_textLevelDescriptionText");
            m_btnClose = FindChildComponent<Button>("PauseMenu/Container/m_btnClose");
            m_btnMainMenu = FindChildComponent<Button>("PauseMenu/Container/Buttons/m_btnMainMenu");
            m_btnRestart = FindChildComponent<Button>("PauseMenu/Container/Buttons/m_btnRestart");
            m_btnClose.onClick.AddListener(OnClickCloseBtn);
            m_btnMainMenu.onClick.AddListener(OnClickMainMenuBtn);
            m_btnRestart.onClick.AddListener(OnClickRestartBtn);
        }
        #endregion

        #region 事件
        private void OnClickCloseBtn()
        {
            Close();
        }
        private void OnClickMainMenuBtn()
        {
            
        }
        private void OnClickRestartBtn()
        {
            GameEvent.Send(LevelEvent.OnReloadLevel);
            Close();
        }
        #endregion

        protected override void OnCreate()
        {
            base.OnCreate();

            Level levelData = DataLevelManager.Instance.CurrentLevel;

            m_textLevelTitleText.text = levelData.Name;
            m_textLevelDescriptionText.text = levelData.Description;

        }
    }
}