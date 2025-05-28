using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LevelSystem : Singleton<LevelSystem>
    {
        private LevelControl levelControl;
        private GameObject _roomRoot;

        protected override void OnInit()
        {
            base.OnInit();
            Utility.Unity.AddUpdateListener(Update);
        }

        public async UniTaskVoid LoadLevelSystem(string levelName)
        {
            await GameModule.Scene.LoadSceneAsync(levelName);

            // 创建房间根对象
            _roomRoot = new GameObject("BattleRoom");

            LevelManager levelPathManager = UnityEngine.GameObject.Find("LevelManager").GetComponent<LevelManager>();
            if (levelPathManager == null)
            {
                Log.Error("Can not find LevelPathManager instance in scene");
                return;
            }

            CameraInput cameraInput = UnityEngine.GameObject.Find("GameCamera").GetComponent<CameraInput>();
            if (cameraInput == null)
            {
                Log.Error("Can not find CameraInput instance in scene");
                return;
            }

            levelControl = LevelControl.Create(_roomRoot, levelPathManager, cameraInput);

            // 监听游戏事件
            GameEvent.AddEventListener(EventDefine.ChangeScene, OnChangeScene);
            GameEvent.AddEventListener(EventDefine.LoadLevel, OnLoadLevel);
            GameEvent.AddEventListener(EventDefine.ReloadLevel, OnReLoadLevel);
            GameEvent.AddEventListener(EventDefine.GameOver, OnGameOver);
            GameEvent.AddEventListener(EventDefine.ShowPreviewTower, OnShowPreviewTower);

            levelControl.OnEnter();
        }

        private void DestoryLevelSystem()
        {
            GameEvent.RemoveEventListener(EventDefine.ChangeScene, OnChangeScene);
            GameEvent.RemoveEventListener(EventDefine.LoadLevel, OnLoadLevel);
            GameEvent.RemoveEventListener(EventDefine.ReloadLevel, OnReLoadLevel);
            GameEvent.RemoveEventListener(EventDefine.GameOver, OnGameOver);
            GameEvent.RemoveEventListener(EventDefine.ShowPreviewTower, OnShowPreviewTower);
        }

        public void Update()
        {
            if (levelControl != null)
                levelControl.Update();
        }

        private void OnChangeScene()
        {
        }

        private void OnLoadLevel()
        {
        }

        private void OnReLoadLevel()
        {
        }

        private void OnGameOver()
        {
        }

        private void OnShowPreviewTower()
        {
            levelControl.ShowPreviewTower();
        }
    }
}