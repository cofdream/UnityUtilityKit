using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cofdream.ToolKitEditor;
using UnityEngine.UIElements;
using System.IO;
using System.Security.Cryptography;
using System;
using System.Reflection;

namespace Develop
{
    public class GradientImage : VisualElement
    {
        // Factory class, required to expose this custom control to UXML
        public new class UxmlFactory : UxmlFactory<GradientImage, UxmlTraits> { }

        // Traits class
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        // Use CustomStyleProperty<T> to fetch custom style properties from USS
        static readonly CustomStyleProperty<Color> S_GradientFrom = new CustomStyleProperty<Color>("--gradient-from");
        static readonly CustomStyleProperty<Color> S_GradientTo = new CustomStyleProperty<Color>("--gradient-to");

        // Image child element and its texture
        Texture2D m_Texture2D;
        Image m_Image;



        public GradientImage()
        {
            // Create an Image and a texture for it. Attach Image to self.
            m_Texture2D = new Texture2D(100, 100);
            m_Image = new Image();
            m_Image.image = m_Texture2D;
            Add(m_Image);



            RegisterCallback<CustomStyleResolvedEvent>(OnStylesResolved);
        }

        // When custom styles are known for this control, make a gradient from the colors.
        void OnStylesResolved(CustomStyleResolvedEvent evt)
        {
            Color from, to;

            if (evt.customStyle.TryGetValue(S_GradientFrom, out from)
                && evt.customStyle.TryGetValue(S_GradientTo, out to))
            {
                GenerateGradient(from, to);
            }
        }

        public void GenerateGradient(Color from, Color to)
        {
            for (int i = 0; i < m_Texture2D.width; ++i)
            {
                Color color = Color.Lerp(from, to, i / (float)m_Texture2D.width);
                for (int j = 0; j < m_Texture2D.height; ++j)
                {
                    m_Texture2D.SetPixel(i, j, color);
                }
            }

            m_Texture2D.Apply();
            m_Image.MarkDirtyRepaint();
        }
    }

   
    public class ReorderableListWindow : EditorWindowPlus
    {

        [MenuItem(MenuItemName.ToolKit + "ReorderableList Window")]
        private static void Open()
        {
            GetWindow<ReorderableListWindow>().Show();
        }

        private void CreateGUI()
        {
            var list = new ReorderableList();
            this.rootVisualElement.Add(list);
        }

        //public readonly GUIStyle draggingHandle = "RL DragHandle";
        //public readonly GUIStyle headerBackground = "RL Header";
        //public readonly GUIStyle emptyHeaderBackground = "RL Empty Header";
        //public readonly GUIStyle footerBackground = "RL Footer";
        //public readonly GUIStyle boxBackground = "RL Background";
        //public readonly GUIStyle preButton = "RL FooterButton";
        //public readonly GUIStyle elementBackground = "RL Element";


        private void Awake()
        {
            return;
            index = 2;
            //Create(draggingHandle);
            //Create(headerBackground);
            //Create(emptyHeaderBackground);
            //Create(footerBackground);
            //Create(boxBackground);
            //Create(preButton);
            //Create(elementBackground);
        }

        static int index = 2;
        private void Create(GUIStyle style)
        {
            var path = "Assets/AAA/GUISkin.guiskin";
            var skin = AssetDatabase.LoadAssetAtPath<GUISkin>(path);
            index++;
            skin.customStyles[index] = style;
            //var temp = CreateInstance<TempStyle>();
            //temp.GUIStyle = style;
            //AssetDatabase.DeleteAsset(path + style.name);
            //AssetDatabase.CreateAsset(temp, path + style.name);
        }

        private Vector2 svPosition;

        private void OnGUI()
        {
            return;
            //List<GUIStyle> listSS = new List<GUIStyle>()
            //{
            //    draggingHandle,
            //    headerBackground ,
            //    emptyHeaderBackground,
            //    footerBackground ,
            //    boxBackground ,
            //    preButton ,
            //    elementBackground ,
            //};

            //var rect = new Rect(10, 0, 100, 100);

            //var position = this.position;
            //position.x = 0;
            //position.y = 0;
            //svPosition = GUI.BeginScrollView(position, svPosition, new Rect(0, 0, 10000, 10000));

            //foreach (var item in listSS)
            //{
            //    //if (Event.current.type == EventType.Repaint)
            //    //    item.Draw(rect, false, false, false, false);

            //    if (Event.current.type == EventType.Repaint)
            //        item.Draw(rect, new GUIContent("121212", item.normal.background), false, false, false, false);

            //    var rect2 = rect;
            //    rect2.x += 120;
            //    rect2.width = 500;
            //    GUI.TextField(rect2, item.ToString());

            //    rect2.x += 500;
            //    rect2.width = 100;
            //    DrawTT(item.normal.background, ref rect2);
            //    DrawTT(item.active.background, ref rect2);
            //    DrawTT(item.onNormal.background, ref rect2);
            //    DrawTT(item.onHover.background, ref rect2);
            //    DrawTT(item.onActive.background, ref rect2);
            //    DrawTT(item.focused.background, ref rect2);
            //    DrawTT(item.hover.background, ref rect2);

            //    rect.y += 120;
            //}

            //GUI.EndScrollView();
        }
        private void DrawTT(Texture texture, ref Rect rect)
        {
            if (texture == null) return;

            rect.x += 120;
            GUI.DrawTexture(rect, texture);
        }
    }

}