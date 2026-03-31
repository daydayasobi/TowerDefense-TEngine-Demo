using System;
#if ENABLE_OBFUZ
using Obfuz;
#endif

namespace TEngine
{
    /// <summary>
    /// 事件分组枚举。
    /// </summary>
    public enum EEventGroup
    {
        /// <summary>
        /// UI相关的交互。
        /// </summary>
        GroupUI,

        /// <summary>
        /// 逻辑层内部相关的交互。
        /// </summary>
        GroupLogic,
    }

    [AttributeUsage(AttributeTargets.Interface)]
#if ENABLE_OBFUZ
    [ObfuzIgnore]
#endif
    public class EventInterfaceAttribute : Attribute
    {
        public EEventGroup EventGroup { get; }

        public EventInterfaceAttribute(EEventGroup group)
        {
            EventGroup = group;
        }
    }
}