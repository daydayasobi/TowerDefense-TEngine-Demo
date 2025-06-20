using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    internal class TowerDataManger : Singleton<TowerDataManger>
    {
        /// <summary>
        /// Table。
        /// </summary>
        private TbTowerData TbItem => ConfigSystem.Instance.Tables.TbTowerData;

        /// <summary>
        /// 配置表。
        /// </summary>
        /// <param name="itemId">场景Id。</param>
        public TowerData GetItemConfig(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            return config;
        }
        public List<TowerData> GetAllItemConfig()
        {
            return TbItem.DataList;
        }
    }
}
