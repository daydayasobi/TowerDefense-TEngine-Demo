using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
    public class Attacker : MonoBehaviour
    {
        private Transform[] projectilePoints;
        private Transform epicenter;

        private AttackerDataBase attackerData;
        private ProjectileDataBase projectileData;

        private RandomSound randomSound;

        private Entity ownerEntity;
        private Targetter towerTargetter;
        private ILauncher m_Launcher;
        private float m_FireTimer;
        private EntityTargetableLogic m_TrackingTarget;

        public float SearchRate
        {
            get { return towerTargetter.searchRate; }
            set { towerTargetter.searchRate = value; }
        }

        public EntityTargetableLogic TrackingTarget
        {
            get { return m_TrackingTarget; }
        }

        public Targetter Targetter
        {
            get { return towerTargetter; }
        }

        public void OnInit(object userData)
        {
            randomSound = GetComponent<RandomSound>();
        }

        public void OnShow(object userData)
        {
            //targetMask = mask;

            SetUpTimers();
        }

        public void OnHide(bool isShutdown, object userData)
        {
            ResetAttack();
            EmptyData();
            RemoveTowerTargetter();
            EmptyProjectilePoints();
            EmptyEpicenter();
        }

        public void ResetAttack()
        {
            m_TrackingTarget = null;
            m_FireTimer = 0;
        }

        void OnLostTarget()
        {
            m_TrackingTarget = null;
        }

        void OnAcquiredTarget(EntityTargetableLogic acquiredTarget)
        {
            m_TrackingTarget = acquiredTarget;
        }

        protected virtual void SetUpTimers()
        {
            m_FireTimer = 1 / attackerData.FireRate;
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            m_FireTimer -= elapseSeconds;
            if (TrackingTarget != null && m_FireTimer <= 0.0f)
            {
                OnFireTimer();
                m_FireTimer = 1 / attackerData.FireRate;
            }
        }

        protected virtual void OnFireTimer()
        {
            FireProjectile();
        }

        protected virtual void FireProjectile()
        {
            if (m_TrackingTarget == null)
            {
                return;
            }

            if (attackerData.IsMultiAttack)
            {
                List<EntityTargetableLogic> enemies = towerTargetter.GetAllTargets();
                m_Launcher.Launch(
                    enemies,
                    attackerData,
                    projectileData,
                    epicenter.position,
                    projectilePoints);
            }
            else
            {
                m_Launcher.Launch(
                    m_TrackingTarget,
                    attackerData,
                    projectileData,
                    epicenter.position,
                    projectilePoints);
            }

            if (randomSound != null)
            {
                GameModule.Audio.Play(AudioType.Sound, AssetsDataLoader.Instance.GetItemConfig((int)randomSound.GetRandomSound()).ResourcesName);
            }
        }


        protected virtual int ByDistance(EntityTargetableLogic first, EntityTargetableLogic second)
        {
            float firstSqrMagnitude = Vector3.SqrMagnitude(first.transform.position - epicenter.position);
            float secondSqrMagnitude = Vector3.SqrMagnitude(second.transform.position - epicenter.position);
            return firstSqrMagnitude.CompareTo(secondSqrMagnitude);
        }

        public void SetData(AttackerDataBase attackerData, ProjectileDataBase projectileData)
        {
            if (this.attackerData != null)
                PoolReference.Release(this.attackerData);

            this.attackerData = attackerData;
            this.projectileData = projectileData;
        }

        private void EmptyData()
        {
            if (attackerData != null)
                PoolReference.Release(attackerData);

            this.attackerData = null;
            this.projectileData = null;
        }

        public void SetProjectilePoints(Transform[] tfs)
        {
            projectilePoints = tfs;
        }

        public void EmptyProjectilePoints()
        {
            projectilePoints = null;
        }

        public void SetEpicenter(Transform tf)
        {
            epicenter = tf;
        }

        public void EmptyEpicenter()
        {
            epicenter = null;
        }

        public void SetTargetter(Targetter towerTargetter)
        {
            this.towerTargetter = towerTargetter;
            this.towerTargetter.acquiredTarget += OnAcquiredTarget;
            this.towerTargetter.lostTarget += OnLostTarget;
        }

        public void RemoveTowerTargetter()
        {
            towerTargetter.acquiredTarget -= OnAcquiredTarget;
            towerTargetter.lostTarget -= OnLostTarget;

            this.towerTargetter = null;
        }

        public void SetLaunch(ILauncher launcher)
        {
            this.m_Launcher = launcher;
        }

        public void EmptyLaunch()
        {
            this.m_Launcher = null;
        }

        public void SetOwnerEntity(Entity entity)
        {
            this.ownerEntity = entity;
        }

        public void EmptyOwnerEntity()
        {
            this.ownerEntity = null;
        }
    }
}