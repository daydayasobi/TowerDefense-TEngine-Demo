using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    internal class TowerLevelDataManger : Singleton<TowerLevelDataManger>
    {
        /// <summary>
        /// Table。
        /// </summary>
        private TbTowerLevelData TbItem => ConfigSystem.Instance.Tables.TbTowerLevelData;

        /// <summary>
        /// 配置表。
        /// </summary>
        /// <param name="itemId">场景Id。</param>
        public TowerLevelData GetItemConfig(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            return config;
        }
        public List<TowerLevelData> GetAllItemConfig()
        {
            return TbItem.DataList;
        }
    }
}
