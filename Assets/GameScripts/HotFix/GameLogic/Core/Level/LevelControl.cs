using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LevelControl
    {
        private LevelManager levelManager;
        private CameraInput cameraInput;
        private GameObject roomRoot;

        // private EntityLoader entityLoader;
        private EntityLoadCtrl entityLoaderCtrl;

        // private DataLevel dataLevel;
        // private DataPlayer dataPlayer;
        private DataTower dataTower;
        // private DataEnemy dataEnemy;

        private TowerData previewTowerData;
        private Entity previewTowerEntity;
        private EntityTowerPreview previewTowerEntityLogic;
        private bool isBuilding = false;
        private bool pause = false;

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
                previewTowerEntity = entity;
                previewTowerEntityLogic = entity.Logic as EntityTowerPreview;
                if (previewTowerEntityLogic == null)
                {
                    Log.Error("Entity '{0}' logic type invaild, need EntityTowerPreview", previewTowerEntity.Id);
                    return;
                }

                previewTowerEntityLogic.OnInit();

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

        public static LevelControl Create(GameObject roomRoot, LevelManager levelPathManager, CameraInput cameraInput)
        {
            // LevelControl levelControl = ReferencePool.Acquire<LevelControl>();
            LevelControl levelControl = new LevelControl();
            // levelControl.level = level;
            levelControl.roomRoot = roomRoot;
            levelControl.levelManager = levelPathManager;
            levelControl.cameraInput = cameraInput;
            return levelControl;
        }

        public void Clear()
        {
        }
    }
}