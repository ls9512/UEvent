/////////////////////////////////////////////////////////////////////////////
//
//  Script   : UEventEditorSetting.cs
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
    [CreateAssetMenu(menuName = "UEvent/Event Editor Setting", fileName = "UEventEditorSetting")]
    public class UEventEditorSetting : ScriptableObject
    {
        #region Instance

        public static UEventEditorSetting Ins
        {
            get
            {
                if (Instance == null)
                {
                    Instance = FindAsset<UEventEditorSetting>();
                }

                return Instance;
            }
        }

        protected static UEventEditorSetting Instance;

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