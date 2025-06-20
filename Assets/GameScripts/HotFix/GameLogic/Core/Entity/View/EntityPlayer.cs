using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic.View
{
    public class EntityPlayer : MonoBehaviour
    {
        public GameObject idleEffect;
        public ParticleSystem chargeEffect;
        public ParticleSystem demagedEffect;

        protected void OnShow(object userData)
        {
            idleEffect.SetActive(true);
        }

        protected  void OnHide(bool isShutdown, object userData)
        {
            idleEffect.SetActive(false);
        }

        public void Charge()
        {
            if (chargeEffect != null)
                chargeEffect.Play();
        }

        public void Damage(int value)
        {
            if (demagedEffect != null)
                demagedEffect.Play();
        }
    }
}
