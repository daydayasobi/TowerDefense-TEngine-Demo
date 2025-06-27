using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class TowerDataBase
    {
        private TowerData dRTower;
        private TowerLevelDataBase[] towerLevels;
        
        public int Id
        {
            get
            {
                return dRTower.Id;
            }
        }

        public string Name
        {
            get
            {
                // TODO: 这里需要根据实际的本地化实现来获取塔的名称
                // return GameEntry.Localization.GetString(dRTower.NameId);
                return dRTower.NameId;
            }
        }

        public string Icon
        {
            get
            {
                return dRTower.Icon;
            }
        }

        public int EntityId
        {
            get
            {
                return dRTower.Id;
            }
        }

        public int PreviewEntityId
        {
            get
            {
                return dRTower.PreviewEntityid;
            }
        }

        public int ProjectileEntityId
        {
            get
            {
                return dRTower.ProjectileEntityid;
            }
        }

        // public string ProjectileType
        // {
        //     get
        //     {
        //         return dRTower.ProjectileType;
        //     }
        // }

        public bool IsMultiAttack
        {
            get
            {
                return dRTower.IsMultiAttack;
            }
        }

        public float MaxHP
        {
            get
            {
                return dRTower.MaxHp;
            }
        }

        public IntVector2 Dimensions
        {
            get;
            private set;
        }

        public string Type
        {
            get
            {
                return dRTower.Type;
            }
        }
        
        public TowerDataBase(TowerData dRTower, TowerLevelDataBase[] towerLevels)
        {
            this.dRTower = dRTower;
            this.towerLevels = towerLevels;

            List<int> dimensions = dRTower.Dimensions;
            if (dimensions == null || dimensions.Count < 2)
            {
                Log.Error("Tower ('{0}') dimensions data invaild", dRTower.Id);
                return;
            }

            Dimensions = new IntVector2(dimensions[0], dimensions[1]);
        }

        public TowerLevelDataBase GetTowerLevelData(int level)
        {
            if (towerLevels == null || level > GetMaxLevel())
                return null;

            return towerLevels[level];
        }

        public int GetMaxLevel()
        {
            return towerLevels == null ? 0 : towerLevels.Length - 1;
        }
    }
}
