using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;
using UnityEngine.WSA;

namespace GameLogic.View
{
    public class EntityEnemy : MonoBehaviour
    {
        public int Id=0;
        public Transform turret;
        public Transform[] projectilePoints;
        public Transform epicenter;
        public Launcher launcher;
        public EnemyData enemyData;

        protected IFsm<EntityEnemy> fsm;


        private Dictionary<int, float> dicSlowDownRates;

        //表示是否死亡或已攻击玩家即将回收，以防重复执行回收逻辑
        private bool hide = false;
        private bool loadSlowDownEffect = false;

        protected List<FsmState<EntityEnemy>> stateList;

        public Vector3 InitialPosition;
        // 目标玩家实体
        public EntityPlayer TargetPlayer { get; private set; }

        private void Start()
        {

        }

        public float CurrentSlowRate
        {
            get;
            private set;
        }

        public bool IsActivation
        { get;  set; }

        public UnityEngine.AI.NavMeshAgent Agent
        {
            get;
            private set;
        }

        public bool isPathBlocked
        {
            get { return Agent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathPartial; }
        }

        public bool isAtDestination
        {
            get { return Agent.remainingDistance <= Agent.stoppingDistance; }
        }

        public LevelPath LevelPath
        {
            get;
            set;
        }


        public bool IsPause
        {
            get;
            private set;
        }
        public void OnInit()
        {
            Agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            dicSlowDownRates = new Dictionary<int, float>();
            stateList = new List<FsmState<EntityEnemy>>();
            CreateFsm();
        }
        public void OnHide()
        {
            Agent.enabled = false;
            hide = true;
            IsActivation = false;
            TargetPlayer = null;
            DestroyFsm();
            RemoveSlowEffect();
            dicSlowDownRates.Clear();
        }
        public void OnShow()
        {
            transform.localPosition = InitialPosition - new Vector3(0, 0.2f, 0);
            transform.localRotation = Quaternion.identity;
            CurrentSlowRate = 1;
            Agent.enabled = true;
            IsActivation = true;
            hide = false;
        }
        protected virtual void AddFsmState()
        {
            // fsm = GameModule.Fsm.CreateFsm("EntityEnemy" + Id, this, new FsmState<EntityEnemy>[] {
            // new EnemyAttackHomeBaseState(),
            // new EnemyMoveState(),
            // new EnemyStandbyState()});
        }

        protected virtual void StartFsm()
        {
            // fsm.Start<EnemyStandbyState>();
        }

        private void CreateFsm()
        {
            AddFsmState();
            StartFsm();
        }

        private void DestroyFsm()
        {
            stateList.Clear();
            fsm = null;
        }
        /// <summary>
        /// 触发器进入事件
        /// </summary>
        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<EntityPlayer>();

            if (player == null)
            {
                return;
            }
            TargetPlayer = player;
        }
        /// <summary>
        /// 攻击后的逻辑
        /// </summary>
        public void AfterAttack()
        {
            if (!hide)
            {
                hide = true;
                GameEvent.Send(LevelEvent.OnHideEnemyEntity, Id);
            }
        }

        private void RemoveSlowEffect()
        {

        }
    }
}
