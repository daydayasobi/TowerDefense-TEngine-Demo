using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class TowerData 
    {
        // private DRTower dRTower;
        private TowerLevelData[] towerLevels;

        public int Id
        {
            get
            {
                return 101;
            }
        }

        public string Name
        {
            get
            {
                return "AssaultCannon_Level1";
            }
        }

        // public string Icon
        // {
        //     get
        //     {
        //         return dRTower.Icon;
        //     }
        // }

        public int EntityId
        {
            get
            {
                return 1007;
            }
        }

        public int PreviewEntityId
        {
            get
            {
                return 1017;
            }
        }

        public bool IsMultiAttack
        {
            get
            {
                return false;
            }
        }

        public float MaxHP
        {
            get
            {
                return 5;
            }
        }
        
        public IntVector2 Dimensions
        {
            get;
            private set;
        }
    }
}
