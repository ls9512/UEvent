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
            var handlerGroup = _getOrAddHandlerGroup(EventDic, eventType);
            var eventList = handlerGroup.Handlers;
            for (var i = 0; i < eventList.Count; i++)
            {
                var eventData = eventList[i];
                lock (eventData)
                {
                    EventManager.ExecuteUpdate(() => { eventData.Invoke(args); });
                }

                if (eventData.Interrupt) break;
            }
        }

        #endregion
    }
}