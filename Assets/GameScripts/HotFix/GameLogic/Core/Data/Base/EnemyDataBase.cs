using System.Collections;
using System.Collections.Generic;
using GameConfig;
using UnityEngine;

namespace GameLogic
{
    public class EnemyDataBase
    {
        private EnemyData dREnemy;
        private ProjectileDataBase projectileData;
        
        public int Id
        {
            get
            {
                return dREnemy.Id;
            }
        }

        public string NameId
        {
            get
            {
                // TODO: 需要多语言
                // return GameEntry.Localization.GetString(dREnemy.NameId);
                return dREnemy.Nameid;
            }
        }

        public int EntityId
        {
            get
            {
                return dREnemy.Entityid;
            }
        }

        public string Type
        {
            get
            {
                return dREnemy.Type;
            }
        }

        public float MaxHP
        {
            get
            {
                return dREnemy.MaxHp;
            }
        }

        public int Damage
        {
            get
            {
                return dREnemy.Damage;
            }
        }

        public int ProjectileEntityId
        {
            get
            {
                return dREnemy.ProjectileEntityid;
            }
        }

        // public string ProjectileType
        // {
        //     get
        //     {
        //         return dREnemy.ProjectileEntityid;
        //     }
        // }

        public ProjectileDataBase ProjectileData
        {
            get
            {
                return projectileData;
            }
        }

        public int ProjectileDataId
        {
            get
            {
                return dREnemy.ProjectileData;
            }
        }

        public float FireRate
        {
            get
            {
                return dREnemy.FireRate;
            }
        }

        public float Range
        {
            get
            {
                return dREnemy.Range;
            }
        }

        public bool IsMultiAttack
        {
            get
            {
                return dREnemy.IsMultiAttack;
            }
        }

        public float AddEnergy
        {
            get
            {
                return dREnemy.AddEnergy;
            }
        }

        public float Speed
        {
            get
            {
                return dREnemy.Speed;
            }
        }

        public EnemyDataBase(EnemyData dREnemy, ProjectileDataBase projectileData)
        {
            this.dREnemy = dREnemy;
            this.projectileData = projectileData;
        }
    }
}
