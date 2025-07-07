using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public sealed class DataSceneManager : Singleton<DataSceneManager>
    {
        private Dictionary<int, SceneDataBase> dicSceneData;

        public void OnInit()
        {

        }

        public void OnLoad()
        {
            dicSceneData = new Dictionary<int, SceneDataBase>();
            List<SceneData> dRScenes = new List<SceneData>(SceneDataLoader.Instance.GetAllItemConfig());
            foreach (var dRScene in dRScenes)
            {
                // DRAssetsPath dRAssetsPath = GameEntry.Data.GetData<DataAssetsPath>().GetDRAssetsPathByAssetsId(dRScene.AssetId);
                SceneDataBase sceneData = new SceneDataBase(dRScene);
                dicSceneData.Add(dRScene.Id, sceneData);
            }
        }

        public SceneDataBase GetSceneData(int id)
        {
            if (dicSceneData.ContainsKey(id))
            {
                return dicSceneData[id];
            }

            return null;
        }

        public SceneDataBase[] GetAllSceneData()
        {
            int index = 0;
            SceneDataBase[] results = new SceneDataBase[dicSceneData.Count];
            foreach (var sceneData in dicSceneData.Values)
            {
                results[index++] = sceneData;
            }

            return results;
        }

        protected void OnUnload()
        {
            dicSceneData = null;
        }

        protected void OnShutdown()
        {
        }
    }
}
