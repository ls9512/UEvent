/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ListenGroupAttribute.cs
//  Info     : 事件监听分组特性
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Events
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ListenGroupAttribute : EventAttributeBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public object Type;

        /// <summary>
        /// 事件分组
        /// </summary>
        public object Group;

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
        /// <param name="eventType">事件类型</param>
        /// <param name="group">事件分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断消息队列</param>
        public ListenGroupAttribute(object eventType, object group, int priority = 0, bool interrupt = false)
        {
            Type = eventType;
            Group = group;
            Priority = priority;
            Interrupt = interrupt;
        }
    }
}
