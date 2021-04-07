/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventManager.cs
//  Info     : 事件管理器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Tip      : 用于创建、获取事件分发器和发送事件
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.Events
{
    public partial class EventManager : MonoBehaviour
    {
        #region Instance

        protected static EventManager Instance;

        internal static EventManager Ins
        {
            get
            {
                if (Instance != null) return Instance;
                Instance = (EventManager)FindObjectOfType(typeof(EventManager));
                if (Instance != null) return Instance;
                var obj = new GameObject
                {
                    hideFlags = HideFlags.HideAndDontSave,
                    name = "EventManager"
                };
                DontDestroyOnLoad(obj);
                Instance = obj.AddComponent<EventManager>();
                return Instance;
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        protected static void Init()
        {
            var ins = Ins;
            UEventCallback.OnError += (eventHandler, eventType, args, exception) =>
            {
                Debug.LogError(exception.ToString());
#if UNITY_EDITOR
                EventHandler.CacheLog(eventHandler, eventType, args, false, exception);
#endif
            };
#if UNITY_EDITOR
            UEventCallback.OnDispatched += (eventHandler, eventType, args) =>
            {
                EventHandler.CacheLog(eventHandler, eventType, args, true, null);
            };
#endif
        }

        #endregion

        #region Execute Unity Thread Update

        internal static bool NoUpdate = true;
        internal static List<Action> UpdateQueue = new List<Action>();
        internal static List<Action> UpdateRunQueue = new List<Action>();

        /// <summary>
        /// 附加到 Update 执行，用于保证线程安全情况下分发事件，但会延迟一帧
        /// </summary>
        /// <param name="action">action</param>
        internal static void ExecuteUpdate(Action action)
        {
            lock (UpdateQueue)
            {
                UpdateQueue.Add(action);
                NoUpdate = false;
            }
        }

        #endregion

        #region MonoBehaviour

        private void Update()
        {
            lock (UpdateQueue)
            {
                if (NoUpdate) return;
                UpdateRunQueue.AddRange(UpdateQueue);
                UpdateQueue.Clear();
                NoUpdate = true;
                for (var i = 0; i < UpdateRunQueue.Count; i++)
                {
                    var action = UpdateRunQueue[i];
                    action?.Invoke();
                }

                UpdateRunQueue.Clear();
            }
        }

        #endregion

        #region Dispatch Safe

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <typeparam name="T">事件类型</typeparam>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public static void DispatchSafe<T>(T eventType, params object[] args)
        {
            GetDispatcher<T>().DispatchSafe(eventType, args);
        }

        /// <summary>
        /// 发送事件
        /// </summary>
        /// <param name="eventType">事件类型</param>
        /// <param name="args">事件参数</param>
        public static void DispatchSafe(object eventType, params object[] args)
        {
            var eventEnumType = eventType.GetType();
            GetDispatcher(eventEnumType).DispatchSafe(eventType, args);
        }

        #endregion
    }
}