/////////////////////////////////////////////////////////////////////////////
//
//  Script   : UEvent.cs
//  Info     : 事件快速调用接口
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////

namespace Aya.Events
{
    public static partial class UEvent
    {
        public static void DispatchSafe<T>(T eventType, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchSafe(eventType, args);
        }
    }
}
