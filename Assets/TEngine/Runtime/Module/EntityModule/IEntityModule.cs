using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace TEngine
{
    public interface IEntityModule
    {
        public int GenerateSerialId();
        
        public void HideEntity(int serialId);
        public void HideAllEntity();
        public void HideEntity(Entity entity);

        public void AddToDic(int serialId, Entity entity);
        public void RemoveFromDic(int serialId);

        public IEnumerable<Entity> GetAllEntities();

        public void Clear();
    }
}