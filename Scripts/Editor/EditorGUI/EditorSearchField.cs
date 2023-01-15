using Cofdream.ToolKit;
using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    [Serializable]
    public class EditorSearchField
    {
        private readonly MethodInfo methodInfo_ToolbarSearchField;
        private readonly object[] args1;
        //private static MethodInfo methodInfo_ToolbarSearchField2;
        //private static object[] args2;

        public string SearchTerm;

        public EditorSearchField()
        {
            var type = typeof(EditorGUILayout);
            methodInfo_ToolbarSearchField = type.GetMethod("ToolbarSearchField", BindingFlags.NonPublic | BindingFlags.Static, null, CallingConventions.VarArgs, new Type[] { typeof(string), typeof(GUILayoutOption[]) }, null);
            args1 = new object[2];
        }

        public string ToolbarSearchField(string text, params GUILayoutOption[] options)
        {
            args1[0] = text;
            args1[1] = options;
            return methodInfo_ToolbarSearchField.Invoke(null, args1) as string;
        }

        //internal static string ToolbarSearchField(string text, string[] searchModes, ref int searchMode, params GUILayoutOption[] options)
        //{
        //    //return EditorGUI.ToolbarSearchField(s_LastRect = GUILayoutUtility.GetRect(0f, kLabelFloatMaxW * 1.5f, 18f, 18f, EditorStyles.toolbarSearchField, options), searchModes, ref searchMode, text);
        //}

        public void ToolbarSearchField(params GUILayoutOption[] options)
        {
            args1[0] = SearchTerm;
            args1[1] = options;
            SearchTerm = methodInfo_ToolbarSearchField.Invoke(null, args1) as string;
        }
    }
}