using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEngine;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using UnityEngine.WSA;

namespace GameLogic.View
{
    public class EntityTower : MonoBehaviour
    {
        public int Id;
        public TowerData towerData;
        public Transform turret;

        protected IFsm<EntityEnemy> fsm;


        private Dictionary<int, float> dicSlowDownRates;

        protected List<FsmState<EntityEnemy>> stateList;

        public List<GameObject> EntityLevelGoList;

        public Vector3 InitialPosition;
        // 目标玩家实体
        public EntityPlayer TargetPlayer { get; private set; }

        private void Start()
        {

        }

        public float CurrentSlowRate
        {
            get;
            private set;
        }

        public bool IsActivation
        { get; set; }

        public bool IsPause
        {
            get;
            private set;
        }
        public void OnInit()
        {
            dicSlowDownRates = new Dictionary<int, float>();
            stateList = new List<FsmState<EntityEnemy>>();
            EntityLevelGoList= new List<GameObject>();
            foreach (var item in towerData.Levels)
            {
                var entityid=TowerLevelDataManger.Instance.GetItemConfig(item);
                var RsourseName=AssetsDataManger.Instance.GetItemConfig(entityid.Entityid).ResourcesName;
                GameObject Entity = GameModule.Resource.LoadGameObject(RsourseName);
                Entity.SetActive(false);
                Entity.transform.localPosition= InitialPosition;
                Entity.transform.localRotation = Quaternion.identity;
                EntityLevelGoList.Add(Entity);
            }
            EntityLevelGoList[0].SetActive(true);
            CreateFsm();
        }
        
        public void OnHide()
        {
            IsActivation = false;
            TargetPlayer = null;
            DestroyFsm();
            dicSlowDownRates.Clear();
        }
        public void OnShow()
        {
            transform.localPosition = InitialPosition;
            transform.localRotation = Quaternion.identity;
            CurrentSlowRate = 1;
            IsActivation = true;
        }
        protected virtual void AddFsmState()
        {

        }

        protected virtual void StartFsm()
        {
        }

        private void CreateFsm()
        {
            AddFsmState();
            StartFsm();
        }

        private void DestroyFsm()
        {
            stateList.Clear();
            fsm = null;
        }
    }
}
