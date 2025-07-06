using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class WaveDataBase : MonoBehaviour
    {
        private WaveData dRWave;
        private WaveElementDataBase[] waveElementDatas;

        public int Id
        {
            get
            {
                return dRWave.Id;
            }
        }

        public float FinishWaitTIme
        {
            get
            {
                return dRWave.FinishWaitTime;
            }
        }

        public WaveElementDataBase[] WaveElementDatas
        {
            get
            {
                return waveElementDatas;
            }
        }

        public WaveDataBase(WaveData dRWave, WaveElementDataBase[] waveElementDatas)
        {
            this.dRWave = dRWave;
            this.waveElementDatas = waveElementDatas;
        }
    }
}
