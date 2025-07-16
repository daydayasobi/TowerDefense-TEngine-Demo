using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class DataEnemyManager : Singleton<DataEnemyManager>
    {
        private Dictionary<int, EnemyDataBase> dicEnemyData;
        // private Dictionary<int, ProjectileDataBase> dicProjectile;
        
        public void OnInit()
        {

        }
        

        public void OnLoad()
        {
            // dtEnemy = GameEntry.DataTable.GetDataTable<DREnemy>();
            // if (dtEnemy == null)
            //     throw new System.Exception("Can not get data table Enemy");

            // List<ProjectileData> drProjectiles = ProjectileDataLoader.Instance.GetAllItemConfig();
            // DataProjectileManager.Instance.OnLoad();
            
            // List<ProjectileData> drProjectiles = ProjectileDataLoader.Instance.GetAllItemConfig();
            // Dictionary<int, ProjectileDataBase> dicProjectile = new Dictionary<int, ProjectileDataBase>(12);
            // foreach (var drProjectile in drProjectiles)
            // {
            //     dicProjectile.Add(drProjectile.Id, new ProjectileDataBase(drProjectile));
            // }
            //
            // dicEnemyData = new Dictionary<int, EnemyDataBase>();
            //
            // List<EnemyData> dREnemies = EnemyDataLoader.Instance.GetAllItemConfig();
            // foreach (var drEnemy in dREnemies)
            // {
            //     if (dicProjectile.ContainsKey(drEnemy.ProjectileData))
            //     {
            //         EnemyDataBase enemyData = new EnemyDataBase(drEnemy, dicProjectile[drEnemy.ProjectileData]);
            //         dicEnemyData.Add(drEnemy.Id, enemyData);
            //     }
            //     else
            //     {
            //         Debug.LogError($"EnemyData {drEnemy.Id} has invalid ProjectileData {drEnemy.ProjectileData}");
            //     }
            // }

            DataProjectileManager.Instance.OnLoad();
            dicEnemyData = new Dictionary<int, EnemyDataBase>();

            List<EnemyData> dREnemies = EnemyDataLoader.Instance.GetAllItemConfig();
            foreach (var drEnemy in dREnemies)
            {
                EnemyDataBase enemyData = new EnemyDataBase(drEnemy, DataProjectileManager.Instance.GetProjectileData(drEnemy.ProjectileData));
                dicEnemyData.Add(drEnemy.Id, enemyData);
            }
        }

        public EnemyDataBase GetEnemyData(int id)
        {
            if (dicEnemyData.ContainsKey(id))
            {
                return dicEnemyData[id];
            }

            return null;
        }

        public EnemyDataBase[] GetAllEnemyData()
        {
            int index = 0;
            EnemyDataBase[] results = new EnemyDataBase[dicEnemyData.Count];
            foreach (var enemyData in dicEnemyData.Values)
            {
                results[index++] = enemyData;
            }

            return results;
        }

        public void OnUnload()
        {
            dicEnemyData = null;
        }

        public void OnShutdown()
        {
        }
    }
}
