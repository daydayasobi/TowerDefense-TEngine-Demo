using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class Level : IMemory
    {
        private Queue<Wave> waves;

        private float spawnEnemyTimer = 0;
        private float updateWaveInfoTimer = 0;
        private static readonly float UPDATE_WAVE_INFO_RATE = 0.5f;

        private bool startWave = false;

        public Wave CurrentWave
        {
            get
            {
                if (waves == null || waves.Count <= 0)
                    return null;
                else
                    return waves.Peek();
            }
        }

        public int CurrentWaveIndex
        {
            get;
            private set;
        }

        public int WaveCount
        {
            get
            {
                if (WaveDatas == null)
                    return 0;
                else
                    return WaveDatas.Length;
            }
        }

        public bool Finish
        {
            get;
            private set;
        }

        #region LevelData
        public LevelDataBase LevelData
        {
            get;
            private set;
        }

        public int Id
        {
            get
            {
                return LevelData.Id;
            }
        }

        public string Name
        {
            get
            {
                return LevelData.Name;
            }
        }

        public string Description
        {
            get
            {
                return LevelData.Description;
            }
        }

        public int InitEnergy
        {
            get
            {
                return LevelData.InitEnergy;
            }
        }

        public Vector3 PlayerPosition
        {
            get
            {
                return LevelData.PlayerPosition;
            }
        }

        public Quaternion PlayerQuaternion
        {
            get
            {
                return LevelData.PlayerQuaternion;
            }
        }

        public WaveDataBase[] WaveDatas
        {
            get
            {
                return LevelData.WaveDatas;
            }
        }

        public List<int> AllowTowers
        {
            get
            {
                return LevelData.AllowTowers;
            }
        }

        public SceneDataBase SceneData
        {
            get
            {
                return LevelData.SceneData;
            }
        }

        #endregion

        public void ProcessLevel(float elapseSeconds, float realElapseSeconds)
        {
            if (!startWave || Finish)
                return;

            spawnEnemyTimer += elapseSeconds;
            updateWaveInfoTimer += elapseSeconds;

            if (waves.Count > 0)
            {
                Wave wave = waves.Peek();

                if (updateWaveInfoTimer >= UPDATE_WAVE_INFO_RATE)
                {
                    // TODO: event
                    // GameEntry.Event.Fire(this, WaveInfoUpdateEventArgs.Create(CurrentWaveIndex, WaveCount, wave.Progress));
                    updateWaveInfoTimer -= UPDATE_WAVE_INFO_RATE;
                }

                wave.ProcessWave(elapseSeconds, realElapseSeconds);
                if (wave.Finish)
                {
                    wave = waves.Dequeue();
                    spawnEnemyTimer -= wave.NextWaveTime;
                    if (waves.Count > 0)
                        CurrentWaveIndex++;

                    // TODO: event
                    // GameEntry.Event.Fire(this, WaveInfoUpdateEventArgs.Create(CurrentWaveIndex, WaveCount, wave.Progress));
                    PoolReference.Release(wave);
                    wave = null;
                }
            }
            else
            {
                Finish = true;

            }
        }

        public Level()
        {
            waves = new Queue<Wave>();
            LevelData = null;
            CurrentWaveIndex = 1;
            spawnEnemyTimer = 0;
            updateWaveInfoTimer = 0;
            Finish = false;
            startWave = false;
        }

        public void StartWave()
        {
            startWave = true;
        }

        public static Level Create(LevelDataBase levelData)
        {
            Level level = PoolReference.Acquire<Level>();
            level.LevelData = levelData;

            for (int i = 0; i < levelData.WaveDatas.Length; i++)
            {
                level.waves.Enqueue(Wave.Create(levelData.WaveDatas[i]));
            }

            return level;
        }

        public void Clear()
        {
            while (waves.Count > 0)
            {
                Wave wave = waves.Dequeue();
                PoolReference.Release(wave);
            }
            waves.Clear();

            LevelData = null;
            CurrentWaveIndex = 1;
            spawnEnemyTimer = 0;
            updateWaveInfoTimer = 0;
            Finish = false;
            startWave = false;
        }
    }
}
