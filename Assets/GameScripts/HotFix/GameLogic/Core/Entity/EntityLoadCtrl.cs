using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityLoadCtrl
    {
        private Dictionary<int, Action<Entity>> dicCallback;
        private Dictionary<int, Entity> dicSerial2Entity;

        private List<int> tempList;
        private GameObject entityRoot;

        public EntityLoadCtrl()
        {
            dicSerial2Entity = new Dictionary<int, Entity>();
            dicCallback = new Dictionary<int, Action<Entity>>();
            tempList = new List<int>();
        }

        public int ShowEntity<T>(EnumEntity enumEntity, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            return ShowEntity<T>((int)enumEntity, onShowSuccess, userData);
        }

        public int ShowEntity<T>(int entityId, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            int serialId = EntityManager.GenerateSerialId();
            dicCallback.Add(serialId, onShowSuccess);
            // GameObject obj = PoolObject.Instance.GetGameObject("AssaultCannonPreview", parent: entityRoot.transform);
            // if (obj.GetComponent<Entity>() != null)
            // {
            //     obj.GetComponent<Entity>().OnInit(serialId, "AssaultCannonPreview", true, null);
            //     dicSerial2Entity.Add(entityId, obj.GetComponent<Entity>());
            //     GameEvent.Send(EventDefine.OnShowEntitySuccess, serialId);
            // }
            EntityManager.ShowEntity<T>(serialId, entityId, userData);

            return serialId;
        }

        private void OnShowEntitySuccess(int _serialId)
        {
            Log.Debug("OnShowEntitySuccess");
            if (_serialId <= 0)
            {
                return;
            }

            Action<Entity> callback = null;
            if (!dicCallback.TryGetValue(_serialId, out callback))
            {
                return;
            }

            callback?.Invoke(dicSerial2Entity.GetValueOrDefault(_serialId));
        }

        private static void OnShowEntityFail()
        {
            Log.Debug("OnShowEntityFail");
        }

        public static EntityLoadCtrl Create(GameObject entityRoot)
        {
            EntityLoadCtrl entityLoaderCtrl = new EntityLoadCtrl();
            entityLoaderCtrl.entityRoot = entityRoot;
            GameEvent.AddEventListener<int>(EventDefine.OnShowEntitySuccess, entityLoaderCtrl.OnShowEntitySuccess);
            GameEvent.AddEventListener(EventDefine.OnShowEntityFail, OnShowEntityFail);

            return entityLoaderCtrl;
        }

        public void Clear()
        {
            GameEvent.RemoveEventListener<int>(EventDefine.OnShowEntitySuccess, OnShowEntitySuccess);
            GameEvent.RemoveEventListener(EventDefine.OnShowEntityFail, OnShowEntityFail);
            dicSerial2Entity.Clear();
            dicCallback.Clear();
        }
    }
}