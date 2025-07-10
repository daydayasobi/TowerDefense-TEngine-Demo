using UnityEngine;
using UnityEngine.UI;
using TEngine;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UIGameOverForm : UIWindow
    {
        #region 脚本工具生成的代码
        private Image m_img1;
        private Image m_img2;
        private Image m_img3;
        private Text m_textTitleText;
        private Button m_btnNextLevel;
        private Button m_btnMainmenu;
        private Button m_btnRestart;
        protected override void ScriptGenerator()
        {
            m_img1 = FindChildComponent<Image>("Container/Top/Score Panel/m_img1");
            m_img2 = FindChildComponent<Image>("Container/Top/Score Panel/m_img2");
            m_img3 = FindChildComponent<Image>("Container/Top/Score Panel/m_img3");
            m_textTitleText = FindChildComponent<Text>("Container/Top/m_textTitleText");
            m_btnNextLevel = FindChildComponent<Button>("Container/VerticalButtons/m_btnNextLevel");
            m_btnMainmenu = FindChildComponent<Button>("Container/VerticalButtons/m_btnMainmenu");
            m_btnRestart = FindChildComponent<Button>("Container/VerticalButtons/m_btnRestart");
            m_btnNextLevel.onClick.AddListener(OnClickNextLevelBtn);
            m_btnMainmenu.onClick.AddListener(OnClickMainmenuBtn);
            m_btnRestart.onClick.AddListener(OnClickRestartBtn);
        }

        #endregion

        #region 事件
        private void OnClickNextLevelBtn()
        {
            // 测试用
            OnClickMainmenuBtn();
            Close();
        }
        private void OnClickMainmenuBtn()
        {
            LevelDataControl.Instance.ExitLevel();
            Close();
        }
        private void OnClickRestartBtn()
        {
            LevelDataControl.Instance.LoadLevel(LevelDataControl.Instance.CurrentLevelIndex);
            Close();
        }
        #endregion
        
        protected override void OnCreate()
        {
            base.OnCreate();

            int starCount = 0;
            foreach (object obj in base._userDatas)
            {
                if (obj is int)
                {
                    starCount = (int)obj;
                }
            }
        }
    }
}
