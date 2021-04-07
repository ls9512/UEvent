/////////////////////////////////////////////////////////////////////////////
//
//  Script   : UEventCallback.cs
//  Info     : 事件全局回调
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Events
{
    public static class UEventCallback
    {
        public static Action<EventHandler> OnAdded = delegate { };
        public static Action<EventHandler> OnRemoved = delegate { };
        public static Action<EventHandler, object, object[]> OnDispatched = delegate { };
        public static Action<EventHandler, object, object[], Exception> OnError = (eventHandler, eventType, args, exception) => Console.WriteLine(exception.ToString());
    }
}
