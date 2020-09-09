/////////////////////////////////////////////////////////////////////////////
//
//  Script   : IEventListener.cs
//  Info     : 事件处理器接口
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
    public partial interface IEventListener
    {
        EventHandler AddListener<T>(T eventType, MethodInfo method, object group = null, int priority = 0, bool interrupt = false);
        EventHandler<T> AddListener<T>(T eventType, Action<T, object[]> action, object group = null, int priority = 0, bool interrupt = false);

        bool HasListener<T>(T eventType);

        List<EventHandler> GetListeners<T>(T eventType);

        void RemoveListener<T>(T eventType, MethodInfo method);
        void RemoveListener<T>(T eventType, Action<T, object[]> action);

        void Dispatch<T>(T eventType, params object[] args);
        void DispatchTo<T>(T eventType, object target, params object[] args);
        void DispatchTo<T>(T eventType, Predicate<object> predicate, params object[] args);
        void DispatchGroup<T>(T eventType, object group, params object[] args);
    }
}
