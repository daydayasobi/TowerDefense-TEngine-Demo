using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    [Serializable]
    public class EntityDataTower : EntityData
    {
        public Tower Tower { get; private set; }

        public EntityDataTower() : base()
        {
            Tower = null;
        }

        public static EntityDataTower Create(Tower tower, object userData = null)
        {
            EntityDataTower entityData = PoolReference.Acquire<EntityDataTower>();
            entityData.Tower = tower;
            return entityData;
        }

        public static EntityDataTower Create(Tower tower, Vector3 position, Quaternion rotation, Transform parent, object userData = null)
        {
            EntityDataTower entityData = PoolReference.Acquire<EntityDataTower>();
            entityData.Tower = tower;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.m_Parent = parent;
            return entityData;
        }

        public static EntityDataTower Create(Tower tower, Vector3 position, Quaternion rotation, Transform parent, int serialId, object userData = null)
        {
            EntityDataTower entityData = PoolReference.Acquire<EntityDataTower>();
            entityData.Tower = tower;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.m_Parent = parent;
            entityData.m_SerialId = serialId;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            Tower = null;
        }
    }
}