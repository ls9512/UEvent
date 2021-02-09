/////////////////////////////////////////////////////////////////////////////
//
//  Script   : UEvent.cs
//  Info     : 事件快速调用接口
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aya.Events
{
    public static partial class UEvent
    {
        #region Listen Action

        public static void Listen<T>(T eventType, Action action, object group = null, int priority = 0, bool interrupt = false)
        {
            EventManager.GetDispatcher<T>().AddListener(eventType, action, group, priority, interrupt);
        }

        public static void Remove<T>(T eventType, Action action)
        {
            EventManager.GetDispatcher<T>().RemoveListener(eventType, action);
        } 

        #endregion

        #region Listen Action<T>

        public static void Listen<T>(T eventType, Action<T> action, object group = null, int priority = 0, bool interrupt = false)
        {
            EventManager.GetDispatcher<T>().AddListener(eventType, action, group, priority, interrupt);
        }

        public static void Remove<T>(T eventType, Action<T> action)
        {
            EventManager.GetDispatcher<T>().RemoveListener(eventType, action);
        } 

        #endregion

        #region Listen Action<T, object[]>

        public static void Listen<T>(T eventType, Action<T, object[]> action, object group = null, int priority = 0, bool interrupt = false)
        {
            EventManager.GetDispatcher<T>().AddListener(eventType, action, group, priority, interrupt);
        }

        public static void Remove<T>(T eventType, Action<T, object[]> action)
        {
            EventManager.GetDispatcher<T>().RemoveListener(eventType, action);
        } 

        #endregion

        #region Listen MehtodInfo

        public static void Listen<T>(T eventType, object target, MethodInfo methodInfo, object group = null, int priority = 0, bool interrupt = false)
        {
            EventManager.GetDispatcher<T>().AddListener(eventType, target, methodInfo, group, priority, interrupt);
        }

        public static void Remove<T>(T eventType, object target, MethodInfo methodInfo)
        {
            EventManager.GetDispatcher<T>().RemoveListener(eventType, target, methodInfo);
        }

        #endregion

        #region Contains Listener

        public static bool Contains<T>(T eventType)
        {
            return EventManager.GetDispatcher<T>().ContainsListener(eventType);
        }

        #endregion

        #region Get Listener

        public static List<EventHandler> Get<T>(T eventType)
        {
            return EventManager.GetDispatcher<T>().GetListeners(eventType);
        }

        public static List<EventHandler> Get<T>(T eventType, object target)
        {
            return EventManager.GetDispatcher<T>().GetListeners(eventType, target);
        }

        #endregion

        #region Dispatch

        public static void Dispatch<T>(T eventType, params object[] args)
        {
            EventManager.GetDispatcher<T>().Dispatch(eventType, args);
        }

        public static void DispatchTo<T>(T eventType, object target, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchTo(eventType, target, args);
        }

        public static void DispatchTo<T>(T eventType, Predicate<object> predicate, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchTo(eventType, predicate, args);
        }

        public static void DispatchGroup<T>(T eventType, object group, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchGroup(eventType, group, args);
        } 

        #endregion
    }
}
