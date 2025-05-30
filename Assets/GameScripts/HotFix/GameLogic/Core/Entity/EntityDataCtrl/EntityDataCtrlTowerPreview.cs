using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class EntityDataCtrlTowerPreview : EntityDataCtrl
    {
        public TowerData TowerData
        {
            get;
            private set;
        }

        public EntityDataCtrlTowerPreview() : base()
        {
            TowerData = null;
        }

        public static EntityDataCtrlTowerPreview Create(TowerData towerData, object userData = null)
        {
            EntityDataCtrlTowerPreview entityData = PoolReference.Acquire<EntityDataCtrlTowerPreview>();
            entityData.TowerData = towerData;
            return entityData;
        }

        public override void Clear()
        {
            base.Clear();
            TowerData = null;
        }
    }
}
