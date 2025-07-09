using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using UnityEngine.AI;

namespace GameLogic
{
    public class FlyingEnemyPushingThroughState : FsmState<EntityEnemyLogic>, IMemory
    {
        private static readonly float m_WaitTime = 0.5f;
        private float m_CurrentWaitTime;

        private EntityEnemyLogic owner;
        private int targetPathNodeIndex = 0;

        public FlyingEnemyPushingThroughState()
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
            owner.Agent.enabled = false;
            m_CurrentWaitTime = m_WaitTime;

            targetPathNodeIndex = procedureOwner.GetData<int>(Constant.ProcedureData.TargetPathNodeIndex);
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


            m_CurrentWaitTime -= elapseSeconds;

            // Move the agent, overriding its NavMeshAgent until it reaches its destination
            owner.transform.LookAt(owner.LevelPath.PathNodes[targetPathNodeIndex].position, Vector3.up);
            owner.transform.position += owner.transform.forward * owner.Agent.speed * elapseSeconds;
            if (m_CurrentWaitTime > 0)
            {
                return;
            }
            // Check if there is a navmesh under the agent, if not, then reset the timer
            NavMeshHit hit;
            if (!NavMesh.Raycast(owner.transform.position + Vector3.up, Vector3.down, out hit, owner.Agent.areaMask))
            {
                m_CurrentWaitTime = m_WaitTime;
            }
            else
            {
                ChangeState<FlyingEnemyMoveState>(procedureOwner);
            }
        }

        protected override void OnLeave(IFsm<EntityEnemyLogic> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            owner.Agent.enabled = true;
            owner = null;
        }


        protected override void OnDestroy(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }


        public static FlyingEnemyPushingThroughState Create()
        {
            FlyingEnemyPushingThroughState state = PoolReference.Acquire<FlyingEnemyPushingThroughState>();
            return state;
        }

        public void Clear()
        {
            m_CurrentWaitTime = 0;
            targetPathNodeIndex = 0;
            owner = null;
        }
    }
}
