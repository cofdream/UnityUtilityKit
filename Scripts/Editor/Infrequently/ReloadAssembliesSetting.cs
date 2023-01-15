using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    [InitializeOnLoad]
    public class ReloadAssembliesSetting
    {
        internal const string MENUKEY = MenuItemName.ToolKit + "LockReloadAssemblies";
        internal const string KEY = "LockReloadAssemblies";
        internal const string playeModelTipKey = "PlayModelLockReloadAssembliesTip";

        internal static bool LockReloadAssemblies;

        static ReloadAssembliesSetting()
        {
            LockReloadAssemblies = EditorPrefs.GetBool(KEY, false);
            Menu.SetChecked(MENUKEY, LockReloadAssemblies);
            if (LockReloadAssemblies)
            {
                EditorApplication.LockReloadAssemblies();
            }
            else
            {
                EditorApplication.UnlockReloadAssemblies();
            }
            EditorApplication.playModeStateChanged += LogPlayModeState;
        }

        private static void LogPlayModeState(PlayModeStateChange state)
        {
            if (EditorPrefs.GetBool(playeModelTipKey, true))
            {
                if (state == PlayModeStateChange.EnteredPlayMode && LockReloadAssemblies)
                {
                    Debug.LogWarning("重新加载程序集已被锁定。");
                    int value = EditorUtility.DisplayDialogComplex("警告", "已锁定重新加载程序集，请注意！！！", "继续进入播放模式(下次不提示)", "继续进入播放模式", "取消播放");
                    switch (value)
                    {
                        case 0: break;
                        case 1: break;
                        case 2:
                            EditorApplication.isPlaying = false;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        [MenuItem(MENUKEY, priority = int.MaxValue)]
        private static void SetLockReloadAssemblies()
        {
            if (LockReloadAssemblies)
            {
                Debug.Log("重新加载程序集已解锁。");
                EditorApplication.UnlockReloadAssemblies();
                LockReloadAssemblies = !LockReloadAssemblies;
                EditorPrefs.SetBool(KEY, false);
                Menu.SetChecked(MENUKEY, false);
            }
            else
            {
                if (EditorUtility.DisplayDialog("提示", "是否锁定 重新加载程序集 \n\n锁定以后无法重新加载程序集,\n也不会触发脚本编译。", "继续锁定", "取消"))
                {
                    Debug.Log("重新加载程序集已锁定。");
                    EditorApplication.LockReloadAssemblies();
                    LockReloadAssemblies = !LockReloadAssemblies;
                    EditorPrefs.SetBool(KEY, true);
                    Menu.SetChecked(MENUKEY, true);
                }
            }
        }
    }
}