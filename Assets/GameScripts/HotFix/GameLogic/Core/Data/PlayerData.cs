using GameConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TEngine;

namespace GameLogic
{
    /// <summary>
    /// 静态玩家数据管理类
    /// 【功能说明】
    /// 1. 管理玩家基础属性（血量、能量）
    /// 2. 提供调试功能支持
    /// 3. 处理玩家状态重置逻辑
    /// 【注意事项】
    /// - 所有方法/属性均为静态访问
    /// - 需确保在正确场景上下文调用能量相关操作
    /// </summary>
    public static class PlayerData
    {
        /// <summary>
        /// 当前玩家血量（自动处理归零逻辑）
        /// 【取值范围】0 ~ 最大生命值
        /// </summary>
        public static int HP { get; private set; }

        /// <summary>
        /// 私有能量存储字段
        /// 【数据安全】通过Energy属性进行受控访问
        /// </summary>
        private static float energy;

        /// <summary>
        /// 调试模式开关（默认开启）
        /// 【功能说明】启用时允许使用调试方法添加能量
        /// </summary>
        public static bool IsEnableDebugEnergy { get; private set; }

        /// <summary>
        /// 调试模式单次添加能量量（默认1000）
        /// 【使用条件】需IsEnableDebugEnergy=true时生效
        /// </summary>
        public static float DebugAddEnergyCount { get; private set; }

        /// <summary>
        /// 静态构造函数（自动执行初始化）
        /// 【执行顺序】在首次访问类成员时自动调用
        /// </summary>
        static PlayerData()
        {
            // 初始化调试配置参数
            IsEnableDebugEnergy = true;
            DebugAddEnergyCount = 1000;

            // 加载静态配置数据（当前为空实现，待扩展）
            LoadStaticData();
        }

        /// <summary>
        /// 玩家能量属性（带安全访问控制）
        /// 【设计说明】
        /// - 对外提供受控访问接口
        /// - 设置器保持私有以保证数据安全
        /// </summary>
        public static float Energy
        {
            get => energy;
            private set => energy = value;
        }

        /// <summary>
        /// 静态数据加载方法（待实现）
        /// 【预留扩展】用于后续从配置系统加载初始数据
        /// </summary>
        private static void LoadStaticData()
        {
            // TODO: 从GameConfig加载玩家初始数据
            // 示例：HP = GameEntry.Config.GetInt(Constant.Config.PlayerHP);
        }

        /// <summary>
        /// 造成伤害处理
        /// 【逻辑流程】
        /// 1. 校验有效伤害值（value > 0）
        /// 2. 更新血量并检测死亡状态
        /// 3. 触发游戏结束逻辑（若血量归零）
        /// </summary>
        /// <param name="value">伤害值（需正数）</param>
        public static void Damage(int value)
        {
            if (value == 0) return;

            int lastHP = HP;
            HP = Math.Max(HP - value, 0); // 血量最小值保护

            // 检测游戏结束条件
            bool gameover = HP <= 0;
            if (gameover)
            {
                GameOver();
            }
        }

        /// <summary>
        /// 添加能量值
        /// 【业务规则】
        /// - 仅允许正数操作
        /// - 实际业务需添加场景状态校验（当前简化实现）
        /// </summary>
        /// <param name="value">添加量（正数）</param>
        public static void AddEnergy(float value)
        {
            if (value <= 0) return;
            Energy += value;
        }

        /// <summary>
        /// 调试模式快速加能量
        /// 【使用限制】依赖IsEnableDebugEnergy开关状态
        /// </summary>
        public static void DebugAddEnergy() => AddEnergy(DebugAddEnergyCount);

        /// <summary>
        /// 重置玩家状态（待完整实现）
        /// 【设计目标】
        /// - 重置血量到初始值
        /// - 重置能量到关卡初始值
        /// 【待完善】需要接入关卡数据系统
        /// </summary>
        public static void Reset()
        {
            // TODO: 实现完整重置逻辑
            int lastHP = HP;
            // 待接入：HP = GameEntry.Config.GetInt(Constant.Config.PlayerHP);
        }

        /// <summary>
        /// 游戏结束处理（待实现）
        /// 【规划功能】需要接入关卡失败逻辑
        /// 【历史参考】原实现：GameEntry.Data.GetData<DataLevel>().GameFail();
        /// </summary>
        private static void GameOver()
        {
            // TODO: 实现游戏结束逻辑
        }
    }
}