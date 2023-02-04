using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace Cofdream.ToolKitEditor
{
    public class Transform2DSort : EditorWindowPlus
    {
        [System.Serializable]
        public class Transform2DSortData /*: ScriptableObject*/
        {
            public Transform Transform;
            public float Offset;
            public Transform2DSortData()
            {
                Offset = 0;
            }
            public Transform2DSortData(Transform transform) : this()
            {
                Transform = transform;
            }
        }

        //[CustomPropertyDrawer(typeof(Transform2DSortData))]
        public class DataEditor : PropertyDrawer
        {
            public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
            {
                position.SplitHorizontal(position.width * 0.5f, out Rect tran, out Rect last);


                var property1 = property.FindPropertyRelative("Transform");
                EditorGUI.PropertyField(tran, property1);


                var property2 = property.FindPropertyRelative("Offset");
                EditorGUI.PropertyField(last, property2);
            }
            public override bool CanCacheInspectorGUI(SerializedProperty property)
            {
                return base.CanCacheInspectorGUI(property);
            }
        }


        [SerializeField] Transform2DSortData[] datas;
        Transform[] transforms;
        SerializedObject serializedObject;
        SerializedProperty serializedProperty;

        bool selectMode;
        GUIContent bySelectionObjectGUIContent;
        GUIContent cacheTitleGUIContent;
        GUIContent byCacheObjectsGUIContent;

        bool isAutoSort;

        float spacing;

        void Awake()
        {
            selectMode = true;
            spacing = 0;

            bySelectionObjectGUIContent = new GUIContent("基于Hierarchy视图内选中的游戏物体");
            cacheTitleGUIContent = new GUIContent("排列对象");
            byCacheObjectsGUIContent = new GUIContent("基于对象集合内的游戏物体");
        }

        void OnEnable()
        {
            serializedObject = new SerializedObject(this);
            serializedProperty = serializedObject.FindProperty("datas");
            serializedProperty.isExpanded = true;

            Selection.selectionChanged += SelectionChange;
        }
        private void OnDisable()
        {
            Selection.selectionChanged -= SelectionChange;
        }


        void OnGUI()
        {
            // 水平 垂直
            {
                var boxRect = EditorGUILayout.BeginVertical("FrameBox");
                {
                    // invoke
                    {
                        EditorGUILayout.LabelField("水平 垂直 排列", EditorStyles.boldLabel);

                        spacing = EditorGUILayout.FloatField("Space", spacing, GUILayout.ExpandWidth(false));

                        EditorGUILayout.BeginHorizontal();
                        {
                            isAutoSort = GUILayout.Toggle(isAutoSort, "Auto Sort");

                            if (isAutoSort || GUILayout.Button("Sort", GUILayout.MaxWidth(70)))
                            {
                                if (transforms != null && transforms.Length > 1)
                                {
                                    Undo.RecordObjects(transforms, "Horizontal Position");
                                    //Sort
                                    Vector3 position = datas[0].Transform.position;
                                    for (int i = 1; i < datas.Length; i++)
                                    {
                                        position += new Vector3(spacing + datas[i].Offset, 0, 0);
                                        datas[i].Transform.position = position;
                                    }
                                }
                            }

                            GUILayout.FlexibleSpace();
                        }
                        EditorGUILayout.EndHorizontal();

                    }

                    GUILayout.Space(8);

                    //set datas
                    {

                        selectMode = GUILayout.Toggle(selectMode, selectMode ? bySelectionObjectGUIContent : byCacheObjectsGUIContent);

                        if (selectMode == false)
                        {
                            serializedObject.Update();
                            EditorGUI.BeginChangeCheck();
                            EditorGUILayout.PropertyField(serializedProperty, cacheTitleGUIContent, true);

                            if (EditorGUI.EndChangeCheck())
                            {
                                serializedObject.ApplyModifiedProperties();

                                transforms = (from d in datas
                                              select d.Transform
                                            ).ToArray();
                            }
                        }

                    }
                }
                EditorGUILayout.EndVertical();
            }
        }

        private void SelectionChange()
        {
            if (selectMode)
            {
                var gos = SelectionExtensions.Components<Transform>();

                datas = (from go in gos
                         select new Transform2DSortData(go)
                         ).ToArray();

                transforms = (from d in datas
                              select d.Transform
                            ).ToArray();
            }
        }
    }
}