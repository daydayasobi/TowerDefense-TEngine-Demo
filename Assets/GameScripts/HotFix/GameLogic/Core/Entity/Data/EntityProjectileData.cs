using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class EntityProjectileData : EntityData
    {
        public EntityTargetableLogic EntityTargetableLogic { get; private set; }

        public ProjectileDataBase ProjectileData { get; private set; }
        public Vector3 Origin { get; private set; }

        public Transform FiringPoint { get; private set; }

        public EntityProjectileData() : base()
        {
            EntityTargetableLogic = null;
            Origin = Vector3.zero;
            FiringPoint = null;
        }

        public static EntityProjectileData Create(EntityTargetableLogic entityTargetable, ProjectileDataBase projectileData, Vector3 origin, Transform firingPoint,
            object userData = null)
        {
            EntityProjectileData entityData = PoolReference.Acquire<EntityProjectileData>();
            entityData.EntityTargetableLogic = entityTargetable;
            entityData.ProjectileData = projectileData;
            entityData.Origin = origin;
            entityData.FiringPoint = firingPoint;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityProjectileData Create(EntityTargetableLogic entityTargetable, ProjectileDataBase projectileData, Vector3 origin, Transform firingPoint,
            Vector3 position, Quaternion rotation, object userData = null)
        {
            EntityProjectileData entityData = PoolReference.Acquire<EntityProjectileData>();
            entityData.EntityTargetableLogic = entityTargetable;
            entityData.ProjectileData = projectileData;
            entityData.Origin = origin;
            entityData.FiringPoint = firingPoint;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.UserData = userData;
            return entityData;
        }
        
        public static EntityProjectileData Create(EntityTargetableLogic entityTargetable, ProjectileDataBase projectileData, Vector3 origin, Transform firingPoint,
            Vector3 position, Quaternion rotation,int serialId, object userData = null)
        {
            EntityProjectileData entityData = PoolReference.Acquire<EntityProjectileData>();
            entityData.EntityTargetableLogic = entityTargetable;
            entityData.ProjectileData = projectileData;
            entityData.Origin = origin;
            entityData.FiringPoint = firingPoint;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.m_SerialId = serialId;
            entityData.UserData = userData;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            EntityTargetableLogic = null;
            ProjectileData = null;
            Origin = Vector3.zero;
            FiringPoint = null;
        }
    }
}