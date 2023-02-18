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

        private static List<VisualElement> _delazyToolbarZoneLeftAlign = new List<VisualElement>();
        private static List<VisualElement> _delazyToolbarZonePlayMode = new List<VisualElement>();
        private static List<VisualElement> _delazyToolbarZoneRightAlign = new List<VisualElement>();



        static MainToolbarExtensions()
        {
            EditorApplication.update += Update;
        }

        private static void LoadToobarVisualElement()
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

        private static void Update()
        {
            try
            {
                if (ToolbarZoneLeftAlign == null || ToolbarZonePlayMode == null || ToolbarZoneRightAlign == null)
                {
                    LoadToobarVisualElement();
                }

                if (ToolbarZoneLeftAlign != null && ToolbarZonePlayMode != null && ToolbarZoneRightAlign != null)
                {
                    EditorApplication.update -= Update;

                    foreach (var item in _delazyToolbarZoneLeftAlign)
                    {
                        ToolbarZoneLeftAlign.Add(item);
                    }
                    _delazyToolbarZoneLeftAlign = null;

                    foreach (var item in _delazyToolbarZonePlayMode)
                    {
                        ToolbarZonePlayMode.Add(item);
                    }
                    _delazyToolbarZonePlayMode = null;

                    foreach (var item in _delazyToolbarZoneRightAlign)
                    {
                        ToolbarZoneRightAlign.Add(item);
                    }
                    _delazyToolbarZoneRightAlign = null;
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public static void AddToolbarZoneLeftAlign(VisualElement element)
        {
            if (ToolbarZoneLeftAlign == null)
                _delazyToolbarZoneLeftAlign.Add(element);
            else
                ToolbarZoneLeftAlign.Add(element);
        }
        public static void AddToolbarZonePlayMode(VisualElement element)
        {
            if (ToolbarZonePlayMode == null)
                _delazyToolbarZonePlayMode.Add(element);
            else
                ToolbarZonePlayMode.Add(element);
        }
        public static void AddToolbarZoneRightAlign(VisualElement element)
        {
            if (ToolbarZoneRightAlign == null)
                _delazyToolbarZoneRightAlign.Add(element);
            else
                ToolbarZoneRightAlign.Add(element);
        }
    }
}