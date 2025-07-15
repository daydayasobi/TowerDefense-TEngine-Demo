using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
    public class EntityPlayerLogic : EntityLogicWithData
    {
        public GameObject idleEffect;
        public ParticleSystem chargeEffect;
        public ParticleSystem damagedEffect;

        public void OnInit(object userData)
        {
            base.OnInit(userData);
        }

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            
            idleEffect.SetActive(true);
        }

        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            idleEffect.SetActive(false);
        }

        public void Charge()
        {
            if (chargeEffect != null)
                chargeEffect.Play();

            GameModule.Audio.Play(AudioType.Sound,AssetsDataLoader.Instance.GetItemConfig((int)EnumSound.zone_enter).ResourcesName);
        }

        public void Damage(int value)
        {
            if (damagedEffect != null)
                damagedEffect.Play();
            
            GameModule.Audio.Play(AudioType.Sound,AssetsDataLoader.Instance.GetItemConfig((int)EnumSound.base_attack).ResourcesName);
            PlayerDataControl.Instance.Damage(value);
        }
    }
}