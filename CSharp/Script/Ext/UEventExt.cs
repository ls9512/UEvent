/////////////////////////////////////////////////////////////////////////////
//
//  Script   : UEventExt.cs
//  Info     : 事件快速调用接口 - 非枚举类型事件扩展
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Aya.Events
{
    public static partial class UEvent<T> where T : new()
    {
        #region Listen Action

        public static void Listen(Action action, object group = null, int priority = 0, bool interrupt = false) 
        {
            EventManager.GetDispatcher<T>().AddListener<T>(action, group, priority, interrupt);
        }

        public static void Remove(Action action)
        {
            EventManager.GetDispatcher<T>().RemoveListener<T>(action);
        } 

        #endregion

        #region Listen Action<T>

        public static void Listen(Action<T> action, object group = null, int priority = 0, bool interrupt = false)
        {
            EventManager.GetDispatcher<T>().AddListener<T>(action, group, priority, interrupt);
        }

        public static void Remove(Action<T> action)
        {
            EventManager.GetDispatcher<T>().RemoveListener<T>(action);
        } 

        #endregion

        #region Listen MehtodInfo

        public static void Listen(object target, MethodInfo methodInfo, object group = null, int priority = 0, bool interrupt = false)
        {
            EventManager.GetDispatcher<T>().AddListener<T>(target, methodInfo, group, priority, interrupt);
        }

        public static void Remove(object target, MethodInfo methodInfo)
        {
            EventManager.GetDispatcher<T>().RemoveListener(target, methodInfo);
        }

        #endregion

        #region Contains Listener

        public static bool Contains()
        {
            return EventManager.GetDispatcher<T>().ContainsListener<T>();
        }

        #endregion

        #region Get Listener

        public static List<EventHandler> Get()
        {
            return EventManager.GetDispatcher<T>().GetListeners<T>();
        }

        public static List<EventHandler> Get(object target)
        {
            return EventManager.GetDispatcher<T>().GetListeners<T>(target);
        }

        #endregion

        #region Dispatch

        public static void Dispatch(T eventData)
        {
            EventManager.GetDispatcher<T>().Dispatch<T>(eventData);
        }

        public static void DispatchTo(object target, T eventData)
        {
            EventManager.GetDispatcher<T>().DispatchTo<T>(target, eventData);
        }

        public static void DispatchTo(Predicate<object> predicate, T eventData)
        {
            EventManager.GetDispatcher<T>().DispatchTo<T>(predicate, eventData);
        }

        public static void DispatchGroup(object group, T eventData)
        {
            EventManager.GetDispatcher<T>().DispatchGroup<T>(group, eventData);
        } 

        #endregion
    }
}
