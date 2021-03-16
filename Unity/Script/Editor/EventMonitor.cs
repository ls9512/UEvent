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

        public static GUIStyle DispatchRowStyle
        {
            get
            {
                if (_dispatchRowStyle == null)
                {
                    _dispatchRowStyle = new GUIStyle()
                    {
                        normal = new GUIStyleState()
                        {
                            background = MakeTex(2, 2, new Color(0f, 1f, 0f, 0.15f))
                        }
                    };
                }

                return _dispatchRowStyle;
            }
        }

        private static GUIStyle _dispatchRowStyle;

        #endregion

        #region Draw Table

        private string _searchEvent;
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
                DrawCellHandler
            };

            _tableHeaders = new[] {"Event", "Target", "Group", "Priority", "Interrupt", "Handler"};
            _tableCellWidthWeights = new[] {2f, 1.5f, 1f, 0.35f, 0.4f, 2.5f};
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
                            GUILayout.Label("Event", GUILayout.Width(EditorGUIUtility.labelWidth / 3f));
                            _searchEvent = EditorGUILayout.TextArea(_searchEvent, EditorStyles.toolbarSearchField,
                                GUILayout.Width(EditorGUIUtility.labelWidth));
                            GUILayout.Label("Event Type", GUILayout.Width(EditorGUIUtility.labelWidth / 2f));
                            _searchEventType = EditorGUILayout.TextArea(_searchEventType,
                                EditorStyles.toolbarSearchField, GUILayout.Width(EditorGUIUtility.labelWidth));
                        }
                    }
                }

                // Event Table
                using (new GUIScrollView(ref _tableScrollPos))
                {
                    var dispatcherDic = EventManager.DispatcherDic;
                    foreach (var dispatcherKv in dispatcherDic)
                    {
                        var dispatchType = dispatcherKv.Key;
                        var dispatcher = dispatcherKv.Value;

                        if (!string.IsNullOrEmpty(_searchEvent))
                        {
                            if (!dispatchType.Name.Contains(_searchEvent))
                            {
                                continue;
                            }
                        }

                        using (new GUIFoldOut(this, dispatchType.Name))
                        {
                            if (GUIFoldOut.GetState(this, dispatchType.Name))
                            {
                                var tableWidth = (Screen.width - 58f * EditorGUIUtility.pixelsPerPoint) /
                                                 EditorGUIUtility.pixelsPerPoint;

                                using (new GUITable<EventHandler>(
                                    _tableHeaders,
                                    // _tableCellDrawers,
                                    (rowIndex, columnWidths, eventHandler) =>
                                    {
                                        // var isDispatched = DispatchedList.Contains(eventHandler);
                                        // var rowStyle = isDispatched ? DispatchRowStyle : null;
                                        if (!string.IsNullOrEmpty(_searchEventType))
                                        {
                                            if (!eventHandler.Type.ToString().Contains(_searchEventType))
                                            {
                                                return;
                                            }
                                        }

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
                                    },
                                    ForeachRow(dispatcher),
                                    tableWidth,
                                    _tableCellWidthWeights)
                                )
                                {
                                    GUI.enabled = true;
                                }

                            }
                        }
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
            GUILayout.Label(eventHandler.Type.ToString());
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
                GUILayout.Label(eventHandler.Method.ToString());
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

        #endregion

        #region Callback

        // public List<EventHandler> DispatchedList = new List<EventHandler>();

        public void OnAdded(EventHandler handler)
        {

        }

        public void OnRemoved(EventHandler eventHandler)
        {

        }

        public void OnDispatched(EventHandler eventHandler, object[] args)
        {
            // DispatchedList.Add(eventHandler);
        }

        public void OnError(EventHandler eventHandler, Exception exception)
        {

        }

        #endregion

        #region GUI Table

        public struct GUITable<T> : IDisposable
        {
            public static GUIStyle HeaderStyle = new GUIStyle()
            {
                // alignment = TextAnchor.MiddleCenter,
                normal = new GUIStyleState()
                {
                    textColor = Color.white,
                    background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.2f))
                }
            };

            public GUITable(string[] headers, Action<int, float, T>[] cellDrawers, IEnumerable<T> dataList, float tableWidth, float[] columnWidthWeights)
            {
                GUILayout.BeginVertical();
                var columnWidths = CalcColumnWidths(tableWidth, columnWidthWeights);

                // Header
                DrawHeader(headers, columnWidths);

                // Rows
                var index = 0;
                foreach (var data in dataList)
                {
                    var row = index;
                    using (new GUITableRow(row))
                    {
                        for (var i = 0; i < cellDrawers.Length; i++)
                        {
                            var cellWidth = columnWidths[i];
                            var drawer = cellDrawers[i];
                            using (new GUITableCell(row, i, cellWidth))
                            {
                                drawer(row, cellWidth, data);
                            }

                            if (i < cellDrawers.Length)
                            {
                                // GUILayout.Space(1);
                            }
                        }
                    }

                    index++;
                }
            }

            public GUITable(string[] headers, Action<int, float[], T> rowDrawer, IEnumerable<T> dataList, float tableWidth, float[] columnWidthWeights)
            {
                GUILayout.BeginVertical();
                var columnWidths = CalcColumnWidths(tableWidth, columnWidthWeights);

                // Header
                DrawHeader(headers, columnWidths);

                // Rows
                var index = 0;
                foreach (var data in dataList)
                {
                    var row = index;
                    using (new GUITableRow(row))
                    {
                        rowDrawer(row, columnWidths, data);
                    }

                    index++;
                }
            }

            public void DrawHeader(string[] headers, float[] columnWidths)
            {
                GUILayout.BeginHorizontal(HeaderStyle);
                for (var i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    GUILayout.Label(header, GUILayout.Width(columnWidths[i]));
                }

                GUILayout.EndHorizontal();
            }

            public float[] CalcColumnWidths(float tableWidth, float[] columnWidthWeights)
            {
                var columnWidths = new float[columnWidthWeights.Length];
                var weightCount = 0f;
                foreach (var weight in columnWidthWeights)
                {
                    weightCount += weight;
                }

                for (var i = 0; i < columnWidthWeights.Length; i++)
                {
                    var weight = columnWidthWeights[i];
                    var columnWidth = weight / weightCount * tableWidth;
                    columnWidths[i] = columnWidth;
                }

                return columnWidths;
            }

            public void Dispose()
            {
                GUILayout.EndVertical();
            }
        }

        public struct GUITableCell : IDisposable
        {
            public GUITableCell(int rowIndex, int columnIndex, float cellWidth)
            {
                GUILayout.BeginHorizontal(GUILayout.Width(cellWidth));
            }

            public void Dispose()
            {
                GUILayout.EndHorizontal();
            }
        }

        public struct GUITableRow : IDisposable
        {
            public static GUIStyle OddRowStyle = new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.035f))
                }
            };

            public static GUIStyle EvenRowStyle = new GUIStyle()
            {
                normal = new GUIStyleState()
                {
                    background = MakeTex(2, 2, new Color(1f, 1f, 1f, 0.05f))
                }
            };

            public GUITableRow(int rowIndex, GUIStyle style = null, params GUILayoutOption[] options)
            {
                if (style == null)
                {
                    if (rowIndex % 2 == 0)
                    {
                        GUILayout.BeginHorizontal(EvenRowStyle);
                    }
                    else
                    {
                        GUILayout.BeginHorizontal(OddRowStyle);
                    }
                }
                else
                {
                    GUILayout.BeginHorizontal(style, options);
                }
            }

            public GUITableRow(int rowIndex, params GUILayoutOption[] options)
            {
                if (rowIndex % 2 == 0)
                {
                    GUILayout.BeginHorizontal(EvenRowStyle, options);
                }
                else
                {
                    GUILayout.BeginHorizontal(OddRowStyle, options);
                }
            }

            public void Dispose()
            {
                GUILayout.EndHorizontal();
            }
        }

        public static Texture2D MakeTex(int width, int height, Color col)
        {
            var pix = new Color[width * height];

            for (var i = 0; i < pix.Length; i++)
            {
                pix[i] = col;
            }

            var result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
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