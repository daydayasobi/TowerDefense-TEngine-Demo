using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;
using System;

namespace GameLogic
{
    public class EntityManager : MonoBehaviour
    {
        private static EntityManager _instance;

        public static EntityManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Utility.Unity.FindObjectOfType<EntityManager>();

                    if (_instance != null)
                    {
                        return _instance;
                    }
                }

                return _instance;
            }
        }

        private static int s_SerialId = 0;

        private static Transform s_entityRoot;

        public static int GenerateSerialId()
        {
            return ++s_SerialId;
        }

        public static void SetEntityRoot(Transform entityRoot)
        {
            s_entityRoot = entityRoot;
        }

        public static void ShowEntity()
        {
            Log.Debug("ShowEntity not implemented in EntityManager");
        }

        public static void ShowEntity<T>(int serialId, int entityId,  object userData = null)
        {
            //测试用
            string assetName = null;
            switch (entityId)
            {
                case 1001: assetName = "AssaultCannonPreview";
                    break;
            }
            if (string.IsNullOrEmpty(assetName))
            {
                Log.Error("Entity ID {0} does not have a corresponding asset name.", entityId);
                return;
            }
            // GameObject entity = PoolObject.Instance.GetGameObject(assetName, parent: s_entityRoot).GetOrAddComponent<Entity>();
             // if (entity.GetComponent<Entity>() != null)
             // {
             //     entity.GetComponent<Entity>().OnInit(serialId, "AssaultCannonPreview", true, null);
             //     dicSerial2Entity.Add(entityId, obj.GetComponent<Entity>());
             //     GameEvent.Send(EventDefine.OnShowEntitySuccess, serialId);
             // }
        }

        private static void CreateEntity()
        {
            
        }

        public static int ShowEntity(int entityId, int entityGroupId, object userData = null)
        {
            return 0;
        }
    }
}