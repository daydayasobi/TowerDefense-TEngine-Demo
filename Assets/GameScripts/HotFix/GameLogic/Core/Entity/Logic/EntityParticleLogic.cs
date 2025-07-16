using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
    public class EntityParticleLogic : EntityLogicWithData, IPause
    {
        protected ParticleSystem ps;

        protected bool pause = false;
        private float pauseTime;

        protected EntityFollowerData EntityFollowerData;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            ps = GetComponentInChildren<ParticleSystem>();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            EntityFollowerData = userData as EntityFollowerData;
            if (EntityFollowerData == null)
            {
                return;
            }

            // TODO: play Sound
            // GameEntry.Sound.PlaySound(entityDataFollower.ShowSound, Entity);
            // GameModule.Audio.Play(AudioType.Sound,)

            transform.localScale = EntityFollowerData.Scale;

            ps.Play(true);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            if (EntityFollowerData != null && EntityFollowerData.Follow != null)
            {
                transform.position = EntityFollowerData.Follow.position + EntityFollowerData.Offset;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            EntityFollowerData = null;

            transform.localScale = Vector3.one;

            pauseTime = 0;
            ps.Stop(true);
        }

        public void Pause()
        {
            pause = true;
            ps.Pause(true);
            pauseTime = ps.time;
        }

        public void Resume()
        {
            pause = false;
            ps.Play();
            ps.time = pauseTime;
        }
    }
}
