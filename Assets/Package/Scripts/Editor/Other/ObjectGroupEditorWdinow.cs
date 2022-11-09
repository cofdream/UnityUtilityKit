using Cofdream.ToolKitEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

namespace Cofdream.Test.Editor
{
    // 保存一些选中的对象
    public class ObjectGroupEditorWdinow : EditorWindowPlus
    {

        [MenuItem("Test/Open Group")]
        static void OpenWidnow()
        {
            GetWindow<ObjectGroupEditorWdinow>("ObjectGroup", typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow")).Show();
        }

        private EditorSearchField searchField;
        private string searchText;

        private Vector2 scroolViewPosition;

        private List<UnityEngine.Object> objects;

        private List<Group> groups;

        private string groupName;

        private void Awake()
        {
            groups = new List<Group>();
            objects = new List<UnityEngine.Object>();
        }

        private void OnEnable()
        {
            searchField = new EditorSearchField();

            objects.Clear();
            objects.AddRange(Selection.objects);
        }

        private void OnGUI()
        {
            searchText = searchField.ToolbarSearchField(searchText);

            EditorGUILayout.BeginVertical("box");
            {

                EditorGUILayout.BeginHorizontal();
                {
                    groupName = EditorGUILayout.TextField(groupName);

                    if (GUILayout.Button(" Create ", GUILayout.ExpandWidth(false)))
                    {
                        Debug.Log(groupName);
                        int length = objects.Count;
                        var groupItems = new GroupItem[length];
                        for (int i = 0; i < length; i++)
                        {
                            groupItems[i] = new GroupItem(objects[i]);
                        }
                        groups.Add(new Group(groupName, groupItems));

                        groupName = string.Empty;
                    }
                }
                EditorGUILayout.EndHorizontal();

                foreach (var item in objects)
                {
                    EditorGUILayout.SelectableLabel(item.name, GUILayout.Height(18));
                }
            }
            EditorGUILayout.EndVertical();

            scroolViewPosition = EditorGUILayout.BeginScrollView(scroolViewPosition, false, false, GUIStyle.none, "verticalscrollbar", "scrollview");
            {
                foreach (var item in groups)
                {
                    item.Draw();
                }
            }
            EditorGUILayout.EndScrollView();

            if (removeGroups == null) removeGroups = new List<Group>();
            foreach (var item in removeGroups)
            {
                groups.Remove(item);
            }
            removeGroups.Clear();
        }

        private void OnSelectionChange()
        {
            objects.Clear();
            objects.AddRange(Selection.objects);

            Repaint();
        }

        private List<Group> removeGroups;
        private static void CloseGroup(Group group)
        {
            var wdinow = GetWindow<ObjectGroupEditorWdinow>();
            if (wdinow.removeGroups == null)
            {
                wdinow.removeGroups = new List<Group>();
            }
            wdinow.removeGroups.Add(group);
        }

        [System.Serializable]
        private class Group
        {
            public string groupName;
            public GroupItem[] groupItems;

            private bool foldout;

            public Group(string groupName, GroupItem[] groupItems)
            {
                this.groupName = string.IsNullOrWhiteSpace(groupName) ? "Group" : groupName;
                this.groupItems = groupItems;
                foldout = true;
            }

            public void Draw()
            {
                var rect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.foldoutHeader, GUILayout.ExpandWidth(true));
                rect.width -= 20 + 40;
                foldout = EditorGUI.Toggle(rect, foldout, EditorStyles.foldoutHeader);

                // draw group name
                rect.x += 20;
                EditorGUI.LabelField(rect, groupName);

                // draw  select all



                rect.x = rect.width;
                rect.width = 19;
                if (GUI.Button(rect, "X"))
                {
                    CloseGroup(this);
                    return;
                }
                if (foldout)
                {
                    foreach (var item in groupItems)
                    {
                        item.Draw();
                    }
                }
            }
        }
        [System.Serializable]
        private class GroupItem
        {
            public string path;
            public string assetName;
            public GUIContent GUIContent;

            public GroupItem(UnityEngine.Object obj)
            {
                this.path = AssetDatabase.GetAssetPath(obj);
                assetName = System.IO.Path.GetFileNameWithoutExtension(path);

                GUIContent = new GUIContent(string.Empty, AssetDatabase.GetCachedIcon(path));
            }

            public void Draw()
            {
                EditorGUILayout.BeginHorizontal();
                {
                    //draw icon
                    bool selection = false;
                    var rect = GUILayoutUtility.GetRect(GUIContent, EditorStyles.label, GUILayout.ExpandWidth(true), GUILayout.Height(20));
                    var rect2 = rect;
                    if (rect2.Contains(Event.current.mousePosition))
                    {
                        if (Event.current.type == EventType.MouseDown)
                        {
                            selection = true;
                        }
                    }

                    float width = rect.width;

                    rect.width = 20;
                    EditorGUI.LabelField(rect, GUIContent);

                    // draw name
                    rect.x += 22;
                    rect.width = width;
                    EditorGUI.SelectableLabel(rect, assetName, EditorStyles.label);

                    if (selection)
                    {
                        Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                    }


                    GUILayout.FlexibleSpace();
                }
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}
