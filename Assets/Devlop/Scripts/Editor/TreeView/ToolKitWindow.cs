using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public class ToolKitWindow : EditorWindow, IHasCustomMenu
    {
        [MenuItem("ToolKit/Development Tool Window 2.0")]
        static void OpenWindow()
        {
            GetWindow<ToolKitWindow>().Show();
        }


        //left
        string treeSearch;
        [SerializeField] SearchField searchField;
        ToolTreeView treeView;
        TreeViewState treeViewState;

        //line
        float lineX;
        EditorSplitLine splitLine;

        ToolProviderData providerData;


        private void Awake()
        {
            treeViewState = new TreeViewState();
            splitLine = new EditorSplitLine();
            lineX = 150;

            providerData = new ToolProviderData();
            providerData.Providers = FetchToolProviderFromAttribute().ToArray();
        }

        private void OnEnable()
        {
            searchField = new SearchField();
            treeView = new ToolTreeView(treeViewState, providerData);
        }

        private void OnDestroy()
        {
            if (providerData.Providers != null)
            {
                foreach (var provider in providerData.Providers)
                {
                    provider.OnDestroy();
                }
                providerData.Providers = null;
            }
            providerData = null;
        }

        private void OnGUI()
        {
            Rect rect = new Rect(0, 0, position.width, position.height);
            rect.SplitHorizontal(lineX, out Rect left, out Rect right);

            //绘制分割线
            lineX = splitLine.Vertical(lineX, rect.y, 80, 200, rect.height);

            //left
            {
                left.SplitVertical(22, out Rect top, out Rect bottom);

                // Top
                {
                    GUI.Label(top, GUIContent.none, EditorStyles.toolbar);
                    treeSearch = searchField.OnGUI(top.GetPadding(2, 2, 2, 0), treeSearch);
                }

                //bottom
                {
                    treeView.OnGUI(bottom.GetPadding(2, 2, 2, 0));

                    // Clear select
                    {
                        var e = Event.current;
                        if (e.type == EventType.MouseDown && e.button == 0 && bottom.Contains(e.mousePosition))
                        {
                            treeViewState.selectedIDs = new List<int>(0);
                            e.Use();
                        }
                        //右键 todo
                        else if (e.type == EventType.MouseDown && e.button == 1 && bottom.Contains(e.mousePosition))
                        {
                            // 打开脚本文件
                            //e.Use();
                        }
                    }
                }
            }

            //right
            {
                if (providerData.CurrentProvider != null)
                {
                    var rightContent = right.GetPadding(4, 0, 4, 0);

                    GUI.BeginGroup(rightContent);
                    {
                        EditorGUILayout.BeginVertical(GUILayout.Width(rightContent.width), GUILayout.Height(rightContent.height));
                        {
                            providerData.CurrentProvider.OnDraw();
                        }
                        EditorGUILayout.EndVertical();
                    }
                    GUI.EndGroup();
                }
            }
        }

        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(EditorGUIUtility.TrTextContent("Reload tools"), false, () => { Debug.LogWarning("Not do."); });
            this.EditScript(menu);
        }


        IEnumerable<ToolProvider> FetchToolProviderFromAttribute()
        {
            // todo 其他程序集

            var methodInfos = TypeCache.GetMethodsWithAttribute<ToolProviderAttribute>();
            var providers = new List<ToolProvider>(methodInfos.Count());

            foreach (var method in methodInfos)
            {
                try
                {
                    if (method.IsStatic && method.IsGenericMethod == false)
                    {
                        var callback = Delegate.CreateDelegate(typeof(Func<ToolProvider>), method) as Func<ToolProvider>;
                        var provider = callback?.Invoke();
                        if (provider != null)
                        {
                            providers.Add(provider);
                        }
                    }
                }
                catch (Exception e)
                {
                    Debug.LogError($"Cannot create Settings Providers for: {method.Name}\n  {e}");
                }
            }

            return providers;
        }
    }
}