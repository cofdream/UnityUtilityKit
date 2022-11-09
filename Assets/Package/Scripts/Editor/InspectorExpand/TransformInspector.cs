using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{

#if COFDREAM_CUSTOM_EDITOR
    [CustomEditor(typeof(Transform), true)]
#endif
    [CanEditMultipleObjects]
    internal sealed class TransformInspector : Editor
    {
        GUIContent restPosition = new GUIContent(" P ", (Texture)null, "当前物体的本地坐标归0");
        GUIContent resetRotation = new GUIContent(" R ", (Texture)null, "当前物体的本地旋转归0");
        GUIContent resetScale = new GUIContent(" S ", (Texture)null, "当前物体的本地缩放归1");

        GUIContent add = new GUIContent("+");//EditorGUIUtility.IconContent("Toolbar Plus");
        GUIContent reduce = new GUIContent("-");//EditorGUIUtility.IconContent("Toolbar Minus");
        GUIContent refresh = new GUIContent("x"); //EditorGUIUtility.IconContent("Refresh");
        GUILayoutOption btnGLO = GUILayout.ExpandWidth(false);

        GUIContent lPosition;
        GUIContent lRotation;
        GUIContent lScale;

        Vector3 positionValue;
        Vector3 rotationValue;
        Vector3 scaleValue;

        Editor defaultEditor;

        bool foldout;

        static readonly Type type = Type.GetType("UnityEditor.TransformInspector, UnityEditor");

        private void Awake()
        {
            if (defaultEditor == null)
            {
                Editor.CreateCachedEditor(targets, type, ref defaultEditor);
                //defaultEditor = Editor.CreateEditor(targets, type);
            }
            //"ToolHandleGlobal"
            var icon = EditorGUIUtility.FindTexture("ToolHandleLocal");
            lPosition = new GUIContent("Position", icon);
            lRotation = new GUIContent("Rotation", icon);
            lScale = new GUIContent("Scale", icon);
        }

        private void OnEnable()
        {
            //OnEnable  居然public了
            var m_OnEnable = type.GetMethod("OnEnable", BindingFlags.Public | BindingFlags.Instance);
            m_OnEnable.Invoke(defaultEditor, null);
        }
        private void OnDestroy()
        {
            DestroyImmediate(defaultEditor);
        }

        public override void OnInspectorGUI()
        {
            defaultEditor.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            {
                foldout = EditorGUILayout.Toggle(foldout, EditorStyles.foldout, GUILayout.Width(18));
                DrawRestBtn();
            }
            EditorGUILayout.EndHorizontal();

            if (foldout)
            {
                DrawMulitChangeBtn();
            }
        }

        private void DrawRestBtn()
        {
            if (GUILayout.Button(restPosition, btnGLO))
            {
                foreach (var item in targets)
                {
                    var transform = (Transform)item;

                    Undo.RecordObject(transform, "Reset localPosition");
                    transform.localPosition = Vector3.zero;
                }
            }

            if (GUILayout.Button(resetRotation, btnGLO))
            {
                foreach (var item in targets)
                {
                    var transform = (Transform)item;

                    Undo.RecordObject(transform, "Reset localRotation");
                    transform.localRotation = Quaternion.identity;
                }
            }

            if (GUILayout.Button(resetScale, btnGLO))
            {
                foreach (var item in targets)
                {
                    var transform = (Transform)item;

                    Undo.RecordObject(transform, "Reset localScale");
                    transform.localScale = Vector3.one;
                }
            }

        }
        private void DrawMulitChangeBtn()
        {


            //魔法 别问数值怎么来的，问就是魔法
            if (EditorGUIUtility.currentViewWidth < 330)
            {
                EditorGUIUtility.labelWidth = EditorStyles.label.CalcSize(lPosition).x + EditorGUIUtility.currentViewWidth - 275;
            }
            EditorGUILayout.BeginHorizontal();
            {
                positionValue = EditorGUILayout.Vector3Field(lPosition, positionValue);

                if (GUILayout.Button(add, btnGLO))
                {
                    foreach (var item in targets)
                    {
                        var transform = (Transform)item;

                        Undo.RecordObject(transform, "Add localPosition");
                        transform.localPosition += positionValue;
                    }

                }

                if (GUILayout.Button(reduce, btnGLO))
                {
                    foreach (var item in targets)
                    {
                        var transform = (Transform)item;

                        Undo.RecordObject(transform, "Reduce localPosition");
                        transform.localPosition -= positionValue;
                    }
                }

                if (GUILayout.Button(refresh, btnGLO))
                {
                    rotationValue = Vector3.zero;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                rotationValue = EditorGUILayout.Vector3Field(lRotation, rotationValue);

                if (GUILayout.Button(add, btnGLO))
                {
                    foreach (var item in targets)
                    {
                        var transform = (Transform)item;

                        Undo.RecordObject(transform, "Add localEulerAngles");
                        transform.localEulerAngles += rotationValue;
                    }

                }

                if (GUILayout.Button(reduce, btnGLO))
                {
                    foreach (var item in targets)
                    {
                        var transform = (Transform)item;

                        Undo.RecordObject(transform, "Reduce localEulerAngles");
                        transform.localEulerAngles -= rotationValue;
                    }
                }

                if (GUILayout.Button(refresh, btnGLO))
                {
                    rotationValue = Vector3.zero;
                }
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                scaleValue = EditorGUILayout.Vector3Field(lScale, scaleValue);

                if (GUILayout.Button(add, btnGLO))
                {
                    foreach (var item in targets)
                    {
                        var transform = (Transform)item;

                        Undo.RecordObject(transform, "Add localScale");
                        transform.localScale += scaleValue;
                    }
                }

                if (GUILayout.Button(reduce, btnGLO))
                {
                    foreach (var item in targets)
                    {
                        var transform = (Transform)item;

                        Undo.RecordObject(transform, "Reduce localScale");
                        transform.localScale -= scaleValue;
                    }
                }

                if (GUILayout.Button(refresh, btnGLO))
                {
                    scaleValue = Vector3.zero;
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}