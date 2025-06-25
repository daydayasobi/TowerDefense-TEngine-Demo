using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    [Serializable]
    public class EntityDataRadiusVisualiser : EntityData
    {
        public float Radius
        {
            get;
            private set;
        }

        public EntityDataRadiusVisualiser() : base()
        {
            Radius = 0f;
        }

        public static EntityDataRadiusVisualiser Create(float radius, object userData = null)
        {
            EntityDataRadiusVisualiser entityData = MemoryPool.Acquire<EntityDataRadiusVisualiser>();
            // EntityDataRadiusVisualiser entityData = PoolManager.Instance.GetObject<EntityDataRadiusVisualiser>();
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
