/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ListenAttribute.cs
//  Info     : 事件监听器特性，用于标注需要监听特定事件的方法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Events
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class ListenAttribute : EventAttributeBase
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public object[] Types;

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority;

        /// <summary>
        /// 是否中断事件队列
        /// </summary>
        public bool Interrupt;

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        public ListenAttribute(object eventType, int priority = 0, bool interrupt = false)
        {
            Types = new object[] {eventType};
            Priority = priority;
            Interrupt = interrupt;
        }

        /// <summary>
        /// 监听事件
        /// </summary>
        /// <param name="eventTypes">事件类型数组(不可重复，监听多事件不能设置优先级)</param>
        public ListenAttribute(params object[] eventTypes)
        {
            Types = eventTypes;
            Priority = 0;
            Interrupt = false;
        }
    }
}