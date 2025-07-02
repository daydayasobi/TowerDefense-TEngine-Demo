using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class DataTower
    {
        private Dictionary<int, TowerDataBase> dicTowerData;
        private Dictionary<int, TowerLevelDataBase> dicTowerLevelData;
        private Dictionary<int, Tower> dicTower;

        private int serialId = 0;
        
        protected void OnInit()
        {

        }

        protected void OnLoad()
        {
            
        }
    }
}
