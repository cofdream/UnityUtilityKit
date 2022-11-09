using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.EditorTools;

namespace Cofdream.ToolKitEditor
{
    // Tagging a class with the EditorTool attribute and no target type registers a global tool. Global tools are valid for any selection, and are accessible through the top left toolbar in the editor.
    [EditorTool("Platform Tool")]
    class PlatformTool : EditorTool
    {
        // Serialize this value to set a default value in the Inspector.
        [SerializeField]
        Texture2D m_ToolIcon;

        GUIContent m_IconContent;

        void OnEnable()
        {
            m_IconContent = new GUIContent()
            {
                image = m_ToolIcon,
                text = "Platform Tool",
                tooltip = "Platform Tool"
            };
        }

        public override GUIContent toolbarIcon
        {
            get { return m_IconContent; }
        }

        // This is called for each window that your tool is active in. Put the functionality of your tool here.
        public override void OnToolGUI(EditorWindow window)
        {
            var graphic = (GameObject)target;
            if (graphic == null)
            {
                return;
            }
            var rect = graphic.GetComponent<RectTransform>();
            if (rect != null)
            {
                var rect2 = Tools.handleRect;
                rect2.position = Tools.handlePosition;

                using (new Handles.DrawingScope(Color.red))
                {
                    Handles.DrawWireCube(rect2.position, rect2.size);
                }
            }

            return;
            //EditorGUI.BeginChangeCheck();

            //Vector3 position = Tools.handlePosition;

            //using (new Handles.DrawingScope(Color.green))
            //{
            //    position = Handles.Slider(position, Vector3.right);
            //}

            //if (EditorGUI.EndChangeCheck())
            //{
            //    Vector3 delta = position - Tools.handlePosition;

            //    Undo.RecordObjects(Selection.transforms, "Move Platform");

            //    foreach (var transform in Selection.transforms)
            //        transform.position += delta;
            //}
        }
    }
}