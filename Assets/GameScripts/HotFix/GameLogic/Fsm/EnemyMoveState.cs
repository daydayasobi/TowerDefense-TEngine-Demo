using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EnemyMoveState : FsmState<EntityEnemyLogic>
    {
        // 当前状态的拥有者，即敌方单位对象
        private EntityEnemyLogic owner;
        // 目标路径节点的索引，用于记录当前移动到路径中的哪个节点
        private int targetPathNodeIndex = 0;
        public EnemyMoveState()
        {

        }

        protected override void OnInit(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnInit(procedureOwner);
        }

        protected override void OnEnter(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnEnter(procedureOwner);

            // 获取当前状态的拥有者
            owner = procedureOwner.Owner;
            // 启动敌方单位的导航代理，允许其移动
            owner.Agent.isStopped = false;
            // 设置导航目标为路径的第一个节点
            owner.Agent.SetDestination(owner.LevelPath.PathNodes[targetPathNodeIndex].position);
        }
        protected override void OnUpdate(IFsm<EntityEnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            // 如果发现目标玩家，切换到攻击基地状态
            if (owner.TargetPlayer != null)
            {
                ChangeState<EnemyAttackHomeBaseState>(procedureOwner);
                return;
            }

            // 检查路径是否有效
            if (owner.LevelPath == null || owner.LevelPath.PathNodes == null || owner.LevelPath.PathNodes.Length == 0)
                return;

            // 如果到达当前目标节点，并且不是最后一个节点，则设置下一个节点为目标
            if (owner.LevelPath.PathNodes.Length > targetPathNodeIndex && owner.isAtDestination)
            {
                if (owner.LevelPath.PathNodes.Length - 1 != targetPathNodeIndex)
                {
                    owner.Agent.SetDestination(owner.LevelPath.PathNodes[++targetPathNodeIndex].position);
                }
            }

            // 更新导航速度，考虑减速效果
            owner.Agent.speed = owner.EntityEnemyLogic.Speed * owner.CurrentSlowRate;
        }

        protected override void OnLeave(IFsm<EntityEnemyLogic> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);

        }


        protected override void OnDestroy(IFsm<EntityEnemyLogic> procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }



        public void Clear()
        {
            targetPathNodeIndex = 0;
            owner = null;
        }
    }
}
