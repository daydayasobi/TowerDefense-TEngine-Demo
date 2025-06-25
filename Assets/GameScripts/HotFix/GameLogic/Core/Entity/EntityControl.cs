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
            towerEntity.Clear();
            towerEntity = EntityBase.CreateTower(
                EntityId,
                towerType,
                position,
                rotation,
                parent
            );
            towerEntity.EntityTypeId = entityTypeId;
        }

        public void ShowEntity<T>(int entityTypeId, Transform parent, object userData = null) where T : EntityLogic
        {
            var towerType = TowerDataManger.Instance.GetItemConfig(entityTypeId);
            //
            // Log.Debug("ShowEntity: {0}", entityTypeId);
            // // EntityBase towerEntity = PoolManager.Instance.GetObject<EntityBase>();
            //
            // GameObject entity = PoolManager.Instance.GetGameObject(AssetsDataManger.Instance.GetItemConfig(towerData.PreviewEntityid).ResourcesName, parent: parent).GetOrAddComponent<Entity>();
            // if (entity.GetComponent<Entity>() != null)
            // {
            //     entity.GetComponent<Entity>().OnInit(serialId, "AssaultCannonPreview", true, null);
            //     // dicSerial2Entity.Add(entityId, obj.GetComponent<Entity>());
            //     // GameEvent.Send(EventDefine.OnShowEntitySuccess, serialId);
            // }
        }

        public void ShowTowerEntity()
        {
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