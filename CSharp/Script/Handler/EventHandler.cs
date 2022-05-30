/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventHandler.cs
//  Info     : 事件处理器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Reflection;

namespace Aya.Events
{
    [Serializable]
    public partial class EventHandler
    {
        #region Field

        /// <summary>
        /// 事件类型
        /// </summary>
        public Type Type;

        /// <summary>
        /// 事件类型值
        /// </summary>
        public object EventType;

        /// <summary>
        /// 事件目标对象
        /// </summary>
        public object Target;

        /// <summary>
        /// 监听分组
        /// </summary>
        public object Group;

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority;

        /// <summary>
        /// 是否中断事件队列
        /// </summary>
        public bool Interrupt;

        /// <summary>
        /// 监听委托
        /// </summary>
        public Action Action;

        /// <summary>
        /// 监听方法
        /// </summary>
        public MethodInfo Method;

        /// <summary>
        /// 监听方法的参数
        /// </summary>
        public ParameterInfo[] Parameters;

        #endregion

        #region Invoke

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType">事件类型值</param>
        /// <param name="args">参数</param>
        /// <returns>执行结果</returns>
        public virtual bool Invoke(object eventType, params object[] args)
        {
            var success = InvokeMethod(this, eventType, args);
            UEventCallback.OnDispatched?.Invoke(this, eventType, args);
            return success;
        }

        /// <summary>
        /// 执行监听方法
        /// </summary>
        /// <param name="eventHandler">监听事件数据</param>
        /// <param name="eventType">事件类型值</param>
        /// <param name="args">事件参数</param>
        /// <returns>执行结果</returns>
        internal static bool InvokeMethod(EventHandler eventHandler, object eventType, params object[] args)
        {
            var method = eventHandler.Method;
            var parameters = eventHandler.Parameters;
            var target = eventHandler.Target;
            if (method == null) return false;

            try
            {
                object returnValue = null;
                if (parameters == null || parameters.Length == 0)
                {
                    returnValue = method.Invoke(target, null);
                }
                else
                {
                    // Auto first param
                    var needEventTypeArg = parameters[0].Name == "eventType" || parameters[0].ParameterType == eventHandler.Type;
                    var argIndexOffset = 0;
                    argIndexOffset += needEventTypeArg ? 1 : 0;

                    if (parameters.Length == 1 + argIndexOffset && parameters[argIndexOffset].ParameterType == typeof(object[]))
                    {
                        // params eventType, object[]
                        returnValue = method.Invoke(target, needEventTypeArg ? new object[] { eventType, args } : new object[] { args });
                    }
                    else if (parameters.Length == argIndexOffset && needEventTypeArg)
                    {
                        // params eventType
                        returnValue = method.Invoke(target, new object[] {eventType});
                    }
                    else if (parameters.Length == args.Length)
                    {
                        returnValue = method.Invoke(target, args);
                    }
                    else
                    {
                        var argsOverride = new object[parameters.Length];

                        if (needEventTypeArg)
                        {
                            argsOverride[0] = eventType;
                        }

                        for (var i = argIndexOffset; i < parameters.Length && i < args.Length + argIndexOffset; i++)
                        {
                            argsOverride[i] = args[i - argIndexOffset];
                        }

                        returnValue = method.Invoke(target, argsOverride);
                    }
                }

                if (returnValue != null)
                {

                }
            }
            catch (Exception exception)
            {
                UEventCallback.OnError?.Invoke(eventHandler, eventType, args, exception);
                return false;
            }

            return true;
        }

        #endregion

        #region Internal
        
        internal static Type GetInternalType(object eventType)
        {
            if (eventType is Type type)
            {
                return type;
            }

            return eventType.GetType();
        }

        internal static object GetInternalEventType(object eventType)
        {
            if (eventType is Enum || eventType is string || eventType is Type)
            {
                return eventType;
            }

            return eventType.GetType();
        } 

        #endregion
    }
}