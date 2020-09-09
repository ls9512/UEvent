/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ObjectListener.cs
//  Info     : 常规类型事件处理器
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
    public abstract partial class ObjectListener : IEventListener
    {
        #region Property

        /// <summary>
        /// 事件监听器
        /// </summary>
        public EventListener EventListener { get; }

        #endregion

        #region Construct

        protected ObjectListener()
        {
            EventListener = new EventListener(this);
            EventListener.Register();
        }

        protected ObjectListener(object listener)
        {
            EventListener = new EventListener(listener);
            EventListener.Register();
        }

        ~ObjectListener()
        {
            EventListener.DeRegister();
        }

        #endregion

        #region Add Listener

        /// <summary>
        /// 添加监听
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="method">监听方法</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>结果</returns>
        public EventHandler AddListener<T>(T eventType, MethodInfo method, object group = null, int priority = 0, bool interrupt = false)
        {
            return EventManager.GetDispatcher<T>().AddListener(eventType, this, method, group, priority, interrupt);
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        /// <returns>结果</returns>
        public EventHandler<T> AddListener<T>(T eventType, Action<T, object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            return EventManager.GetDispatcher<T>().AddListener(eventType, action, group, priority, interrupt);
        }

        #endregion

        #region Has Listener

        /// <summary>
        /// 是否包含指定类型事件的监听器
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <returns>结果</returns>
        public bool HasListener<T>(T eventType)
        {
            return EventManager.GetDispatcher<T>().HasListener(eventType);
        }

        #endregion

        #region Get Listeners

        /// <summary>
        /// 获取注册于当前对象上指定事件类型的所有监听事件数据
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <returns>监听事件数据列表</returns>
        public List<EventHandler> GetListeners<T>(T eventType)
        {
            return EventManager.GetDispatcher<T>().GetListeners(eventType, this);
        }

        #endregion

        #region Remove Listener

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="method">监听方法</param>
        public void RemoveListener<T>(T eventType, MethodInfo method)
        {
            EventManager.GetDispatcher<T>().RemoveListener(eventType, this, method);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        public void RemoveListener<T>(T eventType, Action<T, object[]> action)
        {
            EventManager.GetDispatcher<T>().RemoveListener(eventType, action);
        }

        #endregion

        #region Dispatch

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public void Dispatch<T>(T eventType, params object[] args)
        {
            EventManager.GetDispatcher<T>().Dispatch(eventType, args);
        }

        #endregion

        #region Dispatch To

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="target">事件接收目标</param>
        /// <param name="args">事件参数</param>
        public void DispatchTo<T>(T eventType, object target, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchTo(eventType, target, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="predicate">事件接收目标判断条件</param>
        /// <param name="args">事件参数</param>
        public void DispatchTo<T>(T eventType, Predicate<object> predicate, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchTo(eventType, predicate, args);
        }

        #endregion

        #region Dispatch Group

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="group">监听分组</param>
        /// <param name="args">事件参数</param>
        public void DispatchGroup<T>(T eventType, object group, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchGroup(eventType, group, args);
        }

        #endregion
    }
}