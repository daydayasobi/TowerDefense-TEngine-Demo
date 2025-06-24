using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.Data
{
    [Serializable]
    public class EntityRadiusVisualiserData : EntityData
    {
        public float Radius
        {
            get;
            private set;
        }

        public EntityRadiusVisualiserData() : base()
        {
            Radius = 0f;
        }

        public static EntityRadiusVisualiserData Create(float radius, object userData = null)
        {
            // EntityRadiusVisualiserData entityData = ReferencePool.Acquire<EntityRadiusVisualiserData>();
            EntityRadiusVisualiserData entityData = PoolManager.Instance.GetObject<EntityRadiusVisualiserData>();
            entityData.Radius = radius;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            Radius = 0f;
        }
    }
}
