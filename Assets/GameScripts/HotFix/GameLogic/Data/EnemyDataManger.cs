using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class EnemyDataManger : Singleton<EnemyDataManger>
    {
        public EnemyData CurrentLevel
        {
            get;
            private set;
        }
        /// <summary>
        /// Table。
        /// </summary>
        private TbEnemyData TbItem => ConfigSystem.Instance.Tables.TbEnemyData;

        /// <summary>
        /// 获取怪物表。
        /// </summary>
        /// <param name="itemId">怪物Id。</param>
        public EnemyData GetItemConfig(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            CurrentLevel = config;
            return config;
        }
        public List<EnemyData> GetAllItemConfig()
        {
            return TbItem.DataList;
        }
    }
}
