using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class DRTower
    {
        public int Id { get; set; }
        public int NameId { get; set; }
        public string Icon { get; set; }
        public int EntityId { get; set; }
        public int PreviewEntityId { get; set; }
        public int ProjectileEntityId { get; set; }
        public int MaxHP { get; set; }
        
        public IntVector2 Dimensions { get; set; }
        public string Type { get; set; }
    }

    public class TowerData
    {
        private DRTower dRTower;
        private TowerLevelData[] towerLevels;

        public int Id
        {
            get { return dRTower.Id; }
        }

        public string Name
        {
            get { return dRTower.NameId.ToString(); }
        }

        public string Icon
        {
            get { return dRTower.Icon; }
        }

        public int EntityId
        {
            get { return dRTower.EntityId; }
        }

        public int PreviewEntityId
        {
            get { return dRTower.PreviewEntityId; }
        }

        public int ProjectileEntityId
        {
            get { return dRTower.ProjectileEntityId; }
        }

        // public string ProjectileType
        // {
        //     get { return dRTower.ProjectileType; }
        // }
        //
        // public bool IsMultiAttack
        // {
        //     get { return dRTower.IsMultiAttack; }
        // }

        public float MaxHP
        {
            get { return dRTower.MaxHP; }
        }

        public IntVector2 Dimensions { get; private set; }

        public string Type
        {
            get { return dRTower.Type; }
        }

        public TowerData(DRTower dRTower, TowerLevelData[] towerLevels)
        {
            this.dRTower = dRTower;
            this.towerLevels = towerLevels;
        }
        
        public TowerData(DRTower dRTower)
        {
            this.dRTower = dRTower;
        }

        public TowerLevelData GetTowerLevelData(int level)
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