using UnityEngine;
using UnityEditor;
using System;

namespace Cofdream.ToolKitEditor
{
    public class EditorDrag /*: EditorWindow*/
    {
        ////Test
        //[MenuItem("Test/Open Drag")]
        //static void OpenWindow()
        //{
        //    GetWindow<EditorDrag>().Show();
        //}


        //private void OnGUI()
        //{
        //    drag = (DragAndDropVisualMode)EditorGUILayout.EnumPopup(drag);
        //    var rect = GUILayoutUtility.GetRect(200, 200, "box");
        //    EditorGUI.DrawRect(rect, new Color(1, 1, 1, 0.8f / 255f));

        //    DrawDrag(rect);

        //    if (TryGetDragObject(out var obj))
        //    {
        //        Debug.Log("Get drag obj" + obj.name);
        //    }
        //    if (TryGetDragPath(out var path))
        //    {
        //        Debug.Log("Get drag obj" + path);
        //    }
        //}

        DragAndDropVisualMode drag = DragAndDropVisualMode.Link;
        bool dragEnd;

        public void DrawDrag(Rect rect)
        {
            dragEnd = false;
            var e = Event.current;
            switch (e.type)
            {
                case EventType.DragUpdated:

                    if (rect.Contains(e.mousePosition))
                    {
                        DragAndDrop.visualMode = drag;
                        e.Use();
                    }
                    break;
                case EventType.DragPerform:
                    if (rect.Contains(e.mousePosition))
                    {
                        dragEnd = true;
                        DragAndDrop.AcceptDrag();

                        e.Use();
                    }
                    break;
                case EventType.DragExited:
                    if (rect.Contains(e.mousePosition) == false)
                    {
                        e.Use();
                    }
                    break;
            }
        }
        public bool TryGetDragObject(out UnityEngine.Object obj)
        {
            if (dragEnd)
            {
                var objs = DragAndDrop.objectReferences;
                if (objs.Length == 0)
                    obj = null;
                else
                    obj = objs[0];
                return true;
            }
            obj = null;
            return false;
        }
        public bool TryGetDragObjects(out UnityEngine.Object[] objects)
        {
            if (dragEnd)
            {
                objects = DragAndDrop.objectReferences;
                return true;
            }
            objects = null;
            return false;
        }
        public bool TryGetDragPath(out string path)
        {
            if (dragEnd)
            {
                var paths = DragAndDrop.paths;
                if (paths.Length == 0)
                    path = null;
                else
                    path = paths[0];
                return true;
            }
            path = null;
            return false;
        }
        public bool TryGetDragPaths(out string[] paths)
        {
            if (dragEnd)
            {
                paths = DragAndDrop.paths;
                return true;
            }
            paths = null;
            return false;
        }
    }
}