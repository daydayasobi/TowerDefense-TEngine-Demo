using System;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace TEngine
{
    public abstract class Entity : MonoBehaviour, IMemory
    {
        private int m_Id;
        private int m_SerialId; //序列号Id
        private string m_EntityAssetName;
        private EntityLogic m_EntityLogic;
        private List<int> childSerialIds = new List<int>(5);

        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int Id
        {
            get { return m_Id; }
        }

        /// <summary>
        /// 获取实体序列号Id。
        /// </summary>
        public int SerialId
        {
            get { return m_SerialId; }
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName
        {
            get { return m_EntityAssetName; }
        }

        /// <summary>
        /// 获取实体实例。
        /// </summary>
        public object Handle
        {
            get { return gameObject; }
        }

        /// <summary>
        /// 获取实体逻辑。
        /// </summary>
        public EntityLogic Logic
        {
            get { return m_EntityLogic; }
        }

        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnInit(int entityId, int serialId, string entityAssetName, EntityLogic entityLogic)
        {
            m_Id = entityId;
            m_SerialId = serialId;
            m_EntityAssetName = entityAssetName;
            m_EntityLogic = entityLogic;
        }

        /// <summary>
        /// 实体回收。
        /// </summary>
        public void OnRecycle()
        {
            try
            {
                m_EntityLogic.OnRecycle();
                m_EntityLogic.enabled = false;
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnRecycle with exception '{2}'.", m_Id.ToString(), m_EntityAssetName, exception.ToString());
            }

            m_Id = 0;
        }

        /// <summary>
        /// 实体显示。
        /// </summary>
        /// <param name="userData">用户自定义数据。</param>
        public void OnShow(object userData)
        {
            // ShowEntityInfo showEntityInfo = (ShowEntityInfo)userData;
            try
            {
                // m_EntityLogic.OnShow(showEntityInfo.UserData);
                m_EntityLogic.OnShow(userData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnShow with exception '{2}'.", m_Id.ToString(), m_EntityAssetName, exception.ToString());
            }
        }

        /// <summary>
        /// 实体隐藏。
        /// </summary>
        /// <param name="isShutdown">是否是关闭实体管理器时触发。</param>
        /// <param name="userData">用户自定义数据。</param>
        public virtual void OnHide(bool isShutdown, object userData)
        {
            try
            {
                m_EntityLogic.OnHide(isShutdown, userData);
                m_Id = 0;
                m_EntityAssetName = string.Empty;
                childSerialIds.Clear();
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnHide with exception '{2}'.", m_Id.ToString(), m_EntityAssetName, exception.ToString());
            }
        }

        public void OnAttachedId(int SerialId)
        {
            childSerialIds.Add(SerialId);
        }

        public void OnDetachedId(int SerialIds)
        {
            for (int i = 0; i < childSerialIds.Count; i++)
            {
                if (childSerialIds[i] == SerialIds)
                {
                    childSerialIds.RemoveAt(i);
                    break;
                }
            }
        }

        public List<int> GetChildrenIds()
        {
            return childSerialIds;
        }

        public virtual void Clear()
        {
            m_Id = 0;
            m_EntityAssetName = string.Empty;
            childSerialIds.Clear();
            // PoolManager.Instance.PushGameObject(this.gameObject);
        }

        /*


        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="childEntity">附加的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnAttached(IEntity childEntity, object userData)
        {
            AttachEntityInfo attachEntityInfo = (AttachEntityInfo)userData;
            try
            {
                m_EntityLogic.OnAttached(((Entity)childEntity).Logic, attachEntityInfo.ParentTransform, attachEntityInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnAttached with exception '{2}'.", m_Id.ToString(), m_EntityAssetName, exception.ToString());
            }
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="childEntity">解除的子实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnDetached(IEntity childEntity, object userData)
        {
            try
            {
                m_EntityLogic.OnDetached(((Entity)childEntity).Logic, userData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnDetached with exception '{2}'.", m_Id.ToString(), m_EntityAssetName, exception.ToString());
            }
        }

        /*

        /// <summary>
        /// 实体附加子实体。
        /// </summary>
        /// <param name="parentEntity">被附加的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnAttachTo(IEntity parentEntity, object userData)
        {
            AttachEntityInfo attachEntityInfo = (AttachEntityInfo)userData;
            try
            {
                m_EntityLogic.OnAttachTo(((Entity)parentEntity).Logic, attachEntityInfo.ParentTransform, attachEntityInfo.UserData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnAttachTo with exception '{2}'.", m_Id.ToString(), m_EntityAssetName, exception.ToString());
            }

            PoolReference.Release(attachEntityInfo);
        }

        /// <summary>
        /// 实体解除子实体。
        /// </summary>
        /// <param name="parentEntity">被解除的父实体。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnDetachFrom(IEntity parentEntity, object userData)
        {
            try
            {
                m_EntityLogic.OnDetachFrom(((Entity)parentEntity).Logic, userData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnDetachFrom with exception '{2}'.", m_Id.ToString(), m_EntityAssetName, exception.ToString());
            }
        }

         */

        /// <summary>
        /// 实体轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            try
            {
                m_EntityLogic.OnUpdate(elapseSeconds, realElapseSeconds);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnUpdate with exception '{2}'.", m_Id.ToString(), m_EntityAssetName, exception.ToString());
            }
        }
    }
}