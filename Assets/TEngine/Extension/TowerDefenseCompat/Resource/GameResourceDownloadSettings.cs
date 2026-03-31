using System.Collections.Generic;
using UnityEngine;

namespace TEngine
{
    [CreateAssetMenu(menuName = "TEngine/Extension/Resource Download Settings", fileName = "GameResourceDownloadSettings")]
    public class GameResourceDownloadSettings : ScriptableObject
    {
        private const string ResourcePath = "GameResourceDownloadSettings";

        private static GameResourceDownloadSettings _instance;

        public static GameResourceDownloadSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Resources.Load<GameResourceDownloadSettings>(ResourcePath);
                }

                return _instance;
            }
        }

        public List<string> DefaultPackageTags = new List<string> { "Base" };
    }
}
