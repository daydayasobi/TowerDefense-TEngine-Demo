using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class CommonLauncher : Launcher
    {
        public ParticleSystem fireParticleSystem;

        public override void Launch(EntityTargetableLogic target, AttackerDataBase attackerData, ProjectileDataBase projectileData, Vector3 origin, Transform firingPoint)
        {
            // GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
            //     attackerData.ProjectileEntityId,
            //     TypeUtility.GetEntityType(attackerData.ProjectileType),
            //     null,
            //     EntityDataProjectile.Create(target, projectileData, origin, firingPoint, firingPoint.position, firingPoint.rotation)));

            PlayParticles(fireParticleSystem, firingPoint.position, target.transform.position);
        }
    }
}
