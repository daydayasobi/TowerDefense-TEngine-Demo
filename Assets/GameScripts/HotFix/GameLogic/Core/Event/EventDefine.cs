using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public static class EventDefine
    {
        public static readonly int ChangeScene = RuntimeId.ToRuntimeId("EventDefine.ChangeScene");
        public static readonly int LoadLevel = RuntimeId.ToRuntimeId("EventDefine.LoadLevel");
        public static readonly int LevelStateChange = RuntimeId.ToRuntimeId("EventDefine.LevelStateChange");
        public static readonly int ReloadLevel = RuntimeId.ToRuntimeId("EventDefine.ReloadLevel");
        public static readonly int GameOver = RuntimeId.ToRuntimeId("EventDefine.GameOver");
        public static readonly int ShowPreviewTower = RuntimeId.ToRuntimeId("EventDefine.ShowPreviewTower");
        public static readonly int BuildTower = RuntimeId.ToRuntimeId("EventDefine.BuildTower");
        public static readonly int HideTowerInLevel = RuntimeId.ToRuntimeId("EventDefine.HideTowerInLevel");
        public static readonly int StartWave = RuntimeId.ToRuntimeId("EventDefine.StartWave");
        public static readonly int SpawnEnemy = RuntimeId.ToRuntimeId("EventDefine.SpawnEnemy");
        public static readonly int HideEnemy = RuntimeId.ToRuntimeId("EventDefine.HideEnemy");
        public static readonly int ShowEntityInLevel = RuntimeId.ToRuntimeId("EventDefine.ShowEntityInLevel");
        public static readonly int HideEntityInLevel = RuntimeId.ToRuntimeId("EventDefine.HideEntityInLevel");
        
        //entity loder
        public static readonly int OnShowEntitySuccess = RuntimeId.ToRuntimeId("EventDefine.OnShowEntitySuccess");
        public static readonly int OnShowEntityFail = RuntimeId.ToRuntimeId("EventDefine.OnShowEntityFail");
    }

    public static class MenuEvent
    {
        // 进入选择关卡流程
        public static readonly int ChangeLevelSelect = RuntimeId.ToRuntimeId("IMenu_Event.ChangeLevelSelect");

        // 打开设置
        public static readonly int OpenOptions = RuntimeId.ToRuntimeId("IMenu_Event.OpenOptions");

        // 退出游戏
        public static readonly int QuitGame = RuntimeId.ToRuntimeId("IMenu_Event.QuitGame");

        //打开菜单
        public static readonly int OpenMenu = RuntimeId.ToRuntimeId("IMenu_Event.OpenMenu");
    }
    
    public partial class MainMenuEvent
    {
        // 进入选择关卡流程
        public static readonly int ChangeLevelSelect = RuntimeId.ToRuntimeId("IMainMenu_Event.ChangeLevelSelect");

        // 打开设置
        public static readonly int OpenOptions = RuntimeId.ToRuntimeId("IMainMenu_Event.OpenOptions");

        // 退出游戏
        public static readonly int QuitGame = RuntimeId.ToRuntimeId("IMainMenu_Event.QuitGame");

        //打开菜单
        public static readonly int OpenMenu = RuntimeId.ToRuntimeId("IMainMenu_Event.OpenMenu");
    }
    
    public partial class ChangeSceneEvent
    {
        // 返回菜单流程
        public static readonly int MenuSelect = RuntimeId.ToRuntimeId("IChangeScene_Event.MenuSelect");

        // 进入关卡流程
        public static readonly int LevelSelect = RuntimeId.ToRuntimeId("IChangeScene_Event.LevelSelect");
    }
    
    public partial class PoolEvent
    {
        // 推出gameobject
        public static readonly int OnPushGameObject = RuntimeId.ToRuntimeId("IPoolEvent_Event.OnPushGameObject");
        
        // 获取gameobject
        public static readonly int OnGetGameObject = RuntimeId.ToRuntimeId("IPoolEvent_Event.OnGetGameObject");
        
        // 推出object
        public static readonly int OnGetObject = RuntimeId.ToRuntimeId("IPoolEvent_Event.OnGetObject");
        
        // 获取object
        public static readonly int OnPushObject = RuntimeId.ToRuntimeId("IPoolEvent_Event.OnPushObject");
    }
    
    public partial class LevelEvent
    {
        // 定义了一系列静态只读的运行时事件ID，这些ID用于标识不同的事件类型。
        // 这些事件ID通过RuntimeId.ToRuntimeId方法生成，确保每个事件ID的唯一性。

        // 场景切换事件ID
        public static readonly int OnChangeScene = RuntimeId.ToRuntimeId("ILevel_Event.OnChangeScene");

        // 加载关卡事件ID
        public static readonly int OnLoadLevel = RuntimeId.ToRuntimeId("ILevel_Event.OnLoadLevel");
        
        // 加载关卡事件ID
        public static readonly int OnLoadLevelFinish = RuntimeId.ToRuntimeId("ILevel_Event.OnLoadLevelFinish");

        // 关卡状态变化事件ID
        public static readonly int OnLevelStateChange = RuntimeId.ToRuntimeId("ILevel_Event.OnLevelStateChange");

        // 游戏结束事件ID
        public static readonly int OnGameOver = RuntimeId.ToRuntimeId("ILevel_Event.OnGameOver");

        // 重新加载关卡事件ID
        public static readonly int OnReloadLevel = RuntimeId.ToRuntimeId("ILevel_Event.OnReloadLevel");

        // 显示预览塔事件ID
        public static readonly int OnShowPreviewTower = RuntimeId.ToRuntimeId("ILevel_Event.OnShowPreviewTower");

        // 建造塔事件ID
        public static readonly int OnBuildTower = RuntimeId.ToRuntimeId("ILevel_Event.OnBuildTower");

        // 出售塔事件ID
        public static readonly int OnSellTower = RuntimeId.ToRuntimeId("ILevel_Event.OnSellTower");

        // 生成敌人事件ID
        public static readonly int OnSpawnEnemy = RuntimeId.ToRuntimeId("ILevel_Event.OnSpawnEnemy");

        // 隐藏敌人实体事件ID
        public static readonly int OnHideEnemyEntity = RuntimeId.ToRuntimeId("ILevel_Event.OnHideEnemyEntity");

        // 在关卡中显示实体事件ID
        public static readonly int OnShowEntityInLevel = RuntimeId.ToRuntimeId("ILevel_Event.OnShowEntityInLevel");

        // 在关卡中隐藏实体事件ID
        public static readonly int OnHideEntityInLevel = RuntimeId.ToRuntimeId("ILevel_Event.OnHideEntityInLevel");

        // 更改生命数值事件ID
        public static readonly int OnPlayerHPChange = RuntimeId.ToRuntimeId("ILevel_Event.OnPlayerHPChange");

        // 更改能量数值事件ID
        public static readonly int OnPlayerEnergyChange = RuntimeId.ToRuntimeId("ILevel_Event.OnPlayerEnergyChange");

        // 关卡状态变化事件
        public static readonly int OnWaveUpdate = RuntimeId.ToRuntimeId("ILevel_Event.OnWaveUpdate");

        // 游戏开始事件ID
        public static readonly int OnGameStartWave = RuntimeId.ToRuntimeId("ILevel_Event.OnGameStartWave");
        
        // 防御塔升级ID
        public static readonly int OnUpgradeTower = RuntimeId.ToRuntimeId("ILevel_Event.OnUpgradeTower");
    }

}
