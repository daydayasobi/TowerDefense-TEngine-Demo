using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static UnityEditor.Progress;

namespace GameLogic
{
    internal class SceneDataManger : Singleton<SceneDataManger>
    {
        /// <summary>
        /// 场景Table。
        /// </summary>
        private TbSceneData TbItem => ConfigSystem.Instance.Tables.TbSceneData;

        /// <summary>
        /// 获取场景配置表。
        /// </summary>
        /// <param name="itemId">场景Id。</param>
        public SceneData GetItemConfig(int itemId)
        {
            TbItem.DataMap.TryGetValue(itemId, out var config);
            return config;
        }
        public List<SceneData> GetAllItemConfig()
        {
            return TbItem.DataList;
        }
    }
}
