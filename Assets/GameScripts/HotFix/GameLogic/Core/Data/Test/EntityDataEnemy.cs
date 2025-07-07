using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class EntityDataEnemy : EntityData
    {
        public EnemyDataBase EnemyData
        {
            get;
            private set;
        }

        public LevelPath LevelPath
        {
            get;
            private set;
        }

        public EntityDataEnemy() : base()
        {
            EnemyData = null;
        }

        public static EntityDataEnemy Create(EnemyDataBase enemyData, LevelPath levelPath, object userData = null)
        {
            EntityDataEnemy entityData = PoolReference.Acquire<EntityDataEnemy>();
            entityData.EnemyData = enemyData;
            entityData.LevelPath = levelPath;
            return entityData;
        }

        public static EntityDataEnemy Create(EnemyDataBase enemyData, LevelPath levelPath, Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityDataEnemy entityData = PoolReference.Acquire<EntityDataEnemy>();
            entityData.EnemyData = enemyData;
            entityData.LevelPath = levelPath;
            entityData.Position = position;
            entityData.Rotation = rotation;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            EnemyData = null;
        }
    }
}
