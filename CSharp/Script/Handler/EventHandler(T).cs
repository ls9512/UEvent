/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventHandler(T).cs
//  Info     : 事件处理器 泛型版本，用于约束指定类型的事件委托
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Events
{
    [Serializable]
    public partial class EventHandler<T> : EventHandler
    {
        public Action<T> ActionT;

        public Action<T, object[]> ActionTArgs;

        public Action<object[]> ActionArgs;

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="eventType">事件类型值</param>
        /// <param name="args">参数</param>
        /// <returns>执行结果</returns>
        public override bool Invoke(object eventType, params object[] args)
        {
            var success = InvokeAction(this, eventType, args);
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
        internal static bool InvokeAction(EventHandler<T> eventHandler, object eventType, params object[] args)
        {
            try
            {
                if (eventHandler.ActionT != null)
                {
                    eventHandler.ActionT.Invoke((T) eventType);
                }
                else if (eventHandler.ActionTArgs != null)
                {
                    eventHandler.ActionTArgs.Invoke((T) eventType, args);
                }
                else if (eventHandler.ActionArgs != null)
                {
                    eventHandler.ActionArgs.Invoke(args);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception exception)
            {
                UEventCallback.OnError?.Invoke(eventHandler, eventType, args, exception);
                return false;
            }

            return true;
        }
    }
}