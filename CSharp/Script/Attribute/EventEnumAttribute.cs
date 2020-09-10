/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventEnumAttribute.cs
//  Info     : 事件枚举特性 （预留，暂未使用）
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2019
//
/////////////////////////////////////////////////////////////////////////////
using System;

namespace Aya.Events
{
    [AttributeUsage(AttributeTargets.Enum, AllowMultiple = false)]
    public class EventEnumAttribute : Attribute
    {
    }
}
