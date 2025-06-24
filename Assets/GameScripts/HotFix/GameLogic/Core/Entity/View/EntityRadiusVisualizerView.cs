using System.Collections;
using System.Collections.Generic;
using GameLogic.Data;
using TEngine;
using UnityEngine;

namespace GameLogic.View
{
    public class EntityRadiusVisualizerView : MonoBehaviour
    {
        public Transform radiusVisualizer;
        public float radiusVisualizerHeight = 0.02f;
        public Color color;

        public Vector3 localEuler;

        private EntityRadiusVisualiserData entityRadiusVisualiserData;

        public void OnShow(object userData)
        {
            radiusVisualizer.localEulerAngles = localEuler;

            entityRadiusVisualiserData = userData as EntityRadiusVisualiserData;
            if (entityRadiusVisualiserData == null)
            {
                Log.Error("EntityDataRadiusVisualiser data is invalid.");
                return;
            }

            radiusVisualizer.localScale = Vector3.one * entityRadiusVisualiserData.Radius * 2.0f;

            var visualizerRenderer = radiusVisualizer.GetComponent<Renderer>();
            if (visualizerRenderer != null)
            {
                visualizerRenderer.material.color = color;
            }
        }

        public void OnHide()
        {
            entityRadiusVisualiserData = null;
            transform.localPosition = Vector3.zero;
            radiusVisualizer.localScale = Vector3.zero;
        }

        public void OnAttachTo(Transform parentTransform, object userData)
        {
            transform.SetParent(parentTransform);
            OnShow(userData);
            transform.localPosition = entityRadiusVisualiserData.Position + new Vector3(0, radiusVisualizerHeight, 0);
        }
    }
}
