using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using GameConfig;
using TEngine;
using UnityEngine;
using UnityEngine.AI;

namespace GameLogic
{
    public class EntityEnemyLogic : EntityTargetableLogic, IPause
    {
        public Transform turret;
        public Transform[] projectilePoints;

        public Transform epicenter;

        public Launcher launcher;
        private EntityEnemyData _entityEnemyData;

        protected IFsm<EntityEnemyLogic> fsm;
        
        private Dictionary<int, float> dicSlowDownRates;

        //表示是否死亡或已攻击玩家即将回收，以防重复执行回收逻辑
        private bool hide = false;

        private Entity slowDownEffect;
        private bool loadSlowDownEffect = false;

        protected List<FsmState<EntityEnemyLogic>> stateList;

        public Targetter Targetter { get; private set; }

        public bool IsActivation { get; set; }

        public Attacker Attacker { get; private set; }

        public override EnumAlignment Alignment
        {
            get { return EnumAlignment.Enemy; }
        }

        protected override float MaxHP
        {
            get
            {
                if (EntityEnemyData != null)
                    return EntityEnemyData.EnemyData.MaxHP;
                else
                    return 0;
            }
        }

        public EntityEnemyData EntityEnemyData { get; private set; }

        public float CurrentSlowRate { get; private set; }

        public NavMeshAgent Agent { get; private set; }

        public bool isPathBlocked
        {
            get { return Agent.pathStatus == NavMeshPathStatus.PathPartial; }
        }

        public bool isAtDestination
        {
            get { return Agent.remainingDistance <= Agent.stoppingDistance; }
        }

        public LevelPath LevelPath { get; private set; }

        public EntityPlayerLogic TargetPlayer { get; private set; }

        public bool IsPause { get; private set; }

        public override void OnInit(object userData)
        {
            base.OnInit(userData);

            Agent = GetComponent<NavMeshAgent>();
            hpBarRoot = transform.Find("HealthBar");
            dicSlowDownRates = new Dictionary<int, float>();
            stateList = new List<FsmState<EntityEnemyLogic>>();
            CurrentSlowRate = 1;
            if (userData != null)
            {
                _entityEnemyData = (EntityEnemyData)userData;
                transform.parent = _entityEnemyData.Parent;
                transform.position = _entityEnemyData.Position;
                transform.rotation = _entityEnemyData.Rotation;
            }

            Targetter = transform.Find("Targetter").GetComponent<Targetter>();
            Attacker = transform.Find("Attack").GetComponent<Attacker>();

            Targetter.OnInit(userData);
            Attacker.OnInit(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (IsPause)
                return;
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            EntityEnemyData = userData as EntityEnemyData;

            if (EntityEnemyData == null)
            {
                Log.Error("Entity enemy '{0}' entity data invaild.");
                return;
            }

            hide = false;
            Agent.enabled = true;
            LevelPath = EntityEnemyData.LevelPath;
            hp = EntityEnemyData.EnemyData.MaxHP;

            Attacker.SetOwnerEntity(Entity);
            Targetter.SetAlignment(Alignment);
            Targetter.SetTurret(turret);
            Targetter.SetSearchRange(EntityEnemyData.EnemyData.Range);
            Targetter.ResetTargetter();

            AttackerDataBase attackerData = AttackerDataBase.Create(EntityEnemyData.EnemyData.Range,
                EntityEnemyData.EnemyData.FireRate,
                EntityEnemyData.EnemyData.IsMultiAttack,
                EntityEnemyData.EnemyData.ProjectileType,
                EntityEnemyData.EnemyData.ProjectileEntityId
                );
            
            Attacker.SetData(attackerData, EntityEnemyData.EnemyData.ProjectileData);
            Attacker.SetTargetter(Targetter);
            Attacker.SetProjectilePoints(projectilePoints);
            Attacker.SetEpicenter(epicenter);
            Attacker.SetLaunch(launcher);
            Attacker.ResetAttack();
            
            Targetter.OnShow(userData);
            Attacker.OnShow(userData);

            CreateFsm();
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            Targetter.OnHide(isShutdown, userData);
            Attacker.OnHide(isShutdown, userData);
            Attacker.EmptyOwnerEntity();

            LevelPath = null;
            EntityEnemyData = null;
            hp = 0;
            Agent.enabled = false;
            TargetPlayer = null;
            hide = true;
            DestroyFsm();
            RemoveSlowEffect();
            dicSlowDownRates.Clear();
        }

        protected virtual void AddFsmState()
        {
            stateList.Add(EnemyMoveState.Create());
            stateList.Add(EnemyAttackHomeBaseState.Create());
            stateList.Add(EnemyAttackTowerState.Create());
        }

        protected virtual void StartFsm()
        {
            fsm.Start<EnemyMoveState>();
        }

        private void CreateFsm()
        {
            AddFsmState();
            Log.Debug("Create Fsm for enemy {0} with states count: {1}", gameObject.name + _entityEnemyData.SerialId, stateList.Count);
            fsm = GameModule.Fsm.CreateFsm<EntityEnemyLogic>(gameObject.name + _entityEnemyData.SerialId, this, stateList);
            StartFsm();
        }

        private void DestroyFsm()
        {
            GameModule.Fsm.DestroyFsm(fsm);
            foreach (var item in stateList)
            {
                PoolReference.Release((IMemory)item);
            }

            stateList.Clear();
            fsm = null;
        }

        public void AfterAttack()
        {
            if (!hide)
            {
                hide = true;
                GameEvent.Send(LevelEvent.OnHideEnemyEntity,_entityEnemyData.SerialId);
            }
        }

        protected override void Dead()
        {
            base.Dead();

            PlayerDataControl.Instance.AddEnergy(EntityEnemyData.EnemyData.AddEnergy);
            if (!hide)
            {
                hide = true;
                GameEvent.Send(LevelEvent.OnHideEnemyEntity,_entityEnemyData.SerialId);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<EntityPlayerLogic>();
            if (player == null)
            {
                return;
            }

            TargetPlayer = player;
        }

        public void ApplySlow(int towerId, float slowRate)
        {
            if (IsDead || !Available)
                return;

            if (dicSlowDownRates.ContainsKey(towerId))
            {
                dicSlowDownRates[towerId] = slowRate;
            }
            else
            {
                dicSlowDownRates.Add(towerId, slowRate);
            }

            foreach (var item in dicSlowDownRates)
            {
                CurrentSlowRate = Mathf.Min(CurrentSlowRate, item.Value);
            }

            //Debug.LogError(string.Format("apply slow by tower {0},slow rate: {1},current slow rate:{2}", towerId, slowRate, CurrentSlowRate));

            ApplySlowEffect();
        }

        public void RemoveSlow(int towerId)
        {
            if (dicSlowDownRates.ContainsKey(towerId))
            {
                dicSlowDownRates.Remove(towerId);
                if (dicSlowDownRates.Count == 0)
                {
                    CurrentSlowRate = 1;
                    RemoveSlowEffect();
                }

                //Debug.LogError(string.Format("remove slow by tower {0},current slow rate:{1}", towerId, CurrentSlowRate));
            }
        }

        private void ApplySlowEffect()
        {
            if (slowDownEffect == null && !loadSlowDownEffect)
            {
                int serialId = GameModule.Entity.GenerateSerialId();
                ShowEntityEventData eventData = PoolReference.Acquire<ShowEntityEventData>();
                eventData.EntityId = (int)EnumEntity.SlowFx;
                eventData.SerialId = serialId;
                eventData.LogicType = typeof(EntityAnimationLogic);
                eventData.OnShowSuccess = OnLoadSlowEffectSuccess;
                eventData.UserData = EntityFollowerData.Create(transform,
                        ApplyEffectOffset,
                        Vector3.one * ApplyEffectScale,
                        EnumSound.None,
                        transform.position,
                        transform.rotation,
                        serialId);
                
                GameEvent.Send(LevelEvent.OnShowEntityInLevel, eventData);

                loadSlowDownEffect = true;
            }
        }

        private void OnLoadSlowEffectSuccess(Entity entity)
        {
            slowDownEffect = entity;
            //若减速效果加载出后后，此敌人已经死亡或回收，则立马移除特效
            if (hide)
            {
                RemoveSlowEffect();
            }
        }

        private void RemoveSlowEffect()
        {
            if (slowDownEffect != null)
            {
                GameEvent.Send(LevelEvent.OnHideEntityInLevel,slowDownEffect);
                slowDownEffect = null;
                loadSlowDownEffect = false;
            }
        }

        public void Pause()
        {
            IsPause = true;
            Agent.speed = 0;
        }

        public void Resume()
        {
            IsPause = false;
            Agent.speed = EntityEnemyData.EnemyData.Speed * CurrentSlowRate;
        }
    }
}