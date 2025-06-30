using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class EntityTowerLevel : EntityLogic
    {
        public Transform turret;
        public Transform[] projectilePoints;
        public Transform epicenter;
        // public Launcher launcher;
        public ParticleSystem effect;
    }
}
