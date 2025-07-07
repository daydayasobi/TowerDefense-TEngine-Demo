using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class ProjectileDataLoader: Singleton<ProjectileDataLoader>
    {        
        /// <summary>
             /// 炮弹Table。
             /// </summary>
        private TbProjectileData TbItem => ConfigSystem.Instance.Tables.TbProjectileData;

        /// <summary>
        /// 获取场景配置表。
        /// </summary>
        /// <param name="itemId">场景Id。</param>
        public ProjectileData GetItemConfig(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            return config;
        }
        public List<ProjectileData> GetAllItemConfig()
        {
            return TbItem.DataList;
        }
    }
}
