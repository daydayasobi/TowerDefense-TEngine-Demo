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
        
        public void OnEnter()
        {
            GameModule.UI.ShowUI<LevelMainInfoUI>();
        }

        public void Update()
        {
            
        }

        public void ShowPreviewTower()
        {
            Log.Info("1111111111111111111");
        }
        
        public void StartWave()
        {
            
        }

        public void Pause()
        {
            
        }
        
        public static LevelControl Create(LevelManager levelPathManager, CameraInput cameraInput)
        {
            // LevelControl levelControl = ReferencePool.Acquire<LevelControl>();
            LevelControl levelControl = new LevelControl();
            // levelControl.level = level;
            levelControl.levelManager = levelPathManager;
            levelControl.cameraInput = cameraInput;
            return levelControl;
        }
    }
}
