using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cofdream.ToolKitEditor
{
    [InitializeOnLoad]
    public static class ReloadAssemblies
    {
        internal const string MENU_ITEM_KEY = MenuItemName.ToolKit + "LockReloadAssemblies";
        internal const string LOCK_KEY = "LockReloadAssemblies";
        internal const string LOCK_PLAY_MODE_TIP_KEY = "PlayModelLockReloadAssembliesTip";

        internal static bool LockReloadAssemblies;

        static ReloadAssemblies()
        {
            LockReloadAssemblies = EditorPrefs.GetBool(LOCK_KEY, false);
            Menu.SetChecked(MENU_ITEM_KEY, LockReloadAssemblies);
            if (LockReloadAssemblies)
            {
                EditorApplication.LockReloadAssemblies();
            }
            else
            {
                EditorApplication.UnlockReloadAssemblies();
            }
            EditorApplication.playModeStateChanged += PlayModeStateChangedCallback;

            CreateMainToolbarVisualElement();
        }

        private static void PlayModeStateChangedCallback(PlayModeStateChange state)
        {
            if (EditorPrefs.GetBool(LOCK_PLAY_MODE_TIP_KEY, true))
            {
                if (state == PlayModeStateChange.EnteredPlayMode && LockReloadAssemblies)
                {
                    Debug.Log("重新加载程序集已被锁定。");
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

        [MenuItem(MENU_ITEM_KEY, priority = int.MaxValue)]
        public static void SetLockReloadAssembliesDialog()
        {
            if (LockReloadAssemblies)
            {
                Debug.Log("重新加载程序集已解锁。");
                SetLockReloadAssemblies();
            }
            else
            {
                if (EditorUtility.DisplayDialog("提示", "是否锁定 重新加载程序集 \n\n锁定以后无法重新加载程序集,\n也不会触发脚本编译。", "继续锁定", "取消"))
                {
                    Debug.Log("重新加载程序集已锁定。");
                    SetLockReloadAssemblies();
                }
            }
        }

        public static void SetLockReloadAssemblies()
        {
            if (LockReloadAssemblies)
                EditorApplication.UnlockReloadAssemblies();
            else
                EditorApplication.LockReloadAssemblies();

            LockReloadAssemblies = !LockReloadAssemblies;
            EditorPrefs.SetBool(LOCK_KEY, LockReloadAssemblies);
            Menu.SetChecked(MENU_ITEM_KEY, LockReloadAssemblies);
        }

        private static void CreateMainToolbarVisualElement()
        {
            var lockButton = new Button();
            lockButton.name = $"Lock{nameof(ReloadAssemblies)}";

            var backgroundImage = new Image();
            lockButton.Add(backgroundImage);
            backgroundImage.name = "background";
            backgroundImage.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("AssemblyDefinitionAsset Icon");
            backgroundImage.style.overflow = Overflow.Visible;
            backgroundImage.style.alignSelf = Align.Center;
            backgroundImage.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            backgroundImage.style.width = 16f;
            backgroundImage.style.height = 16f;

            var lockOff = new Image();
            lockButton.Add(lockOff);
            lockOff.name = "lock off";
            lockOff.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("IN LockButton");
            lockOff.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            lockOff.style.alignSelf = Align.Center;
            lockOff.style.width = 16f;
            lockOff.style.height = 16f;

            var lockOn = new Image();
            lockButton.Add(lockOn);
            lockOn.name = "lock on";
            lockOn.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("IN LockButton on");
            lockOn.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            lockOn.style.alignSelf = Align.Center;
            lockOn.style.width = 16f;
            lockOn.style.height = 16f;


            lockOff.style.display = LockReloadAssemblies ? DisplayStyle.Flex : DisplayStyle.None;
            lockOn.style.display = !LockReloadAssemblies ? DisplayStyle.Flex : DisplayStyle.None;


            lockButton.style.backgroundColor = new Color(56f / 255f, 56f / 255f, 56f / 255f, 255f / 255f);
            lockButton.style.overflow = Overflow.Visible;
            lockButton.style.flexDirection = FlexDirection.Row;

            lockButton.clicked += () =>
            {
                SetLockReloadAssemblies();
                SetLockImageDisplay();
            };

            void SetLockImageDisplay()
            {
                lockOff.style.display = LockReloadAssemblies ? DisplayStyle.None : DisplayStyle.Flex;
                lockOn.style.display = LockReloadAssemblies ? DisplayStyle.Flex : DisplayStyle.None;
                lockButton.tooltip = $"锁定程序集编译\n当前状态：{(LockReloadAssemblies ? "锁定编译" : "开启编译")}";
            }

            SetLockImageDisplay();

            MainToolbarExtensions.ToolbarZoneRightAlign.Add(lockButton);
        }
    }
}