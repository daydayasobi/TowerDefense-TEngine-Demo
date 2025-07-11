using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public abstract class EntityLogicWithData : EntityLogic
    {
        [SerializeField]
        private EntityData m_EntityData = null;

        public int Id
        {
            get { return Entity.Id; }
        }

        public Animation CachedAnimation { get; private set; }

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            CachedAnimation = GetComponent<Animation>();
            if (userData.GetType() == typeof(EntityData))
            {
                m_EntityData = (EntityData)userData;
            }
        }

        protected override void OnRecycle()
        {
            base.OnRecycle();
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

            //初始化位置
            m_EntityData = (EntityData)userData;
            transform.position = m_EntityData.Position;
            transform.rotation = m_EntityData.Rotation;
            transform.parent = m_EntityData.Parent;
            CachedTransform.localScale = Vector3.one;
            // Name = Utility.Text.Format("[Entity {0}]", Id.ToString());
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);

            // TODO：使用内存池会报错
            // PoolReference.Release(m_EntityData);
        }

        protected override void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
            base.OnAttached(childEntity, parentTransform, userData);
        }

        protected override void OnDetached(EntityLogic childEntity, object userData)
        {
            base.OnDetached(childEntity, userData);
        }

        protected override void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            base.OnAttachTo(parentEntity, parentTransform, userData);
        }

        protected override void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            base.OnDetachFrom(parentEntity, userData);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }
    }
}