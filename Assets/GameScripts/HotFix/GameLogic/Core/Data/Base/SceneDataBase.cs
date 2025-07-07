using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class SceneDataBase
    {
        private SceneData dRScene;
        // private DRAssetsPath dRAssetsPath;
    
        public int Id
        {
            get
            {
                return dRScene.Id;
            }
        }
    
        public int AssetPath
        {
            get
            {
                return dRScene.SceneEntityId;
            }
        }
        
        public SceneDataBase(SceneData dRScene)
        {
            this.dRScene = dRScene;
        }
    }
    
}
