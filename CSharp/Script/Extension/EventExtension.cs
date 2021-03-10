/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventExtension.cs
//  Info     : 事件系统相关扩展方法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Reflection;

namespace Aya.Events
{
    public static class EventExtension
    {
        #region Add Listener

        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="method">监听方法</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        public static void AddListener<T>(this object obj, T eventType, MethodInfo method, object group = null, int priority = 0, bool interrupt = false)
        {
            EventManager.GetDispatcher<T>().AddListener(eventType, obj, method, group, priority, interrupt);
        }

        /// <summary>
        /// 添加监听
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        /// <param name="group">监听分组</param>
        /// <param name="priority">优先级</param>
        /// <param name="interrupt">是否中断事件队列</param>
        public static void AddListener<T>(this object obj, T eventType, Action<T, object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            EventManager.GetDispatcher<T>().AddListener(eventType, action, group, priority, interrupt);
        }
        
        #endregion

        #region Remove Listener
        
        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="method">监听方法</param>
        public static void RemoveListener<T>(this object obj, T eventType, MethodInfo method)
        {
            EventManager.GetDispatcher<T>().RemoveListener(eventType, obj, method);
        }

        /// <summary>
        /// 移除监听
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="action">监听委托</param>
        public static void RemoveListener<T>(this object obj, T eventType, Action<T, object[]> action)
        {
            EventManager.GetDispatcher<T>().RemoveListener(eventType, action);
        }
        
        #endregion

        #region Dispatch
       
        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public static void Dispatch<T>(this object obj, T eventType, params object[] args)
        {
            EventManager.GetDispatcher<T>().Dispatch(eventType, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="obj">对象</param>        
        /// <param name="eventType">事件类型</param>
        /// <param name="target">事件接收目标</param>
        /// <param name="args">事件参数</param>
        public static void DispatchTo<T>(this object obj, T eventType, object target, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchTo(eventType, target, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="obj">对象</param>        
        /// <param name="eventType">事件类型</param>
        /// <param name="predicate">事件接收目标判断条件</param>
        /// <param name="args">事件参数</param>
        public static void DispatchTo<T>(this object obj, T eventType, Predicate<object> predicate, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchTo(eventType, predicate, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="obj">对象</param>        
        /// <param name="eventType">事件类型</param>
        /// <param name="group">监听分组</param>
        /// <param name="args">事件参数</param>
        public static void DispatchGroup<T>(this object obj, T eventType, object group, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchGroup(eventType, group, args);
        }

        #endregion
    }
}
