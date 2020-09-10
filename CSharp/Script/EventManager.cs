/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventManager.cs
//  Info     : 事件管理器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Tip      : 用于创建、获取事件分发器和发送事件
//
//  Copyright : Aya Game Studio 2019
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
        /// <param name="eventEnumType">事件枚举类型</param>
        /// <returns>事件分发器</returns>
        public static EventDispatcher GetDispatcher(Type eventEnumType)
        {
            if (DispatcherDic.ContainsKey(eventEnumType))
            {
                return DispatcherDic[eventEnumType];
            }
            else
            {
                var dispatcher = new EventDispatcher(eventEnumType);
                DispatcherDic[eventEnumType] = dispatcher;
                return dispatcher;
            }
        }

        #endregion

        #region Dispatch

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public static void Dispatch<T>(T eventType, params object[] args)
        {
            GetDispatcher<T>().Dispatch(eventType, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public static void Dispatch(object eventType, params object[] args)
        {
            var eventEnumType = eventType.GetType();
            GetDispatcher(eventEnumType).Dispatch(eventType, args);
        }

        #endregion

        #region Dispatch To

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">事件接收目标</param>
        /// <param name="args">事件参数</param>
        public static void DispatchTo<T>(T eventType, object target, params object[] args)
        {
            GetDispatcher<T>().DispatchTo(eventType, target, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">事件接收目标</param>
        /// <param name="args">事件参数</param>
        public static void DispatchTo(object eventType, object target, params object[] args)
        {
            var eventEnumType = eventType.GetType();
            GetDispatcher(eventEnumType).DispatchTo(eventType, target, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="predicate">事件接收目标判断条件</param>
        /// <param name="args">事件参数</param>
        public static void DispatchTo<T>(T eventType, Predicate<object> predicate, params object[] args)
        {
            GetDispatcher<T>().DispatchTo(eventType, predicate, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="predicate">事件接收目标判断条件</param>
        /// <param name="args">事件参数</param>
        public static void DispatchTo(object eventType, Predicate<object> predicate, params object[] args)
        {
            var eventEnumType = eventType.GetType();
            GetDispatcher(eventEnumType).DispatchTo(eventType, predicate, args);
        }

        #endregion

        #region Dispatch Group

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="group">监听分组</param>
        /// <param name="args">事件参数</param>
        public static void DispatchGroup<T>(T eventType, object group, params object[] args)
        {
            GetDispatcher<T>().DispatchGroup(eventType, group, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="group">监听分组</param>
        /// <param name="args">事件参数</param>
        public static void DispatchGroup(object eventType, object group, params object[] args)
        {
            var eventEnumType = eventType.GetType();
            GetDispatcher(eventEnumType).DispatchGroup(eventType, group, args);
        }

        #endregion
    }
}