using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityHPBarLogic : EntityLogic
    {
         public Transform healthBar;
        public Transform backgroundBar;
        private Transform cameraToFace;

        private EntityFollowerData _entityFollowerData;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            cameraToFace = Camera.main.transform;

            _entityFollowerData = userData as EntityFollowerData;
            if (_entityFollowerData == null)
            {
                Log.Error("EntityHPBar param invaild");
                return;
            }
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (cameraToFace != null)
            {
                Vector3 direction = cameraToFace.transform.forward;
                transform.forward = -direction;
            }

            if (_entityFollowerData != null && _entityFollowerData.Follow != null)
            {
                transform.position = _entityFollowerData.Follow.position + _entityFollowerData.Offset;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            cameraToFace = null;
            _entityFollowerData = null;
            UpdateHealth(1);
            SetVisible(false);
        }

        public void UpdateHealth(float normalizedHealth)
        {
            Vector3 scale = Vector3.one;

            if (healthBar != null)
            {
                scale.x = normalizedHealth;
                healthBar.transform.localScale = scale;
            }

            if (backgroundBar != null)
            {
                scale.x = 1 - normalizedHealth;
                backgroundBar.transform.localScale = scale;
            }

            SetVisible(normalizedHealth < 1.0f);
        }

        public void SetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }
    }
}
