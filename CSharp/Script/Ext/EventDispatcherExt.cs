/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventDispatcherExt.cs
//  Info     : 事件分发器 - 非枚举类型事件扩展
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aya.Events
{
    public partial class EventDispatcher
    {
        #region Add Listener

        /// <summary>
        /// 添加监听委托<para/>
        /// 无需指定对象
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="action">监听委托</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(Action action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventType = typeof(T);
            var eventHandler = _createEventHandler(eventType, action, group, priority, interrupt);
            _cacheEventHandler(eventType, eventHandler);
            return eventHandler;
        }

        /// <summary>
        /// 添加监听委托<para/>
        /// 无需指定对象
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="action">监听委托</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(Action<T> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventType = typeof(T);
            var eventHandler = _createEventHandler<T>(action, group, priority, interrupt);
            _cacheEventHandler(eventType, eventHandler);
            return eventHandler;
        }

        /// <summary>
        /// 添加监听方法<para/>
        /// 需要指定对象
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="target">目标对象</param>
        /// <param name="method">监听方法</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(object target, MethodInfo method, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventType = typeof(T);
            var eventHandler = _createEventHandler(eventType, target, method, group, priority, interrupt);
            _cacheEventHandler(eventType, eventHandler);
            return eventHandler;
        }

        #endregion

        #region  Contains Listener

        /// <summary>
        /// 是否包含指定类型事件的监听器
        /// </summary>
        /// <returns>结果</returns>
        public bool ContainsListener<T>()
        {
            var eventType = typeof(T);
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var result = eventHandlerGroup.Handlers.Count > 0;
            return result;
        }

        #endregion

        #region Get Listeners

        /// <summary>
        /// 获取指定事件类型的所有监听事件数据
        /// </summary>
        /// <returns>监听事件数据列表</returns>
        public List<EventHandler> GetListeners<T>()
        {
            var eventType = typeof(T);
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var handlers = handlerGroup.Handlers;
            return handlers;
        }

        /// <summary>
        /// 获取指定事件类型和指定目标的所有监听事件数据
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <returns>监听事件数据列表</returns>
        public List<EventHandler> GetListeners<T>(object target)
        {
            var eventType = typeof(T);
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
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="action">监听委托</param>
        public void RemoveListener<T>(Action action)
        {
            var eventType = typeof(T);
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                var eventHandler = eventHandlers[i];
                if (eventHandler.Action.Equals(action))
                {
                    eventHandlerGroup.Remove(eventHandler);
                }
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="action">监听委托</param>
        public void RemoveListener<T>(Action<T> action)
        {
            var eventType = typeof(T);
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                if (!(eventHandlers[i] is EventHandler<T> eventHandler)) continue;
                if (eventHandler.ActionT1.Equals(action))
                {
                    eventHandlerGroup.Remove(eventHandler);
                }
            }
        }

        /// <summary>
        /// 移除某个监听对象的指定方法的监听
        /// </summary>
        /// <param name="target">监听对象</param>
        /// <param name="method">监听方法</param>
        public void RemoveListener<T>(object target, MethodInfo method)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup<T>(EventDic);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                var eventHandler = eventHandlers[i];
                if (eventHandler.Target == null || (eventHandler.Target.Equals(target) && eventHandler.Method.Equals(method)))
                {
                    eventHandlerGroup.Remove(eventHandler);
                }
            }
        }

        #endregion

        #region Dispatch

        /// <summary>
        /// 事件分发
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventData">事件参数</param>
        public void Dispatch<T>(T eventData)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup<T>(EventDic);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                eventHandler.Invoke(eventData);
                if (eventHandler.Interrupt) break;
            }
        }

        #endregion

        #region Disptach To

        /// <summary>
        /// 事件分发
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="target">事件发送目标</param>
        /// <param name="eventData">事件参数</param>
        public void DispatchTo<T>(object target, T eventData)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup<T>(EventDic);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                var condition1 = eventHandler.Target == null && target == null;
                var condition2 = eventHandler.Target != null && eventHandler.Target.Equals(target);
                var check = condition1 || condition2;
                if (!check) continue;
                eventHandler.Invoke(eventData);
                if (eventHandler.Interrupt) break;
            }
        }

        /// <summary>
        /// 事件分发
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="predicate">事件接收目标判断条件</param>
        /// <param name="eventData">事件参数</param>
        public void DispatchTo<T>(Predicate<object> predicate, T eventData)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup<T>(EventDic);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                if (!predicate(eventHandler.Target)) continue;
                eventHandler.Invoke(eventData);
                if (eventHandler.Interrupt) break;
            }
        }

        #endregion

        #region Dispatch Group

        /// <summary>
        /// 事件分发
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="group">监听分组</param>
        /// <param name="eventData">事件参数</param>
        public void DispatchGroup<T>(object group, T eventData)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup<T>(EventDic);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                var condition1 = eventHandler.Group == null && group == null;
                var condition2 = eventHandler.Group != null && eventHandler.Group.Equals(group);
                var check = condition1 || condition2;
                if (!check) continue;
                eventHandler.Invoke(eventData);
                if (eventHandler.Interrupt) break;
            }
        }

        #endregion

        #region Private

        private EventHandler _createEventHandler<T>(Action action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = new EventHandler
            {
                Type = typeof(T),
                Target = action.Target,
                Group = group,
                Priority = priority,
                Interrupt = interrupt,
                Method = action.Method,
                Parameters = action.Method.GetParameters(),
                Action = action
            };

            return eventHandler;
        }

        private EventHandler _createEventHandler<T>(Action<T> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = new EventHandler<T>
            {
                Type = typeof(T),
                Target = action.Target,
                Group = group,
                Priority = priority,
                Interrupt = interrupt,
                Method = action.Method,
                Parameters = action.Method.GetParameters(),
                Action = null,
                ActionT1 = action,
                ActionT2 = null
            };

            return eventHandler;
        }

        private EventHandler _createEventHandler<T>(object target, MethodInfo methodInfo, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = new EventHandler
            {
                Type = typeof(T),
                Target = target,
                Group = group,
                Priority = priority,
                Interrupt = interrupt,
                Method = methodInfo,
                Parameters = methodInfo.GetParameters(),
                Action = null
            };

            return eventHandler;
        }

        private void _cacheEventHandler<T>(EventHandler eventHandler)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup<T>(EventDic);
            eventHandlerGroup.Add(eventHandler);
        }

        /// <summary>
        /// 尝试从字典获取对象，如不存在则添加
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="dic">事件缓存字典</param>
        /// <returns>结果</returns>
        private static EventHandlerGroup _getOrAddHandlerGroup<T>(IDictionary<object, EventHandlerGroup> dic)
        {
            var eventType = typeof(T);
            if (dic.TryGetValue(eventType, out var ret))
            {
                return ret;
            }
            else
            {
                ret = new EventHandlerGroup(eventType, eventType);
                dic.Add(eventType, ret);
                return ret;
            }
        }

        #endregion
    }
}