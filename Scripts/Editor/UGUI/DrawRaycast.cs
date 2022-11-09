using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System;

namespace Cofdream.ToolKitEditor
{
    public class DrawRaycast : EditorWindowPlus
    {
        private static Data _data;

        void OnEnable()
        {
            if (_data == null)
            {
                _data = new Data();

                _data.colorArray = new string[]
                {
                    "Red", "Green", "Blue","white","black", "yellow", "cyan","magenta","gray", "grey", "clear"
                };
                _data.fourCorners = new Vector3[4];
                _data.selectIndex = 2;
                _data.SetColor();
            }
        }

        void OnDestroy()
        {
            _data = null;
        }

        void OnGUI()
        {
            _data.isShow = EditorGUILayout.Toggle("Enable", _data.isShow);

            _data.isPadding = EditorGUILayout.Toggle("Paddind", _data.isPadding);

            _data.selectIndex = EditorGUILayout.Popup(_data.selectIndex, _data.colorArray, GUILayout.ExpandWidth(false));
            if (GUI.changed) _data.SetColor();

            _data.drawColor = EditorGUILayout.ColorField(_data.drawColor, GUILayout.ExpandWidth(false));


            if (_data.isShow && GUI.changed)
            {
                SceneView.lastActiveSceneView.Repaint();
            }
        }


        [DrawGizmo(GizmoType.Selected, typeof(Graphic))]
        static void DrawRayRect(Graphic graphic, GizmoType gizmoType)
        {
            if (_data != null && _data.isShow)
            {
                if (graphic.TryGetComponent(out RectTransform rect))
                {
                    rect.GetWorldCorners(_data.fourCorners);

                    Gizmos.color = _data.drawColor;

                    Vector4 padding = Vector4.zero;
                    if (_data.isPadding)
                    {
#if UNITY_2020_3_OR_NEWER
                        padding = graphic.raycastPadding * rect.lossyScale.x;
#endif
                    }

                    //TODO x y也需要增加

                    Gizmos.DrawLine(_data.fourCorners[0] + new Vector3(padding.x, 0, 0), _data.fourCorners[1] + new Vector3(padding.x, 0, 0));

                    Gizmos.DrawLine(_data.fourCorners[1] + new Vector3(0, -padding.w, 0), _data.fourCorners[2] + new Vector3(0, -padding.w, 0));

                    Gizmos.DrawLine(_data.fourCorners[2] + new Vector3(-padding.z, 0, 0), _data.fourCorners[3] + new Vector3(-padding.z, 0, 0));

                    Gizmos.DrawLine(_data.fourCorners[3] + new Vector3(0, padding.y, 0), _data.fourCorners[0] + new Vector3(0, padding.y, 0));
                }
            }
        }

        [Serializable]
        private class Data
        {
            public bool isShow;
            public string[] colorArray;
            public int selectIndex;
            public Vector3[] fourCorners;
            public Color drawColor;
            public bool isPadding;

            public void SetColor()
            {
                switch (selectIndex)
                {
                    case 0: drawColor = Color.red; break;
                    case 1: drawColor = Color.green; break;
                    case 2: drawColor = Color.blue; break;
                    case 3: drawColor = Color.white; break;
                    case 4: drawColor = Color.black; break;
                    case 5: drawColor = Color.yellow; break;
                    case 6: drawColor = Color.cyan; break;
                    case 7: drawColor = Color.magenta; break;
                    case 8: drawColor = Color.gray; break;
                    case 9: drawColor = Color.grey; break;
                    case 10: drawColor = Color.clear; break;
                }
            }
        }
    }
}