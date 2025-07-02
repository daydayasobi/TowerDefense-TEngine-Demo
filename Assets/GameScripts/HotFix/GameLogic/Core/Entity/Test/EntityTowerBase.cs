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
        protected Entity entityLevel;
        protected EntityTowerLevel entityLevelLogic;

        protected bool pause = false;

        public void OnInit(object userData)
        {
            base.OnInit(userData);
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
            
            GameEvent.AddEventListener<Tower>(LevelEvent.OnUpgradeTower, OnUpgradeTower);
        }

        public void SetSerialId(int serialId)
        {
            if (entityDataTower != null)
                entityDataTower.SerialId = serialId;
        }

        public void OnShow()
        {
        }

        protected internal override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown,userData);
            entityDataTower = null;
            entityLevel = null;
            entityLevelLogic = null;
            GameEvent.RemoveEventListener<Tower>(LevelEvent.OnUpgradeTower, OnUpgradeTower);
        }

        private void Update()
        {
        }

        private void ShowTowerLevelEntity(int level)
        {
            if (entityLevel != null)
                HideEntity(entityLevel);

            int entityTypeId = entityDataTower.Tower.GetLevelId(level);

            int serialId = EntityControl.Instance.GenerateSerialId();
            EntityControl.Instance.ShowTowerLevelEntity(entityTypeId, serialId, OnShowTowerLevelSuccess, EntityData.Create(transform.position, transform.rotation, transform));
        }

        private void OnShowTowerLevelSuccess(Entity entity)
        {
            EntityTowerLevel entityLogicTowerLevel = entity.Logic as EntityTowerLevel;
            if (entityLogicTowerLevel != null)
            {
                entityLevelLogic = entityLogicTowerLevel;
                this.Entity.OnAttachedId(entityLevelLogic.Entity.SerialId);
            }
            else
            {
                Log.Error("EntityTowerLevel logic is null or not of type EntityTowerLevel.");
            }
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

        private void OnUpgradeTower(Tower tower)
        {
            if (tower == null)
                return;
        
            if (tower.SerialId != entityDataTower.Tower.SerialId)
                return;

            // entityDataTower.Tower = tower;
        
            ShowTowerLevelEntity(tower.Level);
        }

        private void HideEntity(Entity entity)
        {
            Entity.OnHide(true, null);
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