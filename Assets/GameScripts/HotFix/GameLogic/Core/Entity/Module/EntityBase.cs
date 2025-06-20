using System;
using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic.View
{
    public class EntityBase : ObjectBase
    {
        public int EntityId;
        public int EntityTypeId;
        
        public static GameLogic.View.EntityPlayer CreatePlayer(Vector3 PlayerPosition, Vector3 PlayereulerAngles)
        {
            GameObject Entity = GameModule.Resource.LoadGameObject("EntityPlayer");
            Entity.transform.localPosition = PlayerPosition;
            Entity.transform.eulerAngles = PlayereulerAngles;
            return Entity.GetComponent<GameLogic.View.EntityPlayer>();
        }
        
        public static GameLogic.Module.EntityTower CreateTower(int entityid, TowerData data, Vector3 Position, Quaternion rotation)
        {
            Type procedureType = Type.GetType(string.Format("GameLogic.Module.{0}", data.Type));
            GameLogic.Module.EntityTower entitybase = (GameLogic.Module.EntityTower)MemoryPool.Acquire(procedureType);
            entitybase.EntityId = entityid;
            GameObject Entity = GameModule.Resource.LoadGameObject(data.NameId);
            var entity= Entity.AddComponent<GameLogic.View.EntityTower>();
            entity.InitialPosition = Position;
            entity.Id = entityid;
            entity.towerData = data;
            entity.OnInit();
            entitybase.Initialize(entityid.ToString(), Entity);
            return entitybase;
        }
        
        /// <summary>
        /// 释放对象
        /// </summary>
        /// <param name="isShutdown">是否是关闭对象池时触发</param>
        protected override void Release(bool isShutdown)
        {
            
        }
    }
}
