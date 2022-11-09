using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Cofdream.Test.Editor
{
    public class SearchDropDownMenu : EditorWindow
    {

        [MenuItem("Test/Open SearchDropDownMenu")]
        static void Open()
        {
            GetWindow<SearchDropDownMenu>().Show();
        }

        SearchField searchField;
        string searchString;

        [SerializeField] string[] menus;

        SerializedObject serializedObject;
        SerializedProperty menusSP;

        List<string> searchDatas;

        private bool select;

        private void Awake()
        {
            searchString = string.Empty;

            searchDatas = new List<string>();
        }

        private void OnEnable()
        {
            searchField = new SearchField();

            serializedObject = new SerializedObject(this);
            menusSP = serializedObject.FindProperty("menus");
        }

        private void OnGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(menusSP);

            serializedObject.ApplyModifiedProperties();

            GUILayout.Space(20);

            searchString = searchField.OnGUI(searchString);

            var rect = GUILayoutUtility.GetRect(150, 200, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

            
            //if (string.IsNullOrWhiteSpace(searchString) == false)
            //{

            //    foreach (var item in menus)
            //    {
            //        if (item.Contains(searchString))
            //        {
            //            searchDatas.Add(item);
            //        }
            //    }

            //    GenericMenu menu = new GenericMenu();

            //    foreach (var item in searchDatas)
            //    {
            //        AddMenuItem(menu, item, false, item);
            //    }

            //    menu.ShowAsContext();
            //}
        }
        void AddMenuItem(GenericMenu menu, string menuPath, bool on, string data)
        {
            menu.AddItem(new GUIContent(menuPath), on, OnSelected, data);
        }

        void OnSelected(object data)
        {
            Debug.Log($"select {data}");
        }

    }
}

public class GenericMenuExample : EditorWindow
{
    // open the window from the menu item Example -> GUI Color
    [MenuItem("Example/GUI Color")]
    static void Init()
    {
        EditorWindow window = GetWindow<GenericMenuExample>();
        window.position = new Rect(50f, 50f, 200f, 24f);
        window.Show();
    }

    // serialize field on window so its value will be saved when Unity recompiles
    [SerializeField]
    Color m_Color = Color.white;

    void OnEnable()
    {
        titleContent = new GUIContent("GUI Color");
    }

    // a method to simplify adding menu items
    void AddMenuItemForColor(GenericMenu menu, string menuPath, Color color)
    {
        // the menu item is marked as selected if it matches the current value of m_Color
        menu.AddItem(new GUIContent(menuPath), m_Color.Equals(color), OnColorSelected, color);
    }

    // the GenericMenu.MenuFunction2 event handler for when a menu item is selected
    void OnColorSelected(object color)
    {
        m_Color = (Color)color;
    }

    void OnGUI()
    {
        // set the GUI to use the color stored in m_Color
        GUI.color = m_Color;

        // display the GenericMenu when pressing a button
        if (GUILayout.Button("Select GUI Color"))
        {
            // create the menu and add items to it
            GenericMenu menu = new GenericMenu();

            // forward slashes nest menu items under submenus
            AddMenuItemForColor(menu, "RGB/Red", Color.red);
            AddMenuItemForColor(menu, "RGB/Green", Color.green);
            AddMenuItemForColor(menu, "RGB/Blue", Color.blue);

            // an empty string will create a separator at the top level
            menu.AddSeparator("");

            AddMenuItemForColor(menu, "CMYK/Cyan", Color.cyan);
            AddMenuItemForColor(menu, "CMYK/Yellow", Color.yellow);
            AddMenuItemForColor(menu, "CMYK/Magenta", Color.magenta);
            // a trailing slash will nest a separator in a submenu
            menu.AddSeparator("CMYK/");
            AddMenuItemForColor(menu, "CMYK/Black", Color.black);

            menu.AddSeparator("");

            AddMenuItemForColor(menu, "White", Color.white);

            // display the menu
            menu.ShowAsContext();
        }
    }
}