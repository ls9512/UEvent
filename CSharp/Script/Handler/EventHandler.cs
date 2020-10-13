/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventHandler.cs
//  Info     : 事件处理器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Reflection;

namespace Aya.Events
{
    [Serializable]
    public class EventHandler
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public object Type { get; set; }

        /// <summary>
        /// 事件目标对象
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// 监听分组
        /// </summary>
        public object Group { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        public int Priority { get; set; }

        /// <summary>
        /// 是否中断事件队列
        /// </summary>
        public bool Interrupt { get; set; }

        /// <summary>
        /// 监听委托
        /// </summary>
        public Action Action { get; set; }

        /// <summary>
        /// 监听方法
        /// </summary>
        public MethodInfo Method { get; set; }

        /// <summary>
        /// 监听方法的参数
        /// </summary>
        public ParameterInfo[] Parameters { get; set; }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>执行结果</returns>
        public virtual bool Invoke(params object[] args)
        {
            var result = false;
            if (Action != null)
            {
                result = InvokeAction(this);
            }
            else if (Method != null)
            {
                result = InvokeMethod(this, args);
            }

            return result;
        }

        /// <summary>
        /// 执行监听委托
        /// </summary>
        /// <param name="eventHandler">监听事件数据</param>
        /// <returns>执行结果</returns>
        internal static bool InvokeAction(EventHandler eventHandler)
        {
            var action = eventHandler.Action;
            if (action == null) return false;

            try
            {
                action.Invoke();
            }
            catch (Exception exception)
            {
                EventInterface.OnError(exception);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 执行监听方法
        /// </summary>
        /// <param name="eventHandler">监听事件数据</param>
        /// <param name="args">事件参数</param>
        /// <returns>执行结果</returns>
        internal static bool InvokeMethod(EventHandler eventHandler, params object[] args)
        {
            var eventType = eventHandler.Type;
            var method = eventHandler.Method;
            var parameters = eventHandler.Parameters;
            var target = eventHandler.Target;
            if (method == null) return false;
            if (target == null) return false;

            try
            {
                object result = null;
                if (parameters == null || parameters.Length == 0)
                {
                    result = method.Invoke(target, null);
                }
                else
                {
                    // 自动附加第一个事件类型参数
                    var needEventTypeArg = parameters[0].Name == "eventType";
                    var argIndexOffset = needEventTypeArg ? 1 : 0;

                    if (parameters.Length == 1 + argIndexOffset && parameters[argIndexOffset].ParameterType == typeof(object[]))
                    {
                        // params object[]
                        result = method.Invoke(target, needEventTypeArg ? new object[] { eventType, args } : new object[] { args });
                    }
                    else if (parameters.Length == args.Length)
                    {
                        result = method.Invoke(target, args);
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

                        result = method.Invoke(target, argsOverride);
                    }
                }

                if (result != null)
                {

                }
            }
            catch (Exception exception)
            {
                EventInterface.OnError(exception);
                return false;
            }

            return true;
        }
    }
}