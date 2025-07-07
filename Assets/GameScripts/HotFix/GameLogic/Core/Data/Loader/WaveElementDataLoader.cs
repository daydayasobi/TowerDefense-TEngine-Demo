using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    internal class WaveElementDataLoader : Singleton<WaveElementDataLoader>
    {
        public WaveElementData CurrentLevel
        {
            get;
            private set;
        }
        /// <summary>
        /// 关卡Table。
        /// </summary>
        private TbWaveElementData TbItem => ConfigSystem.Instance.Tables.TbWaveElementData;

        /// <summary>
        /// 获取关卡表。
        /// </summary>
        /// <param name="itemId">关卡Id。</param>
        public WaveElementData GetItemConfig(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            CurrentLevel = config;
            return config;
        }
        public List<WaveElementData> GetAllItemConfig()
        {
            return TbItem.DataList;
        }
        public void LoadWave(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            CurrentLevel = config;
        }
    }
}
