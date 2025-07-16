using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class DataProjectileManager : Singleton<DataProjectileManager>
    {
        private Dictionary<int, ProjectileDataBase> dicProjectile;

        public void OnInit()
        {
        }

        public void OnLoad()
        {
            List<ProjectileData> drProjectiles = ProjectileDataLoader.Instance.GetAllItemConfig();
            dicProjectile = new Dictionary<int, ProjectileDataBase>();
            foreach (var drProjectile in drProjectiles)
            {
                ProjectileDataBase projectileData = new ProjectileDataBase(drProjectile);
                dicProjectile.Add(drProjectile.Id, projectileData);
            }
        }

        public ProjectileDataBase GetProjectileData(int id)
        {
            if (dicProjectile.ContainsKey(id))
            {
                return dicProjectile[id];
            }

            return null;
        }

        public ProjectileDataBase[] GetAllProjectileData()
        {
            int index = 0;
            ProjectileDataBase[] results = new ProjectileDataBase[dicProjectile.Count];
            foreach (var poolParamData in dicProjectile.Values)
            {
                results[index++] = poolParamData;
            }

            return results;
        }

        public void OnUnload()
        {
            dicProjectile = null;
        }

        public void OnShutdown()
        {
        }
    }
}