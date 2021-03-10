/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventHandlerGroup.cs
//  Info     : 事件处理器组
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Aya.Events
{
    [Serializable]
    public class EventHandlerGroup
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// 事件类型值
        /// </summary>
        public object EventType { get; }

        /// <summary>
        /// 事件处理器列表
        /// </summary>
        public List<EventHandler> Handlers { get; internal set; }

        /// <summary>
        /// 事件处理器数量
        /// </summary>
        public int Count => Handlers.Count;

        /// <summary>
        /// 是否需要排序
        /// </summary>
        public bool NeedSort { get; internal set; }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="type">事件枚举类型</param>
        /// <param name="eventType">事件类型值</param>
        public EventHandlerGroup(Type type, object eventType)
        {
            Type = type;
            EventType = eventType;
            Handlers = new List<EventHandler>();
            NeedSort = false;
        }

        /// <summary>
        /// 添加到事件组
        /// </summary>
        /// <param name="eventHandler">事件处理器</param>
        public void Add(EventHandler eventHandler)
        {
            Handlers.Add(eventHandler);
            if (eventHandler.Priority != 0 && !NeedSort)
            {
                NeedSort = true;
            }

            if (NeedSort)
            {
                SortEvents();
            }

            UEventCallback.OnAdded?.Invoke(eventHandler);
        }

        /// <summary>
        /// 从事件组移除
        /// </summary>
        /// <param name="eventHandler">事件处理器</param>
        public void Remove(EventHandler eventHandler)
        {
            Handlers.Remove(eventHandler);
            UEventCallback.OnRemoved?.Invoke(eventHandler);
        }

        /// <summary>
        /// 事件列表按优先级排序
        /// </summary>
        public void SortEvents()
        {
            Handlers.Sort((e1, e2) => e2.Priority - e1.Priority);
        }
    }
}
