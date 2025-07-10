using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class CommonLauncher : Launcher
    {
        public ParticleSystem fireParticleSystem;

        public override void Launch(EntityTargetableLogic target, AttackerDataBase attackerData, ProjectileDataBase projectileData, Vector3 origin, Transform firingPoint)
        {
            int serialId = GameModule.Entity.GenerateSerialId();

            // GameEvent.Send(LevelEvent.OnShowEntityInLevel,
            //     new EntityShowOptionsArgs(
            //         entityId,
            //         serialId,
            //         false,
            //         typeof(EntityProjectileHitscanLogic),
            //         null,
            //         EntityProjectileData.Create(target, projectileData, origin, firingPoint, firingPoint.position, firingPoint.rotation, serialId)
            //     ));
            // GameEvent.Send(LevelEvent.OnShowEntityInLevel,
            //     entityId,
            //     serialId,
            //     typeof(EntityProjectileHitscanLogic),
            //     EntityProjectileData.Create(target, projectileData, origin, firingPoint, firingPoint.position, firingPoint.rotation, serialId));
            
            var eventData = new ShowEntityEventData
            {
                EntityId = attackerData.ProjectileEntityId,
                SerialId = serialId,
                LogicType = typeof(EntityProjectileHitscanLogic),
                UserData = EntityProjectileData.Create(target, projectileData, origin, firingPoint, firingPoint.position, firingPoint.rotation, serialId)
            };
            GameEvent.Send(LevelEvent.OnShowEntityInLevel, eventData);

            PlayParticles(fireParticleSystem, firingPoint.position, target.transform.position);
        }
    }
}