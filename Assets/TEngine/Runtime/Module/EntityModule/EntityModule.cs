using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    public class EntityModule : Module, IUpdateModule, IEntityModule
    {
        private Dictionary<int, Entity> _dicSerial2Entity;

        // protected Dictionary<int, Entity> DicSerial2Entity { get; private set; }
        protected int serialId;
        private IEntityModule _entityModuleImplementation;

        /// <summary>
        /// 获取游戏框架模块优先级。
        /// </summary>
        /// <remarks>优先级较高的模块会优先轮询，并且关闭操作会后进行。</remarks>
        public override int Priority => 7;

        public override void OnInit()
        {
            _dicSerial2Entity = new Dictionary<int, Entity>();
            serialId = 0;
        }

        public override void Shutdown()
        {
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in _dicSerial2Entity.Values)
            {
                if (entity != null && entity.Logic != null)
                {
                    entity.Logic.OnUpdate(elapseSeconds, realElapseSeconds);
                }
            }
        }


        public int GenerateSerialId()
        {
            serialId += 1;
            return serialId;
        }

        public void HideEntity(Entity entity)
        {
            if (entity == null)
                return;

            Log.Debug("EntityModuleEx Hide Entity {0}", entity.SerialId);
            HideEntity(entity.SerialId);
        }

        public void HideEntity(int serialId)
        {
            Entity entity = null;
            if (!_dicSerial2Entity.TryGetValue(serialId, out entity))
            {
                Log.Error("Can find entity('serial id:{0}') ", serialId);
            }


            Entity tempEntity = _dicSerial2Entity[serialId];
            List<int> childSerialIds = tempEntity.GetChildrenIds();
            RemoveFromDic(serialId);

            Log.Debug("HideEntity serialId:{0} entity count:{1} childSerialIds:{2}", serialId, _dicSerial2Entity.Count, childSerialIds.Count);

            if (childSerialIds.Count > 0)
            {
                foreach (var item in childSerialIds)
                {
                    if (_dicSerial2Entity.ContainsKey(item))
                    {
                        HideEntity(item);
                    }
                }
            }

            tempEntity.OnHide(true, null);
            tempEntity.Clear();
        }

        public void HideAllEntity()
        {
        }

        public void AddToDic(int serialId, Entity entity)
        {
            if (!_dicSerial2Entity.ContainsKey(serialId))
            {
                _dicSerial2Entity.Add(serialId, entity);
                Log.Debug("AddToDic serialId:{0} entity count:{1}", serialId, _dicSerial2Entity.Count);
            }
            else
            {
                _dicSerial2Entity[serialId] = entity;
                Log.Debug("UpdateToDic serialId:{0} entity count:{1}", serialId, _dicSerial2Entity.Count);
            }
        }

        public void RemoveFromDic(int serialId)
        {
            if (_dicSerial2Entity.ContainsKey(serialId))
            {
                _dicSerial2Entity.Remove(serialId);
            }
        }

        public void Clear()
        {
        }
    }
}