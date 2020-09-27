/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventInterface.cs
//  Info     : 事件外部接口
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Events
{
    public static class EventInterface
    {
        /// <summary>
        /// 异常处理回调
        /// </summary>
        public static Action<Exception> OnError { get; set; } = exception => Console.WriteLine(exception.ToString());
    }
}
