/////////////////////////////////////////////////////////////////////////////
//
//  Script   : IEventListener.cs
//  Info     : 事件处理器接口
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2020
//
/////////////////////////////////////////////////////////////////////////////

namespace Aya.Events
{
    public partial interface IEventListener
    {
        void DispatchSafe<T>(T eventType, params object[] args);
    }
}