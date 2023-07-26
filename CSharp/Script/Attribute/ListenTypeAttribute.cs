/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ListenTypeAttribute.cs
//  Info     : 监听指定类型事件特性
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Events
{
    [AttributeUsage(AttributeTargets.Method)]
    public class ListenTypeAttribute : EventAttributeBase
    {
        /// <summary>
        /// 监听事件枚举类型
        /// </summary>
        public Type Type;

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority;

        /// <summary>
        /// 是否中断事件队列
        /// </summary>
        public bool Interrupt;

        /// <summary>
        /// 监听所有事件
        /// </summary>
        /// <param name="eventEnumType">事件类型</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断消息队列</param>
        public ListenTypeAttribute(Type eventEnumType, int priority = 0, bool interrupt = false)
        {
            Type = eventEnumType;
            Priority = priority;
            Interrupt = interrupt;
        }
    }
}
