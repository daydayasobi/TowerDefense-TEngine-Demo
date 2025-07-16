using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class EntityEnemyData : EntityData
    {
        public EnemyDataBase EnemyData { get; private set; }

        public LevelPath LevelPath { get; private set; }

        public EntityEnemyData() : base()
        {
            EnemyData = null;
        }

        public static EntityEnemyData Create(EnemyDataBase enemyData, LevelPath levelPath, object userData = null)
        {
            EntityEnemyData entityEnemyData = PoolReference.Acquire<EntityEnemyData>();
            entityEnemyData.EnemyData = enemyData;
            entityEnemyData.LevelPath = levelPath;
            return entityEnemyData;
        }

        public static EntityEnemyData Create(EnemyDataBase enemyData, LevelPath levelPath, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityEnemyData entityEnemyData = PoolReference.Acquire<EntityEnemyData>();
            entityEnemyData.EnemyData = enemyData;
            entityEnemyData.LevelPath = levelPath;
            entityEnemyData.Position = position;
            entityEnemyData.Rotation = rotation;
            return entityEnemyData;
        }

        public static EntityEnemyData Create(int serialId, EnemyDataBase enemyData, LevelPath levelPath, Vector3 position, Quaternion rotation, Transform transform,
            object userData = null)
        {
            EntityEnemyData entityEnemyData = PoolReference.Acquire<EntityEnemyData>();
            entityEnemyData.m_SerialId = serialId;
            entityEnemyData.EnemyData = enemyData;
            entityEnemyData.LevelPath = levelPath;
            entityEnemyData.Position = position;
            entityEnemyData.Rotation = rotation;
            entityEnemyData.Parent = transform;
            return entityEnemyData;
        }

        public override void Clear()
        {
            base.Clear();
            EnemyData = null;
        }
    }
}