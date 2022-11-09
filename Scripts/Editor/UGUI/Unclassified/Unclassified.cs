using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Cofdream.ToolKitEditor
{
    public class Unclassified : EditorWindowPlus
    {

        private void OnGUI()
        {
            EditorGUILayout.BeginVertical("Framebox");
            {
                EditorGUILayout.LabelField("ChangeRaycast");
                ChangeRaycast();
            }
            EditorGUILayout.EndVertical();

            GUILayout.Space(5);
        }



        private void ChangeRaycast()
        {
            var targets = Selection.gameObjects;
            bool enable = targets.Length == 0;
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUI.BeginDisabledGroup(enable);
                {
                    if (GUILayout.Button("开启全部射线检测"))
                    {
                        Undo.RecordObjects(targets, "Open graphics raycast");
                        foreach (var target in targets)
                        {
                            var graphic = target.GetComponent<Graphic>();
                            graphic.raycastTarget = true;
                        }
                    }
                    if (GUILayout.Button("关闭全部射线检测"))
                    {
                        Undo.RecordObjects(targets, "Close graphics raycast");
                        foreach (var target in targets)
                        {
                            var graphic = target.GetComponent<Graphic>();
                            graphic.raycastTarget = false;
                        }
                    }
                }
                EditorGUI.EndDisabledGroup();
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}