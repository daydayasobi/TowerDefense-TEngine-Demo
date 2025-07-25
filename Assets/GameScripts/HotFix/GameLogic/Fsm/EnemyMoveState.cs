using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using ProcedureOwner = TEngine.IFsm<GameLogic.EntityEnemyLogic>;


namespace GameLogic
{
    public class EnemyMoveState : FsmState<EntityEnemyLogic>, IMemory
    {
        // 当前状态的拥有者，即敌方单位对象
        private EntityEnemyLogic owner;
        // 目标路径节点的索引，用于记录当前移动到路径中的哪个节点
        private int targetPathNodeIndex = 0;
        protected EntityTargetableLogic m_TargetTower;
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
            // 禁用敌方单位的攻击器，防止在移动状态下进行攻击
            // owner.Attacker.enabled = false;
            // 设置导航目标为路径的第一个节点
            owner.Agent.SetDestination(owner.LevelPath.PathNodes[targetPathNodeIndex].position);
            // 更新目标塔的位置信息
            // owner.Targetter.transform.position = owner.transform.position;
        }
        protected override void OnUpdate(IFsm<EntityEnemyLogic> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (owner.IsPause)
                return;
            
            owner.Targetter.OnUpdate(elapseSeconds, realElapseSeconds);
            
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
            owner.Agent.speed = owner.EntityEnemyData.EnemyData.Speed * owner.CurrentSlowRate;
            
            // 如果路径被阻塞，更新目标塔的位置为路径结束位置
            if (owner.isPathBlocked)
            {
                owner.Targetter.transform.position = owner.Agent.pathEndPosition;
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
            }
            // 检查当前目标塔是否存在
            if (m_TargetTower == null)
            {
                return;
            }
            // 如果目标塔被摧毁，清除目标塔引用
            float distanceToTower = Vector3.Distance(owner.transform.position, m_TargetTower.transform.position);
            if (distanceToTower > owner.EntityEnemyData.EnemyData.Range)
            {
                return;
            }

            ChangeState<EnemyAttackTowerState>(procedureOwner);
        }

        protected override void OnLeave(IFsm<EntityEnemyLogic> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            owner.Targetter.transform.position = owner.transform.position;
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
        
        public static EnemyMoveState Create()
        {
            EnemyMoveState state = PoolReference.Acquire<EnemyMoveState>();
            return state;
        }

        public void Clear()
        {
            targetPathNodeIndex = 0;
            owner = null;
            m_TargetTower = null;
        }
    }
}
