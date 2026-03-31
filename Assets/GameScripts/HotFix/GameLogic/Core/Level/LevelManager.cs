using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class LevelManager : MonoBehaviour
    {
        [System.Serializable]
        public class LevelPathConfig
        {
            public LevelPath levelPath;
            public float weight;
        }
        
        [SerializeField]
        private LevelPathConfig[] levelPathConfigs;
        private float totalWeight;
        
        private void Awake()
        {
            for (int i = 0; i < levelPathConfigs.Length; i++)
            {
                totalWeight += levelPathConfigs[i].weight;
            }
        }

        private bool TryGetRandomLevelPath(out LevelPath levelPath)
        {
            levelPath = null;
            if (levelPathConfigs == null || levelPathConfigs.Length <= 0 || totalWeight <= 0f)
            {
                return false;
            }

            float random = Random.Range(0f, totalWeight);

            float sum = 0;

            for (int i = 0; i < levelPathConfigs.Length; i++)
            {
                sum += levelPathConfigs[i].weight;

                if (sum > random)
                {
                    levelPath = levelPathConfigs[i].levelPath;
                    return levelPath != null;
                }
            }

            return false;
        }

        public LevelPath GetLevelPath()
        {
            return TryGetRandomLevelPath(out LevelPath levelPath) ? levelPath : null;
        }

        public bool TryGetLevelPath(out LevelPath levelPath, out Transform startPathNode)
        {
            startPathNode = null;
            if (!TryGetRandomLevelPath(out levelPath))
            {
                return false;
            }

            if (levelPath.PathNodes == null || levelPath.PathNodes.Length <= 0)
            {
                levelPath = null;
                return false;
            }

            startPathNode = levelPath.PathNodes[0];
            return true;
        }

        public Transform GetStartPathNode()
        {
            return TryGetLevelPath(out _, out Transform startPathNode) ? startPathNode : null;
        }
    }
}
