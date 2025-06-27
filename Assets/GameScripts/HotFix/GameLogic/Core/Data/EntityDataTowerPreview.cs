using System;
using UnityEngine;
using GameLogic;
using TEngine;
using TowerData = GameConfig.TowerData;

namespace GameLogic
{
    [Serializable]
    public class EntityDataTowerPreview: IMemory
    {
        public TowerDataBase TowerData
        {
            get;
            private set;
        }

        public IntVector2 Dimensions
        {
            // get { return new IntVector2(TowerData.Dimensions[0], TowerData.Dimensions[1]); }
            get { return TowerData.Dimensions; }
            set { }
        }

        public EntityDataTowerPreview() : base()
        {
            TowerData = null;
        }

        public static EntityDataTowerPreview Create(TowerDataBase towerData, object userData = null)
        {
            EntityDataTowerPreview entityData = MemoryPool.Acquire<EntityDataTowerPreview>();
            entityData.TowerData = towerData;
            return entityData;
        }

        public  void Clear()
        {
            TowerData = null;
        }
    }
}


