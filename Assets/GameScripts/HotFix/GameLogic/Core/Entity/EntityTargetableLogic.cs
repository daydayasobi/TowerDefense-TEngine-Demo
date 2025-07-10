using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public abstract class EntityTargetableLogic : EntityLogicWithData
    {
        protected Transform hpBarRoot;

        private Vector3 m_CurrentPosition, m_PreviousPosition;

        private RandomSound randomSound;

        public virtual EnumAlignment Alignment
        {
            get { return EnumAlignment.None; }
        }

        protected float hp;

        public float HP
        {
            get { return hp; }

            protected set { hp = value; }
        }

        protected abstract float MaxHP { get; }

        public bool IsDead
        {
            get { return HP <= 0; }
        }

        public Vector3 DeadEffectOffset
        {
            get
            {
                if (deadEffect == null)
                    return Vector3.zero;

                return deadEffect.deadEffectOffset;
            }
        }

        public Vector3 ApplyEffectOffset
        {
            get
            {
                if (effectPointData == null)
                    return Vector3.zero;

                return effectPointData.applyEffectOffset;
            }
        }

        public float ApplyEffectScale
        {
            get
            {
                if (effectPointData == null)
                    return 1;

                return effectPointData.applyEffectScale;
            }
        }

        public Vector3 Velocity { get; private set; }

        private bool loadedHPBar = false;
        private EntityHPBarLogic entityHPBar;

        public event Action<EntityTargetableLogic> OnDead;
        public event Action<EntityTargetableLogic> OnHidden;

        private EffectPointData effectPointData;
        private DeadEffect deadEffect;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            randomSound = GetComponent<RandomSound>();
            effectPointData = GetComponent<EffectPointData>();
            deadEffect = GetComponent<DeadEffect>();
            hpBarRoot = transform.Find("HealthBar");
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            if (OnHidden != null)
                OnHidden(this);

            OnHidden = null;
            OnDead = null;

            HideHpBar();
        }

        protected virtual void FixedUpdate()
        {
            m_CurrentPosition = transform.position;
            Velocity = (m_CurrentPosition - m_PreviousPosition) / Time.fixedDeltaTime;
            m_PreviousPosition = m_CurrentPosition;
        }

        public virtual void Damage(float value)
        {
            if (IsDead)
                return;

            if (!loadedHPBar)
            {
                int serialId = GameModule.Entity.GenerateSerialId();
                var eventData = new ShowEntityEventData
                {
                    EntityId = (int)EnumEntity.HPBar,
                    SerialId = serialId,
                    LogicType = typeof(EntityHPBarLogic),
                    UserData = EntityDataFollower.Create(hpBarRoot),
                    OnShowSuccess = (entity) =>
                    {
                        OnLoadHpBarSuccess(entity);
                    },
                };
                GameEvent.Send(LevelEvent.OnShowEntityInLevel, eventData);

                loadedHPBar = true;
            }

            hp -= value;

            if (entityHPBar)
            {
                entityHPBar.UpdateHealth(hp / MaxHP);
            }


            if (hp <= 0)
            {
                hp = 0;
                Dead();
            }
        }

        protected virtual void Dead()
        {
            if (OnDead != null)
                OnDead(this);

            if (deadEffect != null)
            {
                // TODO:Show Effect Entity
                // int serialId = GameModule.Entity.GenerateSerialId();
                // var eventData = new ShowEntityEventData
                // {
                //     EntityId = attackerData.ProjectileEntityId,
                //     SerialId = serialId,
                //     LogicType = typeof(EntityProjectileHitscanLogic),
                //     UserData = EntityProjectileData.Create(target, projectileData, origin, firingPoint, firingPoint.position, firingPoint.rotation, serialId)
                // };
                // GameEntry.Event.Fire(this, ShowEntityInLevelEventArgs.Create(
                //     (int)deadEffect.deadEffectEntity,
                //     typeof(EntityParticleAutoHide),
                //     null,
                //     EntityDataFollower.Create(randomSound ? randomSound.GetRandomSound() : EnumSound.None, transform.position + DeadEffectOffset, transform.rotation)));
            }
        }

        private void OnLoadHpBarSuccess(Entity entity)
        {
            entityHPBar = entity.Logic as EntityHPBarLogic;
            if (IsDead || !Available)
            {
                HideHpBar();
            }
            else
            {
                entityHPBar.UpdateHealth(hp / MaxHP);
            }
        }

        private void HideHpBar()
        {
            if (entityHPBar)
            {
                GameEvent.Send(LevelEvent.OnHideEntityInLevel, entityHPBar);
                loadedHPBar = false;
                entityHPBar = null;
            }
        }
    }
}