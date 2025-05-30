using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityRadiusVisualizer : EntityLogic
    {
        public Transform radiusVisualizer;
        public float radiusVisualizerHeight = 0.02f;
        public Color color;

        public Vector3 localEuler;

        private EntityDataCtrlRadiusVisualiser entityDataCtrlRadiusVisualiser;

        public override void OnShow(object userData)
        {
            base.OnShow(userData);

            radiusVisualizer.localEulerAngles = localEuler;

            entityDataCtrlRadiusVisualiser = userData as EntityDataCtrlRadiusVisualiser;
            if (entityDataCtrlRadiusVisualiser == null)
            {
                Log.Error("EntityDataRadiusVisualiser data is invalid.");
                return;
            }

            radiusVisualizer.localScale = Vector3.one * entityDataCtrlRadiusVisualiser.Radius * 2.0f;

            var visualizerRenderer = radiusVisualizer.GetComponent<Renderer>();
            if (visualizerRenderer != null)
            {
                visualizerRenderer.material.color = color;
            }
        }

        protected override void Update()
        {
            base.Update();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            if (entityDataCtrlRadiusVisualiser != null)
            {
                PoolReference.Release(entityDataCtrlRadiusVisualiser);
                entityDataCtrlRadiusVisualiser = null;
            }
            
            transform.localPosition = Vector3.zero;
            radiusVisualizer.localScale = Vector3.zero;
        }

        // protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        // {
        //     base.OnAttachTo(parentEntity, parentTransform, userData);
        //
        //     transform.localPosition = entityDataRadiusVisualiser.Position + new Vector3(0, radiusVisualizerHeight, 0);
        // }
    }
}