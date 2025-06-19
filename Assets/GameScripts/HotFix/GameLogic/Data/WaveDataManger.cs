using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEngine;
using static UnityEditor.Progress;

namespace GameLogic
{
    internal class WaveDataManger : Singleton<WaveDataManger>
    {
        public WaveData CurrentLevel
        {
            get;
            private set;
        }
        /// <summary>
        /// 关卡Table。
        /// </summary>
        private TbWaveData TbItem => ConfigSystem.Instance.Tables.TbWaveData;

        /// <summary>
        /// 获取关卡表。
        /// </summary>
        /// <param name="itemId">关卡Id。</param>
        public WaveData GetItemConfig(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            CurrentLevel = config;
            return config;
        }
        public List<WaveData> GetAllItemConfig()
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
