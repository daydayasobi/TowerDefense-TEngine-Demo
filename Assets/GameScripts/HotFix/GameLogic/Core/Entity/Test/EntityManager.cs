using System;
using System.Collections;
using System.Collections.Generic;
using GameLogic.Module;
using UnityEngine;

namespace GameLogic
{
    public class EntityManager : Singleton<EntityManager>
    {

        // public EntityBase ShowTowerEntity(int entityId, EntityDataTower entityDataTower)
        // {
        //     var data = TowerDataManger.Instance.GetItemConfig(entityId);
        //     int serialId = GenerateSerialId();
        //     GameLogic.Module.EntityTower entitybase = (GameLogic.Module.EntityTower)PoolReference.Acquire(typeof(EntityTower));
        //     entitybase.EntityId = entityId;
        //     GameObject Entity = PoolManager.Instance.GetGameObject(data.NameId);
        //     var entity = Entity.AddComponent<GameLogic.View.EntityTower>();
        //     entity.InitialPosition = entityDataTower.Position;
        //     entity.Id = entityId;
        //     entity.towerData = data;
        //     entity.OnInit();
        //
        //     return entitybase;
        // }

        public EntityTowerBase ShowTowerEntity(int entityId, object userData)
        {
            var data = TowerDataManger.Instance.GetItemConfig(entityId);
            // int serialId = GenerateSerialId();
            GameObject Entity = PoolManager.Instance.GetGameObject(data.NameId);
            EntityTowerBase entitybase = (EntityTowerBase)PoolReference.Acquire(typeof(EntityTowerBase));
            var entity = Entity.AddComponent(entitybase.GetType());
            entitybase.OnInit(userData);
            
            return entitybase;
        }
    }
}