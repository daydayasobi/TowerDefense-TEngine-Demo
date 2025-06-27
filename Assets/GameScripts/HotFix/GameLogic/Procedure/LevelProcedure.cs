using System.Collections;
using System.Collections.Generic;
using GameConfig;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LevelProcedure : ProcedureBase
    {
        LevelControl levelControl;
        protected override void OnEnter(IFsm<IProcedureModule> procedureOwner)
        {
            base.OnEnter(procedureOwner);
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
            levelControl = LevelControl.Create(LevelDataManger.Instance.CurrentLevel, levelPathManager, cameraInput, entityRoot);
            GameModule.UI.ShowUI<UILevelMainInfoForm>();
            
            // 在初始化或注册事件的地方，将每个事件ID绑定到对应的事件处理方法
            GameEvent.AddEventListener(LevelEvent.OnChangeScene, OnChangeScene);
            GameEvent.AddEventListener(LevelEvent.OnLoadLevel, OnLoadLevel);
            GameEvent.AddEventListener(LevelEvent.OnLevelStateChange, OnLevelStateChange);
            GameEvent.AddEventListener(LevelEvent.OnGameOver, OnGameOver);
            GameEvent.AddEventListener(LevelEvent.OnReloadLevel, OnReloadLevel);
            GameEvent.AddEventListener<TowerDataBase>(LevelEvent.OnShowPreviewTower, OnShowPreviewTower);
            GameEvent.AddEventListener<TowerDataBase, IPlacementArea, IntVector2, Vector3, Quaternion>(LevelEvent.OnBuildTower, OnBuildTower);
            GameEvent.AddEventListener<int>(LevelEvent.OnSellTower, OnSellTower);
            GameEvent.AddEventListener<int>(LevelEvent.OnSpawnEnemy, OnSpawnEnemy);
            GameEvent.AddEventListener<int>(LevelEvent.OnHideEnemyEntity, OnHideEnemyEntity);
            GameEvent.AddEventListener(LevelEvent.OnShowEntityInLevel, OnShowEntityInLevel);
            GameEvent.AddEventListener<int>(LevelEvent.OnHideEntityInLevel, OnHideEntityInLevel);
            GameEvent.AddEventListener(LevelEvent.OnGameStartWave, OnStartWave);
            GameModule.UI.ShowUI<UITowerListForm>();
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
            GameEvent.RemoveEventListener(LevelEvent.OnLevelStateChange, OnLevelStateChange);
            GameEvent.RemoveEventListener(LevelEvent.OnGameOver, OnGameOver);
            GameEvent.RemoveEventListener(LevelEvent.OnReloadLevel, OnReloadLevel);
            GameEvent.RemoveEventListener<TowerDataBase>(LevelEvent.OnShowPreviewTower, OnShowPreviewTower);
            GameEvent.RemoveEventListener<TowerDataBase, IPlacementArea, IntVector2, Vector3, Quaternion>(LevelEvent.OnBuildTower, OnBuildTower);
            GameEvent.RemoveEventListener<int>(LevelEvent.OnSellTower, OnSellTower);
            GameEvent.RemoveEventListener<int>(LevelEvent.OnSpawnEnemy, OnSpawnEnemy);
            GameEvent.RemoveEventListener<int>(LevelEvent.OnHideEnemyEntity, OnHideEnemyEntity);
            GameEvent.RemoveEventListener(LevelEvent.OnShowEntityInLevel, OnShowEntityInLevel);
            GameEvent.RemoveEventListener<int>(LevelEvent.OnHideEntityInLevel, OnHideEntityInLevel);
            GameEvent.RemoveEventListener(LevelEvent.OnGameStartWave, OnStartWave);
            levelControl.Quick();
            MemoryPool.Release(levelControl);
            levelControl = null;
        }
        
        /// <summary>
        /// 处理场景切换事件
        /// </summary>
        private void OnChangeScene()
        {
            // 处理场景切换的逻辑
        }

        /// <summary>
        /// 处理加载关卡事件
        /// </summary>
        private void OnLoadLevel()
        {
            // 处理加载关卡的逻辑
        }

        /// <summary>
        /// 处理关卡状态变化事件
        /// </summary>
        private void OnLevelStateChange()
        {

        }

        /// <summary>
        /// 处理游戏结束事件
        /// </summary>
        private void OnGameOver()
        {
            // 处理游戏结束的逻辑
            // levelControl.Gameover();
        }

        /// <summary>
        /// 处理重新加载关卡事件
        /// </summary>
        private void OnReloadLevel()
        {
            // 处理重新加载关卡的逻辑
            // levelControl.Restart();
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
        private void OnBuildTower(TowerDataBase towerData, IPlacementArea placementArea, IntVector2 placeGrid, Vector3 position, Quaternion rotation)
        {
            // 处理建造塔的逻辑
            levelControl.CreateTower(towerData,placementArea,placeGrid,position,rotation);
        }

        /// <summary>
        /// 处理出售塔事件
        /// </summary>
        private void OnSellTower(int EntityId)
        {
            // 处理出售塔的逻辑
            // levelControl.HideTower(EntityId);
        }

        /// <summary>
        /// 处理生成敌人事件
        /// </summary>
        private void OnSpawnEnemy(int EntityId)
        {
            // 处理生成敌人的逻辑
            // levelControl.SpawnEnemy(EntityId);
        }

        /// <summary>
        /// 处理隐藏敌人实体事件
        /// </summary>
        private void OnHideEnemyEntity(int EntityId)
        {
            // 处理隐藏敌人实体的逻辑
            // levelControl.HideEnemyEntity(EntityId);
        }

        /// <summary>
        /// 处理显示实体在关卡中事件
        /// </summary>
        private void OnShowEntityInLevel()
        {
            // 处理显示实体在关卡中的逻辑
            // levelControl.ShowEntity();
        }

        /// <summary>
        /// 处理隐藏实体在关卡中事件
        /// </summary>
        private void OnHideEntityInLevel(int EntityId)
        {
            // 处理隐藏实体在关卡中的逻辑
            // levelControl.HideEntity(EntityId);
        }

        private void OnStartWave()
        {
            levelControl.StartWave();
        }
    }
}
