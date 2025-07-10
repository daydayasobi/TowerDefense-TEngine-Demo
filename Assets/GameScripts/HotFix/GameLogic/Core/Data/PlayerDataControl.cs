using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class PlayerDataControl : Singleton<PlayerDataControl>
    {
        public int HP { get; private set; }

        private float energy;

        public float Energy
        {
            get
            {
                if (!LevelDataControl.Instance.IsInLevel)
                {
                    Log.Error("Is invaild to get player energy outsiede level scene");
                    return 0;
                }

                return energy;
            }

            private set { energy = value; }
        }


        public bool IsEnableDebugEnergy { get; private set; }
        public float DebugAddEnergyCount { get; private set; }

        protected override void OnInit()
        {
            OnLoad();
        }

        protected void OnPreload()
        {
        }

        protected void OnLoad()
        {
            energy = LevelDataControl.Instance.GetLevelData(LevelDataControl.Instance.CurrentLevelIndex).InitEnergy;;
            HP = LevelDataControl.Instance.GetLevelData(LevelDataControl.Instance.CurrentLevelIndex).InitHp;
            IsEnableDebugEnergy = true;
            DebugAddEnergyCount = 1000;
        }

        public void Damage(int value)
        {
            if (value == 0)
                return;

            int lastHP = HP;
            HP -= value;

            bool gameover = false;

            if (HP <= 0)
            {
                HP = 0;
                gameover = true;
            }

            GameEvent.Send(LevelEvent.OnPlayerHPChange, lastHP, HP);

            if (gameover)
                GameOver();
        }

        public void AddEnergy(float value)
        {
            if (value == 0)
                return;

            float lastEnergy = Energy;
            Energy += value;

            GameEvent.Send(LevelEvent.OnPlayerEnergyChange, lastEnergy, Energy);
        }

        public void DebugAddEnergy()
        {
            AddEnergy(DebugAddEnergyCount);
        }

        public void Reset()
        {
            int lastHP = HP;
            HP = LevelDataControl.Instance.GetLevelData(LevelDataControl.Instance.CurrentLevelIndex).InitHp;
            // TODO: test
            HP = 5;
            GameEvent.Send(LevelEvent.OnPlayerHPChange, lastHP, HP);

            float lastEnergy = Energy;
            // DataLevel dataLevel = GameEntry.Data.GetData<DataLevel>();
            // if (!dataLevel.IsInLevel)
            // {
            //     Log.Error("Is invaild to get player energy outsiede level scene");
            //     Energy = lastEnergy;
            // }
            // else
            // {
            //     LevelData levelData = dataLevel.GetLevelData(dataLevel.CurrentLevelIndex);
            //     Energy = levelData.InitEnergy;
            // }
            
            Energy = LevelDataControl.Instance.GetLevelData(LevelDataControl.Instance.CurrentLevelIndex).InitEnergy;

            GameEvent.Send(LevelEvent.OnPlayerEnergyChange, lastEnergy, Energy);

        }

        public bool BuyTower(int towerId)
        {
            return false;
        }

        public void SellTower()
        {
        }

        private void GameOver()
        {
            LevelDataControl.Instance.GameFail();
        }

        protected void OnUnload()
        {
        }

        protected void OnShutdown()
        {
        }
    }
}