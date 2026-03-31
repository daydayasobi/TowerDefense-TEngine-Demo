using UnityEngine;

namespace TEngine
{
    public abstract class EntityLogic : MonoBehaviour, IMemory
    {
        private bool _available;
        private bool _visible;
        private Entity _entity;
        private Transform _cachedTransform;
        private int _originalLayer;
        private Transform _originalTransform;

        public Entity Entity => _entity;

        public string Name
        {
            get => gameObject.name;
            set => gameObject.name = value;
        }

        public bool Available => _available;

        public bool Visible
        {
            get => _available && _visible;
            set
            {
                if (!_available)
                {
                    Log.Warning("Entity '{0}' is not available.", Name);
                    return;
                }

                if (_visible == value)
                {
                    return;
                }

                _visible = value;
                InternalSetVisible(value);
            }
        }

        public Transform CachedTransform => _cachedTransform;

        public virtual void OnInit(object userData)
        {
            if (_cachedTransform == null)
            {
                _cachedTransform = transform;
            }

            _entity = GetComponent<Entity>();
            _originalLayer = gameObject.layer;
            _originalTransform = _cachedTransform.parent;
        }

        protected internal virtual void OnRecycle()
        {
        }

        protected internal virtual void OnShow(object userData)
        {
            _available = true;
            Visible = true;
        }

        protected internal virtual void OnHide(bool isShutdown, object userData)
        {
            if (this == null || gameObject == null)
            {
                Log.Warning("EntityLogic.OnHide: Object already destroyed.");
                return;
            }

            Visible = false;
            _available = false;
        }

        protected internal virtual void OnAttached(EntityLogic childEntity, Transform parentTransform, object userData)
        {
        }

        protected internal virtual void OnDetached(EntityLogic childEntity, object userData)
        {
        }

        protected internal virtual void OnAttachTo(EntityLogic parentEntity, Transform parentTransform, object userData)
        {
            CachedTransform.SetParent(parentTransform);
        }

        protected internal virtual void OnDetachFrom(EntityLogic parentEntity, object userData)
        {
            CachedTransform.SetParent(_originalTransform);
        }

        protected internal virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        protected virtual void InternalSetVisible(bool visible)
        {
            gameObject.SetActive(visible);
        }

        public void Clear()
        {
        }
    }
}
