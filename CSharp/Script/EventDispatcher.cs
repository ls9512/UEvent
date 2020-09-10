/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventDispatcher.cs
//  Info     : 事件分发器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aya.Events
{
    public partial class EventDispatcher
    {
        #region Property

        /// <summary>
        /// 事件类型 - 事件处理器组
        /// </summary>
        internal static Dictionary<object, EventHandlerGroup> EventDic = new Dictionary<object, EventHandlerGroup>();

        /// <summary>
        /// 事件枚举类型
        /// </summary>
        public Type Type { get; internal set; }

        #endregion

        #region Construct

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type"></param>
        public EventDispatcher(Type type)
        {
            Type = type;
        }

        #endregion

        #region Add Listener

        /// <summary>
        /// 添加监听方法<para/>
        /// 需要指定对象
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">目标对象</param>
        /// <param name="method">监听方法</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(T eventType, object target, MethodInfo method, object group = null, int priority = 0, bool interrupt = false)
        {
            var handler = new EventHandler<T>
            {
                Type = eventType,
                Target = target,
                Group = group,
                Method = method,
                Parameters = method.GetParameters(),
                Action = _createDelegateFromMethodInfo<T>(target, method),
                Priority = priority,
                Interrupt = interrupt,
            };

            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            if (priority != 0 && !handlerGroup.NeedSort)
            {
                handlerGroup.NeedSort = true;
            }

            handlerGroup.Handlers.Add(handler);
            if (handlerGroup.NeedSort)
            {
                handlerGroup.SortEvents();
            }

            return handler;
        }

        /// <summary>
        /// 添加监听委托<para/>
        /// 无需指定对象
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler<T> AddListener<T>(T eventType, Action<T, object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var handler = new EventHandler<T>
            {
                Type = eventType,
                Target = null,
                Group = group,
                Method = null,
                Parameters = null,
                Action = action,
                Priority = priority,
                Interrupt = interrupt
            };

            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            if (priority != 0 && !handlerGroup.NeedSort)
            {
                handlerGroup.NeedSort = true;
            }

            handlerGroup.Handlers.Add(handler);
            if (handlerGroup.NeedSort)
            {
                handlerGroup.SortEvents();
            }

            return handler;
        }

        #endregion

        #region Has Listener

        /// <summary>
        /// 是否包含指定类型事件的监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <returns>结果</returns>
        public bool HasListener(object eventType)
        {
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var result = handlerGroup != null && handlerGroup.Handlers.Count > 0;
            return result;
        }

        #endregion

        #region Get Listeners

        /// <summary>
        /// 获取指定事件类型的所有监听事件数据
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <returns>监听事件数据列表</returns>
        public List<EventHandler> GetListeners(object eventType)
        {
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var handlers = handlerGroup.Handlers;
            return handlers;
        }

        /// <summary>
        /// 获取指定事件类型和指定目标的所有监听事件数据
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">目标对象</param>
        /// <returns>监听事件数据列表</returns>
        public List<EventHandler> GetListeners(object eventType, object target)
        {
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var handlers = handlerGroup.Handlers;
            var result = new List<EventHandler>();
            for (var i = 0; i < handlers.Count; i++)
            {
                var eventData = handlers[i];
                if (eventData.Target == target)
                {
                    result.Add(eventData);
                }
            }

            return result;
        }

        #endregion

        #region Remove Listener

        /// <summary>
        /// 移除某个监听对象的指定方法的监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">监听对象</param>
        /// <param name="method">监听方法</param>
        public void RemoveListener(object eventType, object target, MethodInfo method)
        {
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var handlers = handlerGroup.Handlers;
            for (var i = handlers.Count - 1; i >= 0; i--)
            {
                var eventData = handlers[i];
                if (eventData.Target == null || (eventData.Target.Equals(target) && eventData.Method.Equals(method)))
                {
                    handlers.Remove(eventData);
                }
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        public void RemoveListener<T>(T eventType, Action<T, object[]> action)
        {
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var handlers = handlerGroup.Handlers;
            for (var i = handlers.Count - 1; i >= 0; i--)
            {
                if (!(handlers[i] is EventHandler<T> eventData)) continue;
                if (eventData.Action.Equals(action))
                {
                    handlers.Remove(eventData);
                }
            }
        }

        /// <summary>
        /// 移除指定对象的所有监听
        /// </summary>
        /// <param name="target">监听对象</param>
        public void RemoveAllListener(object target)
        {
            foreach (var eventGroup in EventDic.Values)
            {
                var handlers = eventGroup.Handlers;
                for (var i = handlers.Count - 1; i >= 0; i--)
                {
                    var eventData = handlers[i];
                    if (eventData.Target == target)
                    {
                        handlers.RemoveAt(i);
                    }
                }
            }
        }

        /// <summary>
        /// 移除所有监听
        /// </summary>
        public void RemoveAllListener()
        {
            foreach (var eventGroup in EventDic.Values)
            {
                eventGroup.Handlers.Clear();
            }
        }

        #endregion

        #region Dispatch

        /// <summary>
        /// 事件分发
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public void Dispatch<T>(T eventType, params object[] args)
        {
            var eventGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventList = eventGroup.Handlers;
            for (var i = 0; i < eventList.Count; i++)
            {
                var eventData = eventList[i];
                eventData.Invoke(args);
                if (eventData.Interrupt) break;
            }
        }

        #endregion

        #region Disptach To

        /// <summary>
        /// 事件分发
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">事件发送目标</param>
        /// <param name="args">事件参数</param>
        public void DispatchTo<T>(T eventType, object target, params object[] args)
        {
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventList = handlerGroup.Handlers;
            for (var i = 0; i < eventList.Count; i++)
            {
                var eventData = eventList[i];
                var condition1 = eventData.Target == null && target == null;
                var condition2 = eventData.Target != null && eventData.Target.Equals(target);
                var check = condition1 || condition2;
                if (!check) continue;
                eventData.Invoke(args);
                if (eventData.Interrupt) break;
            }
        }

        /// <summary>
        /// 事件分发
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="predicate">事件接收目标判断条件</param>
        /// <param name="args">事件参数</param>
        public void DispatchTo<T>(T eventType, Predicate<object> predicate, params object[] args)
        {
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventList = handlerGroup.Handlers;
            for (var i = 0; i < eventList.Count; i++)
            {
                var eventData = eventList[i];
                if (!predicate(eventData.Target)) continue;
                eventData.Invoke(args);
                if (eventData.Interrupt) break;
            }
        }

        #endregion

        #region Dispatch Group

        /// <summary>
        /// 事件分发
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="group">监听分组</param>
        /// <param name="args">事件参数</param>
        public void DispatchGroup<T>(T eventType, object group, params object[] args)
        {
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventList = handlerGroup.Handlers;
            for (var i = 0; i < eventList.Count; i++)
            {
                var eventData = eventList[i];
                var condition1 = eventData.Group == null && group == null;
                var condition2 = eventData.Group != null && eventData.Group.Equals(group);
                var check = condition1 || condition2;
                if (!check) continue;
                eventData.Invoke(args);
                if (eventData.Interrupt) break;
            }
        }

        #endregion

        #region Private

        /// <summary>
        /// 尝试从方法信息创建出委托
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="target">目标</param>
        /// <param name="method">方法信息</param>
        /// <returns>委托</returns>
        private Action<T, object[]> _createDelegateFromMethodInfo<T>(object target, MethodInfo method)
        {
            try
            {
                var del = Delegate.CreateDelegate(typeof(Action<T, object[]>), target, method);
                var action = del as Action<T, object[]>;
                return action;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// 尝试从字典获取对象，如不存在则添加
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="dic">事件缓存字典</param>
        /// <param name="eventType">事件类型值</param>
        /// <returns>结果</returns>
        private static EventHandlerGroup _getOrAddHandlerGroup<T>(IDictionary<object, EventHandlerGroup> dic, T eventType)
        {
            if (eventType == null) return null;
            if (dic.TryGetValue(eventType, out var ret))
            {
                return ret;
            }
            else
            {
                ret = new EventHandlerGroup(typeof(T), eventType);
                dic.Add(eventType, ret);
                return ret;
            }
        }

        #endregion
    }
}