using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class DefaultConfigDataLoader : Singleton<DefaultConfigDataLoader>
    {
        public DefaultConfigData CurrentConfig { get; private set; }

        /// <summary>
        /// Table。
        /// </summary>
        private TbDefaultConfigData TbDefaultConfig => ConfigSystem.Instance.Tables.TbDefaultConfigData;

        /// <summary>
        /// 获取默认配置数据
        /// </summary>
        /// <param name="key">参数的key。</param>
        public DefaultConfigData GetConfig(string key)
        {
            TbDefaultConfig.DataMap.TryGetValue(key, out var config);
            CurrentConfig = config;
            return config;
        }

        public List<DefaultConfigData> GetAllConfig()
        {
            return TbDefaultConfig.DataList;
        }
    }
}