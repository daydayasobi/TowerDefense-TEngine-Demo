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
}
