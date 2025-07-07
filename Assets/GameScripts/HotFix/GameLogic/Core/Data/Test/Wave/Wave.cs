using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class Wave : IMemory
    {
        private WaveDataBase waveData;
        private Queue<WaveElement> waveElements;

        private float timer = 0;

        #region WaveData
        public int Id
        {
            get
            {
                return waveData.Id;
            }
        }

        public float FinishWaitTIme
        {
            get
            {
                return waveData.FinishWaitTIme;
            }
        }

        public WaveElementDataBase[] WaveElementDatas
        {
            get
            {
                return waveData.WaveElementDatas;
            }
        }
        #endregion

        public float TotalSpawnTime
        {
            get;
            private set;
        }

        public float NextWaveTime
        {
            get;
            private set;
        }

        public int CurrentEnemyIndex
        {
            get;
            private set;
        }

        public int EnemyCount
        {
            get
            {
                if (waveData == null || waveData.WaveElementDatas == null)
                    return 0;
                else
                    return waveData.WaveElementDatas.Length;
            }

        }

        public float Progress
        {
            get
            {
                float result = (timer / NextWaveTime);
                if (result > 1)
                    result = 1;

                return result;
            }
        }

        public bool Finish
        {
            get;
            private set;
        }

        public Wave()
        {
            waveData = null;
            waveElements = new Queue<WaveElement>();
            CurrentEnemyIndex = 1;
            TotalSpawnTime = 0f;
            NextWaveTime = 0f;
            Finish = false;
        }

        public void ProcessWave(float elapseSeconds, float realElapseSeconds)
        {
            if (Finish)
                return;

            timer += elapseSeconds;

            if (waveElements.Count > 0)
            {
                if (timer > waveElements.Peek().CumulativeTime)
                {
                    WaveElement waveElement = waveElements.Dequeue();
                    int enemyId = waveElement.EnemyId;
                    PoolReference.Release(waveElement);
                    waveElement = null;
                    CurrentEnemyIndex++;
                    GameEvent.Send(LevelEvent.OnSpawnEnemy,enemyId);
                }
            }
            else if (!Finish && timer >= NextWaveTime)
            {
                Finish = true;
            }
        }

        public static Wave Create(WaveDataBase waveData)
        {
            Wave wave = PoolReference.Acquire<Wave>();
            wave.waveData = waveData;
            WaveElementDataBase[] waveElementDatas = waveData.WaveElementDatas;
            float timer = 0;

            foreach (var waveElementData in waveElementDatas)
            {
                timer += waveElementData.SpawnTime;
                wave.waveElements.Enqueue(WaveElement.Create(waveElementData, timer));
            }

            wave.TotalSpawnTime = timer;
            wave.NextWaveTime = timer + waveData.FinishWaitTIme;
            return wave;
        }

        public void Clear()
        {
            while (waveElements.Count > 0)
            {
                WaveElement waveElement = waveElements.Dequeue();
                PoolReference.Release(waveElement);
            }

            waveData = null;
            waveElements.Clear();
            CurrentEnemyIndex = 1;
            TotalSpawnTime = 0f;
            NextWaveTime = 0f;
            timer = 0;
            Finish = false;
        }
    }
}
