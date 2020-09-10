/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventHandlerT.cs
//  Info     : 事件处理器 泛型版本，用于约束指定类型的事件委托
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Events
{
    [Serializable]
    public class EventHandler<T> : EventHandler
    {
        /// <summary>
        /// 监听委托
        /// </summary>
        public Action<T, object[]> Action { get; set; }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>执行结果</returns>
        public override bool Invoke(params object[] args)
        {
            if (Action != null)
            {
                var result = InvokeAction(this, args);
                return result;
            }
            else
            {
                return base.Invoke(args);
            }
        }

        /// <summary>
        /// 执行监听委托
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventHandler">监听事件数据</param>
        /// <param name="args">事件参数</param>
        /// <returns>执行结果</returns>
        internal static bool InvokeAction(EventHandler<T> eventHandler, params object[] args)
        {
            var action = eventHandler.Action;
            if (action == null) return false;
            try
            {
                action((T) eventHandler.Type, args);
            }
            catch (Exception exception)
            {
                UnityEngine.Debug.LogError(exception);
                return false;
            }

            return true;
        }
    }
}