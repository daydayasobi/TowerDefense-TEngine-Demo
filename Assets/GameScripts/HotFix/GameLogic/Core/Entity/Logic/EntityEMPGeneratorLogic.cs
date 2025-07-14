using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityEMPGeneratorLogic : EntityTowerBaseLogic
    {
        private Targetter targetter;
        private List<EntityEnemyLogic> slowList;

        private int? soundSerialId = null;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);

            targetter = transform.Find("Targetter").GetComponent<Targetter>();

            slowList = new List<EntityEnemyLogic>();

            targetter.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            targetter.OnShow(userData);

            // soundSerialId = GameEntry.Sound.PlaySound(EnumSound.TDEMPIdle, Entity);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (pause)
                return;

            targetter.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            RemoveAllTarget();

            base.OnHide(isShutdown, userData);

            targetter.OnHide(isShutdown, userData);
            targetter.targetEntersRange -= OnTargetEntersRange;
            targetter.targetExitsRange -= OnTargetExitsRange;

            if (soundSerialId != null)
            {
                // TODO: audio
                // GameEntry.Sound.StopSound((int)soundSerialId);
                soundSerialId = null;
            }
        }

        protected override void OnShowTowerLevelSuccess(Entity entity)
        {
            base.OnShowTowerLevelSuccess(entity);

            EntityTowerLevelLogic entityTowerLevel = entity.Logic as EntityTowerLevelLogic;
            targetter.SetAlignment(Alignment);
            targetter.SetTurret(entityTowerLevel.turret);
            targetter.SetSearchRange(entityTowerData.Tower.Range);
            targetter.ResetTargetter();

            targetter.targetEntersRange += OnTargetEntersRange;
            targetter.targetExitsRange += OnTargetExitsRange;

            foreach (var item in slowList)
            {
                item.ApplySlow(entityTowerData.Tower.SerialId, entityTowerData.Tower.SpeedDownRate);
            }
        }

        private void OnTargetEntersRange(EntityTargetableLogic target)
        {
            EntityEnemyLogic enemy = target as EntityEnemyLogic;
            if (enemy == null)
                return;
            enemy.ApplySlow(entityTowerData.Tower.SerialId, entityTowerData.Tower.SpeedDownRate);
            slowList.Add(enemy);
            enemy.OnDead += RemoveSlowTarget;
            enemy.OnHidden += RemoveSlowTarget;
        }

        private void OnTargetExitsRange(EntityTargetableLogic enmey)
        {
            RemoveSlowTarget(enmey);
        }

        private void RemoveSlowTarget(EntityTargetableLogic target)
        {
            EntityEnemyLogic enemy = target as EntityEnemyLogic;
            if (enemy == null)
                return;

            enemy.RemoveSlow(entityTowerData.Tower.SerialId);

            enemy.OnDead -= RemoveSlowTarget;
            enemy.OnHidden -= RemoveSlowTarget;

            slowList.Remove(enemy);
        }

        private void RemoveAllTarget()
        {
            foreach (var item in slowList)
            {
                item.RemoveSlow(entityTowerData.Tower.SerialId);

                item.OnDead -= RemoveSlowTarget;
                item.OnHidden -= RemoveSlowTarget;
            }

            slowList.Clear();
        }
    }
}
