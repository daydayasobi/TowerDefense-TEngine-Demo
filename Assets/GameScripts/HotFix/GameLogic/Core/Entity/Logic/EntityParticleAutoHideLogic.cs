using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityParticleAutoHideLogic : EntityParticleLogic
    {
        private float hideTime = 0;
        private float timer = 0;


        public override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            hideTime = ps.main.duration;
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            timer += elapseSeconds;

            if (timer > hideTime)
            {
                GameEvent.Send(LevelEvent.OnHideEntityInLevel,Entity.SerialId);
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            timer = 0;
            hideTime = 0;
        }
    }
}
