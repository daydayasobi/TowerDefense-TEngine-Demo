using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public abstract class EntityTowerBaseLogic : EntityTargetableLogic, IPause
    {
        protected EntityTowerData entityTowerData;
        protected Entity entityLevel;
        protected EntityTowerLevelLogic entityLevelLogic;

        protected bool pause = false;

        public override EnumAlignment Alignment
        {
            get { return EnumAlignment.Tower; }
        }

        protected override float MaxHP
        {
            get
            {
                if (entityTowerData == null)
                    return 0;
                else
                    return entityTowerData.Tower.MaxHP;
            }
        }

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (userData.GetType() == typeof(EntityTowerData))
            {
                //初始化位置
                entityTowerData = (EntityTowerData)userData;
                transform.position = entityTowerData.Position;
                transform.rotation = entityTowerData.Rotation;
                transform.parent = entityTowerData.Parent;
            }
            else
            {
                Log.Info("Invalid userData type in OnInit. Expected EntityDataTower.");
            }
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            if (entityTowerData == null)
            {
                Log.Error("Entity tower '{0}' tower data invaild.", Id);
                return;
            }

            hp = entityTowerData.Tower.MaxHP;

            ShowTowerLevelEntity(entityTowerData.Tower.Level);

            GameEvent.AddEventListener<Tower>(LevelEvent.OnUpgradeTower, OnUpgradeTower);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            entityTowerData = null;
            entityLevel = null;
            entityLevelLogic = null;
            GameEvent.RemoveEventListener<Tower>(LevelEvent.OnUpgradeTower, OnUpgradeTower);
        }

        private void ShowTowerLevelEntity(int level)
        {
            if (entityLevel != null)
            {
                HideEntity(entityLevel);
            }

            int serialId = GameModule.Entity.GenerateSerialId();
            EntityDataControl.Instance.ShowTowerLevelEntity(
                entityTowerData.Tower.GetLevelEntityId(level),
                serialId,
                OnShowTowerLevelSuccess,
                EntityData.Create(transform.position, transform.rotation, transform, serialId));
        }

        protected virtual void OnShowTowerLevelSuccess(Entity entity)
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
                entityLevelLogic = entityLogicTowerLevelLogic;
                this.Entity.OnAttachedId(entityLevelLogic.Entity.SerialId);
            }
            else
            {
                Log.Error("EntityTowerLevel logic is null or not of type EntityTowerLevel.");
            }

            Log.Debug("EntityTowerBaseLogic OnShowTowerLevelSuccess: {0} {1}", entity.Id, entity.SerialId);
        }

        public void ShowControlForm()
        {
            if (entityTowerData == null)
            {
                Log.Error("Entity tower data is null.");
                return;
            }

            GameModule.UI.ShowUI<UITowerControllerForm>(entityTowerData.Tower);
        }

        private void OnUpgradeTower(Tower tower)
        {
            if (tower == null)
                return;

            if (tower.SerialId != entityTowerData.Tower.SerialId)
                return;

            // entityDataTower.Tower = tower;
            ShowTowerLevelEntity(tower.Level);
        }

        private void HideEntity(Entity entity)
        {
            EntityDataControl.Instance.HideEntity(entity);
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