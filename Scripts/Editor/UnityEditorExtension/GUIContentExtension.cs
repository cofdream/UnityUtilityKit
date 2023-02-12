using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public static class GUIContentExtension
    {
        public static GUIContent LockOff => EditorGUIUtility.IconContent("IN LockButton");
        public static GUIContent LockOn => EditorGUIUtility.IconContent("IN LockButton on");
        public static GUIContent Compiling => new GUIContent("Compiling...");

    }
}