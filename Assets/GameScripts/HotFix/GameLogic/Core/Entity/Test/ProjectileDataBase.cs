using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class ProjectileDataBase
    {
        private ProjectileData dRProjectile;

        public int Id
        {
            get
            {
                return dRProjectile.Id;
            }
        }

        public float Damage
        {
            get
            {
                return dRProjectile.Damage;
            }
        }

        public float SplashDamage
        {
            get
            {
                return dRProjectile.SplahDamage;
            }
        }

        public float SplashRange
        {
            get
            {
                return dRProjectile.SplashRange;
            }
        }

        public ProjectileDataBase(ProjectileData dRProjectile)
        {
            this.dRProjectile = dRProjectile;
        }
    }
}
