using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class AttackerDataBase : IMemory
    {
        public float Range
        {
            get;
            private set;
        }

        public float FireRate
        {
            get;
            private set;
        }

        public bool IsMultiAttack
        {
            get;
            private set;
        }

        public EnumProjectile ProjectileType
        {
            get;
            private set;
        }

        public int ProjectileEntityId
        {
            get;
            private set;
        }

        public AttackerDataBase()
        {
            this.Range = 0;
            this.FireRate = 0;
            this.IsMultiAttack = false;
            this.ProjectileType = EnumProjectile.None;
            this.ProjectileEntityId = -1;
        }
        
        public static AttackerDataBase Create(float range, float fireRate, bool isMultiAttack, EnumProjectile projectileType, int projectileEntityId)
        {
            AttackerDataBase attackerData = PoolReference.Acquire<AttackerDataBase>();
            attackerData.Range = range;
            attackerData.FireRate = fireRate;
            attackerData.IsMultiAttack = isMultiAttack;
            attackerData.ProjectileType = projectileType;
            attackerData.ProjectileEntityId = projectileEntityId;
            return attackerData;
        }

        public void Clear()
        {
            this.Range = 0;
            this.FireRate = 0;
            this.IsMultiAttack = false;
            this.ProjectileType = EnumProjectile.None;
            this.ProjectileEntityId = -1;
        }
    }
}
