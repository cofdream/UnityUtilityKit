using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace Cofdream.ToolKitEditor
{
    public class StringExtensionEditorWdinow : EditorWindow
    {
        public string ToolPath => "字符串";

        [MenuItem("ToolKit/String Extension")]
        static void OpenWindow()
        {
            EditorWindowExtension.GetWindowInCenter<StringExtensionEditorWdinow>().Show();
        }


        public void AwakeTool() { }
        public void OnDestroyTool() { }

        public void OnGUITool(Rect rect)
        {
            if (GUILayout.Button("把替换粘贴板路径的 反斜杠 替换为为 斜杠", GUILayout.ExpandWidth(false)))
            {
                EditorGUIUtility.systemCopyBuffer = ReplaceBackslash(EditorGUIUtility.systemCopyBuffer);
            }
        }

        private void OnGUI()
        {
           
        }

        private static string ReplaceBackslash(string content)
        {
            return content.Replace('\\', '/');
        }

        public void OnEnableTool()
        {
            throw new System.NotImplementedException();
        }

        public void OnDisableTool()
        {
            throw new System.NotImplementedException();
        }
    }
}