using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    public class UndoExtensions : EditorWindow
    {
        [MenuItem("Edit/Clear All Undo History", false, 13)]
        static void ClearAllUndoHistory()
        {
            if (EditorUtility.DisplayDialog("Clear All Undo History", "Do you want to clear all undo history?", "Yes", "No"))
            {
                Undo.ClearAll();
                Debug.Log("Clear All Undo History!");
            }
        }
    }
}