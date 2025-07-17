using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class TowerLevelDataBase
    {
        private TowerLevelData dRTowerLevel;
        private ProjectileDataBase projectileData;

        public int Id
        {
            get
            {
                return dRTowerLevel.Id;
            }
        }

        public string Des
        {
            get
            {
                if (string.IsNullOrEmpty(dRTowerLevel.Desid))
                    return string.Empty;
                
                return TEngine.Localization.LocalizationManager.GetTermTranslation(dRTowerLevel.Desid);
            }
        }

        public string UpgradeDes
        {
            get
            {
                if (string.IsNullOrEmpty(dRTowerLevel.UpgradeDesid))
                    return string.Empty;
                
                return TEngine.Localization.LocalizationManager.GetTermTranslation(dRTowerLevel.Desid);
            }
        }

        public int LevelId
        {
            get
            {
                return dRTowerLevel.Id;
            }
        }

        public int EntityId
        {
            get
            {
                return dRTowerLevel.Entityid;
            }
        }

        public ProjectileDataBase ProjectileData
        {
            get
            {
                return projectileData;
            }
        }

        public float Damage
        {
            get
            {
                if (projectileData == null)
                    return 0;
                else
                    return projectileData.Damage;
            }
        }

        public float SplashDamage
        {
            get
            {
                if (projectileData == null)
                    return 0;
                else
                    return projectileData.SplashDamage;
            }
        }

        public float SplashRange
        {
            get
            {
                if (projectileData == null)
                    return 0;
                else
                    return projectileData.SplashRange;
            }
        }

        public float FireRate
        {
            get
            {
                return dRTowerLevel.FireRate;
            }
        }

        public float Range
        {
            get
            {
                return dRTowerLevel.Range;
            }
        }

        public float SpeedDownRate
        {
            get
            {
                return dRTowerLevel.SpeedDewnRate;
            }
        }

        public float EnergyRaise
        {
            get
            {
                return dRTowerLevel.EnergyRaise;
            }
        }

        public float EnergyRaiseRate
        {
            get
            {
                return dRTowerLevel.EnergyRaiseRate;
            }
        }

        public float DPS
        {
            get
            {
                return (Damage + SplashDamage) * FireRate;
            }
        }

        public int BuildEnergy
        {
            get
            {
                return dRTowerLevel.BuildEnergy;
            }
        }

        public int SellEnergy
        {
            get
            {
                return dRTowerLevel.SellEnergy;
            }
        }

        public TowerLevelDataBase(TowerLevelData dRTowerLevel, ProjectileDataBase projectileData)
        {
            this.dRTowerLevel = dRTowerLevel;
            this.projectileData = projectileData;
        }
    }
}
