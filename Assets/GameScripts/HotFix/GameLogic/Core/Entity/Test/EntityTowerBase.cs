using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityTowerBase : EntityLogic
    {
        protected EntityDataTower entityDataTower;
        protected Entity entityTowerLevel;
        protected EntityTowerLevel entityLogicTowerLevel;
        private EntityControl itemLoader;

        protected bool pause = false;

        public void OnInit(object userData)
        {
            if (userData.GetType() == typeof(EntityDataTower))
            {
                //初始化位置
                entityDataTower = (EntityDataTower)userData;
                transform.position = entityDataTower.Position;
                transform.rotation = entityDataTower.Rotation;
                transform.parent = entityDataTower.Parent;
                // 加载模型
                ShowTowerLevelEntity(entityDataTower.Tower.Level);
            }
            else
            {
                Log.Info("Invalid userData type in OnInit. Expected EntityDataTower.");
            }
        }

        public void OnShow()
        {
        }

        public void OnHide()
        {
        }

        private void Update()
        {
        }

        private void ShowTowerLevelEntity(int level)
        {
            if (entityTowerLevel != null)
                HideEntity(entityTowerLevel);

            int entityTypeId = entityDataTower.Tower.GetLevelId(level);
            // GetTowerLevelData
            //     
            // towerData.GetTowerLevelData(level)

            ShowEntity(entityTypeId, EntityData.Create(transform.position, transform.rotation));
        }

        public void ShowControlForm()
        {
            if (entityDataTower == null)
            {
                Log.Error("Entity tower data is null.");
                return;
            }

            GameModule.UI.ShowUI<UITowerControllerForm>(entityDataTower.Tower);
        }

        // private void OnUpgradeTower(object sender, GameEventArgs e)
        // {
        //     UpgradeTowerEventArgs ne = (UpgradeTowerEventArgs)e;
        //     if (ne == null)
        //         return;
        //
        //     if (ne.Tower.SerialId != entityDataTower.Tower.SerialId)
        //         return;
        //
        //     ShowTowerLevelEntity(ne.Tower.Level);
        // }

        private void ShowEntity(int entityId, object userData = null)
        {
            if (itemLoader == null)
            {
                itemLoader = EntityControl.Create();
            }

            itemLoader.ShowTowerLevelEntity(entityId, OnShowTowerLevelSuccess, userData);
        }

        private void HideEntity(Entity entity)
        {
        }

        private void OnShowTowerLevelSuccess(EntityLogic entity)
        {
            //
        }


        public void Pause()
        {
            pause = true;
        }

        public void Resume()
        {
            pause = false;
        }
    }
}