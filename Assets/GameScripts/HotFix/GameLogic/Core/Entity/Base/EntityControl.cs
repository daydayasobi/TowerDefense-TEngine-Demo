using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameLogic.View;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityControl : Singleton<EntityControl>
    {
        // 实体字典
        private Dictionary<int, Entity> dicSerial2Entity = new Dictionary<int, Entity>();
        int EntityId = 0;

        private static int serialId = 0;

        // public static EntityControl Create()
        // {
        //     EntityControl entityLoader = MemoryPool.Acquire<EntityControl>();
        //     return entityLoader;
        // }

        public int GenerateSerialId()
        {
            serialId += 1;
            Log.Debug("GenerateSerialId" + serialId);
            return serialId;
        }

        public void ShowPlayerEntity(int serialID, Action<Entity> onShowSuccess, object userData = null)
        {
            GameObject gameObject = PoolManager.Instance.GetGameObject(DataPlayerManager.Instance.AssetName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityPlayerLogic entityLogic = gameObject.GetComponent<EntityPlayerLogic>();
            //初始化entity
            entity.OnInit(0, serialId, DataPlayerManager.Instance.AssetName, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            dicSerial2Entity.Add(serialId, entity);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerLevelEntity(int entityId, int serialId, Action<Entity> onShowSuccess, object userData = null)
        {
            var data = TowerLevelDataManger.Instance.GetItemConfig(entityId);
            GameObject gameObject = PoolManager.Instance.GetGameObject(data.NameId);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityTowerLevelLogic entityLogic = gameObject.GetComponent<EntityTowerLevelLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, data.NameId, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            dicSerial2Entity.Add(serialId, entity);
            Log.Debug("ShowTowerLevelEntity serialId:{0} entity count:{1}", serialId, dicSerial2Entity.Count);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerEntity(int entityId, int serialId, object userData, Action<Entity> onShowSuccess)
        {
            var data = TowerDataManger.Instance.GetItemConfig(entityId);
            //获取预制体
            GameObject gameObject = PoolManager.Instance.GetGameObject(data.NameId);
            Entity entity = gameObject.GetComponent<Entity>();
            var entityLogic = gameObject.GetComponent<EntityTowerLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, data.NameId, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            dicSerial2Entity.Add(serialId, entity);
            Log.Debug("ShowTowerEntity serialId:{0} entity count:{1}", serialId, dicSerial2Entity.Count);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerPreview(int entityTypeId, Transform parent, Action<EntityTowerPreview> onShowSuccess)
        {
            string PreviewName = AssetsDataManger.Instance.GetItemConfig(entityTypeId).ResourcesName;
            var previewTowerGameObject = PoolManager.Instance.GetGameObject(PreviewName);
            previewTowerGameObject.transform.parent = parent;
            if (previewTowerGameObject == null)
            {
                Log.Error("Preview tower GameObject is null for entityTypeId: {0}", entityTypeId);
                return;
            }

            onShowSuccess?.Invoke(previewTowerGameObject.GetComponent<EntityTowerPreview>());
        }

        public void HideEntity(int serialId)
        {
            Entity entity = null;
            if (!dicSerial2Entity.TryGetValue(serialId, out entity))
            {
                Log.Error("Can find entity('serial id:{0}') ", serialId);
            }

            Log.Debug("HideEntity serialId:{0} entity count:{1}", serialId, dicSerial2Entity.Count);

            Entity tempEntity = dicSerial2Entity[serialId];
            List<int> childSerialIds = tempEntity.GetChildrenIds();
            dicSerial2Entity.Remove(serialId);

            if (childSerialIds.Count > 0)
            {
                foreach (var item in childSerialIds)
                {
                    if (dicSerial2Entity.ContainsKey(item))
                    {
                        HideEntity(item);
                    }
                }
            }

            tempEntity.OnHide(true, null);
            tempEntity.Clear();
        }

        public void HideEntity(Entity entity)
        {
            if (entity == null)
                return;

            HideEntity(entity.SerialId);
        }


        public void Clear()
        {
        }
    }
}