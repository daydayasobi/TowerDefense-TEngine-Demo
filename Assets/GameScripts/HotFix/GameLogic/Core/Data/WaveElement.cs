using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class WaveElement : IMemory
    {
        private WaveElementDataBase waveElementData;

        public int Id
        {
            get
            {
                return waveElementData.Id;
            }
        }

        public int EnemyId
        {
            get
            {
                return waveElementData.EnemyId;
            }
        }

        public float SpawnTime
        {
            get
            {
                return waveElementData.SpawnTime;
            }
        }

        public float CumulativeTime
        {
            get;
            private set;
        }

        public WaveElement()
        {
            waveElementData = null;
            CumulativeTime = 0;
        }

        public static WaveElement Create(WaveElementDataBase waveElementData, float cumulativeTime)
        {
            WaveElement waveElement = PoolReference.Acquire<WaveElement>();
            waveElement.waveElementData = waveElementData;
            waveElement.CumulativeTime = cumulativeTime;
            return waveElement;
        }

        public void Clear()
        {
            waveElementData = null;
            CumulativeTime = 0;
        }
    }
}
