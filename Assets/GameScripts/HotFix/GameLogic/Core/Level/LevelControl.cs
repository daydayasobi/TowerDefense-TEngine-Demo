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

        private EntityLoader entityLoader;

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
            entityLoader = EntityLoader.Create(roomRoot);
        }

        public void Update()
        {
            if (isBuilding)
            {
                if (Input.GetMouseButtonDown(0) && previewTowerEntityLogic != null && previewTowerEntityLogic.CanPlace)
                {
                    previewTowerEntityLogic.TryBuildTower();
                }
                if (Input.GetMouseButtonDown(1))
                {
                    HidePreviewTower();
                }
            }
            else
            {
            }
        }

        public void ShowPreviewTower()
        {
            //var gameObject = PoolManager.Instance.GetGameObject("AssaultCannon_Level1", parent: roomRoot.transform);

            previewTowerData = new TowerData();

            entityLoader.ShowEntity<EntityTowerPreview>(previewTowerData.PreviewEntityId, (entity) =>
                {
                    previewTowerEntity = entity;
                    previewTowerEntityLogic = entity.Logic as EntityTowerPreview;
                    if (previewTowerEntityLogic == null)
                    {
                        Log.Error("Entity '{0}' logic type invaild, need EntityTowerPreview", previewTowerEntity.Id);
                        return;
                    }
                },
                EntityDataTowerPreview.Create(previewTowerData));
            isBuilding = true;
        }
        
        public void HidePreviewTower()
        {
            // if (uiMaskFormSerialId != null)
            //     GameEntry.UI.CloseUIForm((int)uiMaskFormSerialId);

            // GameEntry.Event.Fire(this, HidePreviewTowerEventArgs.Create(previewTowerData));

            // if (previewTowerEntity != null)
            //     entityLoader.HideEntity(previewTowerEntity);
            //
            // uiMaskFormSerialId = null;

            previewTowerEntity = null;
            previewTowerData = null;

            isBuilding = false;
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