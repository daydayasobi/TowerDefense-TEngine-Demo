using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
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
            // EntityDataRadiusVisualiser entityData = ReferencePool.Acquire<EntityDataRadiusVisualiser>();
            EntityDataRadiusVisualiser entityData = new EntityDataRadiusVisualiser();
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
