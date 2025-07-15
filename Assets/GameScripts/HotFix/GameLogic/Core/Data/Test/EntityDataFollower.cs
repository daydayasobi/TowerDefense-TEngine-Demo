using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class EntityDataFollower : EntityData
    {
        public Transform Follow { get; private set; }

        public Vector3 Offset { get; private set; }

        public Vector3 Scale { get; private set; }

        public EnumSound ShowSound { get; private set; }

        public EntityDataFollower() : base()
        {
            Follow = null;
            Offset = Vector3.zero;
            Scale = Vector3.one;
            ShowSound = EnumSound.None;
        }

        public static EntityDataFollower Create(Transform follow, int SerialId, object userData = null)
        {
            EntityDataFollower entityData = PoolReference.Acquire<EntityDataFollower>();
            entityData.Follow = follow;
            entityData.UserData = userData;
            entityData.m_SerialId = SerialId;
            return entityData;
        }

        public static EntityDataFollower Create(Transform follow, EnumSound enumSound, int SerialId, object userData = null)
        {
            EntityDataFollower entityData = PoolReference.Acquire<EntityDataFollower>();
            entityData.Follow = follow;
            entityData.ShowSound = enumSound;
            entityData.UserData = userData;
            entityData.m_SerialId = SerialId;
            return entityData;
        }

        public static EntityDataFollower Create(Transform follow, Vector3 offset, int SerialId, object userData = null)
        {
            EntityDataFollower entityData = PoolReference.Acquire<EntityDataFollower>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.UserData = userData;
            entityData.m_SerialId = SerialId;
            return entityData;
        }

        public static EntityDataFollower Create(Transform follow, Vector3 offset, EnumSound enumSound, int SerialId, object userData = null)
        {
            EntityDataFollower entityData = PoolReference.Acquire<EntityDataFollower>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.ShowSound = enumSound;
            entityData.UserData = userData;
            entityData.m_SerialId = SerialId;
            return entityData;
        }

        public static EntityDataFollower Create(Transform follow, Vector3 offset, Vector3 scale, int SerialId, object userData = null)
        {
            EntityDataFollower entityData = PoolReference.Acquire<EntityDataFollower>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.Scale = scale;
            entityData.UserData = userData;
            entityData.m_SerialId = SerialId;
            return entityData;
        }

        public static EntityDataFollower Create(Transform follow, Vector3 offset, Vector3 scale, EnumSound enumSound, int SerialId, object userData = null)
        {
            EntityDataFollower entityData = PoolReference.Acquire<EntityDataFollower>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.Scale = scale;
            entityData.ShowSound = enumSound;
            entityData.UserData = userData;
            entityData.m_SerialId = SerialId;
            return entityData;
        }

        public static EntityDataFollower Create(Transform follow, Vector3 offset, Vector3 scale, EnumSound enumSound, Vector3 position, Quaternion rotation, int SerialId,
            object userData = null)
        {
            EntityDataFollower entityData = PoolReference.Acquire<EntityDataFollower>();
            entityData.Follow = follow;
            entityData.Offset = offset;
            entityData.Scale = scale;
            entityData.ShowSound = enumSound;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.UserData = userData;
            entityData.m_SerialId = SerialId;
            return entityData;
        }

        public static EntityDataFollower Create(EnumSound enumSound, Vector3 position, Quaternion rotation, int SerialId, object userData = null)
        {
            EntityDataFollower entityData = PoolReference.Acquire<EntityDataFollower>();
            entityData.ShowSound = enumSound;
            entityData.Position = position;
            entityData.Rotation = rotation;
            entityData.m_SerialId = SerialId;
            entityData.UserData = userData;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            Follow = null;
            Offset = Vector3.zero;
            Scale = Vector3.one;
            ShowSound = EnumSound.None;
        }
    }
}