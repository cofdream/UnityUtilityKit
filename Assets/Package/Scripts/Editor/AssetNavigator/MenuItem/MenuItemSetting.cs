using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Cofdream.NavigatorMenuItem
{
    public class MenuItemSetting : IMenuItem
    {
        public Type type { get; set; }
        public string str { get; set; }
        public bool isIcon { get; set; }

        public string icon;

        public GUIContent GUIContent
        {
            get
            {
                // 临时显示 str 未 toolTips
                var texture2D = EditorGUIUtility.FindTexture(icon);
                if (texture2D != null)
                    return new GUIContent(texture2D, icon);

                return new GUIContent(icon, icon);
            }
        }

        private Type[] types;
        private string[] typeStrings;
        private GUIContent[] typeGUIContents;

        private int popupIndex;

        public void Awake()
        {
            var thisType = this.GetType();
            var baseType = typeof(IMenuItem);
            var assembly = baseType.Assembly;
            types = assembly.GetTypes().Where((type) =>
            {
                return type.BaseType == baseType && type.IsAbstract == false && type != thisType;
            }).ToArray();

            int length = types.Length;
            typeGUIContents = new GUIContent[length];
            typeStrings = new string[length];
            for (int i = 0; i < length; i++)
            {
                var type = types[i];
                typeGUIContents[i] = new GUIContent(type.Name, type.Namespace);
                typeStrings[i] = type.FullName;
            }
        }
        private void OnEnable()
        {

        }

        private void OnDestroy()
        {

        }

        public void Draw()
        {
            EditorGUILayout.BeginHorizontal();
            {
                popupIndex = EditorGUILayout.Popup(popupIndex, typeGUIContents);

                EditorGUILayout.Space(3, false);

                if (GUILayout.Button(" Add MenuItem ", GUILayout.ExpandWidth(false)))
                {
                    Create(types[popupIndex]);
                }
            }
            EditorGUILayout.EndHorizontal();
        }
        private void Create(Type type)
        {

        }


    }
}