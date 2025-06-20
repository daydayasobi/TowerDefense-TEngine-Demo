using System.Collections;
using System.Collections.Generic;
using GameConfig;
using GameLogic.View;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    /// <summary>
    /// 关卡控制类，负责管理关卡中的各种逻辑，如塔的创建、敌人的生成、暂停、重启等。
    /// </summary>
    public class LevelControl : IMemory
    {
        // 当前关卡数据
        private LevelData leveldata;

        // 关卡管理器，用于管理关卡路径等
        private LevelManager levelManager;

        // 相机输入控制
        private CameraInput cameraInput;

        private bool isBuilding = false; // 是否正在建造塔
        private bool pause = false; // 是否暂停

        // 预览塔相关数据
        private TowerData previewTowerData; // 当前预览的塔数据

        private EntityControl entityloader;

        // private Level Level;
        private EntityTowerPreview previewTowerEntityLogic; // 预览塔的逻辑组件

        private EntityPlayer player;

        // 构造函数，初始化字典
        public LevelControl()
        {
        }

        // 实体的根节点，用于存放塔和敌人的实体
        private GameObject roomRoot;

        public void OnEnter()
        {
            // Level = Level.Create(leveldata);
            entityloader = EntityControl.Create();
            player = EntityBase.CreatePlayer(new Vector3(leveldata.PlayerPosition.X, leveldata.PlayerPosition.Y, leveldata.PlayerPosition.Z),
                new Vector3(leveldata.PlayerQuaternion.X, leveldata.PlayerQuaternion.Y, leveldata.PlayerQuaternion.Z));
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
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 显示预览塔
        /// </summary>
        public void ShowPreviewTower(TowerData towerData)
        {
            if (towerData == null)
                return;

            previewTowerData = towerData;
            string PreviewName = AssetsDataManger.Instance.GetItemConfig(towerData.PreviewEntityid).ResourcesName;
            previewTowerEntityLogic = GameModule.Resource.LoadGameObject(PreviewName).GetComponent<EntityTowerPreview>();
            previewTowerEntityLogic.OnShow(EntityDataTowerPreview.Create(towerData));
            isBuilding = true;
        }

        /// <summary>
        /// 隐藏预览塔
        /// </summary>
        public void HidePreviewTower()
        {
            GameObject.Destroy(previewTowerEntityLogic.gameObject);
            previewTowerEntityLogic = null;
            isBuilding = false;
        }

        /// <summary>
        /// 创建塔
        /// </summary>
        public void CreateTower(TowerData towerData, IPlacementArea placementArea, IntVector2 placeGrid, Vector3 position, Quaternion rotation)
        {
            // // 1. 通过EntityControl创建塔实体
            // entityloader.AddEntityTower(towerData.Id, position, rotation);
            //
            // // 2. 隐藏预览塔
            // HidePreviewTower();

            // 3. 记录调试日志
            // Log.Debug($"创建塔 [{towerData.NameId}] 在位置 {position}");
        }

        /// <summary>
        /// 隐藏指定序列ID的塔。
        /// </summary>
        /// <param name="towerSerialId">塔的序列ID。</param>
        public void HideTower(int towerSerialId)
        {
        }

        /// <summary>
        /// 隐藏所有塔，通常在重启关卡或游戏结束时调用。
        /// </summary>
        private void HideAllTower()
        {
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
        public static LevelControl Create(LevelData level, LevelManager levelPathManager, CameraInput cameraInput)
        {
            var levelControl = MemoryPool.Acquire<LevelControl>();
            levelControl.leveldata = level;
            levelControl.levelManager = levelPathManager;
            levelControl.cameraInput = cameraInput;
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