using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityDataCtrl : IMemory
    {
        protected Vector3 m_Position = Vector3.zero;

        protected Quaternion m_Rotation = Quaternion.identity;
        
        public EntityDataCtrl()
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
            get
            {
                return m_Position;
            }
            set
            {
                m_Position = value;
            }
        }

        /// <summary>
        /// 实体朝向。
        /// </summary>
        public Quaternion Rotation
        {
            get
            {
                return m_Rotation;
            }
            set
            {
                m_Rotation = value;
            }
        }

        public object UserData
        {
            get;
            protected set;
        }

        public static EntityDataCtrl Create(object userData = null)
        {
            EntityDataCtrl EntityDataCtrl = PoolReference.Acquire<EntityDataCtrl>();
            EntityDataCtrl.Position = Vector3.zero;
            EntityDataCtrl.Rotation = Quaternion.identity;
            EntityDataCtrl.UserData = userData;
            return EntityDataCtrl;
        }

        public static EntityDataCtrl Create(Vector3 position, object userData = null)
        {
            EntityDataCtrl EntityDataCtrl = PoolReference.Acquire<EntityDataCtrl>();
            EntityDataCtrl.Position = position;
            EntityDataCtrl.Rotation = Quaternion.identity;
            EntityDataCtrl.UserData = userData;
            return EntityDataCtrl;
        }

        public static EntityDataCtrl Create(Vector3 position, Quaternion quaternion, object userData = null)
        {
            EntityDataCtrl EntityDataCtrl = PoolReference.Acquire<EntityDataCtrl>();
            EntityDataCtrl.Position = position;
            EntityDataCtrl.Rotation = quaternion;
            EntityDataCtrl.UserData = userData;
            return EntityDataCtrl;
        }

        public virtual void Clear()
        {
            m_Position = Vector3.zero;
            m_Rotation = Quaternion.identity;
            UserData = null;
        }
    }
}
