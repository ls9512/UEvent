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
        /// <summary>
        /// 监听委托
        /// </summary>
        public Action<T> ActionT1 { get; set; }

        /// <summary>
        /// 监听委托
        /// </summary>
        public Action<T, object[]> ActionT2 { get; set; }
    }
}