using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    [CustomEditor(typeof(TextAsset))]
    [CanEditMultipleObjects]
    internal class TT : Editor
    {
        private const int kMaxChars = 7000;
        [NonSerialized]
        private GUIStyle m_TextStyle;

        public virtual void OnEnable()
        {
            //alwaysAllowExpansion = true;
        }

        public override void OnInspectorGUI()
        {
            if (m_TextStyle == null)
                m_TextStyle = "ScriptText";

            bool enabledTemp = GUI.enabled;
            GUI.enabled = true;
            TextAsset textAsset = target as TextAsset;
            if (textAsset != null)
            {
                string text = string.Empty;
                if (targets.Length > 1)
                {
                    text = GetPreviewTitle().text;
                }
                else if (Path.GetExtension(AssetDatabase.GetAssetPath(textAsset)) != ".bytes")
                {
                    text = textAsset.text;
                    if (text.Length > kMaxChars)
                        text = text.Substring(0, kMaxChars) + "...\n\n<...etc...>";
                }
                Rect rect = GUILayoutUtility.GetRect(EditorGUIUtility.TrTempContent(text), m_TextStyle);
                rect.x = 0;
                rect.y -= 3;
                //rect.width = GUIClip.visibleRect.width + 1;
                GUI.Box(rect, text, m_TextStyle);
            }
            GUI.enabled = enabledTemp;
        }
    }



    internal class TextInspector : EditorWindow
    {
        [MenuItem("ToolKit/TextInspector")]
        private static void Open()
        {
            GetWindow<TextInspector>(typeof(TextInspector).Name, typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow")).Show();
        }


        private void OnGUI()
        {
            var obj = Selection.activeObject;
            if (obj == null) return;

            EditorGUI.BeginDisabledGroup(true);

            EditorGUILayout.ObjectField(obj, typeof(UnityEngine.Object), false);

            EditorGUI.EndDisabledGroup();


            var path = AssetDatabase.GetAssetPath(obj);

            if (System.IO.File.Exists(path) == false) return;

            var contnet = System.IO.File.ReadAllText(path);
            
            GUI.enabled = true;
            EditorGUI.BeginDisabledGroup(false);
            GUILayout.Box(contnet, "ScriptText");
            EditorGUI.EndDisabledGroup();


        }
    }
}