using System.Collections;
using System.Collections.Generic;
using GameConfig;
using Unity.Properties;
using UnityEngine;

namespace GameLogic
{
    public class SuperTowerLauncher : Launcher
    {
        public ParticleSystem fireParticleSystem;

        // public override void Launch(List<EntityTargetableLogic> targets, AttackerDataBase attackerData, ProjectileData projectileData, Vector3 origin, Transform[] firingPoints)
        // {
        //     EntityTargetable target = targets[UnityEngine.Random.Range(0, targets.Count)];
        //     Transform firingPoint = GetRandomTransform(firingPoints);
        //     Launch(target, attackerData, projectileData, origin, firingPoint);
        // }
        //
        // public override void Launch(EntityTargetableLogic target, AttackerDataBase attackerData, ProjectileData projectileData, Vector3 origin, Transform firingPoint)
        // {
        //     GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
        //         attackerData.ProjectileEntityId,
        //         TypeUtility.GetEntityType(attackerData.ProjectileType),
        //         null,
        //         EntityDataProjectile.Create(target, projectileData, origin, firingPoint, firingPoint.position, firingPoint.rotation)));
        //
        //     PlayParticles(fireParticleSystem, firingPoint.position, target.transform.position);
        // }
        public override void Launch(EntityTargetableLogic target, AttackerDataBase attackerData, ProjectileDataBase projectileData, Vector3 origin, Transform firingPoint)
        {
            
        }
    }
}
