using System;
using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;
using UnityEngine.UI;

namespace GameLogic
{
    /// <summary>
    /// 防御塔建造按钮控件
    /// 处理建造按钮的显示逻辑与交互行为
    /// </summary>
    [Window(UILayer.UI)]
    public class TowerBuildButton : UIWidget
    {
        #region 脚本工具生成的代码
        private Button m_btnButton;
        private Text m_textText;
        protected override void ScriptGenerator()
        {
            m_btnButton = FindChildComponent<Button>("m_btnButton");
            m_textText = FindChildComponent<Text>("m_textText");
            m_btnButton.onClick.AddListener(OnClickButtonBtn);
        }
        #endregion

        #region 事件
        private void OnClickButtonBtn()
        {
            // 触发回调前进行空引用检查
            if (onClick != null && towerData != null)
            {
                onClick(towerData);
            }
        }
        #endregion
        private Image Icon;
        protected override void OnCreate()
        {
            base.OnCreate();
            Icon = FindChildComponent<Image>("m_btnButton");
        }
        #region 配置参数
        [Header("颜色状态配置")]
        public Color energyDefaultColor;    // 能量充足时图标颜色
        public Color energyInvalidColor;    // 能量不足时警告颜色

        [Header("图标资源")]
        public Sprite[] iconList;           // 预加载的防御塔图标数组
        #endregion

        #region 运行时数据
        private TowerData towerData;        // 绑定的防御塔配置数据
        private TowerLevelData towerLevelData;  // 当前防御塔等级数据（0级为建造数据）
        private Action<TowerData> onClick;  // 点击回调委托
        #endregion

        /// <summary>
        /// 初始化建造按钮数据
        /// </summary>
        /// <param name="towerData">防御塔配置数据</param>
        /// <param name="onClick">点击回调函数</param>
        public void SetTowerBuildButton(TowerData towerData, Action<TowerData> onClick)
        {
            if (towerData == null) return;

            // 绑定基础数据
            this.towerData = towerData;
            this.towerLevelData = TowerLevelDataManger.Instance.GetItemConfig(towerData.Levels[0]);
            this.onClick = onClick;

            // 初始化UI显示
            UpdateEnergyDisplay();
            UpdateTowerIcon();

            UpdateEnergyState(PlayerData.Energy);
        }

        /// <summary>
        /// 更新能量显示状态
        /// </summary>
        /// <param name="ownEnergy">当前玩家能量值</param>
        private void UpdateEnergyState(float ownEnergy)
        {
            if (towerLevelData == null) return;

            // 计算能量是否足够
            bool canBuild = ownEnergy >= towerLevelData.BuildEnergy;



        }

        /// <summary>
        /// 建造按钮点击事件处理
        /// </summary>
        public void OnBuildButtonClick()
        {
            // 安全检查：确保回调和数据有效
            if (onClick != null && towerData != null)
            {
            }
        }

        /// <summary>
        /// 玩家能量变更事件响应
        /// </summary>
        /// <param name="ownEnergy">更新后的能量值</param>
        public void OnPlayerEnergyChange(float ownEnergy)
        {
            UpdateEnergyState(ownEnergy);
        }

        #region UI更新辅助方法
        /// <summary>
        /// 更新能量数值显示
        /// </summary>
        private void UpdateEnergyDisplay()
        {
            if (towerLevelData != null && m_textText != null)
            {
                m_textText.text = towerLevelData.BuildEnergy.ToString();
            }
        }

        /// <summary>
        /// 更新防御塔图标显示
        /// 注意：当前使用线性查找，建议优化为字典查询
        /// </summary>
        private void UpdateTowerIcon()
        {
            Icon.sprite = GameModule.Resource.LoadAsset<Sprite>(towerData.Icon);
        }
        #endregion
    }
}
