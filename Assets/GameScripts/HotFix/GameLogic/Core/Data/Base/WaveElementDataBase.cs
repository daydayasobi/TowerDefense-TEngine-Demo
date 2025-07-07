using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class WaveElementDataBase 
    {
        private WaveElementData dRWaveElement;

        public int Id
        {
            get
            {
                return dRWaveElement.Id;
            }
        }

        public int EnemyId
        {
            get
            {
                return dRWaveElement.Enemyid;
            }
        }

        public float SpawnTime
        {
            get
            {
                return dRWaveElement.SpawnTime;
            }
        } 

        public WaveElementDataBase(WaveElementData dRWaveElement)
        {
            this.dRWaveElement = dRWaveElement;
        }
    }
}
