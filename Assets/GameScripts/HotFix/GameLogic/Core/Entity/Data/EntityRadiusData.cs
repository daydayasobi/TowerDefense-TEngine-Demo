using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class EntityRadiusData : EntityData
    {
        public float Radius { get; private set; }

        public static EntityRadiusData Create(Transform parent, int serialId, float radius, object userData = null)
        {
            EntityRadiusData entityData = PoolReference.Acquire<EntityRadiusData>();
            entityData.m_Parent = parent;
            entityData.m_SerialId = serialId;
            entityData.Radius = radius;
            return entityData;
        }
    }
}