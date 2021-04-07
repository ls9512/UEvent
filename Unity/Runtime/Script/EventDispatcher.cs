/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventDispatcher.cs
//  Info     : 事件分发器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////

namespace Aya.Events
{
    public partial class EventDispatcher
    {
        #region Disptach Safe

        /// <summary>
        /// 事件分发(线程安全)
        /// </summary>
        /// <typeparam name="T">事件枚举类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public void DispatchSafe<T>(T eventType, params object[] args)
        {
            var eventHandlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventHandlers = eventHandlerGroup.Handlers;
            for (var i = 0; i < eventHandlers.Count; i++)
            {
                var eventHandler = eventHandlers[i];
                lock (eventHandler)
                {
                    EventManager.ExecuteUpdate(() => { eventHandler.Invoke(eventType, args); });
                }

                if (eventHandler.Interrupt) break;
            }
        }

        #endregion
    }
}