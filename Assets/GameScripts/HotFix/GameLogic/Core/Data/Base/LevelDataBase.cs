using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine.Localization;
using UnityEngine;

namespace GameLogic
{
    public sealed class LevelDataBase
    {
        private LevelData dRLevel;
        private WaveDataBase[] waveData;
        private SceneDataBase sceneData;
        private string name;
        private string description;

        public int Id
        {
            get
            {
                return dRLevel.Id;
            }
        }

        public string Name
        {
            get
            {
                return LocalizationManager.GetTermTranslation(dRLevel.Name);
            }
        }

        public string Description
        {
            get
            {
                return LocalizationManager.GetTermTranslation(dRLevel.DescriptionName);
            }
        }

        // public string ResourceGroupName
        // {
        //     get
        //     {
        //         return dRLevel.ResourceGroupName;
        //     }
        // }

        public int InitEnergy
        {
            get
            {
                return dRLevel.InitEnergy;
            }
        }
        
        public int InitHp
        {
            get
            {
                return dRLevel.InitHp;
            }
        }
        
        public Vector3 PlayerPosition
        {
            get
            {
                Vector3 temp = new Vector3(dRLevel.PlayerPosition.X, dRLevel.PlayerPosition.Y, dRLevel.PlayerPosition.Z);
                return temp;
            }
        }

        public Quaternion PlayerQuaternion
        {
            get
            {
                Vector3 temp = new Vector3(dRLevel.PlayerQuaternion.X, dRLevel.PlayerQuaternion.Y, dRLevel.PlayerQuaternion.Z);
                return Quaternion.Euler(temp);
            }
        }

        public WaveDataBase[] WaveDatas
        {
            get
            {
                return waveData;
            }
        }

        public List<int> AllowTowers
        {
            get
            {
                return dRLevel.AllowTowers;
            }
        }

        public SceneDataBase SceneData
        {
            get
            {
                return sceneData;
            }
        }

        public LevelDataBase(LevelData dRLevel, WaveDataBase[] waveData, SceneDataBase sceneData)
        {
            this.dRLevel = dRLevel;
            this.waveData = waveData;
            this.sceneData = sceneData;
        }
    }
}
