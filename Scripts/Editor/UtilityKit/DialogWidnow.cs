using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Cofdream.ToolKitEditor
{
    // todo 实现一个自定义的 弹窗提示
    public class DialogWidnow : EditorWindow
    {


        [MenuItem("DialogWidnow/Open")]
        public static void DisplayDialog()
        {
            Display("title");
        }

        private static bool aa;


        private void CreateGUI()
        {
            rootVisualElement.Add(new Label("Hello"));
        }

        private void OnGUI()
        {
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.white;
            GUI.color = Color.white;
            GUILayout.Label("1");


            if (GUILayout.Button("Ok"))
            {
                aa = true;
                Close();
            }
            if (GUILayout.Button("Cancel"))
            {
                aa = false;
                Close();
            }
            if (GUILayout.Button("Open"))
            {
                EditorUtility.DisplayDialog("Title", "Message", "ok", "cancel");
            }
        }

        public static void Display(string title)
        {
            var dialogWidnow = CreateInstance<DialogWidnow>();
            dialogWidnow.titleContent = new GUIContent(title);

            dialogWidnow.maxSize = new Vector2(500, 500);
            dialogWidnow.minSize = new Vector2(100, 100);
            dialogWidnow.ShowModal();
        }
    }
}