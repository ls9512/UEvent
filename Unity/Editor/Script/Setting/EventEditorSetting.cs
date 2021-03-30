/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventEditorSetting.cs
//  Info     : 事件编辑器配置类
//  Author   : ls9512 2021
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Aya.Events
{
    [CreateAssetMenu(menuName = "UEvent/Event Editor Setting", fileName = "EventEditorSetting")]
    public class EventEditorSetting : ScriptableObject
    {
        #region Instance

        public static EventEditorSetting Ins
        {
            get
            {
                if (Instance == null)
                {
                    Instance = FindAsset<EventEditorSetting>();
                }

                return Instance;
            }
        }

        protected static EventEditorSetting Instance;

        internal static T FindAsset<T>() where T : Object
        {
            var guidList = AssetDatabase.FindAssets("t:" + typeof(T).FullName);
            if (guidList != null && guidList.Length > 0)
            {
                return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(guidList[0]));
            }

            return null;
        }

        #endregion

        [Header("Log")] 
        public string DateFormat = "yyyyMMdd HH:mm:ss";
        public int CacheLogCount = 1000;

        [Header("Style")]
        public EventMonitorStyleSetting MonitorStyle;
    }
}
#endif