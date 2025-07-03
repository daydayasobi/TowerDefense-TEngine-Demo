using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    [Serializable]
    public class EntityTowerData : EntityData
    {
        public Tower Tower { get; private set; }

        public EntityTowerData() : base()
        {
            Tower = null;
        }

        public static EntityTowerData Create(Tower tower, object userData = null)
        {
            EntityTowerData entityTowerData = PoolReference.Acquire<EntityTowerData>();
            entityTowerData.Tower = tower;
            return entityTowerData;
        }

        public static EntityTowerData Create(Tower tower, Vector3 position, Quaternion rotation, Transform parent, object userData = null)
        {
            EntityTowerData entityTowerData = PoolReference.Acquire<EntityTowerData>();
            entityTowerData.Tower = tower;
            entityTowerData.Position = position;
            entityTowerData.Rotation = rotation;
            entityTowerData.m_Parent = parent;
            return entityTowerData;
        }

        public static EntityTowerData Create(Tower tower, Vector3 position, Quaternion rotation, Transform parent, int serialId, object userData = null)
        {
            EntityTowerData entityTowerData = PoolReference.Acquire<EntityTowerData>();
            entityTowerData.Tower = tower;
            entityTowerData.Position = position;
            entityTowerData.Rotation = rotation;
            entityTowerData.m_Parent = parent;
            entityTowerData.m_SerialId = serialId;
            return entityTowerData;
        }

        public override void Clear()
        {
            base.Clear();
            Tower = null;
        }
    }
}