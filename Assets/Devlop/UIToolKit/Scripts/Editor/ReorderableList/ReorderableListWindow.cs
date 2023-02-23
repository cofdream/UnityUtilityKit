using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cofdream.ToolKitEditor;
using UnityEngine.UIElements;

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

    public class ReorderableList : VisualElement
    {
        // Factory class, required to expose this custom control to UXML
        public new class UxmlFactory : UxmlFactory<ReorderableList, UxmlTraits> { }

        // Traits class
        public new class UxmlTraits : VisualElement.UxmlTraits { }

        // Use CustomStyleProperty<T> to fetch custom style properties from USS
        static readonly CustomStyleProperty<Color> S_GradientFrom = new CustomStyleProperty<Color>("--gradient-from");
        static readonly CustomStyleProperty<Color> S_GradientTo = new CustomStyleProperty<Color>("--gradient-to");

        // Image child element and its texture
        Texture2D m_Texture2D;
        Image m_Image;

        Label _name;
        TextField _input;
        Foldout title;

        VisualElement ItemsRoot;

        public ReorderableList()
        {
            // Create an Image and a texture for it. Attach Image to self.
            m_Texture2D = new Texture2D(100, 100);
            m_Image = new Image();
            m_Image.image = m_Texture2D;
            //Add(m_Image);

            RegisterCallback<CustomStyleResolvedEvent>(OnStylesResolved);


            title = new Foldout()
            {
                text = "Dictionary",
            };

            Add(title);

            ItemsRoot = new VisualElement();
            Add(ItemsRoot);
            ItemsRoot.style.display = DisplayStyle.None;

            DrawList(ItemsRoot);

            title.RegisterValueChangedCallback((value) =>
            {
                if (value.newValue)
                {
                    ItemsRoot.style.display = DisplayStyle.Flex;
                }
                else
                {
                    ItemsRoot.style.display = DisplayStyle.None;
                }
            });


            var nextVE = new Label("--------------------------------");
            Add(nextVE);
        }

        public readonly GUIStyle draggingHandle = "RL DragHandle";
        public readonly GUIStyle headerBackground = "RL Header";
        public readonly GUIStyle emptyHeaderBackground = "RL Empty Header";
        public readonly GUIStyle footerBackground = "RL Footer";
        public readonly GUIStyle boxBackground = "RL Background";
        public readonly GUIStyle preButton = "RL FooterButton";
        public readonly GUIStyle elementBackground = "RL Element";
        private void DrawList(VisualElement root)
        {
            List<GUIStyle> listSS = new List<GUIStyle>()
            {
                draggingHandle,
                headerBackground ,
                emptyHeaderBackground,
                footerBackground ,
                boxBackground ,
                preButton ,
                elementBackground ,
            };

            foreach (var item in listSS)
            {
                var bg = new Image();

                bg.image = item.normal.background;

                bg.style.width = 30f;
                bg.style.height = 30f;

                root.Add(bg);
            }

            _name = new Label("ReorderableList");
            root.Add(_name);
            _input = new TextField();
            root.Add(_input);
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
            return;
            var list = new ReorderableList();
            this.rootVisualElement.Add(list);
        }

        public readonly GUIStyle draggingHandle = "RL DragHandle";
        public readonly GUIStyle headerBackground = "RL Header";
        public readonly GUIStyle emptyHeaderBackground = "RL Empty Header";
        public readonly GUIStyle footerBackground = "RL Footer";
        public readonly GUIStyle boxBackground = "RL Background";
        public readonly GUIStyle preButton = "RL FooterButton";
        public readonly GUIStyle elementBackground = "RL Element";

        private void OnGUI()
        {
            List<GUIStyle> listSS = new List<GUIStyle>()
            {
                draggingHandle,
                headerBackground ,
                emptyHeaderBackground,
                footerBackground ,
                boxBackground ,
                preButton ,
                elementBackground ,
            };

            var rect = new Rect(10, 0, 100, 200);

            foreach (var item in listSS)
            {

                item.Draw(rect, false, false, false, false);

                var rect2 = rect;
                rect2.x += 120;
                rect2.width = 500;
                GUI.Label(rect2, item.ToString());

                rect.y += 120;
            }
        }
    }
}