using UnityEngine;
using UnityEngine.UI;
using TEngine;
using GameConfig;
using System.Collections.Generic;
using UnityEditor;
using System;
using TEngine.Localization;
namespace GameLogic
{
    [Window(UILayer.UI)]
    public class UITowerListForm: UIWindow
    {
        #region 脚本工具生成的代码
        private Text m_textName;
        private Text m_textDps;
        private Text m_textDescription;
        private GameObject m_goSidebar;
        protected override void ScriptGenerator()
        {
            m_textName = FindChildComponent<Text>("BuildInfoMask/BuildInfo/m_textName");
            m_textDps = FindChildComponent<Text>("BuildInfoMask/BuildInfo/m_textDps");
            m_textDescription = FindChildComponent<Text>("BuildInfoMask/BuildInfo/m_textDescription ");
            m_goSidebar = FindChild("m_goSidebar").gameObject;
        }
        #endregion

        #region 事件
        #endregion
        private Dictionary<int, GameObject> dicTowerId2Button;
        private Dictionary<int, int> dicSerialId2TowerId;

        private LevelData currentLevelData;

        private bool showBuildInfo = false;

        public UITowerListForm()
        {
            dicTowerId2Button = new Dictionary<int, GameObject>();
            dicSerialId2TowerId = new Dictionary<int, int>();
        }

        protected override void OnCreate()
        {
            base.OnCreate();

            currentLevelData = LevelDataManger.Instance.CurrentLevel;

            ShowTowerBuildButtons();
            showBuildInfo = false;

        }

        protected override void OnUpdate()
        {
            base.OnUpdate();

            float targetPosY = showBuildInfo ? 0 : -200;


        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            m_textName.text = string.Empty;
            m_textDescription.text = string.Empty;
        }

        private void ShowTowerBuildButtons()
        {
            dicTowerId2Button.Clear();
            dicSerialId2TowerId.Clear();

            List<int> allowTowers = currentLevelData.AllowTowers;
            foreach (var towers in allowTowers)
            {
                TowerBuildButton item = CreateWidgetByPath<TowerBuildButton>(this.transform, "TowerBuildButton", true);
            
                item.transform.SetParent(m_goSidebar.transform, false);
                item.transform.localScale = Vector3.one;
                item.transform.eulerAngles = Vector3.zero;
                TowerData towerData = TowerDataManger.Instance.GetItemConfig(towers);
                item.SetTowerBuildButton(towerData, ShowBuildInfo);
            }
        }
        public void ShowBuildInfo(TowerData towerData)
        {
            if (towerData == null)
                return;

            TowerLevelData towerLevelData = TowerLevelDataManger.Instance.GetItemConfig(towerData.Levels[0]);
            if (towerLevelData == null)
                return;
            ProjectileData projectileData = ProjectileDataManger.Instance.GetItemConfig(towerLevelData.ProjectileData);
            m_textName.text = towerData.NameId;
            float Dps = (projectileData.Damage + projectileData.SplahDamage) * towerLevelData.FireRate;
            m_textDps.text = Dps.ToString();
            m_textDescription.text = towerLevelData.UpgradeDesid;
            
            GameEvent.Send(LevelEvent.OnShowPreviewTower, towerData);
            showBuildInfo = true;
        }

        private void HideBuildInfo()
        {
            showBuildInfo = false;
        }

        private void OnHidePreviewTower()
        {


            HideBuildInfo();
        }
    }
}
