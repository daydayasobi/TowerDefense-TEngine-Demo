using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    [Window(UILayer.UI)]
    public class UILevelSelectForm : UIWindow
    {
        #region 脚本工具生成的代码
        private GameObject m_gorScroll;
        private Transform m_tfLayout;
        private Button m_btnBack;
        protected override void ScriptGenerator()
        {
            m_gorScroll = FindChild("Levels/m_gorScroll").gameObject;
            m_tfLayout = FindChild("Levels/m_gorScroll/m_tfLayout");
            m_btnBack = FindChildComponent<Button>("m_btnBack");
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
        }
        // public MouseScroll mouseScroll;
        private void ShowLevelSelectionButtonItems()
        {
            int index = 0;
            
            foreach (var itemdata in DataLevelManger.Instance.GetAllLevelData())
            {
                LevelSelectionButton item = CreateWidgetByPath<LevelSelectionButton>(this.transform, "LevelSelectionButton", true);

                item.transform.SetParent(m_tfLayout, false);
                item.transform.localScale = Vector3.one;
                item.transform.eulerAngles = Vector3.zero;
                item.transform.localPosition = new Vector3(index * item.rectTransform.rect.width, 0, 0);
                item.SetLevelData(itemdata);
            }
        }
    }
}
