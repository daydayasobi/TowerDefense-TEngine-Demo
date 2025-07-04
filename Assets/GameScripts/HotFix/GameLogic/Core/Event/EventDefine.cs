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
    
    public partial class MainMenuEvent
    {
        // 进入选择关卡流程
        public static readonly int ChangeLevelSelect = RuntimeId.ToRuntimeId("ILoginUI_Event.ChangeLevelSelect");

        // 打开设置
        public static readonly int OpenOptions = RuntimeId.ToRuntimeId("ILoginUI_Event.OpenOptions");

        // 退出游戏
        public static readonly int QuitGame = RuntimeId.ToRuntimeId("ILoginUI_Event.QuitGame");

        //打开菜单
        public static readonly int OpenMenu = RuntimeId.ToRuntimeId("ILoginUI_Event.OpenMenu");
    }
    
    public partial class ChangeSceneEvent
    {
        // 返回菜单流程
        public static readonly int MenuSelect = RuntimeId.ToRuntimeId("ILoginUI_Event.MenuSelect");

        // 进入关卡流程
        public static readonly int LevelSelect = RuntimeId.ToRuntimeId("ILoginUI_Event.LevelSelect");
    }
    
    public partial class TestEvent
    {
        // 测试事件ID
        public static readonly int OnTest1 = RuntimeId.ToRuntimeId("ITestEvent_Event.OnTest1");
    }
    
    public partial class LevelEvent
    {
        // 定义了一系列静态只读的运行时事件ID，这些ID用于标识不同的事件类型。
        // 这些事件ID通过RuntimeId.ToRuntimeId方法生成，确保每个事件ID的唯一性。

        // 场景切换事件ID
        public static readonly int OnChangeScene = RuntimeId.ToRuntimeId("ILoginUI_Event.OnChangeScene");

        // 加载关卡事件ID
        public static readonly int OnLoadLevel = RuntimeId.ToRuntimeId("ILoginUI_Event.OnLoadLevel");

        // 关卡状态变化事件ID
        public static readonly int OnLevelStateChange = RuntimeId.ToRuntimeId("ILoginUI_Event.OnLevelStateChange");

        // 游戏结束事件ID
        public static readonly int OnGameOver = RuntimeId.ToRuntimeId("ILoginUI_Event.OnGameOver");

        // 重新加载关卡事件ID
        public static readonly int OnReloadLevel = RuntimeId.ToRuntimeId("ILoginUI_Event.OnReloadLevel");

        // 显示预览塔事件ID
        public static readonly int OnShowPreviewTower = RuntimeId.ToRuntimeId("ILoginUI_Event.OnShowPreviewTower");

        // 建造塔事件ID
        public static readonly int OnBuildTower = RuntimeId.ToRuntimeId("ILoginUI_Event.OnBuildTower");

        // 出售塔事件ID
        public static readonly int OnSellTower = RuntimeId.ToRuntimeId("ILoginUI_Event.OnSellTower");

        // 生成敌人事件ID
        public static readonly int OnSpawnEnemy = RuntimeId.ToRuntimeId("ILoginUI_Event.OnSpawnEnemy");

        // 隐藏敌人实体事件ID
        public static readonly int OnHideEnemyEntity = RuntimeId.ToRuntimeId("ILoginUI_Event.OnHideEnemyEntity");

        // 在关卡中显示实体事件ID
        public static readonly int OnShowEntityInLevel = RuntimeId.ToRuntimeId("ILoginUI_Event.OnShowEntityInLevel");

        // 在关卡中隐藏实体事件ID
        public static readonly int OnHideEntityInLevel = RuntimeId.ToRuntimeId("ILoginUI_Event.OnHideEntityInLevel");

        // 更改生命数值事件ID
        public static readonly int OnPlayerHPChange = RuntimeId.ToRuntimeId("ILoginUI_Event.OnPlayerHPChange");

        // 更改能量数值事件ID
        public static readonly int OnPlayerEnergyChange = RuntimeId.ToRuntimeId("ILoginUI_Event.OnPlayerEnergyChange");

        // 关卡状态变化事件
        public static readonly int OnWaveUpdate = RuntimeId.ToRuntimeId("ILoginUI_Event.OnWaveUpdate");

        // 游戏开始事件ID
        public static readonly int OnGameStartWave = RuntimeId.ToRuntimeId("ILoginUI_Event.OnGameStartWave");
        
        // 防御塔升级ID
        public static readonly int OnUpgradeTower = RuntimeId.ToRuntimeId("ILoginUI_Event.OnUpgradeTower");
    }

}
