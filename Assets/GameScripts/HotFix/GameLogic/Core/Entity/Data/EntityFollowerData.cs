using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class EntityFollowerData : EntityData
    {
        public Transform Follow { get; private set; }

        public Vector3 Offset { get; private set; }

        public Vector3 Scale { get; private set; }

        public EnumSound ShowSound { get; private set; }

        public EntityFollowerData() : base()
        {
            Follow = null;
            Offset = Vector3.zero;
            Scale = Vector3.one;
            ShowSound = EnumSound.None;
        }

        public static EntityFollowerData Create(Transform follow, int SerialId, object userData = null)
        {
            EntityFollowerData entityFollowerData = PoolReference.Acquire<EntityFollowerData>();
            entityFollowerData.Follow = follow;
            entityFollowerData.UserData = userData;
            entityFollowerData.m_SerialId = SerialId;
            return entityFollowerData;
        }

        public static EntityFollowerData Create(Transform follow, EnumSound enumSound, int SerialId, object userData = null)
        {
            EntityFollowerData entityFollowerData = PoolReference.Acquire<EntityFollowerData>();
            entityFollowerData.Follow = follow;
            entityFollowerData.ShowSound = enumSound;
            entityFollowerData.UserData = userData;
            entityFollowerData.m_SerialId = SerialId;
            return entityFollowerData;
        }

        public static EntityFollowerData Create(Transform follow, Vector3 offset, int SerialId, object userData = null)
        {
            EntityFollowerData entityFollowerData = PoolReference.Acquire<EntityFollowerData>();
            entityFollowerData.Follow = follow;
            entityFollowerData.Offset = offset;
            entityFollowerData.UserData = userData;
            entityFollowerData.m_SerialId = SerialId;
            return entityFollowerData;
        }

        public static EntityFollowerData Create(Transform follow, Vector3 offset, EnumSound enumSound, int SerialId, object userData = null)
        {
            EntityFollowerData entityFollowerData = PoolReference.Acquire<EntityFollowerData>();
            entityFollowerData.Follow = follow;
            entityFollowerData.Offset = offset;
            entityFollowerData.ShowSound = enumSound;
            entityFollowerData.UserData = userData;
            entityFollowerData.m_SerialId = SerialId;
            return entityFollowerData;
        }

        public static EntityFollowerData Create(Transform follow, Vector3 offset, Vector3 scale, int SerialId, object userData = null)
        {
            EntityFollowerData entityFollowerData = PoolReference.Acquire<EntityFollowerData>();
            entityFollowerData.Follow = follow;
            entityFollowerData.Offset = offset;
            entityFollowerData.Scale = scale;
            entityFollowerData.UserData = userData;
            entityFollowerData.m_SerialId = SerialId;
            return entityFollowerData;
        }

        public static EntityFollowerData Create(Transform follow, Vector3 offset, Vector3 scale, EnumSound enumSound, int SerialId, object userData = null)
        {
            EntityFollowerData entityFollowerData = PoolReference.Acquire<EntityFollowerData>();
            entityFollowerData.Follow = follow;
            entityFollowerData.Offset = offset;
            entityFollowerData.Scale = scale;
            entityFollowerData.ShowSound = enumSound;
            entityFollowerData.UserData = userData;
            entityFollowerData.m_SerialId = SerialId;
            return entityFollowerData;
        }

        public static EntityFollowerData Create(Transform follow, Vector3 offset, Vector3 scale, EnumSound enumSound, Vector3 position, Quaternion rotation, int SerialId,
            object userData = null)
        {
            EntityFollowerData entityFollowerData = PoolReference.Acquire<EntityFollowerData>();
            entityFollowerData.Follow = follow;
            entityFollowerData.Offset = offset;
            entityFollowerData.Scale = scale;
            entityFollowerData.ShowSound = enumSound;
            entityFollowerData.Position = position;
            entityFollowerData.Rotation = rotation;
            entityFollowerData.UserData = userData;
            entityFollowerData.m_SerialId = SerialId;
            return entityFollowerData;
        }

        public static EntityFollowerData Create(EnumSound enumSound, Vector3 position, Quaternion rotation, int SerialId, object userData = null)
        {
            EntityFollowerData entityFollowerData = PoolReference.Acquire<EntityFollowerData>();
            entityFollowerData.ShowSound = enumSound;
            entityFollowerData.Position = position;
            entityFollowerData.Rotation = rotation;
            entityFollowerData.m_SerialId = SerialId;
            entityFollowerData.UserData = userData;
            return entityFollowerData;
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