using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameLogic
{
    public enum EnumSound : int
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 0,

        /// <summary>
        /// 游戏背景音乐
        /// </summary>
        GameBGM = 10001,

        /// <summary>
        /// 主菜单背景音乐
        /// </summary>
        MenuBGM = 10002,

        /// <summary>
        /// Enemy Fire
        /// </summary>
        EnemyFire = 20001,
    }
}
