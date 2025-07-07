using System.Collections;
using System.Collections.Generic;
using GameConfig;
using GameLogic.View;
using TEngine;
using Unity.Properties;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 关卡控制类，负责管理关卡中的各种逻辑，如塔的创建、敌人的生成、暂停、重启等。
    /// </summary>
    partial class LevelControl : IMemory
    {
        // 当前关卡数据
        private Level Level;

        // 关卡管理器，用于管理关卡路径等
        private LevelManager levelManager;

        // 相机输入控制
        private CameraInput cameraInput;

        private bool isBuilding = false; // 是否正在建造塔
        private bool pause = false; // 是否暂停

        // 数据
        // private DataLevelManager dataLevel;(正在实现
        // private DataPlayerManager dataPlayer;(已经实现
        // private DataTowerManager dataTower;(已经实现
        // private DataEnemyManager dataEnemy;(正在实现

        // 预览塔相关数据
        // private TowerDataBase previewTowerData; // 当前预览的塔数据
        private EntityTowerPreviewLogic entityPreviewLogic; // 预览塔的逻辑组件

        // private EntityPlayer player;

        private Dictionary<int, TowerInfo> dicTowerInfo;

        // 构造函数，初始化字典
        public LevelControl()
        {
            dicTowerInfo = new Dictionary<int, TowerInfo>();
        }

        // 实体的根节点，用于存放塔和敌人的实体
        private GameObject entityRoot;

        public void OnEnter()
        {
            // DataPlayerManager.Instance.OnLoad();
            // Vector3 position = new Vector3(leveldata.PlayerPosition.X, leveldata.PlayerPosition.Y, leveldata.PlayerPosition.Z);
            // Quaternion quaternion = Quaternion.Euler(new Vector3(leveldata.PlayerQuaternion.X, leveldata.PlayerQuaternion.Y, leveldata.PlayerQuaternion.Z));
            EntityModuleEx.Instance.ShowPlayerEntity(3039, null, EntityData.Create(Level.PlayerPosition, Level.PlayerQuaternion, entityRoot.transform));
        }

        /// <summary>
        /// 每帧更新方法，处理关卡逻辑的更新。
        /// </summary>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (Level == null)
            {
                Log.Error("Level is null, please check if LevelControl is initialized properly.");
                return;
            }

            if (DataLevelManager.Instance.LevelState != EnumLevelState.Prepare && DataLevelManager.Instance.LevelState != EnumLevelState.Normal)
            {
                Log.Error("Level is not in a valid state for processing. Current state: " + DataLevelManager.Instance.LevelState);
                return;
            }
            
            // 如果关卡未完成，则处理关卡逻辑
            if (!Level.Finish)
                Level.ProcessLevel(elapseSeconds, realElapseSeconds);

            // 如果正在建造塔
            if (isBuilding)
            {
                // 鼠标左键点击且可以放置塔
                if (Input.GetMouseButtonDown(0) && entityPreviewLogic != null && entityPreviewLogic.CanPlace)
                {
                    entityPreviewLogic.TryBuildTower(); // 尝试建造塔
                }

                // 鼠标右键点击，取消预览
                if (Input.GetMouseButtonDown(1))
                {
                    HidePreviewTower();
                }
            }
            else
            {
                // 鼠标左键点击，选择塔
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit raycastHit;
                    if (Physics.Raycast(ray, out raycastHit, float.MaxValue, LayerMask.GetMask("Towers")))
                    {
                        if (raycastHit.collider != null)
                        {
                            EntityTowerLogic entityTowerLogic = raycastHit.collider.gameObject.GetComponent<EntityTowerLogic>();
                            if (entityTowerLogic != null)
                            {
                                entityTowerLogic.ShowControlForm();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 显示预览塔
        /// </summary>
        public void ShowPreviewTower(TowerDataBase towerData)
        {
            if (towerData == null)
                return;

            Tower tower = DataTowerManager.Instance.CreateTower(towerData.Id);
            int serialId = GameModule.Entity.GenerateSerialId();
            float radius = tower.GetRange(1);
            EntityModuleEx.Instance.ShowTowerPreviewEntity(towerData.PreviewEntityId, serialId, (entity) =>
            {
                entityPreviewLogic = entity.Logic as EntityTowerPreviewLogic;
                isBuilding = true;
            }, EntityTowerPreviewData.Create(tower, entityRoot.transform, serialId));
        }

        /// <summary>
        /// 隐藏预览塔
        /// </summary>
        public void HidePreviewTower()
        {
            EntityModuleEx.Instance.HideEntity(entityPreviewLogic.Entity);
            // entityPreviewLogic.Entity.OnHide(true, false);
            entityPreviewLogic = null;
            isBuilding = false;
        }

        /// <summary>
        /// 创建塔
        /// </summary>
        public void CreateTower(Tower tower, IPlacementArea placementArea, IntVector2 placeGrid, Vector3 position, Quaternion rotation)
        {
            if (DataPlayerManager.Instance.Energy < tower.BuildEnergy)
                return;

            DataPlayerManager.Instance.AddEnergy(-tower.BuildEnergy);

            // 1. 通过EntityControl创建塔实体
            int serialId = GameModule.Entity.GenerateSerialId();
            EntityModuleEx.Instance.ShowTowerEntity(tower.EntityId, serialId, (entity) =>
            {
                EntityTowerLogic entityTowerLogic = entity.Logic as EntityTowerLogic;
                dicTowerInfo.Add(tower.SerialId, TowerInfo.Create(tower, entityTowerLogic, placementArea, placeGrid));
            }, EntityTowerData.Create(tower, position, rotation, entityRoot.transform, serialId));

            // 2. 隐藏预览塔
            HidePreviewTower();

            // 3. 记录调试日志
            Log.Debug($"创建塔 [{tower.EntityId}] 在位置 {position}");
        }

        /// <summary>
        /// 隐藏指定序列ID的塔。
        /// </summary>
        /// <param name="towerSerialId">塔的序列ID。</param>
        public void HideTower(int towerSerialId)
        {
            if (!dicTowerInfo.ContainsKey(towerSerialId))
                return;

            TowerInfo towerInfo = dicTowerInfo[towerSerialId];
            EntityModuleEx.Instance.HideEntity(towerInfo.EntityTower.Entity); // 隐藏塔实体
            towerInfo.PlacementArea.Clear(towerInfo.PlaceGrid, towerInfo.Tower.Dimensions);
            DataTowerManager.Instance.DestroyTower(towerInfo.Tower);
            dicTowerInfo.Remove(towerSerialId);
            PoolReference.Release(towerInfo);
        }

        /// <summary>
        /// 隐藏所有塔，通常在重启关卡或游戏结束时调用。
        /// </summary>
        private void HideAllTower()
        {
            List<int> towerSerialIds = new List<int>(dicTowerInfo.Keys);
            for (int i = 0; i < towerSerialIds.Count; i++)
            {
                HideTower(towerSerialIds[i]);
            }
        }

        /// <summary>
        /// 生成敌人，通常在波次开始时调用。
        /// </summary>
        /// <param name="enemyId">敌人的ID。</param>
        public void SpawnEnemy(int enemyId)
        {
            // entityloader.AddEntityEnemy(enemyId,levelManager);
        }

        /// <summary>
        /// 隐藏指定序列ID的敌人实体。
        /// </summary>
        /// <param name="serialId">敌人实体的序列ID。</param>
        public void HideEnemyEntity(int serialId)
        {
            // entityloader.HideEnemyEntity(serialId);
        }

        /// <summary>
        /// 隐藏所有敌人实体，通常在重启关卡或游戏结束时调用。
        /// </summary>
        private void HideAllEnemyEntity()
        {
        }

        /// <summary>
        /// 显示实体，可能用于显示某个特定的游戏实体。
        /// </summary>
        public void ShowEntity()
        {
        }

        /// <summary>
        /// 隐藏指定ID的实体。
        /// </summary>
        /// <param name="entityId">实体的ID。</param>
        public void HideEntity(int entityId)
        {
        }

        /// <summary>
        /// 开始一波敌人，通常在玩家准备好后调用。
        /// </summary>
        public void StartWave()
        {
            Level.StartWave();
        }

        /// <summary>
        /// 暂停游戏，停止所有游戏逻辑的更新。
        /// </summary>
        public void Pause()
        {
            pause = true;
        }

        /// <summary>
        /// 恢复游戏，继续游戏逻辑的更新。
        /// </summary>
        public void Resume()
        {
            pause = false;
        }

        /// <summary>
        /// 重启关卡，重置所有状态并重新开始。
        /// </summary>
        public void Restart()
        {
            if (pause)
            {
                Resume();
                pause = false;
            }

            HideAllTower(); // 隐藏所有塔
            HideAllEnemyEntity(); // 隐藏所有敌人
        }

        /// <summary>
        /// 游戏结束，处理游戏结束的逻辑。
        /// </summary>
        public void Gameover()
        {
        }

        /// <summary>
        /// 快速操作，可能在游戏结束时调用，隐藏所有塔并恢复游戏。
        /// </summary>
        public void Quick()
        {
            if (pause)
            {
                Resume();
                pause = false;
            }

            HideAllTower(); // 隐藏所有塔
        }

        // 创建关卡控制器
        public static LevelControl Create(Level level, LevelManager levelPathManager, CameraInput cameraInput, GameObject entityRoot)
        {
            var levelControl = MemoryPool.Acquire<LevelControl>();
            levelControl.Level = level;
            levelControl.levelManager = levelPathManager;
            levelControl.cameraInput = cameraInput;
            levelControl.entityRoot = entityRoot;
            return levelControl;
        }

        /// <summary>
        /// 清理关卡控制类的资源，可能在关卡结束时调用。
        /// </summary>
        public void Clear()
        {
            levelManager = null;
            cameraInput = null;

            // previewTowerData = null;

            entityPreviewLogic = null;
            isBuilding = false;
        }
    }
}