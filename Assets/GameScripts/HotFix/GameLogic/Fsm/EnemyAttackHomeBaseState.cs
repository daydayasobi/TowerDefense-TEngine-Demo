using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EnemyAttackHomeBaseState : FsmState<EntityEnemyLogic>, IMemory
    {
        private EntityEnemyLogic owner;
        private bool attacked = false;
        private float attackTimer = 0;

        protected override void OnInit(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            attackTimer = 0;
            attacked = false;
            owner = procedureOwner.Owner; // 获取状态拥有者
            // 如果目标玩家存在，则让目标玩家进入充电状态
            if (owner.TargetPlayer != null)
                owner.TargetPlayer.Charge();
        }

        protected override void OnUpdate(IFsm<EntityEnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsPause)
                return;

            if (owner.TargetPlayer != null)
            {
                if (owner.TargetPlayer != null && !attacked)
                {
                    attackTimer += elapseSeconds;
                    if (attackTimer > 1)
                    {
                        owner.TargetPlayer.Damage(owner.EntityDataEnemy.EnemyData.Damage);
                        attacked = true;
                        owner.AfterAttack();
                    }
                }
            }
        }

        protected override void OnLeave(IFsm<EntityEnemyLogic> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }


        protected override void OnDestroy(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        public static EnemyAttackHomeBaseState Create()
        {
            EnemyAttackHomeBaseState state = PoolReference.Acquire<EnemyAttackHomeBaseState>();
            return state;
        }

        public void Clear()
        {
            owner = null;
            attacked = false;
            attackTimer = 0;
        }
    }
}