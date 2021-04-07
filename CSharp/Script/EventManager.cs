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
        /// 事件类型 - 事件分发器 字典
        /// </summary>
        internal static readonly Dictionary<Type, EventDispatcher> DispatcherDic;

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
            var type = typeof(T);
            return GetDispatcher(type);
        }

        /// <summary>
        /// 获取事件分发器
        /// </summary>
        /// <param name="type">事件类型</param>
        /// <returns>事件分发器</returns>
        public static EventDispatcher GetDispatcher(Type type)
        {
            if (DispatcherDic.ContainsKey(type))
            {
                return DispatcherDic[type];
            }
            else
            {
                var dispatcher = new EventDispatcher(type);
                DispatcherDic[type] = dispatcher;
                return dispatcher;
            }
        }

        #endregion
    }
}