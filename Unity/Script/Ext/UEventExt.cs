/////////////////////////////////////////////////////////////////////////////
//
//  Script   : UEventExt.cs
//  Info     : 事件快速调用接口 - 非枚举类型事件扩展
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////

namespace Aya.Events
{
    public static partial class UEvent<T> where T : new()
    {
        public static void DispatchSafe(T eventData)
        {
            EventManager.GetDispatcher<T>().DispatchSafe<T>(eventData);
        }
    }
}
