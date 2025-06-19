using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 关卡控制类，负责管理关卡中的各种逻辑，如塔的创建、敌人的生成、暂停、重启等。
    /// </summary>
    public class LevelControl: IMemory
    {
        // 当前关卡数据
        private LevelData leveldata;
        // 关卡管理器，用于管理关卡路径等
        private LevelManager levelManager;
        // 相机输入控制
        private CameraInput cameraInput;
        
        private bool isBuilding = false; // 是否正在建造塔
        private bool pause = false; // 是否暂停
        
        // 预览塔相关数据
        private TowerData previewTowerData; // 当前预览的塔数据
        private EntityControl entityloader;
        // private Level Level;
        private EntityTowerPreview previewTowerEntityLogic; // 预览塔的逻辑组件
        
        /// <summary>
        /// /////////
        /// </summary>
        
        private GameObject roomRoot;

        // private EntityLoader entityLoader;
        private EntityLoadCtrl entityLoaderCtrl;

        // private DataLevel dataLevel;
        // private DataPlayer dataPlayer;
        private DataTower dataTower;
        // private DataEnemy dataEnemy;

        public void OnEnter()
        {
            GameModule.UI.ShowUI<LevelMainInfoUI>();
            // entityLoader = EntityLoader.Create(roomRoot);
            entityLoaderCtrl = EntityLoadCtrl.Create(roomRoot);
        }

        public void Update()
        {
            if (isBuilding)
            {
                if (Input.GetMouseButtonDown(0) && previewTowerEntityLogic != null && previewTowerEntityLogic.CanPlace)
                {
                    previewTowerEntityLogic.TryBuildTower();
                }
                // if (Input.GetMouseButtonDown(1))
                // {
                //     HidePreviewTower();
                // }
            }
            else
            {
            }
        }

        public void ShowPreviewTower()
        {
            DRTower tmpTestData = new DRTower();
            tmpTestData.Dimensions = new IntVector2(2, 2);
            tmpTestData.EntityId = 101;
            tmpTestData.PreviewEntityId = 1001;
            tmpTestData.Type = "AssaultCannonPreview";
            TowerData towerData = new TowerData(tmpTestData);

            EntityManager.ShowEntity();
            EntityManager.SetEntityRoot(roomRoot.transform);
            
            previewTowerData = towerData;
            entityLoaderCtrl.ShowEntity<EntityTowerPreview>(towerData.PreviewEntityId, (entity) =>
            {
                // previewTowerEntityLogic = entity;
                // previewTowerEntityLogic = entity.Logic as EntityTowerPreview;
                // if (previewTowerEntityLogic == null)
                // {
                //     Log.Error("Entity '{0}' logic type invaild, need EntityTowerPreview", previewTowerEntity.Id);
                //     return;
                // }
                //
                // previewTowerEntityLogic.OnInit();

                //测试数据
                // towerData.Dimensions = new IntVector2(2, 2);
                // previewTowerEntityLogic.OnShow(EntityDataCtrlTowerPreview.Create(towerData));

                //生成半径
                // EntityDataCtrlRadiusVisualiser entityDataRadiusVisualiser = EntityDataCtrlRadiusVisualiser.Create(10);
                // entityLoaderCtrl.ShowEntity<EntityRadiusVisualizer>(EnumEntity.RadiusVisualiser, (entityRadiusVisualizer) =>
                // {
                //     // GameEntry.Entity.AttachEntity(entityRadiusVisualizer, previewTowerEntity);
                // },
                // entityDataRadiusVisualiser);

                isBuilding = true;
            },EntityDataCtrlTowerPreview.Create(towerData));
        }

        public void StartWave()
        {
        }

        public void Pause()
        {
            pause = true;
        }

        public void Resume()
        {
            pause = false;
        }

        /// <summary>
        /// 创建关卡控制器
        /// </summary>
        /// <param name="level"></param>
        /// <param name="levelPathManager"></param>
        /// <param name="cameraInput"></param>
        /// <returns></returns>
        public static LevelControl Create(LevelData level, LevelManager levelPathManager, CameraInput cameraInput)
        {
            var levelControl = MemoryPool.Acquire<LevelControl>();
            levelControl.leveldata = level;
            levelControl.levelManager = levelPathManager;
            levelControl.cameraInput = cameraInput;
            return levelControl;
        }

        public void Clear()
        {
        }
    }
}