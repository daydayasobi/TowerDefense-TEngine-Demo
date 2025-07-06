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
    public class EntityControl : IMemory
    {
        // 实体字典
        public Dictionary<int, Dictionary<int, EntityBase>> EntityDic = new Dictionary<int, Dictionary<int, EntityBase>>();
        private Dictionary<int, Entity> dicSerial2Entity = new Dictionary<int, Entity>();
        int EntityId = 0;

        public static EntityControl Create()
        {
            EntityControl entityLoader = MemoryPool.Acquire<EntityControl>();
            return entityLoader;
        }

        public void AddEntityTower(int entityTypeId, Vector3 position, Quaternion rotation, Transform parent)
        {
            // todo: 需要优化对象池的运用。
            var towerType = TowerDataManger.Instance.GetItemConfig(entityTypeId);

            EntityBase towerEntity = PoolManager.Instance.GetObject<EntityBase>();
            // towerEntity.Clear();
            towerEntity = EntityBase.CreateTower(
                EntityId,
                towerType,
                position,
                rotation,
                parent
            );
            towerEntity.EntityTypeId = entityTypeId;
        }

        public void ShowTowerLevelEntity(int levelId, Action<Entity> onShowSuccess, object userData = null)
        {
            var data = TowerLevelDataManger.Instance.GetItemConfig(levelId);
            int serialId = EntityManager.Instance.GenerateSerialId();

            EntityData entityData = null;
            // TODO: 测试用
            if (userData.GetType() == typeof(EntityData))
            {
                entityData = (EntityData)userData;
            }

            string pathName = "";
            if (levelId == 10101)
            {
                pathName = "AssaultCannon_Level1";
            }
            else if (levelId == 10102)
            {
                pathName = "AssaultCannon_Level2";
            }

            if (String.IsNullOrEmpty(pathName))
            {
                Log.Error("Path name is null or empty for levelId: {0}", levelId);
                return;
            }

            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityTowerLevel entityTowerLevel = gameObject.GetComponent<EntityTowerLevel>();
            //初始化entity
            entity.OnInit(levelId, pathName);
            gameObject.transform.position = entityData.Position;
            gameObject.transform.rotation = entityData.Rotation;
            dicSerial2Entity.Add(serialId, entity);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerEntity(int entityId, object userData, Action<Entity> onShowSuccess)
        {
            var data = TowerDataManger.Instance.GetItemConfig(entityId);
            int serialId = EntityManager.Instance.GenerateSerialId();
            //获取预制体
            GameObject gameObject = PoolManager.Instance.GetGameObject(data.NameId);
            Entity entity = gameObject.GetComponent<Entity>();
            var towerbase = gameObject.GetComponent<EntityTowerBase>();
            //初始化entity
            entity.OnInit(entityId, data.NameId);
            //初始化entity logic
            towerbase.OnInit(userData);
            dicSerial2Entity.Add(serialId, entity.GetComponent<Entity>());
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

        private void HideTowerLevel(int seriaId)
        {
        }

        public void HideTowerEntity(int serialId)
        {
            Entity entity = null;
            if (!dicSerial2Entity.TryGetValue(serialId, out entity))
            {
                Log.Error("Can find entity('serial id:{0}') ", serialId);
            }

            dicSerial2Entity.Remove(serialId);

            // Entity[] entities = GameEntry.Entity.GetChildEntities(entity);
            // if (entities != null)
            // {
            //     foreach (var item in entities)
            //     {
            //         //若Child Entity由这个Loader对象托管，则由此Loader释放
            //         if (dicSerial2Entity.ContainsKey(item.Id))
            //         {
            //             HideEntity(item);
            //         }
            //         else//若Child Entity不由这个Loader对象托管，则从Parent Entity脱离
            //             GameEntry.Entity.DetachEntity(item);
            //     }
            // }
            //
            // GameEntry.Entity.HideEntity(entity);
        }


        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}