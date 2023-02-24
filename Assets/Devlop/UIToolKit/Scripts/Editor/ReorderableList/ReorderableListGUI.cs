using Cofdream.ToolKitEditor;
using Develop;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;


public class ExpandWindow : EditorWindowPlus
{
    [MenuItem(MenuItemName.ToolKit + "ReorderableListGUI Window")]
    private static void Open()
    {
        GetWindow<ExpandWindow>().Show();
    }


    private ReorderableListGUI _reorderableListGUI;

    private void Awake()
    {
        _reorderableListGUI = new ReorderableListGUI();
    }
    private void Enable()
    {
        _reorderableListGUI = new ReorderableListGUI();
    }

    private void OnDisable()
    {
        _reorderableListGUI = null;
    }
    private void OnDestroy()
    {
        _reorderableListGUI = null;
    }

    private void OnGUI()
    {
        _reorderableListGUI.OnGUI();

    }
}

[System.Serializable]
public class ReorderableListGUI
{
    //public readonly GUIStyle emptyHeaderBackground = "RL Empty Header";
    //public readonly GUIStyle footerBackground = "RL Footer";
    //public readonly GUIStyle boxBackground = "RL Background";

    private UnityEditorInternal.ReorderableList _reorderableList = new UnityEditorInternal.ReorderableList(new List<int>() { 1, 2, 3 }, typeof(int));


    public void OnGUI()
    {
        var rect = new Rect(0, 0, 100, 100);

        try
        {
            _reorderableList.DoLayoutList();

            //UnityEditorInternal.ReorderableList.defaultBehaviours.DrawFooter(rect, _reorderableList);
        }
        catch (System.Exception e)
        {

            Debug.Log(e);
        }
        

        //rect.size = new Vector2(100, 20);
        //if (Event.current.type == EventType.Repaint)
        //    emptyHeaderBackground.Draw(rect, false, false, false, false);

        ////rect.y += 20 + rect.height;
        ////rect.size = new Vector2(100, 20);
        ////if (Event.current.type == EventType.Repaint)
        ////    footerBackground.Draw(rect, false, false, false, false);

        //rect.y += 20 + rect.height;
        //if (Event.current.type == EventType.Repaint)
        //    boxBackground.Draw(rect, false, false, false, false);
    }
}
