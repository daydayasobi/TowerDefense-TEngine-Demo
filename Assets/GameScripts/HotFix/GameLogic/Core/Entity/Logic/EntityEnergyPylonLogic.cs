using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;
using AudioType = UnityEngine.AudioType;

namespace GameLogic
{
    public class EntityEnergyPylonLogic : EntityTowerBaseLogic
    {
        private float timer;

        public override void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);

        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);

            if (LevelDataControl.Instance.LevelState == EnumLevelState.Normal)
            {
                timer += elapseSeconds;

                if (timer > (1 / entityTowerData.Tower.EnergyRaiseRate))
                {
                    timer -= (1 / entityTowerData.Tower.EnergyRaiseRate);
                    RaiseEnergy();
                }
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            timer = 0;
        }

        protected override void OnShowTowerLevelSuccess(Entity entity)
        {
            base.OnShowTowerLevelSuccess(entity);
            if (entityLevelLogic != null && entityLevelLogic.effect != null)
            {
                entityLevelLogic.effect.Stop(entityLevelLogic);
                entityLevelLogic.effect.gameObject.SetActive(false);
            }
        }

        private void RaiseEnergy()
        {
            PlayerDataControl.Instance.AddEnergy(entityTowerData.Tower.EnergyRaise);
            GameModule.Audio.Play(TEngine.AudioType.Sound, AssetsDataLoader.Instance.GetItemConfig((int)EnumSound.TDCurrency).ResourcesName);
            if (entityLevelLogic != null && entityLevelLogic.effect != null)
            {
                entityLevelLogic.effect.gameObject.SetActive(true);
                entityLevelLogic.effect.Play(true);
            }
        }
    }
}
