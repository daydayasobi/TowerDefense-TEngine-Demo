using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityData : IMemory
    {
        protected Vector3 m_Position = Vector3.zero;

        protected Quaternion m_Rotation = Quaternion.identity;

        protected Transform m_Parent = null;

        protected int m_SerialId = 0;

        public EntityData()
        {
            m_Position = Vector3.zero;
            m_Rotation = Quaternion.identity;
            UserData = null;
        }

        /// <summary>
        /// 实体位置。
        /// </summary>
        public Vector3 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public Quaternion Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }

        /// <summary>
        /// 实体根节点
        /// </summary>
        public Transform Parent
        {
            get { return m_Parent; }
            set { m_Parent = value; }
        }

        /// <summary>
        /// 序列号码
        /// </summary>
        public int SerialId
        {
            get { return m_SerialId; }
            set { m_SerialId = value; }
        }

        public object UserData { get; protected set; }

        public static EntityData Create(int serialId, object userData = null)
        {
            EntityData entityData = PoolReference.Acquire<EntityData>();
            entityData.Position = Vector3.zero;
            entityData.Rotation = Quaternion.identity;
            entityData.UserData = userData;
            return entityData;
        }

        public static EntityData Create(Vector3 position, int serialId, object userData = null)
        {
            EntityData entityData = PoolReference.Acquire<EntityData>();
            entityData.Position = position;
            entityData.Rotation = Quaternion.identity;
            entityData.UserData = userData;
            entityData.m_SerialId = serialId;
            return entityData;
        }

        public static EntityData Create(Vector3 position, Quaternion quaternion, int serialId, object userData = null)
        {
            EntityData entityData = PoolReference.Acquire<EntityData>();
            entityData.Position = position;
            entityData.Rotation = quaternion;
            entityData.UserData = userData;
            entityData.m_SerialId = serialId;
            return entityData;
        }

        // public static EntityData Create(Vector3 position, Quaternion quaternion, Transform parent, int serialId, object userData = null)
        // {
        //     EntityData entityData = PoolReference.Acquire<EntityData>();
        //     entityData.Position = position;
        //     entityData.Rotation = quaternion;
        //     entityData.UserData = userData;
        //     entityData.Parent = parent;
        //     return entityData;
        // }

        public static EntityData Create(Vector3 position, Quaternion quaternion, Transform parent, int serialId, object userData = null)
        {
            EntityData entityData = PoolReference.Acquire<EntityData>();
            entityData.Position = position;
            entityData.Rotation = quaternion;
            entityData.UserData = userData;
            entityData.Parent = parent;
            entityData.m_SerialId = serialId;
            return entityData;
        }

        public virtual void Clear()
        {
            m_Position = Vector3.zero;
            m_Rotation = Quaternion.identity;
            UserData = null;
        }
    }
}