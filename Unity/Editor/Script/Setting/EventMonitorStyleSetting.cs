/////////////////////////////////////////////////////////////////////////////
//
//  Script   : EventMonitorStyleSetting.cs
//  Info     : 事件监视器样式配置类
//  Author   : ls9512 2021
//  E-mail   : ls9512@vip.qq.com
//
/////////////////////////////////////////////////////////////////////////////
#if UNITY_EDITOR
using UnityEngine;

namespace Aya.Events
{
    [CreateAssetMenu(menuName = "UEvent/Event Monitor Style Setting", fileName = "EventMonitorStyleSetting")]
    public class EventMonitorStyleSetting : ScriptableObject
    {
        [Header("Code Color")]
        public Color ActiveUrlColor;
        public Color CodeKeyWordColor;
        public Color CodeTypeColor;
        public Color CodeMethodColor;
        public Color CodeParameterColor;
        public Color CodeNormalColor;

        [Header("Tip Color")] 
        public Color TipListenColor;
        public Color TipSuccessColor;
        public Color TipFailColor;

        [Header("Param")]
        public string DateFormat = "yyyyMMdd HH:mm:ss";
    }
}
#endif