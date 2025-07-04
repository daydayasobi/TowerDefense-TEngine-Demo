using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityRadiusLogic : EntityLogic
    {
        public Transform radiusVisualizer;
        public float radiusVisualizerHeight = 0.02f;
        public Color color;

        public Vector3 localEuler;

        protected EntityRadiusData entityData;
        protected Entity entity;
        protected EntityRadiusLogic entityLogic;


        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (userData.GetType() == typeof(EntityRadiusData))
            {
                entityData = (EntityRadiusData)userData;
                // transform.position = entityData.Position;
                // transform.rotation = entityData.Rotation;
                transform.parent = entityData.Parent;
                transform.localPosition = entityData.Position + new Vector3(0, radiusVisualizerHeight, 0);
                OnShow(userData);
            }
        }

        protected override void OnShow(object userData)
        {
            radiusVisualizer.localEulerAngles = localEuler;

            // entityData = userData as EntityRadiusData;
            // if (entityData == null)
            // {
            //     Log.Error("EntityDataRadiusVisualiser data is invalid.");
            //     return;
            // }

            radiusVisualizer.localScale = Vector3.one * entityData.Radius * 2.0f;
            //
            var visualizerRenderer = radiusVisualizer.GetComponent<Renderer>();
            if (visualizerRenderer != null)
            {
                visualizerRenderer.material.color = color;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            entityData = null;
            transform.localPosition = Vector3.zero;
            radiusVisualizer.localScale = Vector3.zero;
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
            transform.SetParent(parentTransform);
            OnShow(userData);
            transform.localPosition = entityData.Position + new Vector3(0, radiusVisualizerHeight, 0);
        }
    }
}
