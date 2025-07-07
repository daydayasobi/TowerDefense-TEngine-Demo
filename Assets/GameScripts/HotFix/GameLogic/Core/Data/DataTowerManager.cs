using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class DataTowerManager : Singleton<DataTowerManager>
    {
        private Dictionary<int, TowerDataBase> dicTowerData;
        private Dictionary<int, TowerLevelDataBase> dicTowerLevelData;
        private Dictionary<int, Tower> dicTower;
        private int serialId = 0;

        public DataTowerManager()
        {
            dicTowerLevelData = new Dictionary<int, TowerLevelDataBase>();
            dicTowerData = new Dictionary<int, TowerDataBase>();
            dicTower = new Dictionary<int, Tower>();

            List<ProjectileData> dataProjectile = ProjectileDataLoader.Instance.GetAllItemConfig();

            List<TowerLevelData> drTowerLevels = TowerLevelDataLoader.Instance.GetAllItemConfig();
            foreach (var drTowerLevel in drTowerLevels)
            {
                if (dicTowerLevelData.ContainsKey(drTowerLevel.Id))
                {
                    throw new System.Exception(string.Format("Data tower level id '{0}' duplicate.", drTowerLevel.Id));
                }

                ProjectileDataBase projectileData = new ProjectileDataBase(ProjectileDataLoader.Instance.GetItemConfig(drTowerLevel.ProjectileData));
                TowerLevelDataBase towerLevelData = new TowerLevelDataBase(drTowerLevel, projectileData);
                dicTowerLevelData.Add(drTowerLevel.Id, towerLevelData);
            }

            List<TowerData> drTowers = TowerDataLoader.Instance.GetAllItemConfig();
            foreach (var drTower in drTowers)
            {
                TowerLevelDataBase[] towerLevelDatas = new TowerLevelDataBase[drTower.Levels.Count];
                for (int i = 0; i < drTower.Levels.Count; i++)
                {
                    if (!dicTowerLevelData.ContainsKey(drTower.Levels[i]))
                    {
                        throw new System.Exception(string.Format("Can not find tower level id '{0}' in DataTable TowerLevel.", drTower.Levels[i]));
                    }

                    towerLevelDatas[i] = dicTowerLevelData[drTower.Levels[i]];
                }

                TowerDataBase towerData = new TowerDataBase(drTower, towerLevelDatas);
                dicTowerData.Add(drTower.Id, towerData);
            }
        }

        public TowerDataBase GetTowerData(int id)
        {
            if (!dicTowerData.ContainsKey(id))
            {
                Log.Error("Can not find tower data id '{0}'.", id);
                return null;
            }

            return dicTowerData[id];
        }

        private int GenerateSerialId()
        {
            return ++serialId;
        }

        public Tower CreateTower(int towerId, int level = 0)
        {
            if (!dicTowerData.ContainsKey(towerId))
            {
                Log.Error("Can not find tower data id '{0}'.", towerId);
                return null;
            }

            int serialId = GenerateSerialId();
            Tower tower = Tower.Create(dicTowerData[towerId], serialId, level);
            dicTower.Add(serialId, tower);

            return tower;
        }

        public void DestroyTower(int serialId)
        {
            if (!dicTower.ContainsKey(serialId))
            {
                Log.Error("Can not find tower serialId '{0}'.", serialId);
                return;
            }

            PoolReference.Release(dicTower[serialId]);
            dicTower.Remove(serialId);
        }

        public void DestroyTower(Tower tower)
        {
            DestroyTower(tower.SerialId);
        }

        public void UpgradeTower(int serialId)
        {
            if (!dicTower.ContainsKey(serialId))
            {
                Log.Error("Can not find tower serialId '{0}'.", serialId);
                return;
            }

            Tower tower = dicTower[serialId];
            if (tower.Level >= tower.MaxLevel)
            {
                Log.Error("Tower (id:'{0}') has reached the highest level {1}", serialId, tower.MaxLevel);
                return;
            }

            int needEnergy = tower.GetBuildEnergy(tower.Level + 1);
            if (DataPlayerManager.Instance.Energy < needEnergy)
            {
                Log.Error("Energy lack,need {0},current is {1}", needEnergy, DataPlayerManager.Instance.Energy);
                return;
            }
            
            DataPlayerManager.Instance.AddEnergy(-needEnergy);

            int lastLevel = tower.Level;

            tower.Upgrade();
            GameEvent.Send(LevelEvent.OnUpgradeTower, tower);
        }

        public void UpgradeTower(Tower tower)
        {
            UpgradeTower(tower.SerialId);
        }

        public void SellTower(int serialId)
        {
            if (!dicTower.ContainsKey(serialId))
            {
                Log.Error("Can not find tower serialId '{0}'.", serialId);
                return;
            }

            // DataManager dataLevel = GameEntry.Data.GetData<DataLevel>();

            // if (dataLevel.LevelState != EnumLevelState.Prepare && dataLevel.LevelState != EnumLevelState.Normal)
            // {
            //     return;
            // }

            Tower tower = dicTower[serialId];
            DataPlayerManager.Instance.AddEnergy(tower.SellEnergy);
            GameEvent.Send(LevelEvent.OnSellTower, tower.SerialId);

            // DataPlayer dataPlayer = GameEntry.Data.GetData<DataPlayer>();
            // if (dataLevel.LevelState == EnumLevelState.Prepare)
            // {
            //     dataPlayer.AddEnergy(tower.TotalCostEnergy);
            // }
            // else if (dataLevel.LevelState == EnumLevelState.Normal)
            // {
            //     dataPlayer.AddEnergy(tower.SellEnergy);
            // }
            
        }
    }
}