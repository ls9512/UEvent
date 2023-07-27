#if UNITY_EDITOR
using Aya.Events;
using UnityEditor;

namespace Aya.TweenPro
{
    public static class UEventEditorSettingProvider
    {
        #region Project Setting

        [SettingsProvider]
        public static SettingsProvider GetEditorSetting()
        {
            var provider = AssetSettingsProvider.CreateProviderFromObject("Aya Game/UEvent Editor", UEventEditorSetting.Ins);
            provider.keywords = SettingsProvider.GetSearchKeywordsFromSerializedObject(new SerializedObject(UEventEditorSetting.Ins));
            return provider;
        }

        #endregion
    }
}

#endif