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
        #region Property
        
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

        #endregion

        #region Invoke
        
        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>执行结果</returns>
        public virtual bool Invoke(params object[] args)
        {
            var success = false;
            if (Method != null)
            {
                success = InvokeMethod(this, args);
            }

            UEventCallback.OnDispatched?.Invoke(this, args);

            return success;
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

            if (method == null || target == null) return false;

            try
            {
                object returnValue = null;
                if (parameters == null || parameters.Length == 0)
                {
                    returnValue = method.Invoke(target, null);
                }
                else
                {
                    // 自动附加第一个事件类型参数
                    var needEventTypeArg = parameters[0].Name == "eventType";
                    var argIndexOffset = needEventTypeArg ? 1 : 0;

                    if (parameters.Length == 1 + argIndexOffset && parameters[argIndexOffset].ParameterType == typeof(object[]))
                    {
                        // params object[]
                        returnValue = method.Invoke(target, needEventTypeArg ? new object[] { eventType, args } : new object[] { args });
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
                UEventCallback.OnError?.Invoke(eventHandler, args, exception);
                return false;
            }

            return true;
        } 

        #endregion
    }
}