using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameLogic
{
    // 定义一个枚举类型 `EnumLevelState`，表示游戏关卡的不同状态。
    // 枚举的底层类型为 `byte`，以节省内存空间。
    public enum EnumLevelState : byte
    {
        // 默认状态，表示没有任何状态或未初始化。
        None,

        // 加载状态，表示关卡正在加载资源或初始化。
        Loading,

        // 准备状态，表示关卡已加载完成，正在等待开始。
        Prepare,

        // 正常状态，表示关卡正在正常运行，玩家可以进行游戏。
        Normal,

        // 暂停状态，表示关卡被暂停，游戏逻辑暂时停止。
        Pause,

        // 游戏结束状态，表示关卡已结束，可能是胜利或失败。
        Gameover
    }
}
