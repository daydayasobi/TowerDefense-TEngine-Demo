using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    partial class LevelControl : IMemory
    {
        public class TowerInfo : IMemory
        {
            public Tower Tower { get; private set; }

            public EntityTowerBaseLogic EntityTowerBase { get; private set; }

            public IPlacementArea PlacementArea { get; private set; }

            public IntVector2 PlaceGrid { get; private set; }

            public TowerInfo()
            {
                this.Tower = null;
                this.EntityTowerBase = null;
                this.PlacementArea = null;
                this.PlaceGrid = IntVector2.zero;
            }

            public static TowerInfo Create(Tower tower, EntityTowerBaseLogic entityTowerBase, IPlacementArea placementArea, IntVector2 placeGrid)
            {
                TowerInfo towerInfo = PoolReference.Acquire<TowerInfo>();
                towerInfo.Tower = tower;
                towerInfo.EntityTowerBase = entityTowerBase;
                towerInfo.PlacementArea = placementArea;
                towerInfo.PlaceGrid = placeGrid;
                return towerInfo;
            }
            
            public static TowerInfo Create(Tower tower, IPlacementArea placementArea, IntVector2 placeGrid)
            {
                TowerInfo towerInfo = PoolReference.Acquire<TowerInfo>();
                towerInfo.Tower = tower;
                // towerInfo.EntityTower = entityTower;
                towerInfo.PlacementArea = placementArea;
                towerInfo.PlaceGrid = placeGrid;
                return towerInfo;
            }

            public void Clear()
            {
                this.Tower = null;
                this.EntityTowerBase = null;
                this.PlacementArea = null;
                this.PlaceGrid = IntVector2.zero;
            }
        }
    }
}