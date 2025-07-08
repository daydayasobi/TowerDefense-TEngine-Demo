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
            string pathName = AssetsDataLoader.Instance.GetItemConfig(entityId).ResourcesName;
            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityPlayerLogic entityLogic = gameObject.GetComponent<EntityPlayerLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, pathName, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            entity.OnShow(userData);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerLevelEntity(int entityId, Action<Entity> onShowSuccess, object userData = null)
        {
            int serialId = GameModule.Entity.GenerateSerialId();
            var data = TowerLevelDataLoader.Instance.GetItemConfig(entityId);
            string pathName = AssetsDataLoader.Instance.GetItemConfig(entityId).ResourcesName;
            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityTowerLevelLogic entityLogic = gameObject.GetComponent<EntityTowerLevelLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, pathName, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            entity.OnShow(userData);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerEntity(int entityId, int serialId, Action<Entity> onShowSuccess, object userData = null)
        {
            // int serialId = GameModule.Entity.GenerateSerialId();
            var data = TowerDataLoader.Instance.GetItemConfig(entityId);
            //获取预制体
            GameObject gameObject = PoolManager.Instance.GetGameObject(data.NameId);
            Entity entity = gameObject.GetComponent<Entity>();
            var entityLogic = gameObject.GetComponent<EntityTowerAttackerLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, data.NameId, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            entity.OnShow(userData);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowTowerPreviewEntity(int entityId, int serialId, Action<Entity> onShowSuccess, object userData = null)
        {
            // int serialId = GameModule.Entity.GenerateSerialId();
            string pathName = AssetsDataLoader.Instance.GetItemConfig(entityId).ResourcesName;
            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityTowerPreviewLogic entityLogic = gameObject.GetComponent<EntityTowerPreviewLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, pathName, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            entity.OnShow(userData);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowRadiusEntity(int entityId, int serialId, Action<Entity> onShowSuccess, object userData = null)
        {
            string pathName = AssetsDataLoader.Instance.GetItemConfig(entityId).ResourcesName;
            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityRadiusLogic entityLogic = gameObject.GetComponent<EntityRadiusLogic>();
            //初始化entity
            entity.OnInit(entityId, serialId, pathName, entityLogic);
            //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            entity.OnShow(userData);
            onShowSuccess?.Invoke(entity);
        }

        public void ShowEnemyEntity(int entityId, int serialId, Action<Entity> onShowSuccess, object userData = null)
        {
            var pathName = AssetsDataLoader.Instance.GetItemConfig(entityId).ResourcesName;
            // //获取预制体
            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            EntityEnemyLogic entityLogic = gameObject.GetComponent<EntityEnemyLogic>();
            // //初始化entity
            entity.OnInit(entityId, serialId, pathName, entityLogic);
            // //初始化entity logic
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            entity.OnShow(userData);
            onShowSuccess?.Invoke(entity);
        }

        public void HideEntity(Entity entity)
        {
            if (entity == null)
                return;

            // HideEntity(entity.SerialId);
            GameModule.Entity.HideEntity(entity.SerialId);
        }

        public void Clear()
        {
        }
    }
}