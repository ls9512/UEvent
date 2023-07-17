/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventDispatcher.cs
//  Info     : 事件分发器
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
        #region Property

        /// <summary>
        /// 事件类型值 - 事件处理器组
        /// </summary>
        internal Dictionary<object, EventHandlerGroup> EventDic = new Dictionary<object, EventHandlerGroup>();

        /// <summary>
        /// 事件类型
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
            var eventHandler = _createEventHandler<T>(eventType, action, group, priority, interrupt);
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
            var eventHandler = _createEventHandler<T>(eventType, action, group, priority, interrupt);
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
        public EventHandler AddListener<T>(Action<T, object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventType = typeof(T);
            var eventHandler = _createEventHandler<T>(eventType, action, group, priority, interrupt);
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
        public EventHandler AddListener<T>(Action<object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventType = typeof(T);
            var eventHandler = _createEventHandler<T>(eventType, action, group, priority, interrupt);
            _cacheEventHandler(eventType, eventHandler);
            return eventHandler;
        }


        /// <summary>
        /// 添加监听方法<para/>
        /// 需要指定对象
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="target">目标对象</param>
        /// <param name="methodInfo">监听方法</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(object target, MethodInfo methodInfo, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventType = typeof(T);
            var eventHandler = _createEventHandler<T>(eventType, target, methodInfo, group, priority, interrupt);
            _cacheEventHandler(eventType, eventHandler);
            return eventHandler;
        }

        #endregion

        #region Add Listener T

        /// <summary>
        /// 添加监听委托<para/>
        /// 无需指定对象
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(T eventType, Action action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = _createEventHandler<T>(eventType, action, group, priority, interrupt);
            _cacheEventHandler(eventType, eventHandler);
            return eventHandler;
        }

        /// <summary>
        /// 添加监听委托<para/>
        /// 无需指定对象
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(T eventType, Action<T> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = _createEventHandler<T>(eventType, action, group, priority, interrupt);
            _cacheEventHandler(eventType, eventHandler);
            return eventHandler;
        }

        /// <summary>
        /// 添加监听委托<para/>
        /// 无需指定对象
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(T eventType, Action<T, object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = _createEventHandler<T>(eventType, action, group, priority, interrupt);
            _cacheEventHandler(eventType, eventHandler);
            return eventHandler;
        }

        /// <summary>
        /// 添加监听委托<para/>
        /// 无需指定对象
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(T eventType, Action<object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = _createEventHandler<T>(eventType, action, group, priority, interrupt);
            _cacheEventHandler(eventType, eventHandler);
            return eventHandler;
        }

        /// <summary>
        /// 添加监听方法<para/>
        /// 需要指定对象
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">目标对象</param>
        /// <param name="methodInfo">监听方法</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>监听事件数据</returns>
        public EventHandler AddListener<T>(T eventType, object target, MethodInfo methodInfo, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = _createEventHandler<T>(eventType, target, methodInfo, group, priority, interrupt);
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

        /// <summary>
        /// 是否包含指定类型事件的监听器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <returns>结果</returns>
        public bool ContainsListener<T>(T eventType)
        {
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
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            return eventHandlers;
        }

        /// <summary>
        /// 获取指定事件类型和指定目标的所有监听事件数据
        /// </summary>
        /// <param name="target">目标对象</param>
        /// <returns>监听事件数据列表</returns>
        public List<EventHandler> GetListeners<T>(object target)
        {
            var eventType = typeof(T);
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            var result = new List<EventHandler>();
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                if (target.Equals(eventHandler.Target))
                {
                    result.Add(eventHandler);
                }
            }

            return result;
        }

        #endregion

        #region Get Listeners T

        /// <summary>
        /// 获取指定事件类型的所有监听事件数据
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <returns>监听事件数据列表</returns>
        public List<EventHandler> GetListeners<T>(T eventType)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            return eventHandlers;
        }

        /// <summary>
        /// 获取指定事件类型和指定目标的所有监听事件数据
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">目标对象</param>
        /// <returns>监听事件数据列表</returns>
        public List<EventHandler> GetListeners<T>(T eventType, object target)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            var result = new List<EventHandler>();
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                if (target.Equals(eventHandler.Target))
                {
                    result.Add(eventHandler);
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
                if (action.Equals(eventHandler.Action))
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
                if (action.Equals(eventHandler.ActionT))
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
        public void RemoveListener<T>(Action<T, object[]> action)
        {
            var eventType = typeof(T);
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                if (!(eventHandlers[i] is EventHandler<T> eventHandler)) continue;
                if (action.Equals(eventHandler.ActionTArgs))
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
        public void RemoveListener<T>(Action<object[]> action)
        {
            var eventType = typeof(T);
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                if (!(eventHandlers[i] is EventHandler<T> eventHandler)) continue;
                if (action.Equals(eventHandler.ActionArgs))
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
            var eventType = typeof(T);
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                var eventHandler = eventHandlers[i];
                if (target.Equals(eventHandler.Target) && method.Equals(eventHandler.Method))
                {
                    eventHandlerGroup.Remove(eventHandler);
                }
            }
        }

        #endregion

        #region Remove Listener T

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        public void RemoveListener<T>(T eventType, Action action)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                var eventHandler = eventHandlers[i];
                if (action.Equals(eventHandler.Action))
                {
                    eventHandlerGroup.Remove(eventHandler);
                }
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        public void RemoveListener<T>(T eventType, Action<T> action)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                if (!(eventHandlers[i] is EventHandler<T> eventHandler)) continue;
                if (action.Equals(eventHandler.ActionT))
                {
                    eventHandlerGroup.Remove(eventHandler);
                }
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        public void RemoveListener<T>(T eventType, Action<T, object[]> action)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                if (!(eventHandlers[i] is EventHandler<T> eventHandler)) continue;
                if (action.Equals(eventHandler.ActionTArgs))
                {
                    eventHandlerGroup.Remove(eventHandler);
                }
            }
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        public void RemoveListener<T>(T eventType, Action<object[]> action)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                if (!(eventHandlers[i] is EventHandler<T> eventHandler)) continue;
                if (action.Equals(eventHandler.ActionArgs))
                {
                    eventHandlerGroup.Remove(eventHandler);
                }
            }
        }

        /// <summary>
        /// 移除某个监听对象的指定方法的监听
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">监听对象</param>
        /// <param name="method">监听方法</param>
        public void RemoveListener<T>(T eventType, object target, MethodInfo method)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = eventHandlers.Count - 1; i >= 0; i--)
            {
                var eventHandler = eventHandlers[i];
                if (target.Equals(eventHandler.Target) && method.Equals(eventHandler.Method))
                {
                    eventHandlerGroup.Remove(eventHandler);
                }
            }
        }

        /// <summary>
        /// 移除指定对象的所有监听
        /// </summary>
        /// <param name="target">监听对象</param>
        public void RemoveAllListener(object target)
        {
            foreach (var eventHandlerGroup in EventDic.Values)
            {
                var eventHandlers = eventHandlerGroup.Handlers;
                for (var i = eventHandlers.Count - 1; i >= 0; i--)
                {
                    var eventHandler = eventHandlers[i];
                    if (eventHandler.Target == target)
                    {
                        eventHandlerGroup.Remove(eventHandler);
                    }
                }
            }
        }

        /// <summary>
        /// 移除所有监听
        /// </summary>
        public void RemoveAllListener()
        {
            foreach (var eventHandlerGroup in EventDic.Values)
            {
                eventHandlerGroup.Handlers.Clear();
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
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                eventHandler.Invoke(eventType, args);
                if (eventHandler.Interrupt) break;
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
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                var condition1 = eventHandler.Target == null && target == null;
                var condition2 = target != null && target.Equals(eventHandler.Target);
                var check = condition1 || condition2;
                if (!check) continue;
                eventHandler.Invoke(eventType, args);
                if (eventHandler.Interrupt) break;
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
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                if (!predicate(eventHandler.Target)) continue;
                eventHandler.Invoke(eventType, args);
                if (eventHandler.Interrupt) break;
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
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                var condition1 = eventHandler.Group == null && group == null;
                var condition2 = group != null && group.Equals(eventHandler.Group);
                var check = condition1 || condition2;
                if (!check) continue;
                eventHandler.Invoke(eventType, args);
                if (eventHandler.Interrupt) break;
            }
        }

        #endregion

        #region Private

        private EventHandler _createEventHandler<T>(object eventType, Action action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = new EventHandler
            {
                Type = EventHandler.GetInternalType(eventType),
                EventType = eventType,
                Target = action.Target,
                Group = group,
                Priority = priority,
                Interrupt = interrupt,
                Method = action.Method,
                Parameters = action.Method.GetParameters(),
                Action = action,
            };

            return eventHandler;
        }

        private EventHandler _createEventHandler<T>(object eventType, Action<T> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = new EventHandler<T>
            {
                Type = EventHandler.GetInternalType(eventType),
                EventType = eventType,
                Target = action.Target,
                Group = group,
                Priority = priority,
                Interrupt = interrupt,
                Method = action.Method,
                Parameters = action.Method.GetParameters(),
                Action = null,
                ActionT = action,
                ActionTArgs = null,
                ActionArgs = null
            };

            return eventHandler;
        }

        private EventHandler _createEventHandler<T>(object eventType, Action<T, object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = new EventHandler<T>
            {
                Type = EventHandler.GetInternalType(eventType),
                EventType = eventType,
                Target = action.Target,
                Group = group,
                Priority = priority,
                Interrupt = interrupt,
                Method = action.Method,
                Parameters = action.Method.GetParameters(),
                Action = null,
                ActionT = null,
                ActionTArgs = action,
                ActionArgs = null,
            };

            return eventHandler;
        }

        private EventHandler _createEventHandler<T>(object eventType, Action<object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = new EventHandler<T>
            {
                Type = EventHandler.GetInternalType(eventType),
                EventType = eventType,
                Target = action.Target,
                Group = group,
                Priority = priority,
                Interrupt = interrupt,
                Method = action.Method,
                Parameters = action.Method.GetParameters(),
                Action = null,
                ActionT = null,
                ActionTArgs = null,
                ActionArgs = action,
            };

            return eventHandler;
        }

        private EventHandler _createEventHandler<T>(object eventType, object target, MethodInfo methodInfo, object group = null, int priority = 0, bool interrupt = false)
        {
            var eventHandler = new EventHandler
            {
                Type = EventHandler.GetInternalType(eventType),
                EventType = eventType,
                Target = target,
                Group = group,
                Priority = priority,
                Interrupt = interrupt,
                Method = methodInfo,
                Parameters = methodInfo.GetParameters(),
                Action = null,
            };

            return eventHandler;
        }

        private void _cacheEventHandler<T>(T eventType, EventHandler eventHandler)
        {
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            handlerGroup.Add(eventHandler);
        }

        private static EventHandlerGroup _getOrAddHandlerGroup(IDictionary<object, EventHandlerGroup> dic, object eventType)
        {
            eventType = EventHandler.GetInternalEventType(eventType);
            if (dic.TryGetValue(eventType, out var ret))
            {
                return ret;
            }
            else
            {
                var type = EventHandler.GetInternalType(eventType);
                ret = new EventHandlerGroup(type, eventType);
                dic.Add(eventType, ret);
                return ret;
            }
        }

        #endregion
    }
}