using System.Collections;
using System.Collections.Generic;
using ToolKits;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public static class GUIContentExtension
    {
        public static GUIContent InspectorLock = new GUIContent(EditorGUIUtilityExtensions.LoadIconRequired("InspectorLock"));
        public static GUIContent LockOff => new GUIContent(EditorGUIUtilityExtensions.LoadIconRequired("IN LockButton"));
        public static GUIContent LockOn => new GUIContent(EditorGUIUtilityExtensions.LoadIconRequired("IN LockButton on"));
        public static GUIContent Compiling => new GUIContent("Compiling...");

    }
}