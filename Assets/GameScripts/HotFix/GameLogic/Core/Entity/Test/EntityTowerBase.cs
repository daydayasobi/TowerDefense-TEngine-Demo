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
        
        protected bool pause = false;
        
        public void OnInit(EntityDataTower _entityDataTower)
        {
            // if (userData is EntityDataTower entityDataTower)
            // {
            //     // 初始化位置
            //     transform.position = entityDataTower.Position;
            //     transform.rotation = entityDataTower.Rotation;
            //     transform.parent = entityDataTower.Parent;
            //
            //     // 加载模型
            //     // ShowTowerLevelEntity(entityDataTower.Tower.Level);
            // }
            // else
            // {
            //     Debug.LogError("Invalid userData type in OnInit. Expected EntityDataTower.");
            // }
            
            //初始化位置
            entityDataTower = _entityDataTower;
            // transform.position = entityDataTower.Position;
            // transform.rotation = entityDataTower.Rotation;
            // transform.parent = entityDataTower.Parent;

            // ShowTowerLevelEntity(entityDataTower.Tower.Level);

            //加载模型
        }

        public void OnShow()
        {
            
        }

        public void OnHide()
        {
            
        }
        
        public void ShowControlForm()
        {
            // GameEntry.UI.OpenUIForm(EnumUIForm.UITowerControllerForm, entityDataTower.Tower);
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