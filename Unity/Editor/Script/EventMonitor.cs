/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventMonitor.cs
//  Info     : 事件监视器
//  Author   : ls9512 2021
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Aya.Events
{
    public class EventMonitor : EditorWindow
    {
        #region Menu
      
        public static EventMonitor Instance;

        [MenuItem("Tools/Aya Game/UEvent/Event Monitor", false, 0)]
        public static void ShowWindow()
        {
            if (Instance == null)
            {
                Instance = CreateInstance<EventMonitor>();
                Instance.titleContent.text = "Event Monitor";
                // Instance.minSize = new Vector2(1280, 800);
                // Instance.maxSize = new Vector2(Screen.width, Screen.height);
                Instance.maximized = false;
            }

            Instance.Show();
        }

        #endregion

        #region Monobehaviour

        public void OnEnable()
        {
            UEventCallback.OnAdded += OnAdded;
            UEventCallback.OnRemoved += OnRemoved;
            UEventCallback.OnDispatched += OnDispatched;
            UEventCallback.OnError += OnError;

            CacheTableParam();
        }

        public void OnDisable()
        {
            UEventCallback.OnAdded -= OnAdded;
            UEventCallback.OnRemoved -= OnRemoved;
            UEventCallback.OnDispatched -= OnDispatched;
            UEventCallback.OnError -= OnError;
        }

        public void OnGUI()
        {
            if (!Application.isPlaying)
            {
                GUILayout.Label("Player is not running", EditorStyles.largeLabel);
            }
            else
            {
                // GUIResizeArea.Vertical(
                //     "EventMonitor",
                //     position.width,
                //     position.height,
                //     DrawFilter,
                //     DrawEventTable,
                //     DrawLog);

                DrawFilter();
                DrawEventTable();
                DrawLog();
            }
        }

        public void Update()
        {
            if (EditorApplication.isPlaying && !EditorApplication.isPaused)
            {
                Repaint();
            }
        }

        #endregion

        #region Style

        #endregion

        #region Draw Filter

        private string _searchEventType;

        public void DrawFilter()
        {
            using (GUIFoldOut.Create(this, "Filter"))
            {
                if (!GUIFoldOut.GetState(this, "Filter")) return;
                using (GUIHorizontal.Create())
                {
                    GUILayout.Label("Event Type", GUILayout.Width(EditorGUIUtility.labelWidth / 2f));
                    _searchEventType = EditorGUILayout.TextArea(_searchEventType,
                        EditorStyles.toolbarSearchField, GUILayout.Width(EditorGUIUtility.labelWidth));
                }
            }
        } 

        #endregion

        #region Draw Event Table

        private Action<int, float, EventHandler>[] _tableCellDrawers;
        private string[] _tableHeaders;
        private float[] _tableCellWidthWeights;
        private Vector2 _tableScrollPos;

        protected void CacheTableParam()
        {
            _tableCellDrawers = new Action<int, float, EventHandler>[]
            {
                DrawCellEventType,
                DrawCellTarget,
                DrawCellGroup,
                DrawCellPriority,
                DrawCelInterrupt,
                DrawCellHandler,
                DrawCellCounter,
                DrawCellLastTime,
            };
            _tableHeaders = new[] { "Event", "Target", "Group", "Priority", "Interrupt", "Handler", "Dispatch", "Last Time" };
            _tableCellWidthWeights = new[] { 1.5f, 1.5f, 1f, 0.35f, 0.4f, 2.5f, 0.5f, 1f };
        }

        public void DrawEventTable()
        {
            using (GUIFoldOut.Create(this, "Event"))
            {
                if (!GUIFoldOut.GetState(this, "Event")) return;
                using (GUIScrollView.Create(ref _tableScrollPos, GUILayout.Height(Screen.height * 0.6f / EditorGUIUtility.pixelsPerPoint)))
                {
                    var tableWidth = (Screen.width - 58f * EditorGUIUtility.pixelsPerPoint) / EditorGUIUtility.pixelsPerPoint;
                    using (new GUITable<EventHandler>(
                        _tableHeaders,
                        (rowIndex, columnWidths, eventHandler) =>
                        {
                            if (!string.IsNullOrEmpty(_searchEventType) && !eventHandler.EventType.ToString().Contains(_searchEventType)) return;
                            using (new GUIFullColorArea(GetRowColor(eventHandler)))
                            {
                                using (new GUITableRow(rowIndex))
                                {
                                    for (var i = 0; i < _tableCellDrawers.Length; i++)
                                    {
                                        var cellWidth = columnWidths[i];
                                        using (new GUITableCell(rowIndex, i, cellWidth))
                                        {
                                            _tableCellDrawers[i](rowIndex, cellWidth, eventHandler);
                                        }
                                    }
                                }
                            }
                        },
                        ForeachRow(),
                        tableWidth,
                        _tableCellWidthWeights)
                    )
                    {
                        GUI.enabled = true;
                    }
                }
            }
        }

        private Color GetRowColor(EventHandler eventHandler)
        {
            var rowColor = GUI.backgroundColor;
            if (eventHandler.IsInvokeSuccess)
            {
                rowColor = Color.Lerp(EventEditorSetting.Ins.MonitorStyle.TipSuccessColor, GUI.backgroundColor, eventHandler.InvokeSuccessProgress);
            }

            if (eventHandler.IsInvokeFail)
            {
                rowColor = Color.Lerp(EventEditorSetting.Ins.MonitorStyle.TipFailColor, GUI.backgroundColor, eventHandler.InvokeFailProgress);
            }

            if (eventHandler.IsListening)
            {
                rowColor = Color.Lerp(EventEditorSetting.Ins.MonitorStyle.TipListenColor, GUI.backgroundColor, eventHandler.ListeningProgress);
            }

            return rowColor;
        }

        private IEnumerable<EventHandler> ForeachRow()
        {
            foreach (var dispatcher in EventManager.DispatcherDic.Values)
            {
                foreach (var eventKv in dispatcher.EventDic)
                {
                    var handlerGroup = eventKv.Value;
                    foreach (var eventHandler in handlerGroup.Handlers)
                    {
                        yield return eventHandler;
                    }
                }
            }
        }

        private IEnumerable<EventHandler> ForeachRow(EventDispatcher dispatcher)
        {
            foreach (var eventKv in dispatcher.EventDic)
            {
                var handlerGroup = eventKv.Value;
                foreach (var eventHandler in handlerGroup.Handlers)
                {
                    yield return eventHandler;
                }
            }
        }

        #endregion

        #region Draw Table Cell

        public void DrawCellEventType(int index, float width, EventHandler eventHandler)
        {
            if (eventHandler.DataType == EventDataType.Enum)
            {
                GUILayout.Label(eventHandler.EventType.GetType().Name + "." + eventHandler.EventType);
            }
            else
            {
                if (eventHandler.EventType is Type type)
                {
                    GUILayout.Label("[" + eventHandler.DataType + "] " + type.Name);
                }
                else
                {
                    GUILayout.Label("[" + eventHandler.DataType + "] " + eventHandler.EventType);
                }
            }
        }

        public void DrawCellTarget(int index, float width, EventHandler eventHandler)
        {
            var target = eventHandler.Target;
            if (target != null)
            {
                if (target is MonoBehaviour mono)
                {
                    EditorGUILayout.ObjectField(mono, typeof(GameObject), true);
                }
                else
                {

                    GUILayout.Label(target.ToString());
                }
            }
            else
            {
                using (new GUIColorArea(Color.gray))
                {
                    GUILayout.Label("NULL");
                }
            }
        }

        public void DrawCellGroup(int index, float width, EventHandler eventHandler)
        {
            if (eventHandler.Group == null)
            {
                using (new GUIColorArea(Color.gray))
                {
                    GUILayout.Label("Default Group");
                }
            }
            else
            {
                GUILayout.Label(eventHandler.Group.ToString());
            }
        }

        public void DrawCellPriority(int index, float width, EventHandler eventHandler)
        {
            GUILayout.Label(eventHandler.Priority.ToString());
        }

        public void DrawCelInterrupt(int index, float width, EventHandler eventHandler)
        {
            if (eventHandler.Interrupt)
            {
                GUILayout.Label(eventHandler.Interrupt.ToString());
            }
            else
            {
                using (new GUIColorArea(Color.gray))
                {
                    GUILayout.Label(eventHandler.Interrupt.ToString());
                }
            }
        }

        public void DrawCellHandler(int index, float width, EventHandler eventHandler)
        {
            if (eventHandler.Method != null)
            {
                GUIHelper.Url(eventHandler.MethodSignatureRichText, eventHandler.MethodSignature, () =>
                {
                    // EditorUtility.OpenWithDefaultApp();
                });
            }
            else
            {
                using (new GUIColorArea(Color.gray))
                {
                    GUILayout.Label("NULL");
                }
            }

            if (eventHandler.Interrupt)
            {
                GUI.enabled = false;
            }
        }

        public void DrawCellCounter(int index, float width, EventHandler eventHandler)
        {
            if (eventHandler.DispatchCounter > 0)
            {
                GUILayout.Label(eventHandler.DispatchCounter.ToString());
            }
            else
            {
                using (new GUIColorArea(Color.gray))
                {
                    GUILayout.Label("0");
                }
            }
        }

        public void DrawCellLastTime(int index, float width, EventHandler eventHandler)
        {
            if (eventHandler.DispatchCounter > 0)
            {
                GUILayout.Label(eventHandler.LastInvokeDateTime.ToString(EventEditorSetting.Ins.MonitorStyle.DateFormat));
            }
            else
            {
                using (new GUIColorArea(Color.gray))
                {
                    GUILayout.Label("None");
                }
            }
        }

        #endregion

        #region Draw Log

        private Vector2 _logScrollPos;

        public void DrawLog()
        {
            using (GUIFoldOut.Create(this, "Log"))
            {
                if (!GUIFoldOut.GetState(this, "Log")) return;
                using (GUIScrollView.Create(ref _logScrollPos))
                {
                    for (var i = EventHandler.Logs.Count - 1; i >= 0; i--)
                    {
                        var log = EventHandler.Logs[i];
                        if (!string.IsNullOrEmpty(_searchEventType) && !log.EventType.Contains(_searchEventType)) return;
                        if (log.Success)
                        {
                            EditorGUILayout.TextArea(log.ToString());
                        }
                        else
                        {
                            using (GUIContentColorArea.Create(Color.red))
                            {
                                EditorGUILayout.TextArea(log.ToString());
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Callback

        // public List<EventHandler> DispatchedList = new List<EventHandler>();

        public void OnAdded(EventHandler handler)
        {

        }

        public void OnRemoved(EventHandler eventHandler)
        {

        }

        public void OnDispatched(EventHandler eventHandler, object eventType, object[] args)
        {
            eventHandler.LastInvokeSuccessTime = Time.realtimeSinceStartup;
        }

        public void OnError(EventHandler eventHandler, object eventType, object[] args, Exception exception)
        {
            eventHandler.LastInvokeFailTime = Time.realtimeSinceStartup;
        }

        #endregion
    }
}
#endif