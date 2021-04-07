/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventHandler.cs
//  Info     : 事件处理器
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace Aya.Events
{
    public enum EventDataType
    {
        Enum = 0,
        String = 1,
        Class = 2,
        Struct = 3,
    }

    public partial class EventHandler
    {
        #region Editor Property

        internal EventDataType DataType
        {
            get
            {
                if (_dataType == null)
                {
                    if (EventType is Type type)
                    {
                        _dataType = type.IsValueType ? EventDataType.Struct : EventDataType.Class;
                    }
                    else
                    {
                        if (EventType is string eventStr)
                        {
                            _dataType = EventDataType.String;
                        }
                        else
                        {
                            _dataType = EventDataType.Enum;
                        }
                    }
                }

                return _dataType.Value;
            }
        }

        private EventDataType? _dataType;

        #endregion

        #region Log

        internal int DispatchCounter => DispatchSuccessCounter + DispatchFailCounter;
        internal int DispatchSuccessCounter;
        internal int DispatchFailCounter;
        internal DateTime LastInvokeDateTime;
        internal static List<EventLogData> Logs = new List<EventLogData>();

        internal static void CacheLog(EventHandler eventHandler, object eventType, object[] args, bool success, Exception exception)
        {
            if (success)
            {
                eventHandler.DispatchSuccessCounter++;
            }
            else
            {
                eventHandler.DispatchFailCounter++;
            }

            eventHandler.LastInvokeDateTime = DateTime.Now;

            var log = new EventLogData
            {
                EventType = eventHandler.EventType.ToString(),
                DateTime = DateTime.Now,
                Success = success,
                Exception = exception,
            };

            log.Parameters = "[";
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                log.Parameters += arg;
                if (i < args.Length - 1)
                {
                    log.Parameters += ",";
                }
            }

            log.Parameters += "]";
            Logs.Add(log);

            if (Logs.Count > EventEditorSetting.Ins.CacheLogCount)
            {
                Logs.RemoveAt(0);
            }
        }

        #endregion

        #region Animation Info

        internal float AnimationDuration = 0.5f;

        internal float LastInvokeSuccessTime = -1;
        internal bool IsInvokeSuccess => Time.realtimeSinceStartup - LastInvokeSuccessTime < AnimationDuration;
        internal float InvokeSuccessProgress => IsInvokeSuccess ? (Time.realtimeSinceStartup - LastInvokeSuccessTime) / AnimationDuration : 0f;

        internal float LastInvokeFailTime = -1;
        internal bool IsInvokeFail => Time.realtimeSinceStartup - LastInvokeFailTime < AnimationDuration;
        internal float InvokeFailProgress => IsInvokeFail ? (Time.realtimeSinceStartup - LastInvokeFailTime) / AnimationDuration : 0f;

        internal float ListeningTime = Time.realtimeSinceStartup;
        internal bool IsListening => Time.realtimeSinceStartup - ListeningTime < AnimationDuration;
        internal float ListeningProgress => IsListening ? (Time.realtimeSinceStartup - ListeningTime) / AnimationDuration : 0f; 

        #endregion

        #region Signature

        internal string MethodSignature
        {
            get
            {
                if (string.IsNullOrEmpty(_methodSignature))
                {
                    _methodSignature = GetSignature(Method, false);
                }

                return _methodSignature;
            }
        }
        private string _methodSignature;

        internal string MethodSignatureRichText
        {
            get
            {
                if (string.IsNullOrEmpty(_methodSignatureRichText))
                {
                    _methodSignatureRichText = GetSignature(Method, true);
                }

                return _methodSignatureRichText;
            }
        }
        private string _methodSignatureRichText;

        private string GetSignature(MethodInfo methodInfo, bool richText)
        {
            if (methodInfo == null)
            {
                return "";
            }

            var stringBuilder = new StringBuilder();

            if (richText) stringBuilder.Append(GetColorMarkupStart(EventEditorSetting.Ins.MonitorStyle.CodeKeyWordColor));
            if (methodInfo.IsPrivate)
            {
                stringBuilder.Append("private ");
            }
            else if (methodInfo.IsPublic)
            {
                stringBuilder.Append("public ");
            }
            else if (methodInfo.IsFamily)
            {
                stringBuilder.Append("protected ");
            }
            else if (methodInfo.IsAssembly)
            {
                stringBuilder.Append("internal ");
            }
            else if (methodInfo.IsFamilyOrAssembly)
            {
                stringBuilder.Append("protected internal ");
            }

            if (methodInfo.IsStatic)
            {
                stringBuilder.Append("static ");
            }

            if (methodInfo.IsAbstract)
            {
                stringBuilder.Append("abstract ");
            }
            else if (methodInfo.IsVirtual)
            {
                stringBuilder.Append("virtual ");
            }

            if (richText) stringBuilder.Append(GetColorMarkupEnd());

            if (richText) stringBuilder.Append(GetColorMarkupStart(EventEditorSetting.Ins.MonitorStyle.CodeParameterColor));
            stringBuilder.Append(methodInfo.ReturnType.Name);
            if (richText) stringBuilder.Append(GetColorMarkupEnd());
            stringBuilder.Append(" ");

            if (richText) stringBuilder.Append(GetColorMarkupStart(EventEditorSetting.Ins.MonitorStyle.CodeMethodColor));
            stringBuilder.Append(methodInfo.Name);
            if (richText) stringBuilder.Append(GetColorMarkupEnd());
            if (richText) stringBuilder.Append(GetColorMarkupStart(EventEditorSetting.Ins.MonitorStyle.CodeNormalColor));
            stringBuilder.Append("(");
            if (richText) stringBuilder.Append(GetColorMarkupEnd());
            var parameters = methodInfo.GetParameters();
            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                if (richText) stringBuilder.Append(GetColorMarkupStart(EventEditorSetting.Ins.MonitorStyle.CodeParameterColor));
                stringBuilder.Append(parameter.ParameterType.Name);
                if (richText) stringBuilder.Append(GetColorMarkupEnd());
                stringBuilder.Append(" ");
                if (richText) stringBuilder.Append(GetColorMarkupStart(EventEditorSetting.Ins.MonitorStyle.CodeNormalColor));
                stringBuilder.Append(parameter.Name);
                if (i < parameters.Length - 1)
                {
                    stringBuilder.Append(",");
                }

                if (richText) stringBuilder.Append(GetColorMarkupEnd());
            }

            if (richText) stringBuilder.Append(GetColorMarkupStart(EventEditorSetting.Ins.MonitorStyle.CodeNormalColor));
            stringBuilder.Append(")");
            if (richText) stringBuilder.Append(GetColorMarkupEnd());
            return stringBuilder.ToString();
        }

        #endregion

        #region Color Markup

        private string GetColorMarkupStart(Color color)
        {
            var stringBuilder = new StringBuilder();
            var r = (int)(color.r * 255);
            var g = (int)(color.g * 255);
            var b = (int)(color.b * 255);
            stringBuilder.Append("<color=#");
            stringBuilder.Append(r.ToString("x2"));
            stringBuilder.Append(g.ToString("x2"));
            stringBuilder.Append(b.ToString("x2"));
            stringBuilder.Append(">");
            return stringBuilder.ToString();
        }

        private string GetColorMarkupEnd()
        {
            return "</color>";
        }


        #endregion
    }
}
#endif