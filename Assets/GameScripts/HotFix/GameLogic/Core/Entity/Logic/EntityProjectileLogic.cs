using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityProjectileLogic : EntityLogic, IPause
    {
        [Range(0, 1)]
        public float chanceToSpawnCollisionPrefab = 1.0f;

        public int collisionParticlesEntityId;

        protected EntityProjectileData entityProjectileData;

        protected bool pause;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            entityProjectileData = userData as EntityProjectileData;

            if (entityProjectileData == null)
            {
                Log.Error("Entity EntityProjectile '{0}' entity data invaild.");
                return;
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            entityProjectileData = null;
        }

        protected void SpawnCollisionParticles()
        {
            if (collisionParticlesEntityId <= 0 || UnityEngine.Random.value > chanceToSpawnCollisionPrefab)
            {
                return;
            }

            if (!entityProjectileData.EntityTargetableLogic.IsDead)
            {
                Vector3 pos = entityProjectileData.EntityTargetableLogic.transform.position + entityProjectileData.EntityTargetableLogic.ApplyEffectOffset;
                int serialId = GameModule.Entity.GenerateSerialId();
                ShowEntityEventData eventData = PoolReference.Acquire<ShowEntityEventData>();
                eventData.EntityId = collisionParticlesEntityId;
                eventData.SerialId = serialId;
                eventData.LogicType = typeof(EntityParticleAutoHideLogic);
                eventData.UserData = EntityData.Create(pos, transform.rotation, serialId);
                GameEvent.Send(LevelEvent.OnShowEntityInLevel, eventData);
            }
        }

        public virtual void Pause()
        {
            pause = true;
        }

        public virtual void Resume()
        {
            pause = false;
        }
    }
}