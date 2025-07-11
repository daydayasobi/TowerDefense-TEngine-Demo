using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEngine;
using UnityEngine;
using Object = System.Object;

namespace GameLogic
{
    public class EntityDataControl : Singleton<EntityDataControl>
    {
        public void ShowEntity(int entityId, int serialId,Type LogicType, Action<Entity> onShowSuccess, object userData = null)
        {
            ShowEntityEventData tempData = PoolReference.Acquire<ShowEntityEventData>();
            tempData.EntityId = entityId;
            tempData.SerialId = serialId;
            tempData.LogicType = LogicType;
            tempData.OnShowSuccess = onShowSuccess;
            tempData.UserData = userData;
            ShowEntity(tempData);
        }

        public void ShowEntity<TLogic>(
            int entityId,
            int manualSerialId,
            bool autoGenerateSerialId,
            Action<Entity> onShowSuccess,
            object userData = null
        )
            where TLogic : EntityLogic
        {
            int serialId = autoGenerateSerialId ? GameModule.Entity.GenerateSerialId() : manualSerialId;
            string pathName = AssetsDataLoader.Instance.GetItemConfig(entityId).ResourcesName;

            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();
            TLogic entityLogic = gameObject.GetComponent<TLogic>();

            entity.OnInit(entityId, serialId, pathName, entityLogic);
            entityLogic.OnInit(userData);
            GameModule.Entity.AddToDic(serialId, entity);
            entity.OnShow(userData);

            onShowSuccess?.Invoke(entity);
        }

        /// <summary>
        /// 测试用生成Entity
        /// </summary>
        /// <param name="entityId"></param>
        /// <param name="manualSerialId"></param>
        /// <param name="onShowSuccess"></param>
        /// <param name="userData"></param>
        public void ShowEntity<TLogic>(int entityId,
            int serialId,
            Action<Entity> onShowSuccess,
            object userData = null) where TLogic : EntityLogic
        {
            string pathName = AssetsDataLoader.Instance.GetItemConfig(entityId).ResourcesName;
            if (pathName == null)
            {
                Log.Error("EntityDataControl ShowEntity pathName is null, entityId: {0}", entityId);
                return;
            }

            GameObject gameObject = PoolManager.Instance.GetGameObject(pathName);
            Entity entity = gameObject.GetComponent<Entity>();

            // 泛型获取逻辑类
            TLogic entityLogic = gameObject.GetComponent<TLogic>();

            if (entityLogic == null)
            {
                throw new Exception($"GameObject {gameObject.name} 没有组件 {typeof(TLogic).Name}");
            }

            // 初始化Entity
            entity.OnInit(entityId, serialId, pathName, entityLogic);
            // 初始化EntityLogic
            entityLogic.OnInit(userData);

            GameModule.Entity.AddToDic(serialId, entity);
            entity.OnShow(userData);

            onShowSuccess?.Invoke(entity);
        }

        public void ShowEntity(ShowEntityEventData entityData)
        {
            var logicType = entityData.LogicType;
            
            Log.Debug("ShowEntity entityId:{0} ",entityData.EntityId);
            Action<Entity> callback = entity =>
            {
                entityData.OnShowSuccess?.Invoke(entity);
                entityData.Clear();
            };
            
            if (logicType == typeof(EntityPlayerLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityPlayerLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if(logicType == typeof(EntityTowerBaseLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityTowerBaseLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityTowerLevelLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityTowerLevelLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityTowerAttackerLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityTowerAttackerLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityTowerPreviewLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityTowerPreviewLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityRadiusLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityRadiusLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityEnemyLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityEnemyLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityProjectileHitscanLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityProjectileHitscanLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityTowerAttackerLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityTowerAttackerLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityTowerPreviewLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityTowerPreviewLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityEnemyLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityEnemyLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if (logicType == typeof(EntityRadiusLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityRadiusLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if(logicType == typeof(EntityParticleAutoHideLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityParticleAutoHideLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else if(logicType == typeof(EntityHPBarLogic))
            {
                EntityDataControl.Instance.ShowEntity<EntityHPBarLogic>(
                    entityData.EntityId,
                    entityData.SerialId,
                    callback,
                    entityData.UserData);
            }
            else
            {
                throw new InvalidOperationException($"未知的逻辑类型: {logicType.FullName}");
            }
        }


        public void HideEntity(Entity entity)
        {
            if (entity == null)
                return;

            GameModule.Entity.HideEntity(entity);
        }

        public void HideAllEntities()
        {
        }

        public IEnumerable<Entity> GetAllEntities()
        {
            return GameModule.Entity.GetAllEntities();
        }

        public void Clear()
        {
        }
    }

    public class EntityShowOptionsArgs
    {
        public int EntityId;
        public int ManualSerialId;
        public bool AutoGenerateSerialId = true;
        public Type LogicType;
        public Action<Entity> OnShowSuccess;
        public object UserData;

        public EntityShowOptionsArgs(
            int entityId,
            int manualSerialId,
            bool autoGenerateSerialId,
            Type logicType,
            Action<Entity> onShowSuccess,
            object userData)
        {
            EntityId = entityId;
            ManualSerialId = manualSerialId;
            AutoGenerateSerialId = autoGenerateSerialId;
            LogicType = logicType;
            this.OnShowSuccess = onShowSuccess;
            UserData = userData;
        }
    }

    public class ShowEntityEventData : IMemory
    {
        public int EntityId;
        public int SerialId;
        public Type LogicType;
        public Object UserData;
        public Action<Entity> OnShowSuccess = null;

        public void Clear()
        {
            EntityId = 0;
            SerialId = 0;
            LogicType = null;
            UserData = null;
            OnShowSuccess = null;
        }
    }
}