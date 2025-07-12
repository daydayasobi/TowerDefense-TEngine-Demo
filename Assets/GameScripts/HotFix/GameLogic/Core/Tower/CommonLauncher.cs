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
            ShowEntityEventData eventData = PoolReference.Acquire<ShowEntityEventData>();
            eventData.EntityId = attackerData.ProjectileEntityId;
            eventData.SerialId = serialId;
            eventData.LogicType = EntityDataControl.GetProjectileLogicType(attackerData.ProjectileType);
            eventData.UserData = EntityProjectileData.Create(target, projectileData, origin, firingPoint, firingPoint.position, firingPoint.rotation, serialId);
            GameEvent.Send(LevelEvent.OnShowEntityInLevel, eventData);
            Log.Debug("CommonLauncher ProjectileType: {0}, ", attackerData.ProjectileType);

            PlayParticles(fireParticleSystem, firingPoint.position, target.transform.position);
        }
    }
}