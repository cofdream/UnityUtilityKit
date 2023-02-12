using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace Cofdream.ToolKitEditor
{
    /// <summary>
    /// 编辑器窗口 扩展
    /// </summary>
    public static class EditorWindowExtension
    {
        public static T GetWindowInCenter<T>() where T : EditorWindow
        {
            return GetWindowInCenter<T>(new Vector2(600, 300));
        }
        public static T GetWindowInCenter<T>(Vector2 size) where T : EditorWindow
        {
            var window = EditorWindow.GetWindow<T>();
            window.position = GetMainWindowCenteredPosition(size);
            return window;
        }

        public static Rect GetMainWindowCenteredPosition(Vector2 size)
        {
            Rect parentWindowPosition = GetMainWindowPositon();
            var pos = new Rect
            {
                x = 0,
                y = 0,
                width = Mathf.Min(size.x, parentWindowPosition.width * 0.90f),
                height = Mathf.Min(size.y, parentWindowPosition.height * 0.90f)
            };
            var w = (parentWindowPosition.width - pos.width) * 0.5f;
            var h = (parentWindowPosition.height - pos.height) * 0.5f;
            pos.x = parentWindowPosition.x + w;
            pos.y = parentWindowPosition.y + h;
            return pos;
        }

        public static T SetCenterPostion<T>(this T window, float width, float height) where T : EditorWindow
        {
            window.position = GetMainWindowCenteredPosition(new Vector2(width, height));
            return window;
        }

        public static Rect GetMainWindowPositon()
        {
#if UNITY_2020_3_OR_NEWER
            return EditorGUIUtility.GetMainWindowPosition();
#else
            UnityEngine.Object mainWindow = null;
            var containerWinType = TypeCache.GetTypesDerivedFrom(typeof(ScriptableObject)).FirstOrDefault(t => t.Name == "ContainerWindow");
            if (containerWinType == null)
                throw new MissingMemberException("Can't find internal type ContainerWindow. Maybe something has changed inside Unity");
            var showModeField = containerWinType.GetField("m_ShowMode", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (showModeField == null)
                throw new MissingFieldException("Can't find internal fields 'm_ShowMode'. Maybe something has changed inside Unity");
            var windows = Resources.FindObjectsOfTypeAll(containerWinType);
            foreach (var win in windows)
            {
                var showMode = (int)showModeField.GetValue(win);
                if (showMode == 4)
                {
                    mainWindow = win;
                    break;
                }
            }

            if (mainWindow == null)
            {
                Debug.LogWarning("Unity mainWindow API change,Please Check!");
                return new Rect(0f, 0f, 1000f, 600f);
            }

            var positionProperty = mainWindow.GetType().GetProperty("position", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
            if (positionProperty == null)
                throw new MissingFieldException("Can't find internal fields 'position'. Maybe something has changed inside Unity.");

            return ((Rect)positionProperty.GetValue(mainWindow, null));
#endif
        }


        public static void PinScript<T>(this T t, GenericMenu menu) where T : EditorWindow, IHasCustomMenu
        {
            menu.AddItem(EditorGUIUtility.TrTextContent("Pin Script"), false, PinScript, t);
        }
        private static void PinScript(object userData)
        {
            EditorGUIUtility.PingObject(((UnityEngine.Object)userData)?.GetScript());
        }

        public static void EditScript<T>(this T t, GenericMenu menu) where T : EditorWindow, IHasCustomMenu
        {
            menu.AddItem(EditorGUIUtility.TrTextContent("Edit Script"), false, OpenEditScript, t);
        }
        private static void OpenEditScript(object userData)
        {
            var script = ((UnityEngine.Object)userData)?.GetScript();
            if (AssetDatabase.OpenAsset(script) == false)
            {
                Debug.LogError($"打开 {userData?.GetType().FullName} 失败");
            }
        }
    }
}