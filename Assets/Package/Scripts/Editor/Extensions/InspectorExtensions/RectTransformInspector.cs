using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    //为什么不实现反射调用 RectTransform 的生命周期函数？
    //使用的 Editor.CreateEditor 创建，生命周期也由编辑器统一管理。
#if COFDREAM_CUSTOM_EDITOR
    [CustomEditor(typeof(RectTransform), true)]
#endif
    [CanEditMultipleObjects]
    internal sealed class RectTransformInspector : Editor
    {
        GUIContent resetAnchoredPostion = new GUIContent(" AP ", (Texture)null, "当前物体的锚点坐标归0");
        //GUIContent resetPosition = new GUIContent(" P ", (Texture)null, "当前物体的本地坐标归0");
        GUIContent resetRotation = new GUIContent(" R ", (Texture)null, "当前物体的本地旋转归0");
        GUIContent resetScale = new GUIContent(" S ", (Texture)null, "当前物体的本地缩放归1");
        GUIContent add = new GUIContent("+");//EditorGUIUtility.IconContent("Toolbar Plus");
        GUIContent reduce = new GUIContent("-");//EditorGUIUtility.IconContent("Toolbar Minus");
        GUIContent refresh = new GUIContent("x"); //EditorGUIUtility.IconContent("Refresh");

        GUILayoutOption btnGLO = GUILayout.ExpandWidth(false);

        GUIContent lRotation;
        GUIContent lScale;
        GUIContent anchorAdaptation; GUILayoutOption[] anchorAdaptationGLOs = new GUILayoutOption[] { GUILayout.Width(23), GUILayout.Height(20) };

        Vector3 rotationValue;
        Vector3 scaleValue;

        bool foldout;

        Editor defaultEditor;

        static readonly Type type = Type.GetType("UnityEditor.RectTransformEditor, UnityEditor");
        MethodInfo methodInfo_OnSceneGUI;

        private void Awake()
        {
            if (defaultEditor == null)
            {
                Editor.CreateCachedEditor(targets, type, ref defaultEditor);
                //defaultEditor = Editor.CreateEditor(targets, type);
            }
            //"ToolHandleGlobal"
            var icon = EditorGUIUtility.FindTexture("ToolHandleLocal");
            lRotation = new GUIContent("Rotation", icon);
            lScale = new GUIContent("Scale", icon);

            anchorAdaptation = new GUIContent(EditorGUIUtility.FindTexture("d_RectTransformBlueprint"), "锚点自适应 （基于父物体来设置)");
        }
        private void OnEnable()
        {
            methodInfo_OnSceneGUI = type.GetMethod("OnSceneGUI", BindingFlags.NonPublic | BindingFlags.Instance);
            var m_OnEnable = type.GetMethod("OnEnable", BindingFlags.NonPublic | BindingFlags.Instance);
            m_OnEnable.Invoke(defaultEditor, null);
        }
        private void OnDestroy()
        {
            DestroyImmediate(defaultEditor);
        }
        private void OnSceneGUI()
        {
            methodInfo_OnSceneGUI.Invoke(defaultEditor, null);
        }
        private void OnDisable()
        {
            var m_OnDisable = type.GetMethod("OnDisable", BindingFlags.NonPublic | BindingFlags.Instance);
            m_OnDisable.Invoke(defaultEditor, null);
        }

        public override void OnInspectorGUI()
        {
            defaultEditor.OnInspectorGUI();

            EditorGUILayout.BeginHorizontal();
            {
                foldout = EditorGUILayout.Toggle(foldout, EditorStyles.foldout, GUILayout.Width(18));
                DrawRestBtn();

                EditorGUILayout.Space(0, true);

                if (GUILayout.Button(anchorAdaptation, anchorAdaptationGLOs))
                {
                    AnchorAdaptation();
                }
            }
            EditorGUILayout.EndHorizontal();
            if (foldout)
            {
                DrawMulitChangeBtn();
            }
        }

        private void DrawRestBtn()
        {

            if (GUILayout.Button(resetAnchoredPostion, btnGLO))
            {
                foreach (var item in targets)
                {
                    var rectTransform = (RectTransform)item;

                    Undo.RecordObject(rectTransform, "Reset anchoredPosition");
                    rectTransform.anchoredPosition = Vector2.zero;
                }
            }

            if (GUILayout.Button(resetRotation, btnGLO))
            {
                foreach (var item in targets)
                {
                    var rectTransform = (RectTransform)item;

                    Undo.RecordObject(rectTransform, "Reset localRotation");
                    rectTransform.localRotation = Quaternion.identity;
                }
            }

            if (GUILayout.Button(resetScale, btnGLO))
            {
                foreach (var item in targets)
                {
                    var rectTransform = (RectTransform)item;

                    Undo.RecordObject(rectTransform, "Reset localScale");
                    rectTransform.localScale = Vector3.one;
                }
            }
        }
        private void DrawMulitChangeBtn()
        {
            EditorGUILayout.BeginHorizontal();
            {
                rotationValue = EditorGUILayout.Vector3Field(lRotation, rotationValue);

                if (GUILayout.Button(add, btnGLO))
                {
                    foreach (var item in targets)
                    {
                        var rectTran = (RectTransform)item;

                        Undo.RecordObject(rectTran, "Add localEulerAngles");
                        rectTran.localEulerAngles += rotationValue;
                    }

                }

                if (GUILayout.Button(reduce, btnGLO))
                {
                    foreach (var item in targets)
                    {
                        var rectTran = (RectTransform)item;

                        Undo.RecordObject(rectTran, "Reduce localEulerAngles");
                        rectTran.localEulerAngles -= rotationValue;
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
                        var rectTran = (RectTransform)item;

                        Undo.RecordObject(rectTran, "Add localScale");
                        rectTran.localScale += scaleValue;
                    }
                }

                if (GUILayout.Button(reduce, btnGLO))
                {
                    foreach (var item in targets)
                    {
                        var rectTran = (RectTransform)item;

                        Undo.RecordObject(rectTran, "Reduce localScale");
                        rectTran.localScale -= scaleValue;
                    }
                }

                if (GUILayout.Button(refresh, btnGLO))
                {
                    scaleValue = Vector3.zero;
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        private void AnchorAdaptation()
        {
            // 锚点自适应
            Undo.RecordObjects(targets, "Anchor Adaptation Count: " + targets.Length);
            foreach (var item in targets)
            {
                var rectTransform = (RectTransform)item;

                //位置信息
                Vector3 partentPos = rectTransform.parent.position;
                Vector3 localPos = rectTransform.position;
                //------获取rectTransform----
                RectTransform partentRect = (RectTransform)rectTransform.parent;
                if (partentRect == null)
                {
                    break;
                }

                float partentWidth = partentRect.rect.width;
                float partentHeight = partentRect.rect.height;
                float localWidth = rectTransform.rect.width * 0.5f;
                float localHeight = rectTransform.rect.height * 0.5f;
                //---------位移差------
                float offX = localPos.x - partentPos.x;
                float offY = localPos.y - partentPos.y;

                float rateW = offX / partentWidth;
                float rateH = offY / partentHeight;
                rectTransform.anchorMax = rectTransform.anchorMin = new Vector2(0.5f + rateW, 0.5f + rateH);
                rectTransform.anchoredPosition = Vector2.zero;

                partentHeight = partentHeight * 0.5f;
                partentWidth = partentWidth * 0.5f;
                float rateX = (localWidth / partentWidth) * 0.5f;
                float rateY = (localHeight / partentHeight) * 0.5f;
                rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x + rateX, rectTransform.anchorMax.y + rateY);
                rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x - rateX, rectTransform.anchorMin.y - rateY);
                rectTransform.offsetMax = rectTransform.offsetMin = Vector2.zero;
            }
        }
    }
}