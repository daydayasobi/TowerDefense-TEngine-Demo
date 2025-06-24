using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic.View;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityControl : IMemory
    {
        // 实体字典
        public Dictionary<int, Dictionary<int, EntityBase>> EntityDic = new Dictionary<int, Dictionary<int, EntityBase>>();
        // 对象池列表
        public Dictionary<int, IObjectPool<EntityBase>> EntityPoolList = new Dictionary<int, IObjectPool<EntityBase>>();
        int EntityId = 0;
        
        public static EntityControl Create()
        {
            EntityControl entityLoader = MemoryPool.Acquire<EntityControl>();
            return entityLoader;
        }
        
        public void AddEntityTower(int entityTypeId, Vector3 position, Quaternion rotation)
        {
            // todo: 需要优化对象池的运用。
            var towerType = TowerDataManger.Instance.GetItemConfig(entityTypeId);
            
            EntityBase towerEntity = PoolManager.Instance.GetObject<EntityBase>();
            towerEntity.Clear();
            towerEntity= EntityBase.CreateTower(
                EntityId,
                towerType,
                position,
                rotation
            );
            towerEntity.EntityTypeId = entityTypeId;

        }

        
        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
