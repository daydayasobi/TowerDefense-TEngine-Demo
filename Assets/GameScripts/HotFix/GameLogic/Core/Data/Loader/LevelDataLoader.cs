using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEngine;

namespace GameLogic
{
    internal class LevelDataLoader : Singleton<LevelDataLoader>
    {
        public LevelData CurrentLevel
        {
            get;
            private set;
        }
        /// <summary>
        /// 关卡Table。
        /// </summary>
        private TbLevelData TbItem => ConfigSystem.Instance.Tables.TbLevelData;

        /// <summary>
        /// 获取关卡表。
        /// </summary>
        /// <param name="itemId">关卡Id。</param>
        public LevelData GetItemConfig(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            CurrentLevel = config;
            return config;
        }
        public List<LevelData> GetAllItemConfig()
        {
            return TbItem.DataList;
        }
        public void LoadLevel(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            CurrentLevel = config;
        }
    }
}
