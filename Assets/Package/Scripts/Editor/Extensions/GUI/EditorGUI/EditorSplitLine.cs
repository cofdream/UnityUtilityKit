using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    [System.Serializable]
    public class EditorSplitLine
    {
        [System.NonSerialized] private bool isDrag;
        public float Vertical(float x, float y, float min, float max, float height, float space = 1f, float padding = 1f)
        {
            var line = new Rect(x, y, space, height);
            EditorGUI.DrawRect(line, new Color(0, 0, 0, 0.5f));

            line.x -= padding;
            line.width = space + padding + padding;

            EditorGUIUtility.AddCursorRect(line, MouseCursor.SplitResizeLeftRight);


            var e = Event.current;
            var mousePosition = e.mousePosition;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (line.Contains(mousePosition) && e.button == 0)
                    {
                        isDrag = true;
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    if (isDrag && e.button == 0)
                    {
                        isDrag = false;
                        e.Use();
                    }
                    break;

                case EventType.MouseDrag:
                    if (isDrag)
                    {
                        x = Mathf.Clamp(mousePosition.x, min, max);
                        e.Use();
                    }
                    break;
            }

            return x;
        }

        public float Horizontal(float x, float y, float min, float max, float width, float space = 1f, float padding = 1f)
        {
            Rect line = new Rect(x, y, width, space);
            EditorGUI.DrawRect(line, new Color(0, 0, 0, 0.5f));

            line.height = space + padding + padding;
            line.y -= padding;

            EditorGUIUtility.AddCursorRect(line, MouseCursor.SplitResizeUpDown);

            var e = Event.current;
            var mousePosition = e.mousePosition;
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (line.Contains(mousePosition) && e.button == 0)
                    {
                        isDrag = true;
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    if (isDrag && e.button == 0)
                    {
                        isDrag = false;
                        e.Use();
                    }
                    break;

                case EventType.MouseDrag:
                    if (isDrag)
                    {
                        y = Mathf.Clamp(mousePosition.y, min, max);
                        e.Use();
                    }
                    break;
            }

            return y;
        }
    }


    /* 测试
    public class TT : EditorWindow
    {
        [MenuItem("Test22/Oepdasd")]
        static void Open() => GetWindow<TT>().Show();


        this this;
        private void OnEnable()
        {
            this = new this();
            this.Init(75, 50, 100, position.height);
        }


        private void OnGUI()
        {
            this.Length = position.width;
            CEditorGUILayout.SpliteHorizontal(this);
            CEditorGUILayout.SplitVertical(this);

        }
    }
    */

    /* 旧版本

    // 切割视图
    public class GUIthis
    {
        public Vector2 ViewPosition;

        public float X;
        public float Y;
        public float Width;
        public float Height;

        public float MinWidth;
        public float MinHeight;

        public float MaxWidth;
        public float MaxHeight;

        public bool IsSplitHorizontal;
        public bool IsSplitVertiacal;

        public bool ResizeHorizontal;
        public bool ResizeVertical;

        //public float LineSpace;
        //public float LineSpaceOffest;
        //public float lineWidth;

        public GUIthis(float x, float y, float width, float height, float minWidth, float minHeight, float maxWidth, float maxHeight, bool isSplitHorizontal, bool isSplitVertiacal)
        {
            ViewPosition = Vector2.zero;

            X = x;
            Y = y;
            Width = width;
            Height = height;

            MinWidth = minWidth;
            MinHeight = minHeight;
            MaxWidth = maxWidth;
            MaxHeight = maxHeight;

            IsSplitHorizontal = isSplitHorizontal;
            IsSplitVertiacal = isSplitVertiacal;

            ResizeHorizontal = false;
            ResizeVertical = false;

            //LineSpace = 5;
            //LineSpaceOffest = 2;
            //lineWidth = 1;
        }
    }
    public static class GUISplitView
    {

        #region 整合ScroolView API

        public static void BeginSplitView(ref GUIthis data, params GUILayoutOption[] options)
        {
            BeginSplitView(ref data, false, false, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
        }

        public static void BeginSplitView(ref GUIthis data, bool alwaysShowHorizontal, bool alwaysShowVertical, params GUILayoutOption[] options)
        {
            BeginSplitView(ref data, alwaysShowHorizontal, alwaysShowVertical, GUI.skin.horizontalScrollbar, GUI.skin.verticalScrollbar, GUI.skin.scrollView, options);
        }

        public static void BeginSplitView(ref GUIthis data, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
        {
            BeginSplitView(ref data, alwaysShowHorizontal: false, alwaysShowVertical: false, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
        }


        public static void BeginSplitView(ref GUIthis data, GUIStyle style)
        {
            BeginSplitView(ref data, style, options: null);
        }

        public static void BeginSplitView(ref GUIthis data, GUIStyle style, params GUILayoutOption[] options)
        {
            string name = style.name;
            GUIStyle gUIStyle = GUI.skin.FindStyle(name + "VerticalScrollbar");
            if (gUIStyle == null)
            {
                gUIStyle = GUI.skin.verticalScrollbar;
            }

            GUIStyle gUIStyle2 = GUI.skin.FindStyle(name + "HorizontalScrollbar");
            if (gUIStyle2 == null)
            {
                gUIStyle2 = GUI.skin.horizontalScrollbar;
            }

            BeginSplitView(ref data, alwaysShowHorizontal: false, alwaysShowVertical: false, gUIStyle2, gUIStyle, style, options);
        }

        public static void BeginSplitView(ref GUIthis data, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, params GUILayoutOption[] options)
        {
            BeginSplitView(ref data, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, GUI.skin.scrollView, options);
        }

        #endregion

        public static void BeginSplitView(ref GUIthis data, bool alwaysShowHorizontal, bool alwaysShowVertical, GUIStyle horizontalScrollbar, GUIStyle verticalScrollbar, GUIStyle background, params GUILayoutOption[] options)
        {
            GUILayoutOption[] newOptions;
            if (options != null)
            {
                newOptions = new GUILayoutOption[options.Length + 2];
                options.CopyTo(newOptions, 2);
            }
            else
                newOptions = new GUILayoutOption[2];


            newOptions[0] = GUILayout.Width(data.Width);
            newOptions[1] = GUILayout.Height(data.Height);

            data.ViewPosition = GUILayout.BeginScrollView(data.ViewPosition, alwaysShowHorizontal, alwaysShowVertical, horizontalScrollbar, verticalScrollbar, background, newOptions);
        }

        public static void EndSplitView(ref GUIthis data)
        {
            GUILayout.EndScrollView();
            ResizeSplitView(ref data);
        }

        private static void ResizeSplitView(ref GUIthis data)
        {
            float lineSpace = 5f;
            float lineSpaceOffest = 2f; //=lineSpace * 0.5 - 线段的宽度
            Color lineColor = new Color(0, 0, 0, 0.5f);


            //Rect resizeUpLeft = new Rect(data.X + data.Width - lineSpaceOffest, data.Y + data.Height - lineSpaceOffest, lineSpace, lineSpace);
            //EditorGUIUtility.AddCursorRect(resizeUpLeft, MouseCursor.ResizeUpLeft);

            //if (Event.current.type == EventType.MouseDown && resizeUpLeft.Contains(Event.current.mousePosition))
            //{
            //    data.ResizeVertical = true;
            //    data.ResizeHorizontal = true;
            //}

            //if (Event.current.type == EventType.MouseUp)
            //{
            //    data.ResizeVertical = false;
            //    data.ResizeHorizontal = false;
            //}


            if (data.IsSplitHorizontal)
            {
                Rect splitResizeLeftRightRect = new Rect(data.X + data.Width - lineSpaceOffest, data.Y, lineSpace, data.Height);
                EditorGUIUtility.AddCursorRect(splitResizeLeftRightRect, MouseCursor.SplitResizeLeftRight);

                //EditorGUI.DrawRect(splitResizeLeftRightRect, new Color(1, 1, 1, 0.1f));

                Rect lineRect = splitResizeLeftRightRect;
                lineRect.x += lineSpaceOffest;
                lineRect.width = 1f;
                EditorGUI.DrawRect(lineRect, lineColor);



                if (Event.current.type == EventType.MouseDown && splitResizeLeftRightRect.Contains(Event.current.mousePosition))
                    data.ResizeHorizontal = true;

                if (Event.current.type == EventType.MouseUp)
                    data.ResizeHorizontal = false;

                if (data.ResizeHorizontal && Event.current.type == EventType.MouseDrag)
                {
                    data.Width = Event.current.mousePosition.x;
                    if (data.Width > data.MaxWidth)
                    {
                        data.Width = data.MaxWidth;
                    }
                    else if (data.Width < data.MinWidth)
                    {
                        data.Width = data.MinWidth;
                    }
                }
            }

            if (data.IsSplitVertiacal)
            {
                Rect splitResizeUpDownRect = new Rect(data.X, data.Y + data.Height - lineSpaceOffest, data.Width, lineSpace);
                EditorGUIUtility.AddCursorRect(splitResizeUpDownRect, MouseCursor.ResizeVertical);



                //EditorGUI.DrawRect(splitResizeUpDownRect, new Color(1, 1, 1, 0.1f));

                Rect lineRect = splitResizeUpDownRect;
                lineRect.y += lineSpaceOffest;
                lineRect.height = 1f;
                EditorGUI.DrawRect(lineRect, lineColor);



                if (Event.current.type == EventType.MouseDown && splitResizeUpDownRect.Contains(Event.current.mousePosition))
                    data.ResizeVertical = true;

                if (Event.current.type == EventType.MouseUp)
                    data.ResizeVertical = false;


                if (data.ResizeVertical && Event.current.type == EventType.MouseDrag)
                {
                    EditorGUIUtility.AddCursorRect(splitResizeUpDownRect, MouseCursor.SplitResizeUpDown);

                    data.Height = Event.current.mousePosition.y;
                    if (data.Height > data.MaxHeight)
                    {
                        data.Height = data.MaxHeight;
                    }
                    else if (data.Height < data.MinHeight)
                    {
                        data.Height = data.MinHeight;
                    }
                }
            }


        }

        //todo Check GUIthis 的范围，保证在窗口内
    }
    */

    /* 参考 分割线

    public class GUISplitter : EditorWindow
    {
        [MenuItem("GUI/GUISplitter")]
        static void Init()
        {
            GetWindow<GUISplitter>().Show();
        }


        EditorGUISplitView horizontalSplitView = new EditorGUISplitView(EditorGUISplitView.Direction.Horizontal);
        EditorGUISplitView verticalSplitView = new EditorGUISplitView(EditorGUISplitView.Direction.Vertical);

        //[MenuItem("Editor GUISpitView/Show Example Window")]
        //static void Init()
        //{
        //	EditorGUISplitViewExampleWindow window = (EditorGUISplitViewExampleWindow)GetWindow(typeof(EditorGUISplitViewExampleWindow));
        //	window.Show();
        //}

        public void OnGUI()
        {
            horizontalSplitView.BeginSplitView();
            {
                DrawView1();

                horizontalSplitView.Split();

                verticalSplitView.BeginSplitView();
                {
                    DrawView2();
                    verticalSplitView.Split();
                    DrawView2();

                }
                verticalSplitView.EndSplitView();
            }
            horizontalSplitView.EndSplitView();

            Repaint();
        }

        void DrawView1()
        {
            EditorGUILayout.LabelField("A label");
            GUILayout.Button("A Button");
            EditorGUILayout.Foldout(false, "A Foldout");
        }

        void DrawView2()
        {
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            GUILayout.BeginVertical();
            GUILayout.FlexibleSpace();
            GUILayout.Label("Centered text");
            GUILayout.FlexibleSpace();
            GUILayout.EndVertical();
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
    }

    public class EditorGUISplitView
    {
        public enum Direction
        {
            Horizontal,
            Vertical
        }

        Direction splitDirection;
        float splitNormalizedPosition;
        bool resize;
        public Vector2 scrollPosition;
        Rect availableRect;


        public EditorGUISplitView(Direction splitDirection)
        {
            splitNormalizedPosition = 0.5f;
            this.splitDirection = splitDirection;
        }

        public void BeginSplitView()
        {
            Rect tempRect;

            if (splitDirection == Direction.Horizontal)
                tempRect = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
            else
                tempRect = EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));

            if (tempRect.width > 0.0f)
            {
                availableRect = tempRect;
            }

            if (splitDirection == Direction.Horizontal)
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(availableRect.width * splitNormalizedPosition));
            else
                scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(availableRect.height * splitNormalizedPosition));
        }

        public void Split()
        {
            GUILayout.EndScrollView();
            ResizeSplitFirstView();
        }

        public void EndSplitView()
        {

            if (splitDirection == Direction.Horizontal)
                EditorGUILayout.EndHorizontal();
            else
                EditorGUILayout.EndVertical();
        }

        private void ResizeSplitFirstView()
        {

            Rect resizeHandleRect;

            if (splitDirection == Direction.Horizontal)
                resizeHandleRect = new Rect(availableRect.width * splitNormalizedPosition, availableRect.y, 2f, availableRect.height);
            else
                resizeHandleRect = new Rect(availableRect.x, availableRect.height * splitNormalizedPosition, availableRect.width, 2f);

            Rect linRect = resizeHandleRect;
            if (splitDirection == Direction.Horizontal)
                linRect.width = 1f;
            else
                linRect.height = 1f;
            EditorGUI.DrawRect(linRect, new Color(0, 0, 0, 0.5f));


            if (splitDirection == Direction.Horizontal)
                EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeHorizontal);
            else
                EditorGUIUtility.AddCursorRect(resizeHandleRect, MouseCursor.ResizeVertical);

            if (Event.current.type == EventType.MouseDown && resizeHandleRect.Contains(Event.current.mousePosition))
            {
                resize = true;
            }
            if (resize)
            {
                if (splitDirection == Direction.Horizontal)
                    splitNormalizedPosition = Event.current.mousePosition.x / availableRect.width;
                else
                    splitNormalizedPosition = Event.current.mousePosition.y / availableRect.height;
            }
            if (Event.current.type == EventType.MouseUp)
                resize = false;
        }
    }

     */
}
