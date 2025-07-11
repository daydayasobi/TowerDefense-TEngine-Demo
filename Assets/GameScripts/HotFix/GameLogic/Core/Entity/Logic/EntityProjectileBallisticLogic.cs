using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public class EntityProjectileBallisticLogic : EntityHideSelfProjectileLogic
    {
        public BallisticArcHeight arcPreference;
        public BallisticFireMode fireMode;
        [Range(-90, 90)]
        public float firingAngle;
        public float startSpeed;

        public LayerMask mask = -1;
    }
}
