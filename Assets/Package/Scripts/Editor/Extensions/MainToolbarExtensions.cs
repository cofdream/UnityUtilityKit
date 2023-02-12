using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using UnityEngine.UIElements;
using System.Collections.Generic;
using UnityEditor.UIElements;

namespace Cofdream.ToolKitEditor
{
    [InitializeOnLoad]
    public static class MainToolbarExtensions
    {
        public static VisualElement ToolbarZoneLeftAlign { get; private set; }
        public static VisualElement ToolbarZonePlayMode { get; private set; }
        public static VisualElement ToolbarZoneRightAlign { get; private set; }

        public static List<VisualElement> DelayToolbarZoneLeftAlign { get; private set; }
        public static List<VisualElement> DelayToolbarZonePlayMode { get; private set; }
        public static List<VisualElement> DelayToolbarZoneRightAlign { get; private set; }
        
        static MainToolbarExtensions()
        {
            TryToobarVisualElement();

            if (ToolbarZoneLeftAlign == null || ToolbarZonePlayMode == null || ToolbarZoneRightAlign == null)
            {
                DelayToolbarZoneLeftAlign = new List<VisualElement>();
                DelayToolbarZonePlayMode = new List<VisualElement>();
                DelayToolbarZoneRightAlign = new List<VisualElement>();

                EditorApplication.delayCall += DelayCall;
            }
            else
            {
                DelayCall();
            }
        }

        private static void TryToobarVisualElement()
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

            if (toolbarContainerContent.childCount > 2)
            {
                ToolbarZoneLeftAlign = toolbarZoneSet[0];
                ToolbarZonePlayMode = toolbarZoneSet[1];
                ToolbarZoneRightAlign = toolbarZoneSet[2];
            }
        }

        private static void DelayCall()
        {
            EditorApplication.delayCall -= DelayCall;

            TryToobarVisualElement();

            foreach (var item in DelayToolbarZoneLeftAlign)
            {
                ToolbarZoneLeftAlign.Add(item);
            }
            foreach (var item in DelayToolbarZonePlayMode)
            {
                ToolbarZonePlayMode.Add(item);
            }
            foreach (var item in DelayToolbarZoneRightAlign)
            {
                ToolbarZoneRightAlign.Add(item);
            }

            DelayToolbarZoneLeftAlign = null;
            DelayToolbarZonePlayMode = null;
            DelayToolbarZoneRightAlign = null;
        }
    }
}