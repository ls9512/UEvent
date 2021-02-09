/////////////////////////////////////////////////////////////////////////////
//
//  Script   : ObjectListener.cs
//  Info     : 常规类型事件处理器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////

namespace Aya.Events
{
    public abstract partial class ObjectListener
    {
        #region Dispatch Safe

        /// <summary>
        /// 发送事件(线程安全)
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public void DispatchSafe<T>(T eventType, params object[] args)
        {
            EventManager.GetDispatcher<T>().DispatchSafe(eventType, args);
        }

        #endregion
    }
}