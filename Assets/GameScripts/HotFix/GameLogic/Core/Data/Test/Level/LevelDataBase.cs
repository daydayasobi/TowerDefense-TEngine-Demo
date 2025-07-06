using System.Collections;
using System.Collections.Generic;
using GameConfig;
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
                // TODO: 需要多语言
                // return GameEntry.Localization.GetString(dRLevel.NameId);
                return dRLevel.Name;
            }
        }

        public string Description
        {
            get
            {
                // TODO: 需要多语言
                // return GameEntry.Localization.GetString(dRLevel.DescriptionId);
                return dRLevel.Description;
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

        public Vector3 PlayerPosition
        {
            get
            {
                vector3 temp = dRLevel.PlayerPosition;
                return new Vector3(temp.X, temp.Y, temp.Z);
            }
        }

        public Quaternion PlayerQuaternion
        {
            get
            {
                vector3 temp = dRLevel.PlayerQuaternion;
                return Quaternion.Euler(new Vector3(temp.X, temp.Y, temp.Z));
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
