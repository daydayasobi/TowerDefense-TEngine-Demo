using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityProjectileHitscanLogic : EntityProjectileLogic
    {
        public ParticleSystem projectileParticles;
        public float delayTime = 0f;
        private float timer;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            projectileParticles.transform.position = entityProjectileData.FiringPoint.position;
            projectileParticles.transform.LookAt(entityProjectileData.EntityTargetableLogic.transform.position);
            projectileParticles.Play();
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            timer += elapseSeconds;

            if (timer >= delayTime)
            {
                AttackTarget();
                GameEvent.Send(LevelEvent.OnHideEntityInLevel, Entity);
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            timer = 0;
        }

        private void SetProjectileParticles()
        {
        }

        private void AttackTarget()
        {
            SpawnCollisionParticles();

            if (!entityProjectileData.EntityTargetableLogic.IsDead)
                entityProjectileData.EntityTargetableLogic.Damage(entityProjectileData.ProjectileData.Damage);
        }
    }
}