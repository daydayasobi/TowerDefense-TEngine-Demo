using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class EntityTowerPreviewData : EntityData
    {
        public Tower Tower { get; private set; }

        public EntityTowerPreviewData() : base()
        {
            Tower = null;
        }

        public static EntityTowerPreviewData Create(Tower Tower, object userData = null)
        {
            EntityTowerPreviewData entityData = PoolReference.Acquire<EntityTowerPreviewData>();
            entityData.Tower = Tower;
            return entityData;
        }

        public static EntityTowerPreviewData Create(Tower tower, Transform parent, int serialId, object userData = null)
        {
            EntityTowerPreviewData entityData = PoolReference.Acquire<EntityTowerPreviewData>();
            entityData.Tower = tower;
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