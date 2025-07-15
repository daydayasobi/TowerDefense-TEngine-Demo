using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityHideSelfProjectileLogic : EntityProjectileLogic
    {
        public float time;

        private float timer;

        public float yDestroyPoint = -50;
        public float selfDestroyTime = 10;

        protected bool hide = false;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            hide = false;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            timer += elapseSeconds;

            if (transform.position.y < yDestroyPoint || timer > selfDestroyTime)
            {
                if (!hide)
                {
                    GameEvent.Send(LevelEvent.OnHideEntityInLevel, Entity);
                    hide = true;
                }
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            timer = 0;
            hide = false;
        }
    }
}