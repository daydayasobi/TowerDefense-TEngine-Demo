using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityControl
    {
        int EntityId = 0;
        
        public void AddEntityEnemy(int EntityTypeId, LevelManager levelManager)
        {
            var entityType = EnemyDataManger.Instance.GetItemConfig(EntityTypeId);
            
        }
        public void AddEntityTower(int entityTypeId, Vector3 position, Quaternion rotation)
        {
            // var towerType = TowerDataManger.Instance.GetItemConfig(entityTypeId);
            //
            // // 初始化对象池（如果不存在）
            // if (!EntityPoolList.ContainsKey(entityTypeId))
            // {
            //     EntityPoolList.Add(entityTypeId,
            //         GameModule.ObjectPool.CreateSingleSpawnObjectPool<EntityBase>(
            //             $"Tower Instance Pool ({entityTypeId})",
            //             5,  // 初始容量
            //             10, // 最大容量
            //             5,  // 过期数量
            //             1   // 优先级
            //         ));
            // }
            //
            // EntityBase towerEntity;
            //
            // // 尝试从对象池获取
            // if (EntityPoolList[entityTypeId].CanSpawn())
            // {
            //     towerEntity = EntityPoolList[entityTypeId].Spawn();
            // }
            // else // 创建新实例
            // {
            //     EntityId++;
            //     towerEntity = EntityBase.CreateTower(
            //         EntityId,
            //         towerType,
            //         position,
            //         rotation
            //     );
            //     EntityPoolList[entityTypeId].Register(towerEntity, true);
            // }
            //
            // // 初始化实体
            // towerEntity.EntityTypeId = entityTypeId;
            //
            // // 添加到实体字典
            // if (!EntityDic.ContainsKey(entityTypeId))
            // {
            //     EntityDic.Add(entityTypeId, new Dictionary<int, EntityBase>());
            // }
            // EntityDic[entityTypeId].Add(EntityId, towerEntity);

        }
        public static EntityControl Create()
        {
            // EntityControl entityLoader = MemoryPool.Acquire<EntityControl>();
            EntityControl entityLoader = null;
            return entityLoader;
        }

        public void HideEnemyEntity(int serialId)
        {
            // 遍历 EnemyDic 来查找对应的实体
            // foreach (var enemyType in EntityDic)
            // {
            //     if (enemyType.Value.TryGetValue(serialId, out EntityBase entity))
            //     {
            //         // 如果找到了实体，从 EnemyDic 中移除
            //         enemyType.Value.Remove(serialId);
            //
            //         // 将实体回收到对象池中
            //         if (EntityPoolList.TryGetValue(entity.EntityTypeId, out IObjectPool<EntityBase> pool))
            //         {
            //             pool.Unspawn(entity);
            //         }
            //         else
            //         {
            //             Debug.LogError($"Object pool for EntityTypeId {entity.EntityTypeId} not found.");
            //         }
            //
            //         return; // 找到并处理后退出方法
            //     }
            // }
            //
            // // 如果没有找到对应的实体，打印日志或抛出异常
            // Debug.LogError($"Entity with serialId {serialId} not found.");
        }
        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
