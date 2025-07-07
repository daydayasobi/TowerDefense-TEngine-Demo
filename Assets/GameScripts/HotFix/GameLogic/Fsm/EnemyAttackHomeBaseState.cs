using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EnemyAttackHomeBaseState : FsmState<EntityEnemyLogic>
    {
        private EntityEnemyLogic owner;
        private bool attacked = false;
        private float attackTimer;

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


            if (owner.TargetPlayer != null && !attacked)
            {
                attackTimer += elapseSeconds; // 更新攻击计时器

                // 如果计时器超过1秒，则发起攻击
                if (attackTimer > 1)
                {
                    owner.TargetPlayer.Damage(owner.enemyData.Damage); // 对目标玩家造成伤害
                    attacked = true; // 标记已发起攻击
                    owner.AfterAttack();
                    owner.IsActivation = false;
                    ChangeState<EnemyStandbyState>(procedureOwner);
                }
            }
        }

        protected override void OnLeave(IFsm<EntityEnemy> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }


        protected override void OnDestroy(IFsm<EntityEnemy> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }


        public void Clear()
        {
            owner = null;
            attacked = false;
            attackTimer = 0;
        }
    }
}
