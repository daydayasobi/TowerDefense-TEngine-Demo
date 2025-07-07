using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EnemyStandbyState : FsmState<EntityEnemyLogic>
    {
        private EntityEnemyLogic owner;
        protected override void OnInit(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            owner = procedureOwner.Owner; // 获取状态拥有者
        }

        protected override void OnUpdate(IFsm<EntityEnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsActivation)
                ChangeState<EnemyMoveState>(procedureOwner);
        }

        protected override void OnLeave(IFsm<EntityEnemyLogic> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }


        protected override void OnDestroy(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}
