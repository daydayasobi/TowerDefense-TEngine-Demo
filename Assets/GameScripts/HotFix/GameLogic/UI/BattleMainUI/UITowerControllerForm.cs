using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    [Window(UILayer.UI)]
    class UITowerControllerForm : UIWindow
    {
        #region  自定义数据

        private Tower m_tower;
        private bool click = false;
        
        #endregion
        
        #region 脚本工具生成的代码
        private Button m_btnBlocker;
        private Text m_textName;
        private Text m_textDps;
        private Text m_textDescription;
        private Button m_btnUpgradeButton;
        private Text m_textEnergyTextUp;
        private Text m_textUpgradeDescriptionText;
        private Button m_btnSellButton;
        private Button m_btnSellConfirmButton;
        private Text m_textEnergyTextSell;
        protected override void ScriptGenerator()
        {
            m_btnBlocker = FindChildComponent<Button>("m_btnBlocker");
            m_textName = FindChildComponent<Text>("Panel/TitlePanel/m_textName");
            m_textDps = FindChildComponent<Text>("Panel/TitlePanel/m_textDps");
            m_textDescription = FindChildComponent<Text>("Panel/TitlePanel/m_textDescription");
            m_btnUpgradeButton = FindChildComponent<Button>("Panel/Buttons/m_btnUpgradeButton");
            m_textEnergyTextUp = FindChildComponent<Text>("Panel/Buttons/m_btnUpgradeButton/m_textEnergyTextUp");
            m_textUpgradeDescriptionText = FindChildComponent<Text>("Panel/Buttons/m_btnUpgradeButton/m_textUpgradeDescriptionText");
            m_btnSellButton = FindChildComponent<Button>("Panel/Buttons/m_btnSellButton");
            m_textEnergyTextSell = FindChildComponent<Text>("Panel/Buttons/m_btnSellButton/m_textEnergyTextSell");
            m_btnSellConfirmButton = FindChildComponent<Button>("Panel/Buttons/m_btnSellButton/SellConfirmButton");
            m_btnBlocker.onClick.AddListener(OnClickBlockerBtn);
            m_btnUpgradeButton.onClick.AddListener(OnClickUpgradeButtonBtn);
            m_btnSellButton.onClick.AddListener(OnClickSellButtonBtn);
            m_btnSellConfirmButton.onClick.AddListener(OnClickSellConfirmButtonBtn);
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            foreach (object obj in base._userDatas)
            {
                if (obj is Tower)
                {
                    m_tower = obj as Tower;
                }
            }
            
            // TODO: 暂时强制开启炮塔升级
            m_btnUpgradeButton.GetComponent<Button>().interactable = true;
            
            click = true;
            
            if (m_tower == null)
            {
                Log.Error("Open UITowerConrollerForm Param inbaild");
                return;
            }
            
            m_textName.text = m_tower.Name;
            m_textDescription.text = m_tower.Des;
            m_textDps.text = m_tower.DPS.ToString();
            m_textUpgradeDescriptionText.text = m_tower.UpgradeDes;
        }

        protected override void OnRefresh()
        {
            base.OnRefresh();
        }

        protected override void OnUpdate()
        {
            base.OnUpdate();
        }

        #endregion

        #region 事件
        private void OnClickBlockerBtn()
        {
            Close();
        }
        
        private void OnClickUpgradeButtonBtn()
        {
            if (m_tower == null || !click)
                return;
            
            DataTowerManager.Instance.UpgradeTower(m_tower.SerialId);
            Close();
        }
        
        private void OnClickSellButtonBtn()
        {
            if (m_tower == null)
                return;
            
            click = true;
        }

        private void OnClickSellConfirmButtonBtn()
        {
            if (m_tower == null)
                return;
            
            // TODO: 需要返还能量
            DataTowerManager.Instance.SellTower(m_tower.SerialId);
            Close();
        }
        #endregion


        protected override void Hide()
        {
            base.Hide();
        }

        protected override void Close()
        {
            base.Close();

            m_tower = null;
            m_btnSellConfirmButton.gameObject.SetActive(false);
            GameModule.UI.HideUI<UITowerControllerForm>();
        }
    }
}
