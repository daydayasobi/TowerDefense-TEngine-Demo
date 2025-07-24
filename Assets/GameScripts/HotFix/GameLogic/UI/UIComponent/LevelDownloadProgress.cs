using System.Collections;
using System.Collections.Generic;
using TEngine;
using UnityEngine;

namespace GameLogic
{
    public class LevelDownloadProgress : IMemory
    {
        public int LevelId { get; private set; }
        public float Progress { get; private set; } // 范围 0 ~ 1, -1 代表失败

        public LevelDownloadProgress(int levelId, float progress)
        {
            LevelId = levelId;
            Progress = progress;
        }

        public void Clear()
        {
            LevelId = 0;
            Progress = -1; // 重置为失败状态
        }
    }
}
