/////////////////////////////////////////////////////////////////////////////
//
//  Script   : UnityEventExtension.cs
//  Info     : 事件系统相关扩展方法
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
/// 
namespace Aya.Events
{
    public static class UnityEventExtension
    {
        #region Dispatch

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public static void DispatchSafe<T>(this object obj, T eventType, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchSafe(eventType, args);
        }

        #endregion
    }
}
