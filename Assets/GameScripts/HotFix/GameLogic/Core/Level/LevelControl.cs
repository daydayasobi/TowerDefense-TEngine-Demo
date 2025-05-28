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

        private bool isBuilding = false;
        private bool pause = false;

        public void OnEnter()
        {
            GameModule.UI.ShowUI<LevelMainInfoUI>();
        }

        public void Update()
        {
            if (isBuilding)
            {
            }
            else
            {
            }
        }

        public void ShowPreviewTower()
        {
            var gameObject = PoolManager.Instance.GetGameObject("AssaultCannon_Level1", parent: roomRoot.transform);

            isBuilding = true;
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