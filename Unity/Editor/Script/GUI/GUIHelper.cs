/////////////////////////////////////////////////////////////////////////////
//
//  Script   : GUIHelper.cs
//  Info     : GUI辅助类
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
    internal static class GUIHelper
    {
        public static GUIStyle LinkNormalStyle
        {
            get
            {
                if (_linkNormalStyle == null)
                {
                    _linkNormalStyle = new GUIStyle()
                    {
                        normal = new GUIStyleState()
                        {
                            textColor = Color.white
                        },
                        alignment = TextAnchor.MiddleLeft,
                        richText = true
                    };
                }

                return _linkNormalStyle;
            }
        }

        private static GUIStyle _linkNormalStyle;

        public static GUIStyle LinkActiveStyle
        {
            get
            {
                if (_linkActiveStyle == null)
                {
                    _linkActiveStyle = new GUIStyle()
                    {
                        active = new GUIStyleState()
                        {
                            textColor = Color.blue
                        },
                        normal = new GUIStyleState()
                        {
                            textColor = EventEditorSetting.Ins.MonitorStyle.ActiveUrlColor
                        },
                        alignment = TextAnchor.MiddleLeft,
                        richText = false
                    };
                }

                return _linkActiveStyle;
            }
        }

        private static GUIStyle _linkActiveStyle;

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

        public static bool Url(string normalUrl, string activeUrl, Action action = null)
        {
            var rect = EditorGUILayout.GetControlRect();
            if (rect.Contains(Event.current.mousePosition))
            {
                EditorGUIUtility.AddCursorRect(rect, MouseCursor.Link);
                GUI.Label(rect, activeUrl, LinkActiveStyle);
                if (Event.current.type == EventType.MouseUp)
                {
                    action?.Invoke();
                    return true;
                }

                return false;
            }
            else
            {
                GUI.Label(rect, normalUrl, LinkNormalStyle);
                return false;
            }
        }
    }

    internal class GUIResizeData
    {
        public float Width;
        public float Height;
        public float[] ViewWidths;
        public Rect[] CursorChangeRects;
        public bool[] Resizes;
        public float[] WidthWeights;

        public GUIResizeData(float width, float height, int count)
        {
            RefreshSize(width, height, count);
        }

        public void RefreshSize(float width, float height, int count)
        {
            Width = width;
            Height = height;

            WidthWeights = new float[count];
            for (var i = 0; i < count; i++)
            {
                WidthWeights[i] = 1f / count;
            }

            ViewWidths = new float[count];
            for (var i = 0; i < count; i++)
            {
                ViewWidths[i] = Height * WidthWeights[i];
            }

            CursorChangeRects = new Rect[count];
            var rectHeight = 0f;
            for (var i = 0; i < count; i++)
            {
                rectHeight += ViewWidths[i];
                CursorChangeRects[i] = new Rect(0f, rectHeight, Width, 2f);
            }

            Resizes = new bool[count];
        }
    }

    internal struct GUIResizeArea : IDisposable
    {
        public static Dictionary<string, GUIResizeData> ResizeDataDic = new Dictionary<string, GUIResizeData>();

        public static GUIResizeArea Vertical(string key, float width, float height, params Action[] guiActions)
        {
            return new GUIResizeArea(key, width, height, true, guiActions);
        }

        public static GUIResizeArea Horizontal(string key, float width, float height, params Action[] guiActions)
        {
            return new GUIResizeArea(key, width, height, false, guiActions);
        }

        public GUIResizeData ResizeData;
        public bool IsVertical;

        public static Texture2D ResizeBarTex
        {
            get
            {
                if (_resizeBarTex == null)
                {
                    _resizeBarTex = GUIHelper.MakeTex(2, 2, new Color(1f, 1f, 1f, 0.2f));
                }

                return _resizeBarTex;
            }
        }

        private static Texture2D _resizeBarTex;

        public GUIResizeArea(string key, float width, float height, bool vertical, params Action[] guiActions)
        {
            IsVertical = vertical;
            var count = guiActions.Length;
            if (!ResizeDataDic.TryGetValue(key, out var resizeData))
            {
                resizeData = new GUIResizeData(width, height, count);
                ResizeDataDic.Add(key, resizeData);
            }

            ResizeData = resizeData;
            if (Math.Abs(resizeData.Width - width) > 1e-6 || Math.Abs(resizeData.Height - height) > 1e-6)
            {
                resizeData.RefreshSize(width, height, count);
            }

            if (vertical)
            {
                GUILayout.BeginVertical();
            }
            else
            {
                GUILayout.BeginHorizontal();
            }

            for (var i = 0; i < guiActions.Length; i++)
            {
                var action = guiActions[i];
                if (vertical)
                {
                    GUILayout.BeginVertical(GUILayout.Height(ResizeData.ViewWidths[i]));
                }
                else
                {
                    GUILayout.BeginHorizontal(GUILayout.Width(ResizeData.ViewWidths[i]));
                }
                action();
                if (vertical)
                {
                    GUILayout.EndVertical();
                }
                else
                {
                    GUILayout.EndHorizontal();
                }

                if (i < guiActions.Length - 1)
                {
                    ResizeScrollView(i);
                }
            }

            if (vertical)
            {
                GUILayout.EndVertical();
            }
            else
            {
                GUILayout.EndHorizontal();
            }
        }

        public void ResizeScrollView(int index)
        {
            GUI.DrawTexture(ResizeData.CursorChangeRects[index], ResizeBarTex);
            EditorGUIUtility.AddCursorRect(ResizeData.CursorChangeRects[index], MouseCursor.ResizeVertical);

            if (Event.current.type == EventType.MouseDown && ResizeData.CursorChangeRects[index].Contains(Event.current.mousePosition))
            {
                ResizeData.Resizes[index] = true;
            }

            if (ResizeData.Resizes[index])
            {
                var mousePos = IsVertical ? Event.current.mousePosition.y : Event.current.mousePosition.x;
                ChangeSize(index, mousePos);
            }

            if (Event.current.type == EventType.MouseUp)
            {
                ResizeData.Resizes[index] = false;
            }
        }

        public void ChangeSize(int index, float mousePos)
        {
            var rectWidth = 0f;
            for (var i = 0; i < index; i++)
            {
                rectWidth += ResizeData.ViewWidths[i];
            }

            var viewWidth = mousePos - rectWidth;
            ResizeData.ViewWidths[index] = viewWidth;
            ResizeData.WidthWeights[index] = viewWidth / ResizeData.Height;

            ResizeData.CursorChangeRects[index].Set(
                ResizeData.CursorChangeRects[index].x,
                rectWidth + viewWidth,
                ResizeData.CursorChangeRects[index].width,
                ResizeData.CursorChangeRects[index].height);

            rectWidth = ResizeData.Height;
            for (var i = ResizeData.ViewWidths.Length - 1; i > index + 1; i--)
            {
                rectWidth -= ResizeData.ViewWidths[i];
            }

            viewWidth = rectWidth - mousePos;
            ResizeData.ViewWidths[index + 1] = viewWidth;
            ResizeData.WidthWeights[index + 1] = viewWidth / ResizeData.Height;
        }

        public void Dispose()
        {
        }
    }

    internal struct GUIFoldOut : IDisposable
    {
        public static Dictionary<Object, Dictionary<string, bool>> StateCacheDic = new Dictionary<Object, Dictionary<string, bool>>();

        public static GUIFoldOut Create(Object target, string title, bool defaultState = true, params GUILayoutOption[] options)
        {
            return new GUIFoldOut(target, title, defaultState, options);
        }

        public static GUIFoldOut Create(Object target, string title, params GUILayoutOption[] options)
        {
            return new GUIFoldOut(target, title, true, options);
        }

        public GUIFoldOut(Object target, string title, bool defaultState = true, params GUILayoutOption[] options)
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

    internal struct GUITabArea : IDisposable
    {
        public static GUITabArea Create(float tabSize, params GUILayoutOption[] options)
        {
            return new GUITabArea(tabSize, options);
        }

        public GUITabArea(float tabSize, params GUILayoutOption[] options)
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

    internal struct GUIColorArea : IDisposable
    {
        public static GUIColorArea Create(Color color)
        {
            return new GUIColorArea(color);
        }

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

    internal struct GUIBackgroundColorArea : IDisposable
    {
        public static GUIBackgroundColorArea Create(Color color)
        {
            return new GUIBackgroundColorArea(color);
        }

        public Color OriginalColor;

        public GUIBackgroundColorArea(Color color)
        {
            OriginalColor = GUI.backgroundColor;
            GUI.backgroundColor = color;
        }

        public void Dispose()
        {
            GUI.backgroundColor = OriginalColor;
        }
    }

    internal struct GUIContentColorArea : IDisposable
    {
        public static GUIContentColorArea Create(Color color)
        {
            return new GUIContentColorArea(color);
        }

        public Color OriginalColor;

        public GUIContentColorArea(Color color)
        {
            OriginalColor = GUI.contentColor;
            GUI.contentColor = color;
        }

        public void Dispose()
        {
            GUI.contentColor = OriginalColor;
        }
    }

    internal struct GUIFullColorArea : IDisposable
    {
        public static GUIFullColorArea Create(Color color)
        {
            return new GUIFullColorArea(color);
        }

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

    internal struct GUIVertical : IDisposable
    {
        public static GUIVertical Create(params GUILayoutOption[] options)
        {
            return new GUIVertical(options);
        }

        public static GUIVertical Create(GUIStyle style, params GUILayoutOption[] options)
        {
            return new GUIVertical(style, options);
        }

        public GUIVertical(params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(options);
        }

        public GUIVertical(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginVertical(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndVertical();
        }
    }

    internal struct GUIHorizontal : IDisposable
    {
        public static GUIHorizontal Create(params GUILayoutOption[] options)
        {
            return new GUIHorizontal(options);
        }

        public static GUIHorizontal Create(GUIStyle style, params GUILayoutOption[] options)
        {
            return new GUIHorizontal(style, options);
        }

        public GUIHorizontal(params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(options);
        }

        public GUIHorizontal(GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginHorizontal(style, options);
        }

        public void Dispose()
        {
            GUILayout.EndHorizontal();
        }
    }

    internal struct GUIScrollView : IDisposable
    {
        public static GUIScrollView Create(ref Vector2 pos, params GUILayoutOption[] options)
        {
            return new GUIScrollView(ref pos, options);
        }

        public static GUIScrollView Create(ref Vector2 pos, GUIStyle style, params GUILayoutOption[] options)
        {
            return new GUIScrollView(ref pos, style, options);
        }

        public GUIScrollView(ref Vector2 pos, params GUILayoutOption[] options)
        {
            pos = GUILayout.BeginScrollView(pos, options);
        }

        public GUIScrollView(ref Vector2 pos, GUIStyle style, params GUILayoutOption[] options)
        {
            GUILayout.BeginScrollView(pos, style, options);
        }

        public void Dispose()
        {
            GUILayout.EndScrollView();
        }
    }
}
#endif