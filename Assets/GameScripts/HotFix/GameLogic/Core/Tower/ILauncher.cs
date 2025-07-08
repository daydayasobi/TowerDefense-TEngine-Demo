using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public interface ILauncher
    {
        void Launch(EntityTargetableLogic target, AttackerDataBase attackerData, ProjectileDataBase projectileData, Vector3 origin, Transform firingPoint);

        void Launch(EntityTargetableLogic target, AttackerDataBase attackerData, ProjectileDataBase projectileData, Vector3 origin, Transform[] firingPoints);

        void Launch(List<EntityTargetableLogic> targets, AttackerDataBase attackerData, ProjectileDataBase projectileData, Vector3 origin, Transform[] firingPoints);
    }
}
