using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Cysharp.Threading.Tasks;
using GameConfig;
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
        // private Level level;

        // 关卡管理器，用于管理关卡路径等
        private LevelManager levelManager;

        // 相机输入控制
        private CameraInput cameraInput;

        private bool isBuilding = false; // 是否正在建造塔
        private bool pause = false; // 是否暂停

        // 预览塔相关数据
        // private TowerDataBase previewTowerData; // 当前预览的塔数据
        private EntityTowerPreviewLogic entityPreviewLogic; // 预览塔的逻辑组件

        private Dictionary<int, TowerInfo> dicTowerInfo;
        private Dictionary<int, Entity> dicEnemyEntityInfo;

        // 构造函数，初始化字典
        public LevelControl()
        {
            dicTowerInfo = new Dictionary<int, TowerInfo>(20);
            dicEnemyEntityInfo = new Dictionary<int, Entity>(20);
        }

        // 实体的根节点，用于存放塔和敌人的实体
        private GameObject entityRoot;

        public void OnEnter()
        {
            int serialId = GameModule.Entity.GenerateSerialId();
            EntityDataControl.ShowEntity<EntityPlayerLogic>(
                (int)EnumEntity.EntityPlayer, 
                serialId, null, EntityData.Create(
                LevelDataControl.Instance.CurrentLevel.PlayerPosition,
                LevelDataControl.Instance.CurrentLevel.PlayerQuaternion,
                entityRoot.transform,
                serialId));
        }

        /// <summary>
        /// 每帧更新方法，处理关卡逻辑的更新。
        /// </summary>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            if (LevelDataControl.Instance.CurrentLevel == null)
            {
                Log.Info("Level is null, please check if LevelControl is initialized properly.");
                return;
            }

            if (LevelDataControl.Instance.LevelState != EnumLevelState.Prepare && LevelDataControl.Instance.LevelState != EnumLevelState.Normal)
            {
                Log.Info("Level is not in a valid state for processing. Current state: " + LevelDataControl.Instance.LevelState);
                return;
            }

            // 如果关卡未完成，则处理关卡逻辑
            if (!LevelDataControl.Instance.CurrentLevel.Finish)
                LevelDataControl.Instance.CurrentLevel.ProcessLevel(elapseSeconds, realElapseSeconds);

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
                            EntityTowerBaseLogic entityTowerBaseLogic = raycastHit.collider.gameObject.GetComponent<EntityTowerBaseLogic>();
                            if (entityTowerBaseLogic != null)
                            {
                                entityTowerBaseLogic.ShowControlForm();
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
            EntityDataControl.ShowEntity<EntityTowerPreviewLogic>(towerData.PreviewEntityId, serialId, (entity) =>
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
            if (entityPreviewLogic != null)
            {
                GameEvent.Send(LevelEvent.OnHidePreviewTower);
                EntityDataControl.Instance.HideEntity(entityPreviewLogic.Entity);
                entityPreviewLogic = null;
                isBuilding = false;
            }
        }

        /// <summary>
        /// 创建塔
        /// </summary>
        public void CreateTower(Tower tower, IPlacementArea placementArea, IntVector2 placeGrid, Vector3 position, Quaternion rotation)
        {
            if (PlayerDataControl.Instance.Energy < tower.BuildEnergy)
                return;

            PlayerDataControl.Instance.AddEnergy(-tower.BuildEnergy);

            // 1. 通过EntityControl创建塔实体
            int serialId = GameModule.Entity.GenerateSerialId();
            EntityDataControl.ShowEntity<EntityTowerBaseLogic>(tower.EntityId, serialId, (entity) =>
            {
                EntityTowerBaseLogic entityTowerBaseLogic = entity.Logic as EntityTowerBaseLogic;
                dicTowerInfo.Add(tower.SerialId, TowerInfo.Create(tower, entityTowerBaseLogic, placementArea, placeGrid));
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
            EntityDataControl.Instance.HideEntity(towerInfo.EntityTowerBase.Entity); // 隐藏塔实体
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
            // 处理生成敌人的逻辑
            EnemyDataBase enemyData = DataEnemyManager.Instance.GetEnemyData(enemyId);

            if (enemyData == null)
            {
                Log.Error("Can not get enemy data by id '{0}'.", enemyId);
                return;
            }

            int serialId = GameModule.Entity.GenerateSerialId();
            EntityDataControl.ShowEntity<EntityEnemyLogic>(enemyData.EntityId, serialId, (entity) =>
            {
                dicEnemyEntityInfo.Add(entity.SerialId, entity);
            }, EntityEnemyData.Create(serialId,
                enemyData,
                levelManager.GetLevelPath(),
                levelManager.GetStartPathNode().position - new Vector3(0, 0.2f, 0),
                Quaternion.identity,
                entityRoot.transform));
        }

        /// <summary>
        /// 隐藏指定序列ID的敌人实体。
        /// </summary>
        /// <param name="serialId">敌人实体的序列ID。</param>
        public void HideEnemyEntity(int serialId)
        {
            if (dicEnemyEntityInfo.ContainsKey(serialId))
            {
                EntityDataControl.Instance.HideEntity(dicEnemyEntityInfo[serialId]);
                dicEnemyEntityInfo.Remove(serialId);
            }
            else
            {
                Log.Warning("Entity with serial ID '{0}' not found in tower dictionary.", serialId);
            }

            if (LevelDataControl.Instance.CurrentLevel != null && LevelDataControl.Instance.CurrentLevel.Finish && dicEnemyEntityInfo.Count <= 0)
                LevelDataControl.Instance.GameSuccess();
        }

        /// <summary>
        /// 隐藏所有敌人实体，通常在重启关卡或游戏结束时调用。
        /// </summary>
        private void HideAllEnemyEntity()
        {
            List<int> enemyEntitySerialIds = new List<int>(dicEnemyEntityInfo.Keys);
            for (int i = 0; i < enemyEntitySerialIds.Count; i++)
            {
                HideEnemyEntity(enemyEntitySerialIds[i]);
            }
        }

        /// <summary>
        /// 显示实体，可能用于显示某个特定的游戏实体。
        /// </summary>
        public void ShowEntity(ShowEntityEventData data)
        {
            EntityDataControl.Instance.ShowEntity(data);
        }


        /// <summary>
        /// 隐藏指定ID的实体。
        /// </summary>
        /// <param name="serialId">实体的序列号ID。</param>
        public void HideEntity(Entity entity)
        {
            if (entity == null)
            {
                Log.Warning("Entity is null, cannot hide.");
                return;
            }

            EntityDataControl.Instance.HideEntity(entity);
            Log.Debug("Entity with Serial ID '{0}' has been hidden.", entity.SerialId);
        }

        /// <summary>
        /// 开始一波敌人，通常在玩家准备好后调用。
        /// </summary>
        public void StartWave()
        {
            LevelDataControl.Instance.CurrentLevel.StartWave();
        }

        /// <summary>
        /// 暂停游戏，停止所有游戏逻辑的更新。
        /// </summary>
        public void Pause()
        {
            pause = true;

            foreach (var entity in EntityDataControl.Instance.GetAllEntities())
            {
                IPause iPause = entity.Logic as IPause;
                if (iPause != null)
                    iPause.Pause();
            }
        }

        /// <summary>
        /// 恢复游戏，继续游戏逻辑的更新。
        /// </summary>
        public void Resume()
        {
            pause = false;

            foreach (var entity in EntityDataControl.Instance.GetAllEntities())
            {
                IPause iPause = entity.Logic as IPause;
                if (iPause != null)
                    iPause.Resume();
            }

            cameraInput.Resume();
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

            HidePreviewTower();
            HideAllTower(); // 隐藏所有塔
            HideAllEnemyEntity(); // 隐藏所有敌人
        }

        /// <summary>
        /// 游戏结束，处理游戏结束的逻辑。
        /// </summary>
        public void Gameover(int starCount)
        {
            HidePreviewTower();
            Pause();

            GameModule.UI.ShowUI<UIGameOverForm>(starCount);
        }

        /// <summary>
        /// 返回游戏主页
        /// </summary>
        public void BackMainMenu()
        {
            if (pause)
            {
                Resume();
                pause = false;
            }

            HideAllTower(); // 隐藏所有塔
            HideAllEnemyEntity(); // 隐藏所有敌人
            EntityDataControl.Instance.HideAllEntities();
        }

        /// <summary>
        /// 快速操作，可能在游戏结束时调用，隐藏所有塔并恢复游戏。
        /// </summary>
        public void Quick()
        {
            Log.Debug("Quick operation triggered.");
            if (pause)
            {
                Resume();
                pause = false;
            }
        }

        // 创建关卡控制器
        public static LevelControl Create(Level level, LevelManager levelPathManager, CameraInput cameraInput, GameObject entityRoot)
        {
            var levelControl = MemoryPool.Acquire<LevelControl>();
            // levelControl.level = level;
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
            entityPreviewLogic = null;
            isBuilding = false;

            // level = null;
            levelManager = null;
            cameraInput = null;

            isBuilding = false;

            dicTowerInfo.Clear();
            dicEnemyEntityInfo.Clear();
        }
    }
}