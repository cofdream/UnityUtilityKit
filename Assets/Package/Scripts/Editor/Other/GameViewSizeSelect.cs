using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Collections.Generic;

namespace Cofdream.ToolKitEditor
{
    // 快速修改分辨率
    // update： 
    // 1.默认迷你大小
    // 2.默认位置在Game View 的分辨率选择之上
    // 3.可滑动内容为具体的分辨率
    // 4.upadte++ 点击箭头滑动自动选中到对应分辨率的位置上，需要先完成2.

    //2021.12.29.update
    //todo 改成 Overlays 的形式
    public class GameViewSizeSelect : EditorWindowPlus
    {
        private object gameViewSizesInstance;
        private PropertyInfo currentGroup;

        private EditorWindow gameViewWindow;
        private MethodInfo sizeSelectionCallback;
        private int sizeSelectIndex = 0;
        private int max;

        private List<_GameViewSize> _GameViewSizes_Builtin;
        private List<_GameViewSize> _GameViewSizes_Custom;

        Vector2 scroolViewValue;
        private void Awake()
        {
            var type = Type.GetType("UnityEditor.GameViewSizes,UnityEditor");
            currentGroup = type.GetProperty("currentGroup", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
            var singletonType = typeof(ScriptableSingleton<>).MakeGenericType(type);
            gameViewSizesInstance = singletonType.GetProperty("instance", BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty).GetValue(null, null);



            Type gameViewType = Type.GetType("UnityEditor.GameView,UnityEditor");
            gameViewWindow = EditorWindow.GetWindow(gameViewType);
            sizeSelectionCallback = gameViewType.GetMethod("SizeSelectionCallback", BindingFlags.Public | BindingFlags.Instance);

            int index = 0;
            object gameViewSizeGroup = currentGroup.GetValue(gameViewSizesInstance);
            Type gameViewSizeGroupType = gameViewSizeGroup.GetType();

            // 获取 GameViewSize 数据
            IEnumerable<object> m_builtion = gameViewSizeGroupType.GetField("m_Builtin", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gameViewSizeGroup) as IEnumerable<object>;
            PropertyInfo displayText_Get = null;
            foreach (var item in m_builtion)
            {
                Type gameViewSizeType = item.GetType();
                displayText_Get = gameViewSizeType.GetProperty("displayText", BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetProperty);
                break;
            }
            if (displayText_Get != null)
            {
                _GameViewSizes_Builtin = new List<_GameViewSize>();
                foreach (var gameViewSize in m_builtion)
                {
                    _GameViewSizes_Builtin.Add(new _GameViewSize()
                    {
                        displayText = displayText_Get.GetValue(gameViewSize).ToString(),
                        index = index++,
                    });
                }
            }
            IEnumerable<object> m_Custom = gameViewSizeGroupType.GetField("m_Custom", BindingFlags.NonPublic | BindingFlags.Instance).GetValue(gameViewSizeGroup) as IEnumerable<object>;
            if (displayText_Get != null)
            {
                _GameViewSizes_Custom = new List<_GameViewSize>();
                foreach (var gameViewSize in m_Custom)
                {
                    _GameViewSizes_Custom.Add(new _GameViewSize()
                    {
                        displayText = displayText_Get.GetValue(gameViewSize).ToString(),
                        index = index++,
                    });
                }
            }

            int.TryParse(gameViewType.GetProperty("selectedSizeIndex", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.GetProperty).GetValue(gameViewWindow).ToString(), out sizeSelectIndex);

            max = _GameViewSizes_Builtin.Count + _GameViewSizes_Custom.Count - 1;
        }
        private int aa;
        private void OnGUI()
        {
            scroolViewValue = GUILayout.BeginScrollView(scroolViewValue);
            {
                GUILayout.BeginHorizontal();
                {
                    if (GUILayout.Button(EditorGUIUtility.FindTexture("RotateTool")))
                    {
                        // 先这样
                        Awake();
                    }

                    GUILayout.Label($"max: {max}");

                    if (int.TryParse(GUILayout.TextField(sizeSelectIndex.ToString(), GUILayout.Width(40)), out int index))
                    {
                        if (index < max && index >= 0)
                        {
                            sizeSelectIndex = index;
                        }
                    }

                    GUILayout.Space(5);

                    if (GUILayout.Button(new GUIContent("\u2191")))
                    {
                        sizeSelectIndex--;
                        if (sizeSelectIndex < 0)
                        {
                            sizeSelectIndex = 0;
                        }
                        if (sizeSelectIndex >= max)
                        {
                            sizeSelectIndex = max - 1;
                        }
                        SelectGameViewSize();
                    }
                    if (GUILayout.Button(new GUIContent("\u2193")))
                    {
                        sizeSelectIndex++;
                        if (sizeSelectIndex < 0)
                        {
                            sizeSelectIndex = 0;
                        }
                        if (sizeSelectIndex >= max)
                        {
                            sizeSelectIndex = max - 1;
                        }
                        SelectGameViewSize();
                    }

                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndHorizontal();

                GUILayout.Space(2);

                Rect rect = EditorGUILayout.GetControlRect(GUILayout.Width(this.maxSize.x), GUILayout.Height(15));
                int tempIndex = -1;

                DrawGameViewSizeGroup(_GameViewSizes_Builtin, ref rect, ref tempIndex);

                rect.position += new Vector2(0, 8);
                GUI.Label(rect, "");

                DrawGameViewSizeGroup(_GameViewSizes_Custom, ref rect, ref tempIndex);
            }
            GUILayout.EndScrollView();

            base.Repaint();
        }
        private void DrawGameViewSizeGroup(List<_GameViewSize> gameViewSizes, ref Rect rect, ref int index)
        {
            if (gameViewSizes == null) return;

            foreach (var _GameViewSize in gameViewSizes)
            {
                index++;
                if (rect.Contains(Event.current.mousePosition) && Event.current.type == EventType.MouseDown)
                {
                    sizeSelectIndex = index;
                    _GameViewSize.index = sizeSelectIndex;
                    SelectGameViewSize();
                }

                GUIStyle style = GUIStyle.none;
                if (sizeSelectIndex == _GameViewSize.index)
                {
                    style = (GUIStyle)"LODSliderRangeSelected";
                }

                GUI.Label(rect, _GameViewSize.displayText, style);

                string value = index.ToString();
                GUI.Label(new Rect(new Vector2(this.position.size.x - 8 - (6 * value.Length + 1), rect.position.y), new Vector2(18, 18)), value);

                rect.position += new Vector2(0, 15);
            }

        }
        private void SelectGameViewSize()
        {
            sizeSelectionCallback.Invoke(gameViewWindow, new object[] { sizeSelectIndex, null });
        }
        private class _GameViewSize
        {
            public string displayText;
            public int index;
        }
    }
}