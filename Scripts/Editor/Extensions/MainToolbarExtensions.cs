using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.UIElements;

namespace Cofdream.ToolKitEditor
{
    public static class MainToolbarExtensions
    {
        public static VisualElement ToolbarZoneLeftAlign { get; private set; }
        public static VisualElement ToolbarZonePlayMode { get; private set; }
        public static VisualElement ToolbarZoneRightAlign { get; private set; }

        static MainToolbarExtensions()
        {
            var toolbarType = typeof(Editor).Assembly.GetType("UnityEditor.Toolbar");
            var getField = toolbarType?.GetField("get", BindingFlags.Public | BindingFlags.Static);
            var toolbar = getField?.GetValue(null);
            if (toolbar == null)
                return;

            var rootField = toolbarType?.GetField("m_Root", BindingFlags.NonPublic | BindingFlags.Instance);

            if (rootField?.GetValue(toolbar) is not VisualElement root)
                return;

            if (root.childCount == 0)
                return;
            var rootChildren = root.Children() as List<VisualElement>;
            var rootChildrenChildren = rootChildren[0].Children() as List<VisualElement>;
            if (rootChildrenChildren.Count == 0)
                return;

            var toolbarContainerContent = rootChildrenChildren[0];
            var toolbarZoneSet = toolbarContainerContent.Children() as List<VisualElement>;

            if (toolbarContainerContent.childCount < 3)
            {
                ToolbarZoneLeftAlign = new VisualElement();
                ToolbarZonePlayMode = new VisualElement();
                ToolbarZoneRightAlign = new VisualElement();
            }
            else
            {
                ToolbarZoneLeftAlign = toolbarZoneSet[0];
                ToolbarZonePlayMode = toolbarZoneSet[1];
                ToolbarZoneRightAlign = toolbarZoneSet[2];
            }
        }
    }
}