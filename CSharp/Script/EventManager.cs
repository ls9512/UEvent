/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventManager.cs
//  Info     : 事件管理器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Tip      : 用于创建、获取事件分发器和发送事件
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;

namespace Aya.Events
{
    public partial class EventManager
    {
        #region Property

        /// <summary>
        /// 事件枚举的类型 - 事件分发器 字典
        /// </summary>
        private static readonly Dictionary<Type, EventDispatcher> DispatcherDic;

        #endregion

        #region Construct

        protected EventManager()
        {
        }

        static EventManager()
        {
            DispatcherDic = new Dictionary<Type, EventDispatcher>();
        }

        #endregion

        #region Get Dispatcher

        /// <summary>
        /// 获取事件分发器
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <returns>事件分发器</returns>
        public static EventDispatcher GetDispatcher<T>()
        {
            var eventType = typeof(T);
            return GetDispatcher(eventType);
        }

        /// <summary>
        /// 获取事件分发器
        /// </summary>
        /// <param name="eventType">事件枚举类型</param>
        /// <returns>事件分发器</returns>
        public static EventDispatcher GetDispatcher(Type eventType)
        {
            if (DispatcherDic.ContainsKey(eventType))
            {
                return DispatcherDic[eventType];
            }
            else
            {
                var dispatcher = new EventDispatcher(eventType);
                DispatcherDic[eventType] = dispatcher;
                return dispatcher;
            }
        }

        #endregion
    }
}