using System;
using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityBase : ObjectBase
    {
        public int EntityId;
        public int EntityTypeId;
        
        public static GameLogic.View.EntityPlayer CreatePlayer(Vector3 PlayerPosition, Vector3 PlayereulerAngles, Transform parent)
        {
            GameObject Entity = PoolManager.Instance.GetGameObject("EntityPlayer");
            Entity.transform.parent = parent;
            Entity.transform.localPosition = PlayerPosition;
            Entity.transform.eulerAngles = PlayereulerAngles;
            return Entity.GetComponent<GameLogic.View.EntityPlayer>();
        }

        public static GameLogic.Module.EntityTower CreateTower(int entityid, TowerData data, Vector3 Position, Quaternion rotation, Transform parent)
        {
            Type procedureType = Type.GetType(string.Format("GameLogic.Module.{0}", data.Type));
            GameLogic.Module.EntityTower entitybase = (GameLogic.Module.EntityTower)MemoryPool.Acquire(procedureType);
            entitybase.EntityId = entityid;
            GameObject Entity = PoolManager.Instance.GetGameObject(data.NameId);
            Entity.transform.parent = parent;
            var entity= Entity.AddComponent<GameLogic.View.EntityTower>();
            entity.InitialPosition = Position;
            entity.Id = entityid;
            entity.towerData = data;
            entity.OnInit();
            entitybase.Initialize(entityid.ToString(), Entity);
            return entitybase;
        }

        protected override void Release(bool isShutdown)
        {
            
        }
    }
}
