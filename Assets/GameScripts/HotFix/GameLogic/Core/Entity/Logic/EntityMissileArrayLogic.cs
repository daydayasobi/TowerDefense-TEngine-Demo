using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityMissileArrayLogic : EntityTowerAttackerLogic
    {
        public float time;

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

                if (timer > time)
                {
                    GameEvent.Send(LevelEvent.OnHideTower, entityTowerData.Tower.SerialId);
                }
            }
        }

        protected override void OnHide(bool isShutdown, object userData)
        {
            base.OnHide(isShutdown, userData);
            timer = 0;
        }
    }
}