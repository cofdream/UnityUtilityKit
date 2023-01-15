using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public static class GUIContentExtension
    {

        private static GUIContent _lockOff;
        private static GUIContent _lockOn;

        private static GUIContent _compiling;

        public static GUIContent LockOff
        {
            get
            {
                if (_lockOff == null) 
                    _lockOff = EditorGUIUtility.IconContent("IN LockButton");
                return _lockOff;
            } 
        }

        public static GUIContent LockOn
        {
            get
            {
                if (_lockOn == null) _lockOn = EditorGUIUtility.IconContent("IN LockButton on");
                return _lockOn;
            }
        }

        public static GUIContent Compiling
        {
            get
            {
                if (_compiling == null) _compiling = new GUIContent("Compiling...");
                return _compiling;
            }
        }

    }
}