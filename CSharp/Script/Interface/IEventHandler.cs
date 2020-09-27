/////////////////////////////////////////////////////////////////////////////
//
//  Script   : IEventHandler.cs
//  Info     : 事件处理器接口定义
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////

namespace Aya.Events
{
    public partial interface IEventHandler
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        object Type { get; set; }

        /// <summary>
        /// 事件目标对象
        /// </summary>
        object Target { get; set; }

        /// <summary>
        /// 事件分组
        /// </summary>
        object Group { get; set; }

        /// <summary>
        /// 优先级
        /// </summary>
        int Priority { get; set; }

        /// <summary>
        /// 是否中断事件队列
        /// </summary>
        bool Interrupt { get; set; }

        /// <summary>
        /// 执行事件
        /// </summary>
        /// <param name="args">参数</param>
        /// <returns>执行结果</returns>
        bool Invoke(params object[] args);
    }
}
