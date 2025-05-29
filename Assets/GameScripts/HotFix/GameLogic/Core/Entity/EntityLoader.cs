using System;
using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityLoader : MonoBehaviour
    {
        private Dictionary<int, Action<Entity>> dicCallback;
        private Dictionary<int, Entity> dicSerial2Entity;

        private List<int> tempList;

        //test
        private int serialIdTest = 0;
        private GameObject entityRoot;

        public EntityLoader()
        {
            dicSerial2Entity = new Dictionary<int, Entity>();
            dicCallback = new Dictionary<int, Action<Entity>>();
            tempList = new List<int>();
        }

        public int ShowEntity<T>(int entityId, Action<Entity> onShowSuccess, object userData = null) where T : EntityLogic
        {
            int serialId = serialIdTest++;
            dicCallback.Add(serialId, onShowSuccess);
            // GameEntry.Entity.ShowEntity<T>(serialId, entityId, userData);
            // Entity entity = new GameObject("AssaultCannon_Level1").AddComponent<Entity>();
            // entity.transform.SetParent(entityRoot.transform);
            Entity entity = PoolManager.Instance.GetGameObject("AssaultCannonPreview", parent: entityRoot.transform).AddComponent<Entity>();
            entity.OnInit(1, "AssaultCannonPreview", true, userData);
            return serialId;
        }

        public void HideEntity(int serialId)
        {
        }

        private static void OnShowEntitySuccess()
        {
            Log.Debug("OnShowEntitySuccess");
        }

        private static void OnShowEntityFail()
        {
            Log.Debug("OnShowEntityFail");
        }

        public static EntityLoader Create(GameObject entityRoot)
        {
            EntityLoader entityLoader = entityRoot.AddComponent<EntityLoader>();
            entityLoader.entityRoot = entityRoot;
            GameEvent.AddEventListener(EventDefine.OnShowEntitySuccess, OnShowEntitySuccess);
            GameEvent.AddEventListener(EventDefine.OnShowEntityFail, OnShowEntityFail);

            return entityLoader;
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