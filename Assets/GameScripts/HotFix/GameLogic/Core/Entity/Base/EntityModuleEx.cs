using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityModuleEx : Singleton<EntityModuleEx>
    {
        public void ShowPlayerEntity(int entityId, Action<Entity> onShowSuccess, object userData = null)
        {
            int serialId = GameModule.Entity.GenerateSerialId();
            string pathName = AssetsDataManger.Instance.GetItemConfig(entityId).ResourcesName;
            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityPlayerLogic entityLogic = gameObject.GetComponent<EntityPlayerLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, pathName, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerLevelEntity(int entityId, Action<Entity> onShowSuccess, object userData = null)
        {
            int serialId = GameModule.Entity.GenerateSerialId();
            var data = TowerLevelDataManger.Instance.GetItemConfig(entityId);
            string pathName = AssetsDataManger.Instance.GetItemConfig(entityId).ResourcesName;
            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityTowerLevelLogic entityLogic = gameObject.GetComponent<EntityTowerLevelLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, pathName, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerEntity(int entityId,int serialId, Action<Entity> onShowSuccess, object userData = null)
        {
            // int serialId = GameModule.Entity.GenerateSerialId();
            var data = TowerDataManger.Instance.GetItemConfig(entityId);
            //获取预制体
            GameObject gameObject = PoolManager.Instance.GetGameObject(data.NameId);
            Entity entity = gameObject.GetComponent<Entity>();
            var entityLogic = gameObject.GetComponent<EntityTowerLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, data.NameId, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerPreview(int entityId,int serialId ,Action<Entity> onShowSuccess, object userData = null)
        {
            // int serialId = GameModule.Entity.GenerateSerialId();
            string pathName = AssetsDataManger.Instance.GetItemConfig(entityId).ResourcesName;
            var gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityTowerPreviewLogic entityLogic = gameObject.GetComponent<EntityTowerPreviewLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, pathName, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            onShowSuccess?.Invoke(entity);
        }

        // public void HideEntity(int serialId)
        // {
        //     Entity entity = null;
        //     if (!DicSerial2Entity.TryGetValue(serialId, out entity))
        //     {
        //         Log.Error("Can find entity('serial id:{0}') ", serialId);
        //     }
        //
        //     Log.Debug("HideEntity serialId:{0} entity count:{1}", serialId, DicSerial2Entity.Count);
        //
        //     Entity tempEntity = DicSerial2Entity[serialId];
        //     List<int> childSerialIds = tempEntity.GetChildrenIds();
        //     RemoveFromDic(serialId);
        //
        //     if (childSerialIds.Count > 0)
        //     {
        //         foreach (var item in childSerialIds)
        //         {
        //             if (DicSerial2Entity.ContainsKey(item))
        //             {
        //                 HideEntity(item);
        //             }
        //         }
        //     }
        //
        //     tempEntity.OnHide(true, null);
        //     tempEntity.Clear();
        // }

        public void HideEntity(Entity entity)
        {
            if (entity == null)
                return;

            // HideEntity(entity.SerialId);
            GameModule.Entity.HideEntity(entity.Id);
        }

        public void Clear()
        {
        }
    }
}