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
        private Dictionary<int, Entity> dicSerial2Entity;
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

        public void ShowTowerEntity(int entityTypeId, Vector3 position, Quaternion rotation, Transform parent)
        {
            // var towerType = TowerDataManger.Instance.GetItemConfig(entityTypeId);
            //
            // EntityBase towerEntity = PoolManager.Instance.GetObject<EntityBase>();
            // towerEntity.Clear();
            // towerEntity = EntityBase.CreateTower(
            //     EntityId,
            //     towerType,
            //     position,
            //     rotation,
            //     parent
            // );
            // towerEntity.EntityTypeId = entityTypeId;
        }

        public void ShowTowerEntity(int entityId, EntityDataTower entityDataTower, Action<EntityTowerBase> onShowSuccess)
        {
            // int serialId = EntityManager.Instance.GenerateSerialId();

            // ShowEntityInfo.Create(entityLogicType, userData);

            // EntityTowerBase towerEntity = EntityManager.Instance.ShowTowerEntity(entityId, userData);

            var data = TowerDataManger.Instance.GetItemConfig(entityId);
            int serialId = EntityManager.Instance.GenerateSerialId();
            GameObject Entity = PoolManager.Instance.GetGameObject(data.NameId);
            EntityTowerBase entitybase = (EntityTowerBase)PoolReference.Acquire(typeof(EntityTowerBase));
            var entity = Entity.AddComponent(entitybase.GetType());
            entitybase.OnInit(entityDataTower);
            entity.transform.position = entityDataTower.Position;
            entity.transform.parent = entityDataTower.Parent;
            entity.transform.rotation = entityDataTower.Rotation;

            onShowSuccess?.Invoke(entitybase);
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


        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}