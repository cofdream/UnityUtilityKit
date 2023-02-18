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

        private static Button _lock;
        private static Image _lockOff;
        private static Image _lockOn;
       
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
            _lock = new Button();
            _lock.name = $"Lock{nameof(ReloadAssemblies)}";

            var backgroundImage = new Image();
            _lock.Add(backgroundImage);
            backgroundImage.name = "background";
            backgroundImage.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("AssemblyDefinitionAsset Icon");
            backgroundImage.style.overflow = Overflow.Visible;
            backgroundImage.style.alignSelf = Align.Center;
            backgroundImage.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            backgroundImage.style.width = 16f;
            backgroundImage.style.height = 16f;

            _lockOff = new Image();
            _lock.Add(_lockOff);
            _lockOff.name = "lock off";
            _lockOff.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("IN LockButton");
            _lockOff.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            _lockOff.style.alignSelf = Align.Center;
            _lockOff.style.width = 16f;
            _lockOff.style.height = 16f;

            _lockOn = new Image();
            _lock.Add(_lockOn);
            _lockOn.name = "lock on";
            _lockOn.style.backgroundImage = EditorGUIUtilityExtensions.LoadIconRequired("IN LockButton on");
            _lockOn.style.unityBackgroundScaleMode = ScaleMode.ScaleToFit;
            _lockOn.style.alignSelf = Align.Center;
            _lockOn.style.width = 16f;
            _lockOn.style.height = 16f;


            _lockOff.style.display = LockReloadAssemblies ? DisplayStyle.Flex : DisplayStyle.None;
            _lockOn.style.display = !LockReloadAssemblies ? DisplayStyle.Flex : DisplayStyle.None;


            _lock.style.backgroundColor = new Color(56f / 255f, 56f / 255f, 56f / 255f, 255f / 255f);
            _lock.style.overflow = Overflow.Visible;
            _lock.style.flexDirection = FlexDirection.Row;

            _lock.clicked += OnClickLock;

            SetLockImageDisplay();

            MainToolbarExtensions.AddToolbarZoneRightAlign(_lock);
        }

        private static void OnClickLock()
        {
            SetLockReloadAssemblies();
            SetLockImageDisplay();
        }

        private static void SetLockImageDisplay()
        {
            _lockOff.style.display = LockReloadAssemblies ? DisplayStyle.None : DisplayStyle.Flex;
            _lockOn.style.display = LockReloadAssemblies ? DisplayStyle.Flex : DisplayStyle.None;
            _lock.tooltip = $"锁定程序集编译\n当前状态：{(LockReloadAssemblies ? "锁定编译" : "开启编译")}";
        }
    }
}