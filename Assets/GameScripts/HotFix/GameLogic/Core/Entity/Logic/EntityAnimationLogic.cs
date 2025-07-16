using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityAnimationLogic : EntityLogic, IPause
    {
        protected Animation anim;

        protected bool pause = false;

        protected EntityFollowerData entityFollowerData;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            anim = GetComponentInChildren<Animation>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityFollowerData = userData as EntityFollowerData;
            if (entityFollowerData == null)
            {
                Log.Error("EntityParticle '{0}' entity data invaild.");
                return;
            }

            transform.localScale = entityFollowerData.Scale;

            anim.Play();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (entityFollowerData.Follow != null)
            {
                transform.position = entityFollowerData.Follow.position + entityFollowerData.Offset;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityFollowerData = null;

            transform.localScale = Vector3.one;
            anim.Stop();
        }

        public void Pause()
        {
            pause = true;
            anim.Stop();
        }

        public void Resume()
        {
            pause = false;
            anim.Play();
        }
    }
}
