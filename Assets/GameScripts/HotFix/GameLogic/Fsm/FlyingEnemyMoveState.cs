using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class FlyingEnemyMoveState : FsmState<EntityEnemyLogic>, IMemory
    {
        private IFsm<EntityEnemyLogic> m_procedureOwner;
        private EntityEnemyLogic owner;
        private int targetPathNodeIndex = 0;

        public FlyingEnemyMoveState()
        {

        }

        protected override void OnInit(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            owner = procedureOwner.Owner;
            owner.Agent.enabled = true;
            m_procedureOwner = procedureOwner;
            owner.Agent.SetDestination(owner.LevelPath.PathNodes[targetPathNodeIndex].position);
        }

        protected override void OnUpdate(IFsm<EntityEnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsPause)
                return;

            if (owner.TargetPlayer != null)
            {
                ChangeState<EnemyAttackHomeBaseState>(procedureOwner);
                return;
            }


            if (owner.LevelPath == null || owner.LevelPath.PathNodes == null || owner.LevelPath.PathNodes.Length == 0)
                return;


            if (owner.LevelPath.PathNodes.Length > targetPathNodeIndex && owner.isAtDestination)
            {
                if (owner.LevelPath.PathNodes.Length - 1 != targetPathNodeIndex)
                {
                    owner.Agent.SetDestination(owner.LevelPath.PathNodes[++targetPathNodeIndex].position);
                }
            }

            owner.Agent.speed = owner.EntityDataEnemy.EnemyData.Speed * owner.CurrentSlowRate;


            if (owner.isPathBlocked)
            {
                ChangeState<FlyingEnemyPushingThroughState>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<EntityEnemyLogic> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            procedureOwner.SetData<int>(Constant.ProcedureData.TargetPathNodeIndex, targetPathNodeIndex);
            owner = null;
        }


        protected override void OnDestroy(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }


        public static FlyingEnemyMoveState Create()
        {
            FlyingEnemyMoveState state = PoolReference.Acquire<FlyingEnemyMoveState>();
            return state;
        }

        public void Clear()
        {
            targetPathNodeIndex = 0;
            m_procedureOwner.RemoveData(Constant.ProcedureData.TargetPathNodeIndex);
            owner = null;
        }
    }
}
