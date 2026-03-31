using System;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public abstract class Entity : MonoBehaviour, IMemory
    {
        private int _id;
        private int _serialId;
        private string _entityAssetName;
        private EntityLogic _entityLogic;
        private readonly List<int> _childSerialIds = new List<int>(5);

        public int Id => _id;
        public int SerialId => _serialId;
        public string EntityAssetName => _entityAssetName;
        public object Handle => gameObject;
        public EntityLogic Logic => _entityLogic;

        public virtual void OnInit(int entityId, int serialId, string entityAssetName, EntityLogic entityLogic)
        {
            _id = entityId;
            _serialId = serialId;
            _entityAssetName = entityAssetName;
            _entityLogic = entityLogic;
        }

        public void OnRecycle()
        {
            try
            {
                _entityLogic.OnRecycle();
                _entityLogic.enabled = false;
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnRecycle with exception '{2}'.", _id, _entityAssetName, exception);
            }

            _id = 0;
        }

        public void OnShow(object userData)
        {
            try
            {
                _entityLogic.OnShow(userData);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnShow with exception '{2}'.", _id, _entityAssetName, exception);
            }
        }

        public virtual void OnHide(bool isShutdown, object userData)
        {
            try
            {
                Log.Debug("OnHide {0}", _entityAssetName);
                _entityLogic.OnHide(isShutdown, userData);
                _id = 0;
                _entityAssetName = string.Empty;
                _childSerialIds.Clear();
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnHide with exception '{2}'.", _id, _entityAssetName, exception);
            }
        }

        public void OnAttachedId(int serialId)
        {
            _childSerialIds.Add(serialId);
        }

        public void OnDetachedId(int serialId)
        {
            for (var i = 0; i < _childSerialIds.Count; i++)
            {
                if (_childSerialIds[i] == serialId)
                {
                    _childSerialIds.RemoveAt(i);
                    break;
                }
            }
        }

        public List<int> GetChildrenIds()
        {
            return _childSerialIds;
        }

        public virtual void Clear()
        {
            _id = 0;
            _entityAssetName = string.Empty;
            _childSerialIds.Clear();
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            try
            {
                _entityLogic.OnUpdate(elapseSeconds, realElapseSeconds);
            }
            catch (Exception exception)
            {
                Log.Error("Entity '[{0}]{1}' OnUpdate with exception '{2}'.", _id, _entityAssetName, exception);
            }
        }
    }
}
