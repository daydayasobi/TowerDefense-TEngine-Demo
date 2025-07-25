using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class EntityTowerLevelLogic : EntityLogic
    {
        protected EntityData entityData;
        public Transform turret;
        public Transform[] projectilePoints;
        public Transform epicenter;
        public Launcher launcher;
        public ParticleSystem effect;


        public override void OnInit(object userData)
        {
            base.OnInit(userData);
            if (userData.GetType() == typeof(EntityData))
            {
                entityData = (EntityData)userData;
                transform.position = entityData.Position;
                transform.rotation = entityData.Rotation;
                transform.parent = entityData.Parent;
            }
        }
    }
}
