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
        private LevelData leveldata;

        // 关卡管理器，用于管理关卡路径等
        private LevelManager levelManager;

        // 相机输入控制
        private CameraInput cameraInput;

        private bool isBuilding = false; // 是否正在建造塔
        private bool pause = false; // 是否暂停

        // 数据
        // private DataLevelManager dataLevel;
        // private DataPlayerManager dataPlayer;(已经实现
        // private DataTowerManager dataTower;(已经实现
        // private DataEnemyManager dataEnemy;

        // 预览塔相关数据
        private TowerDataBase previewTowerData; // 当前预览的塔数据
        private EntityTowerPreview previewTowerEntityLogic; // 预览塔的逻辑组件

        // private Level Level;

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
            int serialId = EntityControl.Instance.GenerateSerialId();
            Vector3 position = new Vector3(leveldata.PlayerPosition.X, leveldata.PlayerPosition.Y, leveldata.PlayerPosition.Z);
            Quaternion quaternion = Quaternion.Euler(new Vector3(leveldata.PlayerQuaternion.X, leveldata.PlayerQuaternion.Y, leveldata.PlayerQuaternion.Z));
            EntityControl.Instance.ShowPlayerEntity(serialId, null, EntityData.Create(position, quaternion, entityRoot.transform, null));
        }

        /// <summary>
        /// 每帧更新方法，处理关卡逻辑的更新。
        /// </summary>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            // if (Level == null) return;
            //
            // // 如果关卡未完成，则处理关卡逻辑
            // if (!Level.Finish)
            //     Level.ProcessLevel(elapseSeconds, realElapseSeconds);

            // 如果正在建造塔
            if (isBuilding)
            {
                // 鼠标左键点击且可以放置塔
                if (Input.GetMouseButtonDown(0) && previewTowerEntityLogic != null && previewTowerEntityLogic.CanPlace)
                {
                    previewTowerEntityLogic.TryBuildTower(); // 尝试建造塔
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

            previewTowerData = towerData;
            // string PreviewName = AssetsDataManger.Instance.GetItemConfig(towerData.PreviewEntityid).ResourcesName;
            // previewTowerEntityLogic = GameModule.Resource.LoadGameObject(PreviewName).GetComponent<EntityTowerPreview>();
            // previewTowerEntityLogic.OnShow(EntityDataTowerPreview.Create(towerData));

            // entityloader.ShowEntity<EntityTowerPreview>(towerData.PreviewEntityid,entityRoot.transform, EntityDataTowerPreview.Create(towerData));
            EntityControl.Instance.ShowTowerPreview(towerData.PreviewEntityId, entityRoot.transform, (previewTower) =>
            {
                previewTowerEntityLogic = previewTower;
                previewTowerEntityLogic.OnShow(EntityDataTowerPreview.Create(towerData));
            });

            // TowerLevelData towerLevelData = TowerLevelDataManger.Instance.GetItemConfig(towerData.Levels[0]);
            // if (towerLevelData == null)
            // {
            //     Log.Error("Tower '{0}' Level '{1}' data is null.", towerData.NameId, 0);
            // }
            //
            // EntityDataRadiusVisualiser entityDataRadiusVisualiser = EntityDataRadiusVisualiser.Create(towerLevelData.Range);
            // entityRadiusVisualizerView = GameModule.Resource.LoadGameObject("RadiusVisualiser").GetComponent<EntityRadiusVisualizerView>();
            // entityRadiusVisualizerView.OnAttachTo(previewTowerEntityLogic.transform, entityDataRadiusVisualiser);

            isBuilding = true;
        }

        /// <summary>
        /// 隐藏预览塔
        /// </summary>
        public void HidePreviewTower()
        {
            PoolManager.Instance.PushGameObject(previewTowerEntityLogic.gameObject);
            previewTowerEntityLogic = null;
            isBuilding = false;
        }

        /// <summary>
        /// 创建塔
        /// </summary>
        public void CreateTower(TowerDataBase towerData, IPlacementArea placementArea, IntVector2 placeGrid, Vector3 position, Quaternion rotation)
        {
            Tower tower = DataTowerManager.Instance.CreateTower(towerData.Id);
            if (tower == null)
            {
                Log.Error("Create tower fail,Tower data id is '{0}'.", towerData.Id);
                return;
            }
            
            if (DataPlayerManager.Instance.Energy < tower.BuildEnergy)
                return;
            
            DataPlayerManager.Instance.AddEnergy(-tower.BuildEnergy);

            // 1. 通过EntityControl创建塔实体
            int serialId = EntityControl.Instance.GenerateSerialId();
            EntityControl.Instance.ShowTowerEntity(towerData.EntityId, serialId, EntityTowerData.Create(tower, position, rotation, entityRoot.transform, serialId), (entity) =>
            {
                EntityTowerLogic entityTowerLogic = entity.Logic as EntityTowerLogic;
                dicTowerInfo.Add(tower.SerialId, TowerInfo.Create(tower, entityTowerLogic, placementArea, placeGrid));
            });

            // 2. 隐藏预览塔
            HidePreviewTower();

            // 3. 记录调试日志
            Log.Debug($"创建塔 [{towerData.Id}] 在位置 {position}");
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
            EntityControl.Instance.HideEntity(towerInfo.EntityTower.Entity); // 隐藏塔实体
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
            // Level.StartWave();
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
        public static LevelControl Create(LevelData level, LevelManager levelPathManager, CameraInput cameraInput, GameObject entityRoot)
        {
            var levelControl = MemoryPool.Acquire<LevelControl>();
            levelControl.leveldata = level;
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

            previewTowerData = null;

            previewTowerEntityLogic = null;
            isBuilding = false;
        }
    }
}