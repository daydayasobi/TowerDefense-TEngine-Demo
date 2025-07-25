using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public  partial class KeyDefine
    {
        
    }
    
    public  partial class AudioKey
    {
        // 进入选择关卡流程
        public static readonly string MusicVolume = "AudioKey_f_MusicVolume";
        public static readonly string SoundVolume = "AudioKey_f_SoundVolume";
        public static readonly string UISoundVolume = "AudioKey_f_UISoundVolume";
    }
    
    public  partial class LevelKey
    {
        // 关卡存档Key
        public static readonly string LevelStarRecord = "LevelKey_i_Level{0}StarRecord";
    }
}
