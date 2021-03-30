/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventLogData.cs
//  Info     : 事件日志数据
//  Author   : ls9512 2021
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using System;
using System.Text;

namespace Aya.Events
{
    [Serializable]
    public class EventLogData
    {
        public DateTime DateTime;
        public string EventType;
        public string Parameters;
        public bool Success;
        public Exception Exception;

        public string Log
        {
            get
            {
                if (string.IsNullOrEmpty(_log))
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append(DateTime.ToString(EventEditorSetting.Ins.DateFormat));
                    stringBuilder.Append("\t");
                    stringBuilder.Append(EventType);
                    stringBuilder.Append("\t");
                    stringBuilder.Append(Parameters);
                    stringBuilder.Append("\t");
                    if (!Success)
                    {
                        stringBuilder.Append("\n");
                        stringBuilder.Append(Exception);
                    }

                    _log = stringBuilder.ToString();
                }

                return _log;
            }
        }

        private string _log;

        public override string ToString()
        {
            return Log;
        }
    }
}
#endif