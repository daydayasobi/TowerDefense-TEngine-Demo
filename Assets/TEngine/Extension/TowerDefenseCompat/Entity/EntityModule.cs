using System;
using System.Collections.Generic;

namespace TEngine
{
    public class EntityModule : Module, IUpdateModule, IEntityModule
    {
        private Dictionary<int, Entity> _entities;
        private int _serialId;

        public override int Priority => 7;

        public override void OnInit()
        {
            _entities = new Dictionary<int, Entity>();
            _serialId = 0;
        }

        public override void Shutdown()
        {
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            try
            {
                var entities = new List<Entity>(_entities.Values);
                foreach (var entity in entities)
                {
                    if (entity != null && entity.Logic != null)
                    {
                        entity.Logic.OnUpdate(elapseSeconds, realElapseSeconds);
                    }
                }
            }
            catch (Exception exception)
            {
                Log.Error("EntityModule update failed: {0}", exception);
                throw;
            }
        }

        public int GenerateSerialId()
        {
            _serialId += 1;
            return _serialId;
        }

        public void HideEntity(Entity entity)
        {
            if (entity == null)
            {
                return;
            }

            Log.Debug("EntityModule Hide Entity {0}", entity.SerialId);
            HideEntity(entity.SerialId);
        }

        public void HideEntity(int serialId)
        {
            if (!_entities.TryGetValue(serialId, out var entity))
            {
                Log.Error("Can not find entity('serial id:{0}')", serialId);
                return;
            }

            var childSerialIds = entity.GetChildrenIds();
            RemoveFromDic(serialId);

            Log.Debug("HideEntity serialId:{0} entity count:{1} childSerialIds:{2}", serialId, _entities.Count, childSerialIds.Count);

            if (childSerialIds.Count > 0)
            {
                foreach (var childSerialId in childSerialIds)
                {
                    if (_entities.ContainsKey(childSerialId))
                    {
                        HideEntity(childSerialId);
                    }
                }
            }

            entity.OnHide(true, null);
            entity.Clear();
        }

        public void HideAllEntity()
        {
            var entitySerialIds = new List<int>(_entities.Keys);
            for (var i = 0; i < entitySerialIds.Count; i++)
            {
                HideEntity(entitySerialIds[i]);
            }
        }

        public void AddToDic(int serialId, Entity entity)
        {
            if (!_entities.ContainsKey(serialId))
            {
                _entities.Add(serialId, entity);
                Log.Debug("AddToDic serialId:{0} entity count:{1}", serialId, _entities.Count);
            }
            else
            {
                _entities[serialId] = entity;
                Log.Debug("UpdateToDic serialId:{0} entity count:{1}", serialId, _entities.Count);
            }
        }

        public void RemoveFromDic(int serialId)
        {
            if (_entities.ContainsKey(serialId))
            {
                _entities.Remove(serialId);
            }
        }

        public IEnumerable<Entity> GetAllEntities()
        {
            return _entities.Values;
        }

        public void Clear()
        {
        }
    }
}
