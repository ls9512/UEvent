/////////////////////////////////////////////////////////////////////////////
//
//  Script   : GUITable.cs
//  Info     : GUI表格
//  Author   : ls9512 2021
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Aya.Events
{
    internal struct GUITable<T> : IDisposable
    {
        public static GUIStyle HeaderStyle = new GUIStyle()
        {
            // alignment = TextAnchor.MiddleCenter,
            normal = new GUIStyleState()
            {
                textColor = Color.white,
                background = GUIHelper.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.2f))
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

            // GUILayout.FlexibleSpace();
            // DrawBottom();
        }

        public void DrawHeader(string[] headers, float[] columnWidths)
        {
            using (new GUIHorizontal(HeaderStyle))
            {
                for (var i = 0; i < headers.Length; i++)
                {
                    var header = headers[i];
                    GUILayout.Label(header, GUILayout.Width(columnWidths[i]));
                }
            }
        }

        public void DrawBottom()
        {
            using (new GUIHorizontal(HeaderStyle, GUILayout.Height(EditorGUIUtility.singleLineHeight)))
            {
                GUILayout.FlexibleSpace();
                GUILayout.Button("←", GUILayout.Width(30));
                GUILayout.TextArea("1", GUILayout.Width(20));
                GUILayout.Label("/100", GUILayout.Width(20));
                GUILayout.Button("→", GUILayout.Width(30));

                if(GUILayout.Button("10", GUILayout.Width(40)))
                {
                    var pageRowMenu = new GenericMenu();
                    pageRowMenu.AddItem(new GUIContent("10"), true, () => { });
                    pageRowMenu.AddItem(new GUIContent("20"), false, () => { });
                    pageRowMenu.AddItem(new GUIContent("50"), false, () => { });
                    pageRowMenu.AddItem(new GUIContent("100"), false, () => { });
                    pageRowMenu.AddItem(new GUIContent("200"), false, () => { });
                    pageRowMenu.AddItem(new GUIContent("500"), false, () => { });
                    pageRowMenu.AddItem(new GUIContent("1000"), false, () => { });
                    pageRowMenu.ShowAsContext();
                }
            }
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

    internal struct GUITableCell : IDisposable
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

    internal struct GUITableRow : IDisposable
    {
        public static GUIStyle OddRowStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                background = GUIHelper.MakeTex(2, 2, new Color(0f, 0f, 0f, 0.035f))
            }
        };

        public static GUIStyle EvenRowStyle = new GUIStyle()
        {
            normal = new GUIStyleState()
            {
                background = GUIHelper.MakeTex(2, 2, new Color(1f, 1f, 1f, 0.05f))
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

}
#endif