using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameConfig;
using TEngine;
using UnityEngine;
using Object = System.Object;

namespace GameLogic
{
    public class EntityDataControl : Singleton<EntityDataControl>
    {
        private static readonly Dictionary<EnumProjectile, Type> _projectileLogicMap = new()
        {
            { EnumProjectile.EntityProjectileHitscanLogic, typeof(EntityProjectileHitscanLogic) },
            { EnumProjectile.EntityProjectileBallisticLogic, typeof(EntityProjectileBallisticLogic) },
            { EnumProjectile.EntityEnergyPylonLogic, typeof(EntityEnergyPylonLogic) },
            { EnumProjectile.EntityEMPGeneratorLogic, typeof(EntityEMPGeneratorLogic) },
            { EnumProjectile.EntityProjectileWobblingHomingLogic, typeof(EntityProjectileWobblingHomingLogic) }
        };
        
        private static readonly Dictionary<Type, Action<int, int, Action<Entity>, object>> _logicMap = new()
        {
            { typeof(EntityPlayerLogic), (id, serial, cb, data) => ShowEntity<EntityPlayerLogic>(id, serial, cb, data) },
            { typeof(EntityRocketPlatformLogic), (id, serial, cb, data) => ShowEntity<EntityRocketPlatformLogic>(id, serial, cb, data) },
            { typeof(EntityTowerBaseLogic), (id, serial, cb, data) => ShowEntity<EntityTowerBaseLogic>(id, serial, cb, data) },
            { typeof(EntityTowerLevelLogic), (id, serial, cb, data) => ShowEntity<EntityTowerLevelLogic>(id, serial, cb, data) },
            { typeof(EntityTowerAttackerLogic), (id, serial, cb, data) => ShowEntity<EntityTowerAttackerLogic>(id, serial, cb, data) },
            { typeof(EntityTowerPreviewLogic), (id, serial, cb, data) => ShowEntity<EntityTowerPreviewLogic>(id, serial, cb, data) },
            { typeof(EntityRadiusLogic), (id, serial, cb, data) => ShowEntity<EntityRadiusLogic>(id, serial, cb, data) },
            { typeof(EntityEnemyLogic), (id, serial, cb, data) => ShowEntity<EntityEnemyLogic>(id, serial, cb, data) },
            { typeof(EntityProjectileHitscanLogic), (id, serial, cb, data) => ShowEntity<EntityProjectileHitscanLogic>(id, serial, cb, data) },
            { typeof(EntityParticleAutoHideLogic), (id, serial, cb, data) => ShowEntity<EntityParticleAutoHideLogic>(id, serial, cb, data) },
            { typeof(EntityHPBarLogic), (id, serial, cb, data) => ShowEntity<EntityHPBarLogic>(id, serial, cb, data) },
            { typeof(EntityProjectileBallisticLogic), (id, serial, cb, data) => ShowEntity<EntityProjectileBallisticLogic>(id, serial, cb, data) },
            { typeof(EntityHideSelfProjectileLogic), (id, serial, cb, data) => ShowEntity<EntityHideSelfProjectileLogic>(id, serial, cb, data) },
            { typeof(EntityProjectileLogic), (id, serial, cb, data) => ShowEntity<EntityProjectileLogic>(id, serial, cb, data) },
            { typeof(EntityEnergyPylonLogic), (id, serial, cb, data) => ShowEntity<EntityEnergyPylonLogic>(id, serial, cb, data) },
            { typeof(EntityEMPGeneratorLogic), (id, serial, cb, data) => ShowEntity<EntityEMPGeneratorLogic>(id, serial, cb, data) },
            { typeof(EntityProjectileWobblingHomingLogic), (id, serial, cb, data) => ShowEntity<EntityProjectileWobblingHomingLogic>(id, serial, cb, data) },
            { typeof(EntityPlasmaLanceLogic), (id, serial, cb, data) => ShowEntity<EntityPlasmaLanceLogic>(id, serial, cb, data) },
            { typeof(EntityAnimationLogic), (id, serial, cb, data) => ShowEntity<EntityAnimationLogic>(id, serial, cb, data) },
        };
        
        public static Type GetProjectileLogicType(EnumProjectile projectileType)
        {
            if (_projectileLogicMap.TryGetValue(projectileType, out var logicType))
            {
                return logicType;
            }

            Log.Error("未找到对应的 Projectile 逻辑类型: {0}", projectileType);
            return null;
        }
        
        public static void ShowEntity<TLogic>(int entityId,
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
            if (!_logicMap.TryGetValue(entityData.LogicType, out var action))
            {
                throw new InvalidOperationException($"未知的逻辑类型: {entityData.LogicType.FullName}");
            }

            Log.Debug("ShowEntity entityId:{0} ", entityData.EntityId);

            Action<Entity> callback = entity =>
            {
                entityData.OnShowSuccess?.Invoke(entity);
                entityData.Clear();
            };

            action.Invoke(entityData.EntityId, entityData.SerialId, callback, entityData.UserData);
        }


        public void HideEntity(Entity entity)
        {
            if (entity == null)
                return;

            GameModule.Entity.HideEntity(entity);
        }

        public void HideAllEntities()
        {
            Log.Debug("HideAllEntities");
            GameModule.Entity.HideAllEntity();
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