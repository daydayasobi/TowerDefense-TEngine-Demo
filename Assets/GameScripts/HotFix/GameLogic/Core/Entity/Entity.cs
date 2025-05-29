using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class Entity : MonoBehaviour
    {
        private int m_Id;
        private string m_EntityAssetName;
        private EntityLogic m_EntityLogic;
        
        /// <summary>
        /// 获取实体编号。
        /// </summary>
        public int Id
        {
            get
            {
                return m_Id;
            }
        }

        /// <summary>
        /// 获取实体资源名称。
        /// </summary>
        public string EntityAssetName
        {
            get
            {
                return m_EntityAssetName;
            }
        }

        /// <summary>
        /// 获取实体实例。
        /// </summary>
        public object Handle
        {
            get
            {
                return gameObject;
            }
        }
        
        /// <summary>
        /// 获取实体逻辑。
        /// </summary>
        public EntityLogic Logic
        {
            get
            {
                return m_EntityLogic;
            }
        }
        
        /// <summary>
        /// 实体初始化。
        /// </summary>
        /// <param name="entityId">实体编号。</param>
        /// <param name="entityAssetName">实体资源名称。</param>
        /// <param name="entityGroup">实体所属的实体组。</param>
        /// <param name="isNewInstance">是否是新实例。</param>
        /// <param name="userData">用户自定义数据。</param>
        public void OnInit(int entityId, string entityAssetName, bool isNewInstance, object userData)
        {
            // m_Id = entityId;
            // m_EntityAssetName = entityAssetName;
            //
            // m_EntityLogic = gameObject.GetComponent(entityLogicType) as EntityLogic;
            //
            // if (m_EntityLogic == null)
            //     m_EntityLogic = gameObject.AddComponent(entityLogicType) as EntityLogic;
            //
            // if (m_EntityLogic == null)
            // {
            //     Log.Error("Entity '{0}' can not add entity logic.", entityAssetName);
            //     return;
            // }
            //
            // try
            // {
            //     m_EntityLogic.OnInit();
            // }
            // catch (Exception exception)
            // {
            //     Log.Error("Entity '[{0}]{1}' OnInit with exception '{2}'.", m_Id.ToString(), m_EntityAssetName, exception.ToString());
            // }
        }
    }
}
