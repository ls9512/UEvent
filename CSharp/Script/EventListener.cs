/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventListener.cs
//  Info     : 事件监听器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
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
        protected static Dictionary<Type, Dictionary<MethodInfo, List<EventAttributeBase>>> MethodMap = new Dictionary<Type, Dictionary<MethodInfo, List<EventAttributeBase>>>();

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

        public static void Register(Type objType)
        {
            // 如果是未注册过的对象类型，则遍历所有被标记需要监听的方法，进行注册
            if (MethodMap.TryGetValue(objType, out var _)) return;
            var objEventDic = new Dictionary<MethodInfo, List<EventAttributeBase>>();
            MethodMap.Add(objType, objEventDic);
            var methodInfos = objType.GetMethods(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            for (var i = 0; i < methodInfos.Length; i++)
            {
                var methodInfo = methodInfos[i];
                var attributes = methodInfo.GetCustomAttributes(typeof(EventAttributeBase), false);
                if (attributes.Length <= 0) continue;

                var eventAttributeList = new List<EventAttributeBase>();
                objEventDic.Add(methodInfo, eventAttributeList);

                for (var j = 0; j < attributes.Length; j++)
                {
                    var attribute = attributes[j];
                    if (attribute == null) return;
                    var attributeTemp = attribute as EventAttributeBase;
                    if (attributeTemp == null) return;
                    eventAttributeList.Add(attributeTemp);
                }
            }
        }

        /// <summary>
        /// 注册监听器
        /// </summary>
        public void Register()
        {
            var objType = Listener.GetType();
            // 如果该类型对象已经被注册过，则直接遍历该类型所有需要监听的方法并注册到事件分发器
            if (!MethodMap.TryGetValue(objType, out var tempObjEventDic))
            {
                Register(objType);
                tempObjEventDic = MethodMap[objType];
            }

            foreach (var kv in tempObjEventDic)
            {
                var method = kv.Key;
                var eventAttributeList = kv.Value;
                for (var i = 0; i < eventAttributeList.Count; i++)
                {
                    var eventAttribute = eventAttributeList[i];
                    _addListenerWithAttribute(eventAttribute, method);
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
        /// 根据特性标签添加事件监听
        /// </summary>
        /// <param name="attribute">特性</param>
        /// <param name="methodInfo">监听方法</param>
        private void _addListenerWithAttribute(EventAttributeBase attribute, MethodInfo methodInfo)
        {
            // Listen Attribute
            if (attribute is ListenAttribute attrListen)
            {
                var priority = attrListen.Priority;
                var interrupt = attrListen.Interrupt;
                foreach (var eventType in attrListen.Types)
                {
                    _addListener(eventType, methodInfo, null, priority, interrupt);
                }
            }

            // Listen Type Attribute
            if (attribute is ListenTypeAttribute attrListenType)
            {
                var eventEnumType = attrListenType.Type;
                var priority = attrListenType.Priority;
                var interrupt = attrListenType.Interrupt;
                var eventEnumArray = Enum.GetValues(eventEnumType);
                foreach (var eventType in eventEnumArray)
                {
                    _addListener(eventType, methodInfo, null, priority, interrupt);
                }
            }

            // Listen Group Attribute
            if (attribute is ListenGroupAttribute attrListenGroup)
            {
                var eventType = attrListenGroup.Type;
                var group = attrListenGroup.Group;
                var priority = attrListenGroup.Priority;
                var interrupt = attrListenGroup.Interrupt;
                _addListener(eventType, methodInfo, group, priority, interrupt);
            }
        }

        /// <summary>
        /// 将监听器中的方法注册到事件分发器
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="methodInfo">监听 方法</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        private void _addListener(object eventType, MethodInfo methodInfo, object group = null, int priority = 0, bool interrupt = false)
        {
            EventDispatcher dispatcher;
            if (eventType is Type type)
            {
                dispatcher = EventManager.GetDispatcher(type);
            }
            else
            {
                dispatcher = EventManager.GetDispatcher(eventType.GetType());
            }

            dispatcher.AddListener(eventType, Listener, methodInfo, group, priority, interrupt);
            RegisteredDispatchers.Add(dispatcher);
        }

        #endregion
    }
}