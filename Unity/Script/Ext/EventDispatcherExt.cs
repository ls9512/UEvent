/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventDispatcherExt.cs
//  Info     : 事件分发器 - 非枚举类型事件扩展
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
        /// <param name="eventData">事件参数</param>
        public void DispatchSafe<T>(T eventData)
        {
            var handlerGroup = _getOrAddHandlerGroup<T>(EventDic);
            var handlers = handlerGroup.Handlers;
            for (var i = 0; i < handlers.Count; i++)
            {
                var eventHandler = handlers[i];
                lock (eventHandler)
                {
                    EventManager.ExecuteUpdate(() => { eventHandler.Invoke(eventData); });
                }

                if (eventHandler.Interrupt) break;
            }
        }

        #endregion
    }
}