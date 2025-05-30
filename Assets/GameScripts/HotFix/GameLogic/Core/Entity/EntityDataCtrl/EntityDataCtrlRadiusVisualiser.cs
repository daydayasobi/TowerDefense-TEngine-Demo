using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class EntityDataCtrlRadiusVisualiser : EntityData
    {
        public float Radius
        {
            get;
            private set;
        }

        public EntityDataCtrlRadiusVisualiser() : base()
        {
            Radius = 0f;
        }

        public static EntityDataCtrlRadiusVisualiser Create(float radius, object userData = null)
        {
            EntityDataCtrlRadiusVisualiser entityData = PoolReference.Acquire<EntityDataCtrlRadiusVisualiser>();
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
