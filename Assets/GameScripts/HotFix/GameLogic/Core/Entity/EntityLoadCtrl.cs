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

        public int ShowEntity<T>(int entityId, Action<Entity> onShowSuccess) where T : EntityLogic
        {
            int serialId = EntityExtension.GenerateSerialId();
            dicCallback.Add(serialId, onShowSuccess);
            GameObject obj = PoolManager.Instance.GetGameObject("AssaultCannonPreview", parent: entityRoot.transform);
            if (obj.GetComponent<Entity>() != null)
            {
                Log.Debug("111111111111111111111");
            }
            else
            {
                Log.Debug("000000000000000000000000");
            }
            return serialId;
        }
        
        private static void OnShowEntitySuccess()
        {
            Log.Debug("OnShowEntitySuccess");
        }

        private static void OnShowEntityFail()
        {
            Log.Debug("OnShowEntityFail");
        }
        
        public static EntityLoadCtrl Create(GameObject entityRoot)
        {
            EntityLoadCtrl entityLoaderCtrl = new EntityLoadCtrl();
            entityLoaderCtrl.entityRoot = entityRoot;
            GameEvent.AddEventListener(EventDefine.OnShowEntitySuccess, OnShowEntitySuccess);
            GameEvent.AddEventListener(EventDefine.OnShowEntityFail, OnShowEntityFail);

            return entityLoaderCtrl;
        }
        
        public void Clear()
        {
            GameEvent.RemoveEventListener(EventDefine.OnShowEntitySuccess, OnShowEntitySuccess);
            GameEvent.RemoveEventListener(EventDefine.OnShowEntityFail, OnShowEntityFail);
            dicSerial2Entity.Clear();
            dicCallback.Clear();
        }
    }
}
