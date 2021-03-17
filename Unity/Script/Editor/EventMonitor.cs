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
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Aya.Events
{
    public class EventMonitor : EditorWindow
    {
        #region Menu
      
        public static EventMonitor Instance;

        [MenuItem("Aya Game Studio/Plugins/Event Monitor", false, 0)]
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
            DrawTable();
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

        public static GUIStyle RichTextStyle
        {
            get
            {
                if (_richTextStyle == null)
                {
                    _richTextStyle = new GUIStyle()
                    {
                        normal = new GUIStyleState()
                        {
                            textColor = Color.white
                        },
                        alignment = TextAnchor.MiddleLeft,
                        richText = true
                    };
                }

                return _richTextStyle;
            }
        }

        private static GUIStyle _richTextStyle;

        #endregion

        #region Draw Table

        private string _searchEventType;

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

            _tableHeaders = new[] {"Event", "Target", "Group", "Priority", "Interrupt", "Handler", "Dispatch", "Last Time"};
            _tableCellWidthWeights = new[] {1.5f, 1.5f, 1f, 0.35f, 0.4f, 2.5f, 0.5f, 1f};
        }

        public void DrawTable()
        {
            if (!Application.isPlaying)
            {
                GUILayout.Label("Player is not running", EditorStyles.largeLabel);
            }
            else
            {
                using (new GUIFoldOut(this, "Filter"))
                {
                    if (GUIFoldOut.GetState(this, "Filter"))
                    {
                        using (new GUIHorizontal(null))
                        {
                            GUILayout.Label("Event Type", GUILayout.Width(EditorGUIUtility.labelWidth / 2f));
                            _searchEventType = EditorGUILayout.TextArea(_searchEventType,
                                EditorStyles.toolbarSearchField, GUILayout.Width(EditorGUIUtility.labelWidth));
                        }
                    }
                }

                // Event Table
                using (new GUIFoldOut(this, "Event"))
                {
                    if (GUIFoldOut.GetState(this, "Event"))
                    {
                        using (new GUIScrollView(ref _tableScrollPos))
                        {
                            var tableWidth = (Screen.width - 58f * EditorGUIUtility.pixelsPerPoint) / EditorGUIUtility.pixelsPerPoint;
                            using (new GUITable<EventHandler>(
                                _tableHeaders,
                                (rowIndex, columnWidths, eventHandler) =>
                                {
                                    if (!string.IsNullOrEmpty(_searchEventType) && !eventHandler.Type.ToString().Contains(_searchEventType))
                                    {
                                        return;
                                    }

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

                // Log
                using (new GUIFoldOut(this, "Log"))
                {
                    if (GUIFoldOut.GetState(this, "Log"))
                    {

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
            if (eventHandler.Type is Type type)
            {
                GUILayout.Label(type.Name);
            }
            else
            {
                GUILayout.Label(eventHandler.Type.ToString());
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
                GUILayout.Label(eventHandler.MethodSignature, RichTextStyle);
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
                GUILayout.Label(eventHandler.LastInvokeDateTime.ToString("yyyyMMdd HH:mm:ss"));
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

        #region Callback

        // public List<EventHandler> DispatchedList = new List<EventHandler>();

        public void OnAdded(EventHandler handler)
        {

        }

        public void OnRemoved(EventHandler eventHandler)
        {

        }

        public void OnDispatched(EventHandler eventHandler, object[] args, bool success)
        {
            if (success)
            {
                eventHandler.LastInvokeSuccessTime = Time.realtimeSinceStartup;
            }
            else
            {
                eventHandler.LastInvokeFailTime = Time.realtimeSinceStartup;
            }
        }

        public void OnError(EventHandler eventHandler, Exception exception)
        {

        }

        #endregion

        #region GUI Helper

        public struct GUIFoldOut : IDisposable
        {
            public static Dictionary<Object, Dictionary<string, bool>> StateCacheDic = new Dictionary<Object, Dictionary<string, bool>>();

            public GUIFoldOut(Object target, string title, bool defaultState = true, GUILayoutOption[] options = null)
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox, options);
                var rect = EditorGUILayout.GetControlRect();

                var state = GetState(target, title, defaultState);
                var currentState = GUI.Toggle(rect, state, GUIContent.none, EditorStyles.foldout);
                if (currentState != state)
                {
                    SetState(target, title, currentState);
                }

                rect.xMin += rect.height;
                EditorGUI.LabelField(rect, title, EditorStyles.boldLabel);
            }

            public void Dispose()
            {
                EditorGUILayout.EndVertical();
            }

            public static bool GetState(Object target, string title, bool defaultState = true)
            {
                var stateDic = GetStateDic(target);
                if (!stateDic.TryGetValue(title, out var result))
                {
                    stateDic.Add(title, defaultState);
                }

                return result;
            }

            public static void SetState(Object target, string title, bool value)
            {
                var stateDic = GetStateDic(target);
                if (!stateDic.TryGetValue(title, out var result))
                {
                    stateDic.Add(title, value);
                }
                else
                {
                    stateDic[title] = value;
                }
            }

            public static Dictionary<string, bool> GetStateDic(Object target)
            {
                if (!StateCacheDic.TryGetValue(target, out var stateDic))
                {
                    stateDic = new Dictionary<string, bool>();
                    StateCacheDic.Add(target, stateDic);
                }

                return stateDic;
            }
        }

        public struct GUITabArea : IDisposable
        {
            public GUITabArea(float tabSize, GUILayoutOption[] options = null)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Space(tabSize);
                GUILayout.BeginVertical(options);
            }

            public void Dispose()
            {
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
            }
        }

        public struct GUIColorArea : IDisposable
        {
            public Color OriginalColor;

            public GUIColorArea(Color color)
            {
                OriginalColor = GUI.color;
                GUI.color = color;
            }

            public void Dispose()
            {
                GUI.color = OriginalColor;
            }
        }

        public struct GUIFullColorArea : IDisposable
        {
            public Color OriginalBackColor;
            public Color OriginalContentColor;

            public GUIFullColorArea(Color color)
            {
                OriginalBackColor = GUI.backgroundColor;
                GUI.backgroundColor = color;
                OriginalContentColor = GUI.contentColor;
                GUI.contentColor = color;
            }

            public void Dispose()
            {
                GUI.backgroundColor = OriginalBackColor;
                GUI.contentColor = OriginalContentColor;
            }
        }

        public struct GUIVertical : IDisposable
        {
            public GUIVertical(GUILayoutOption[] options = null)
            {
                GUILayout.BeginVertical(options);
            }

            public GUIVertical(GUIStyle style, GUILayoutOption[] options = null)
            {
                GUILayout.BeginVertical(style, options);
            }

            public void Dispose()
            {
                GUILayout.EndVertical();
            }
        }

        public struct GUIHorizontal : IDisposable
        {
            public GUIHorizontal(GUILayoutOption[] options = null)
            {
                GUILayout.BeginHorizontal(options);
            }

            public GUIHorizontal(GUIStyle style, GUILayoutOption[] options = null)
            {
                GUILayout.BeginHorizontal(style, options);
            }

            public void Dispose()
            {
                GUILayout.EndHorizontal();
            }
        }

        public struct GUIScrollView : IDisposable
        {
            public GUIScrollView(ref Vector2 pos, GUILayoutOption[] options = null)
            {
                pos = GUILayout.BeginScrollView(pos, options);
            }

            public GUIScrollView(ref Vector2 pos, GUIStyle style, GUILayoutOption[] options = null)
            {
                GUILayout.BeginScrollView(pos, style, options);
            }

            public void Dispose()
            {
                GUILayout.EndScrollView();
            }
        }

        #endregion
    }
}
#endif