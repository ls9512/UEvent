/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventListener.cs
//  Info     : 事件监听器
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
    public partial class EventListener
    {
        #region Property

        /// <summary>
        /// 监听对象类型 - 监听方法 - 包含监听事件类型列表
        /// </summary>
        protected static Dictionary<Type, Dictionary<MethodInfo, List<object>>> MethodMap = new Dictionary<Type, Dictionary<MethodInfo, List<object>>>();

        /// <summary>
        /// 事件监听对象
        /// </summary>
        public object Listener { get; }

        /// <summary>
        /// 事件分发器注册列表
        /// </summary>
        protected HashSet<EventDispatcher> RegisteredDispatchers = new HashSet<EventDispatcher>();

        #endregion

        #region Construct

        public EventListener(object listener)
        {
            Listener = listener;
        }

        #endregion

        #region Register / DeRegister

        /// <summary>
        /// 注册监听器
        /// </summary>
        public void Register()
        {
            var objType = Listener.GetType();
            // 如果该类型对象已经被注册过，则直接遍历该类型所有需要监听的方法并注册到事件分发器
            if (MethodMap.ContainsKey(objType))
            {
                var tempObjEventDic = MethodMap[objType];
                foreach (var kv in tempObjEventDic)
                {
                    var method = kv.Key;
                    var eventTypeList = kv.Value;
                    for (var i = 0; i < eventTypeList.Count; i++)
                    {
                        var eventType = eventTypeList[i];
                        _addListener(eventType, method);
                    }
                }

                return;
            }

            // 如果是未注册过的对象类型，则遍历所有被标记需要监听的方法，进行注册
            var objEventDic = new Dictionary<MethodInfo, List<object>>();
            MethodMap.Add(objType, objEventDic);
            var methods = objType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            for (var i = 0; i < methods.Length; i++)
            {
                var method = methods[i];
                var attrs = method.GetCustomAttributes(typeof(Attribute), false);
                if (attrs.Length <= 0) continue;

                var eventList = new List<object>();
                objEventDic.Add(method, eventList);

                for (var j = 0; j < attrs.Length; j++)
                {
                    var attrTemp = attrs[j];
                    if (attrTemp == null) return;
                    // Listen Attribute
                    if (attrTemp is ListenAttribute attrListen)
                    {
                        var priority = attrListen.Priority;
                        var interrupt = attrListen.Interrupt;
                        foreach (var eventType in attrListen.Types)
                        {
                            eventList.Add(eventType);
                            _addListener(eventType, method, null, priority, interrupt);
                        }
                    }

                    // Listen Type Attribute
                    if (attrTemp is ListenTypeAttribute attrListenType)
                    {
                        var eventEnumType = attrListenType.Type;
                        var priority = attrListenType.Priority;
                        var interrupt = attrListenType.Interrupt;
                        var eventEnumArray = Enum.GetValues(eventEnumType);
                        foreach (var eventType in eventEnumArray)
                        {
                            eventList.Add(eventType);
                            _addListener(eventType, method, null, priority, interrupt);
                        }
                    }

                    // Listen Group Attribute
                    if (attrTemp is ListenGroupAttribute attrListenGroup)
                    {
                        var eventType = attrListenGroup.Type;
                        var group = attrListenGroup.Group;
                        var priority = attrListenGroup.Priority;
                        var interrupt = attrListenGroup.Interrupt;
                        eventList.Add(eventType);
                        _addListener(eventType, method, group, priority, interrupt);
                    }
                }
            }
        }

        /// <summary>
        /// 反注册监听器
        /// </summary>
        public void DeRegister()
        {
            foreach (var dispatcher in RegisteredDispatchers)
            {
                dispatcher.RemoveAllListener(Listener);
            }

            RegisteredDispatchers.Clear();
        }

        /// <summary>
        /// 更新注册信息
        /// </summary>
        public void UpdateRegister()
        {
            DeRegister();
            Register();
        }

        #endregion

        #region Private

        /// <summary>
        /// 将监听器中的方法注册到事件分发器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="method">监听 方法</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        private void _addListener<T>(T eventType, MethodInfo method, object group = null, int priority = 0, bool interrupt = false)
        {
            var dispatcher = EventManager.GetDispatcher<T>();
            dispatcher.AddListener(eventType, Listener, method, group, priority, interrupt);
            if (!RegisteredDispatchers.Contains(dispatcher))
            {
                RegisteredDispatchers.Add(dispatcher);
            }
        }

        #endregion
    }
}