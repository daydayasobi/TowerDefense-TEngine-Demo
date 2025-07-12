using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityTowerAttackerLogic : EntityTowerBaseLogic
    {
        private Targetter targetter;
        private Attacker attacker;
        
        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            Log.Debug("EntityTowerAttackerLogic OnInit: {0} {1}", Entity.Id, Entity.SerialId);
            
            targetter = transform.Find("Targetter").GetComponent<Targetter>();
            attacker = transform.Find("Attack").GetComponent<Attacker>();

            targetter.OnInit(userData);
            attacker.OnInit(userData);
        }
        
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            targetter.OnUpdate(elapseSeconds, realElapseSeconds);
            attacker.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            Log.Debug("EntityTowerAttackerLogic OnShow: {0} {1}", Entity.Id, Entity.SerialId);
            
            AttackerDataBase attackerData = AttackerDataBase.Create(entityTowerData.Tower.Range,
                entityTowerData.Tower.FireRate,
                entityTowerData.Tower.IsMultiAttack,
                entityTowerData.Tower.ProjectileType,
                entityTowerData.Tower.ProjectileEntityId
                );
            attacker.SetData(attackerData, entityTowerData.Tower.ProjectileData);

            targetter.OnShow(userData);
            attacker.OnShow(userData);
            attacker.SetOwnerEntity(Entity);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            Log.Debug("EntityTowerAttackerLogic OnHide: {0} {1}", Entity.Id, Entity.SerialId);
            targetter.OnHide(isShutdown, userData);
            attacker.OnHide(isShutdown, userData);
            attacker.EmptyOwnerEntity();
        }

        protected override void OnShowTowerLevelSuccess(Entity entity)
        {
            base.OnShowTowerLevelSuccess(entity);
            Log.Debug("EntityTowerAttackerLogic OnShowTowerLevelSuccess: {0} {1}", entity.Id, entity.SerialId);
            EntityTowerLevelLogic entityTowerLevel = entity.Logic as EntityTowerLevelLogic;
            targetter.SetAlignment(Alignment);
            targetter.SetTurret(entityTowerLevel.turret);
            targetter.SetSearchRange(entityTowerData.Tower.Range);
            targetter.ResetTargetter();

            AttackerDataBase attackerData = AttackerDataBase.Create(entityTowerData.Tower.Range,
                entityTowerData.Tower.FireRate,
                entityTowerData.Tower.IsMultiAttack,
                entityTowerData.Tower.ProjectileType,
                entityTowerData.Tower.ProjectileEntityId
            );

            attacker.SetData(attackerData, entityTowerData.Tower.ProjectileData);
            attacker.SetTargetter(targetter);
            attacker.SetProjectilePoints(entityTowerLevel.projectilePoints);
            attacker.SetEpicenter(entityTowerLevel.epicenter);
            attacker.SetLaunch(entityTowerLevel.launcher);
            attacker.ResetAttack();
        }
    }
}