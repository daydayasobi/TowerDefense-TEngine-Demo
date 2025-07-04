using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityTowerLogic : EntityLogic
    {
        protected EntityTowerData EntityTowerData;
        protected Entity entityLevel;
        protected EntityTowerLevelLogic EntityLevelLogic;

        protected bool pause = false;

        public void OnInit(object userData)
        {
            base.OnInit(userData);
            if (userData.GetType() == typeof(EntityTowerData))
            {
                //初始化位置
                EntityTowerData = (EntityTowerData)userData;
                transform.position = EntityTowerData.Position;
                transform.rotation = EntityTowerData.Rotation;
                transform.parent = EntityTowerData.Parent;
                // 加载模型
                ShowTowerLevelEntity(EntityTowerData.Tower.Level);
            }
            else
            {
                Log.Info("Invalid userData type in OnInit. Expected EntityDataTower.");
            }

            GameEvent.AddEventListener<Tower>(LevelEvent.OnUpgradeTower, OnUpgradeTower);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            EntityTowerData = null;
            entityLevel = null;
            EntityLevelLogic = null;
            GameEvent.RemoveEventListener<Tower>(LevelEvent.OnUpgradeTower, OnUpgradeTower);
        }

        private void ShowTowerLevelEntity(int level)
        {
            if (entityLevel != null)
            {
                HideEntity(entityLevel);
            }

            EntityModuleEx.Instance.ShowTowerLevelEntity(
                EntityTowerData.Tower.GetLevelEntityId(level), 
                OnShowTowerLevelSuccess,
                EntityData.Create(transform.position, transform.rotation, transform));
        }

        private void OnShowTowerLevelSuccess(Entity entity)
        {
            if (entity == null)
            {
                Log.Error("Failed to show tower level entity.");
                return;
            }

            entityLevel = entity;
            EntityTowerLevelLogic entityLogicTowerLevelLogic = entity.Logic as EntityTowerLevelLogic;
            if (entityLogicTowerLevelLogic != null)
            {
                EntityLevelLogic = entityLogicTowerLevelLogic;
                this.Entity.OnAttachedId(EntityLevelLogic.Entity.SerialId);
            }
            else
            {
                Log.Error("EntityTowerLevel logic is null or not of type EntityTowerLevel.");
            }
        }

        public void ShowControlForm()
        {
            if (EntityTowerData == null)
            {
                Log.Error("Entity tower data is null.");
                return;
            }

            GameModule.UI.ShowUI<UITowerControllerForm>(EntityTowerData.Tower);
        }

        private void OnUpgradeTower(Tower tower)
        {
            if (tower == null)
                return;

            if (tower.SerialId != EntityTowerData.Tower.SerialId)
                return;

            // entityDataTower.Tower = tower;
            ShowTowerLevelEntity(tower.Level);
        }

        private void HideEntity(Entity entity)
        {
            EntityModuleEx.Instance.HideEntity(entity);
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