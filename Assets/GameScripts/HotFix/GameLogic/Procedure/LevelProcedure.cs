using System;
using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;
using AudioType = TEngine.AudioType;

namespace GameLogic
{
    public class LevelProcedure : ProcedureBase
    {
        LevelControl levelControl;
        IFsm<IProcedureModule> procedureOwner;

        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
            this.procedureOwner = procedureOwner;
            Log.Debug("游戏流程");
            LevelManager levelPathManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
            if (levelPathManager == null)
            {
                Log.Error("Can not find LevelPathManager instance in scene");
                return;
            }

            CameraInput cameraInput = GameObject.Find("GameCamera").GetComponent<CameraInput>();
            if (cameraInput == null)
            {
                Log.Error("Can not find CameraInput instance in scene");
                return;
            }

            GameObject entityRoot = GameObject.Find("EntityRoot");
            if (entityRoot == null)
            {
                Log.Error("Can not find EntityRoot in scene");
                return;
            }

            //加载数据
            DataEnemyManager.Instance.OnLoad();

            levelControl = LevelControl.Create(LevelDataControl.Instance.CurrentLevel, levelPathManager, cameraInput, entityRoot);
            GameModule.UI.ShowUI<UILevelMainInfoForm>();

            // 在初始化或注册事件的地方，将每个事件ID绑定到对应的事件处理方法
            GameEvent.AddEventListener(LevelEvent.OnChangeScene, OnChangeScene);
            GameEvent.AddEventListener(LevelEvent.OnLoadLevel, OnLoadLevel);
            GameEvent.AddEventListener<EnumLevelState, EnumLevelState>(LevelEvent.OnLevelStateChange, OnLevelStateChange);
            GameEvent.AddEventListener(LevelEvent.OnGameOver, OnGameOver);
            GameEvent.AddEventListener(LevelEvent.OnReloadLevel, OnReloadLevel);
            GameEvent.AddEventListener<TowerDataBase>(LevelEvent.OnShowPreviewTower, OnShowPreviewTower);
            GameEvent.AddEventListener<Tower, IPlacementArea, IntVector2, Vector3, Quaternion>(LevelEvent.OnBuildTower, OnBuildTower);
            GameEvent.AddEventListener<int>(LevelEvent.OnSellTower, OnSellTower);
            GameEvent.AddEventListener<int>(LevelEvent.OnSpawnEnemy, OnSpawnEnemy);
            GameEvent.AddEventListener<int>(LevelEvent.OnHideEnemyEntity, OnHideEnemyEntity);
            GameEvent.AddEventListener<ShowEntityEventData>(LevelEvent.OnShowEntityInLevel, OnShowEntityInLevel);
            GameEvent.AddEventListener<Entity>(LevelEvent.OnHideEntityInLevel, OnHideEntityInLevel);
            GameEvent.AddEventListener(LevelEvent.OnGameStartWave, OnStartWave);
            GameModule.UI.ShowUI<UITowerListForm>();
            GameModule.Audio.Play(AudioType.Music, AssetsDataLoader.Instance.GetItemConfig((int)EnumSound.GameBGM).ResourcesName);
            levelControl.OnEnter();
        }

        protected override void OnUpdate(IFsm<IProcedureModule> procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

            if (levelControl != null)
                levelControl.Update(elapseSeconds, realElapseSeconds);
        }

        protected override void OnLeave(IFsm<IProcedureModule> procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            GameModule.UI.CloseUI<UILevelMainInfoForm>();
            GameEvent.RemoveEventListener(LevelEvent.OnChangeScene, OnChangeScene);
            GameEvent.RemoveEventListener(LevelEvent.OnLoadLevel, OnLoadLevel);
            GameEvent.RemoveEventListener<EnumLevelState, EnumLevelState>(LevelEvent.OnLevelStateChange, OnLevelStateChange);
            GameEvent.RemoveEventListener(LevelEvent.OnGameOver, OnGameOver);
            GameEvent.RemoveEventListener(LevelEvent.OnReloadLevel, OnReloadLevel);
            GameEvent.RemoveEventListener<TowerDataBase>(LevelEvent.OnShowPreviewTower, OnShowPreviewTower);
            GameEvent.RemoveEventListener<Tower, IPlacementArea, IntVector2, Vector3, Quaternion>(LevelEvent.OnBuildTower, OnBuildTower);
            GameEvent.RemoveEventListener<int>(LevelEvent.OnSellTower, OnSellTower);
            GameEvent.RemoveEventListener<int>(LevelEvent.OnSpawnEnemy, OnSpawnEnemy);
            GameEvent.RemoveEventListener<int>(LevelEvent.OnHideEnemyEntity, OnHideEnemyEntity);
            GameEvent.RemoveEventListener<ShowEntityEventData>(LevelEvent.OnShowEntityInLevel, OnShowEntityInLevel);
            GameEvent.RemoveEventListener<Entity>(LevelEvent.OnHideEntityInLevel, OnHideEntityInLevel);
            GameEvent.RemoveEventListener(LevelEvent.OnGameStartWave, OnStartWave);
            levelControl.Quick();
            MemoryPool.Release(levelControl);
            levelControl = null;
        }

        /// <summary>
        /// 处理场景切换事件
        /// </summary>
        private async void OnChangeScene()
        {
            // 处理场景切换的逻辑
            levelControl.BackMainMenu();
            await GameModule.Scene.LoadSceneAsync("Menu");
            ChangeState<ChangeSceneProcedure>(procedureOwner);
        }

        /// <summary>
        /// 处理加载关卡事件
        /// </summary>
        private void OnLoadLevel()
        {
            // 处理加载关卡的逻辑
            // TODO: 写死测试数据
            // GameEvent.Send(LevelEvent.OnLoadLevelFinish, 1);
            LevelDataControl.Instance.OnLoadLevelFinish(1);
        }

        /// <summary>
        /// 处理关卡状态变化事件
        /// </summary>
        private void OnLevelStateChange(EnumLevelState lastState, EnumLevelState curState)
        {
            if (curState == EnumLevelState.Pause)
            {
                levelControl.Pause();
            }
            else if (lastState == EnumLevelState.Pause)
            {
                levelControl.Resume();
            }
        }

        /// <summary>
        /// 处理游戏结束事件
        /// </summary>
        private void OnGameOver()
        {
            // 处理游戏结束的逻辑
            levelControl.Gameover();
        }

        /// <summary>
        /// 处理重新加载关卡事件
        /// </summary>
        private void OnReloadLevel()
        {
            // 处理重新加载关卡的逻辑
            levelControl.Restart();
        }

        /// <summary>
        /// 处理显示塔的预览事件
        /// </summary>
        private void OnShowPreviewTower(TowerDataBase towerData)
        {
            // 处理显示塔预览的逻辑
            levelControl.ShowPreviewTower(towerData);
        }

        /// <summary>
        /// 处理建造塔事件
        /// </summary>
        private void OnBuildTower(Tower tower, IPlacementArea placementArea, IntVector2 placeGrid, Vector3 position, Quaternion rotation)
        {
            // 处理建造塔的逻辑
            levelControl.CreateTower(tower, placementArea, placeGrid, position, rotation);
        }

        /// <summary>
        /// 处理出售塔事件
        /// </summary>
        private void OnSellTower(int EntityId)
        {
            // 处理出售塔的逻辑
            levelControl.HideTower(EntityId);
        }

        /// <summary>
        /// 处理生成敌人事件
        /// </summary>
        private void OnSpawnEnemy(int enemyId)
        {
            levelControl.SpawnEnemy(enemyId);
        }

        /// <summary>
        /// 处理隐藏敌人实体事件
        /// </summary>
        private void OnHideEnemyEntity(int SerialId)
        {
            // 处理隐藏敌人实体的逻辑
            levelControl.HideEnemyEntity(SerialId);
        }

        /// <summary>
        /// 处理显示实体在关卡中事件
        /// </summary>
        public void OnShowEntityInLevel(ShowEntityEventData data)
        {
            // 处理显示实体在关卡中的逻辑
            levelControl.ShowEntity(data);
        }

        /// <summary>
        /// 处理隐藏实体在关卡中事件
        /// </summary>
        private void OnHideEntityInLevel(Entity entity)
        {
            // 处理隐藏实体在关卡中的逻辑
            levelControl.HideEntity(entity);
        }

        private void OnStartWave()
        {
            levelControl.StartWave();
        }
    }
}