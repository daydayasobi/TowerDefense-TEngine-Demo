using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UIMainMenuForm : UIWindow
    {
        #region 脚本工具生成的代码
        private Button m_btnLevelSelect;
        private Button m_btnOptions;
        private Button m_btnQuit;
        protected override void ScriptGenerator()
        {
            m_btnLevelSelect = FindChildComponent<Button>("Menu Buttons/m_btnLevelSelect");
            m_btnOptions = FindChildComponent<Button>("Menu Buttons/m_btnOptions");
            m_btnQuit = FindChildComponent<Button>("Menu Buttons/m_btnQuit");
            m_btnLevelSelect.onClick.AddListener(OnClickLevelSelectBtn);
            m_btnOptions.onClick.AddListener(OnClickOptionsBtn);
            m_btnQuit.onClick.AddListener(OnClickQuitBtn);
        }
        #endregion

        #region 事件
        private void OnClickLevelSelectBtn()
        {
            GameEvent.Send(MainMenuEvent.ChangeLevelSelect);
        }
        private void OnClickOptionsBtn()
        {
            GameEvent.Send(MainMenuEvent.OpenOptions);
        }
        private void OnClickQuitBtn()
        {
            GameEvent.Send(MainMenuEvent.QuitGame);
        }
        #endregion
    }
}