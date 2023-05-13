using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cofdream.ToolKitEditor
{
    public static class MainToolbarExtensions
    {
        public static VisualElement ToolbarZoneLeftAlign { get; private set; } = new VisualElement();
        public static VisualElement ToolbarZonePlayMode { get; private set; } = new VisualElement();
        public static VisualElement ToolbarZoneRightAlign { get; private set; } = new VisualElement();

        static MainToolbarExtensions()
        {
            EditorApplication.update -= Update;
            EditorApplication.update += Update;
        }

        private static void Update()
        {
            LoadMenuVE();
        }

        [System.Diagnostics.Conditional("UNITY_2021_1_OR_NEWER")]
        private static void LoadMenuVE()
        {
            var toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
            var getField = toolbarType?.GetField("get", BindingFlags.Public | BindingFlags.Static);
            var toolbar = getField?.GetValue(null);
            if (toolbar == null)
                return;

            var rootField = toolbarType?.GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);

            if (!(rootField?.GetValue(toolbar) is VisualElement root))
                return;

            if (root.childCount == 0)
                return;
            var rootChildren = root.Children() as List<VisualElement>;
            var rootChildrenChildren = rootChildren[0].Children() as List<VisualElement>;
            if (rootChildrenChildren.Count == 0)
                return;

            var toolbarContainerContent = rootChildrenChildren[0];
            var toolbarZoneSet = toolbarContainerContent.Children() as List<VisualElement>;

            if (toolbarContainerContent.childCount > 2)
            {
                toolbarZoneSet[0].Add(ToolbarZoneLeftAlign);
                toolbarZoneSet[1].Add(ToolbarZonePlayMode);
                toolbarZoneSet[2].Add(ToolbarZoneRightAlign);

                EditorApplication.update -= Update;
            }
        }
    }
}