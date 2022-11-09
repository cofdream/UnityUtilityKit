using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public class RectTransformSort : EditorWindowPlus
    {
        [SerializeField] RectTransform[] rectTransforms;
        SerializedObject serializedObject;
        SerializedProperty serializedProperty;

        bool selectMode;
        GUIContent bySelectionObjectGUIContent;
        GUIContent cacheTitleGUIContent;
        GUIContent byCacheObjectsGUIContent;

        //水平 垂直
        float spacing;

        //弧形
        float radius;
        float startAngel;
        float spacingAngel;
        const float startLeftValue = 0f;
        const float startRightValue = 360f;
        const float spacingLeftValue = -360f;
        const float spacingRightValue = 360f;

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
            serializedProperty = serializedObject.FindProperty("rectTransforms");
            serializedProperty.isExpanded = true;
        }


        void OnGUI()
        {
            selectMode = GUILayout.Toggle(selectMode, selectMode ? bySelectionObjectGUIContent : byCacheObjectsGUIContent);

            RectTransform[] targets;
            if (selectMode)
            {
                var gos = Selection.gameObjects;
                targets = (from go in gos
                                  where go.transform as RectTransform != null && go.scene.rootCount > 0
                                  select (RectTransform)go.transform
                         ).ToArray();

                //foreach (var item in targets)
                //{
                //    GUILayout.Label(item.name);
                //}


            }
            else
            {
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedProperty, cacheTitleGUIContent, true);
                serializedObject.ApplyModifiedProperties();

                targets = this.rectTransforms;
            }

            EditorGUILayout.Space(8, false);

            // 水平 垂直
            {
                var boxRect = EditorGUILayout.BeginVertical("FrameBox");
                {
                    EditorGUILayout.LabelField("水平 垂直", EditorStyles.boldLabel);

                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.LabelField("一丨排列间距");

                        spacing = EditorGUILayout.FloatField(spacing, GUILayout.Width(50));
                    }
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    {
                        GUILayoutOption[] buttonOptions = new GUILayoutOption[] { GUILayout.MaxWidth(70) };

                        if (GUILayout.Button(new GUIContent("→", "从左至右排序"), buttonOptions))
                            HorizontalLayoutGroup(targets, true, spacing);

                        if (GUILayout.Button(new GUIContent("←", "从右至左排序"), buttonOptions))
                            HorizontalLayoutGroup(targets, false, spacing);

                        if (GUILayout.Button(new GUIContent("↓", "从上向下排序"), buttonOptions))
                            VerticalLayoutGroup(targets, true, spacing);

                        if (GUILayout.Button(new GUIContent("↑", "从下向上排序"), buttonOptions))
                            VerticalLayoutGroup(targets, false, spacing);
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
            }

            //EditorGUILayout.Space(3, false);

            // 弧形
            {
                EditorGUILayout.BeginVertical("FrameBox");
                {
                    GUILayout.Label("弧形", EditorStyles.boldLabel);

                    radius = EditorGUILayout.FloatField("距离中心点半径", radius);
                    startAngel = EditorGUILayout.Slider("起始角度（0度位置为右中）", startAngel, startLeftValue, startRightValue);
                    spacingAngel = EditorGUILayout.Slider("间隔角度（两个物体之间的）", spacingAngel, spacingLeftValue, spacingRightValue);

                    GUILayout.Label("排列UI位置");

                    if (GUILayout.Button("Sort", GUILayout.ExpandWidth(false)))
                    {
                        RingLayoutGroup(targets, radius, spacingAngel, startAngel);
                    }
                }
                EditorGUILayout.EndVertical();
            }
        }

        void HorizontalLayoutGroup(RectTransform[] targets, bool right, float spacing)
        {
            if (targets != null && targets.Length > 1)
            {
                Undo.RecordObjects(targets, "Horizontal Position");

                bool init = true;
                Vector3 lastPos = Vector3.zero;
                float lastOffestX = 0;
                int direction = right ? 1 : -1;

                int length = targets.Length;
                for (int i = 0; i < length; i++)
                {
                    var target = targets[i];
                    if (target == null) continue;

                    var value = Vector2.one * 0.5f;
                    target.anchorMin = value;
                    target.anchorMax = value;
                    target.pivot = value;

                    if (init)
                    {
                        init = false;
                        lastPos = target.localPosition;
                        lastOffestX = target.rect.width * 0.5f;
                        continue;
                    }

                    //获取坐标偏移
                    float slefOffestX = target.rect.width * 0.5f;
                    lastPos.x += (slefOffestX + lastOffestX) * direction + spacing;

                    target.localPosition = lastPos;
                    //保留下次坐标偏移距离
                    lastOffestX = slefOffestX;
                }
            }
        }
        void VerticalLayoutGroup(RectTransform[] targets, bool down, float spacing)
        {
            if (targets != null && targets.Length > 1)
            {
                Undo.RecordObjects(targets, "Vertical Position");

                bool init = true;
                Vector3 lastPos = Vector3.zero;
                float lastOffestY = 0;
                int direction = down ? -1 : 1;

                int length = targets.Length;
                for (int i = 0; i < length; i++)
                {
                    var target = targets[i];
                    if (target == null) continue;

                    var value = Vector2.one * 0.5f;
                    target.anchorMin = value;
                    target.anchorMax = value;
                    target.pivot = value;

                    if (init)
                    {
                        init = false;
                        lastPos = target.localPosition;
                        lastOffestY = target.rect.height * 0.5f;
                        continue;
                    }

                    //获取坐标偏移
                    float slefOffestY = target.rect.height * 0.5f;
                    lastPos.y += (slefOffestY + lastOffestY) * direction - spacing;

                    target.localPosition = lastPos;
                    //保留下次坐标偏移距离
                    lastOffestY = slefOffestY;
                }
            }
        }

        void RingLayoutGroup(RectTransform[] targets, float radius, float spacingAngel, float startAngel)
        {
            if (targets != null && targets.Length > 0)
            {
                foreach (var target in targets)
                {
                    if (target == null) continue;

                    var temp = Mathf.PI / 180f;
                    float x = radius * Mathf.Cos(startAngel * temp);
                    float y = radius * Mathf.Sin(startAngel * temp);

                    target.anchoredPosition = new Vector3(x, y, 0);

                    startAngel += spacingAngel;
                }
            }
        }
    }
}