/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventHandlerT.cs
//  Info     : 事件处理器 泛型版本，用于约束指定类型的事件委托
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Events
{
    [Serializable]
    public class EventHandler<T> : EventHandler
    {
        /// <summary>
        /// 间厅委托
        /// </summary>
        public Action<T> ActionT1 { get; set; }

        /// <summary>
        /// 监听委托
        /// </summary>
        public Action<T, object[]> ActionT2 { get; set; }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>执行结果</returns>
        public override bool Invoke(params object[] args)
        {
            var result = false;
            if (ActionT2 != null)
            {
                result = InvokeAction(this, args);
                UEventCallback.OnDispatched?.Invoke(this, args);
                return result;
            }
            else
            {
                result = base.Invoke(args);
            }

            return result;
        }

        /// <summary>
        /// 执行监听委托
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventHandler">监听事件数据</param>
        /// <param name="args">事件参数</param>
        /// <returns>执行结果</returns>
        internal static bool InvokeAction(EventHandler<T> eventHandler, params object[] args)
        {
            var actionT1 = eventHandler.ActionT1;
            if (actionT1 != null)
            {
                try
                {
                    actionT1((T) eventHandler.Type);
                }
                catch (Exception exception)
                {
                    UEventCallback.OnError?.Invoke(eventHandler, exception);
                    return false;
                }
            }
            else
            {
                var actionT2 = eventHandler.ActionT2;
                if (actionT2 == null) return false;
                try
                {
                    actionT2((T) eventHandler.Type, args);
                }
                catch (Exception exception)
                {
                    UEventCallback.OnError?.Invoke(eventHandler, exception);
                    return false;
                }
            }

            return true;
        }
    }
}