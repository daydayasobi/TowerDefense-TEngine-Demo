using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    public class AssetsDataLoader : Singleton<AssetsDataLoader>
    {
        public AssetsPathData CurrentLevel
        {
            get;
            private set;
        }
        /// <summary>
        /// Table。
        /// </summary>
        private TbAssetsPathData TbItem => ConfigSystem.Instance.Tables.TbAssetsPathData;

        /// <summary>
        /// 获取实体。
        /// </summary>
        /// <param name="itemId">实体Id。</param>
        public AssetsPathData GetItemConfig(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            CurrentLevel = config;
            return config;
        }
        public List<AssetsPathData> GetAllItemConfig()
        {
            return TbItem.DataList;
        }
    }
}
