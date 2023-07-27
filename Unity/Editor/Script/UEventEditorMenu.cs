#if UNITY_EDITOR
using UnityEditor;

namespace Aya.Events
{
    public partial class UEventEditorMenu
    {
        [MenuItem("Tools/Aya Game/UEvent/Event Monitor", false, 0)]
        public static void OpenEventMonitor()
        {
            EventMonitor.ShowWindow();
        }

        [MenuItem("Tools/Aya Game/UEvent/Editor Setting", false, 1000)]
        public static void OpenRuntimeSetting()
        {
            SettingsService.OpenProjectSettings("Aya Game/UEvent Editor");
        }
    }
}
#endif