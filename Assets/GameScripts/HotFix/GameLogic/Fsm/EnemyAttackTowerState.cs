using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EnemyAttackTowerState : FsmState<EntityEnemyLogic>, IMemory
    {
        private EntityEnemyLogic owner;
        protected EntityTargetableLogic m_TargetTower;
        
        protected override void OnInit(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            owner = procedureOwner.Owner;
            owner.Agent.isStopped = true;
            owner.Attacker.enabled = false;
        }

        protected override void OnUpdate(IFsm<EntityEnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsPause)
                return;

            // TODO: 更新攻击器状态
            // owner.Attacker.OnUpdate(elapseSeconds, realElapseSeconds);

            if (!owner.isPathBlocked)
            {
                ChangeState<EnemyMoveState>(procedureOwner);
                return;
            }

            EntityTargetableLogic tower = owner.Targetter.GetTarget();
            if (tower != m_TargetTower)
            {
                // if the current target is to be replaced, unsubscribe from removed event
                if (m_TargetTower != null)
                {
                    m_TargetTower.OnHidden -= OnTargetTowerDestroyed;
                }

                // assign target, can be null
                m_TargetTower = tower;

                // if new target found subscribe to removed event
                if (m_TargetTower != null)
                {
                    m_TargetTower.OnHidden += OnTargetTowerDestroyed;
                }
            }
            if (m_TargetTower == null)
            {
                ChangeState<EnemyMoveState>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<EntityEnemyLogic> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            owner = null;
            m_TargetTower = null;
        }


        protected override void OnDestroy(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        private void OnTargetTowerDestroyed(EntityTargetableLogic target)
        {
            if (m_TargetTower == target)
            {
                m_TargetTower.OnHidden -= OnTargetTowerDestroyed;
                m_TargetTower = null;
            }
        }

        public static EnemyAttackTowerState Create()
        {
            EnemyAttackTowerState state = PoolReference.Acquire<EnemyAttackTowerState>();
            return state;
        }

        public void Clear()
        {
            owner = null;
            m_TargetTower = null;
        }
    }
}
