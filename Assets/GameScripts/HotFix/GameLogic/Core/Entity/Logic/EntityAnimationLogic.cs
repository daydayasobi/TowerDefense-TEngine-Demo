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

        protected EntityFollowerData EntityFollowerDatafollower;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            anim = GetComponentInChildren<Animation>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            EntityFollowerDatafollower = userData as EntityFollowerData;
            if (EntityFollowerDatafollower == null)
            {
                Log.Error("EntityParticle '{0}' entity data invaild.");
                return;
            }

            transform.localScale = EntityFollowerDatafollower.Scale;

            anim.Play();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (EntityFollowerDatafollower.Follow != null)
            {
                transform.position = EntityFollowerDatafollower.Follow.position + EntityFollowerDatafollower.Offset;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            EntityFollowerDatafollower = null;

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
