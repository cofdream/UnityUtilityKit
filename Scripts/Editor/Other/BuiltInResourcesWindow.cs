using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

namespace Cofdream.ToolKitEditor
{
    public enum ShowType { }

    public class BuiltInWindow : EditorWindow
    {
        [MenuItem("Test/Built-in Window")]
        public static void Open()
        {
            //ScriptableObject.CreateInstance<AAA>().SetCenterPostion(300, 200).ShowUtility();
            GetWindow<BuiltInWindow>().SetCenterPostion(400, 500).Show();
        }

        Texture[] allTextture;

        Vector2 spacing;
        Vector2 scrollViewPosition;
        string searchFilter;
        EditorSearchField searchField;
        RectOffset padding;

        Texture selectTexture;

        float infoHeight;
        float sliderValue;
        float minSliderValue = 1;
        //float maxSliderValue = 5;

        private void OnEnable()
        {
            this.minSize = new Vector2(150, 300);

            searchField = new EditorSearchField();
            padding = new RectOffset(10, 10, 10, 10);
            spacing = new Vector2(3, 3);

            SetSelectTexture(null);
            sliderValue = minSliderValue;

            infoHeight = 150f;


            var type = typeof(EditorGUIUtility);
            var getEditorAssetBundle = type.GetMethod("GetEditorAssetBundle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            AssetBundle assetBundle = getEditorAssetBundle.Invoke(null, null) as AssetBundle;

            allTextture = assetBundle.LoadAllAssets<Texture>();

            //Debug.Log(allTextture2D.Length);
            //foreach (var item in allTextture2D)
            //{
            //    Debug.Log(item.name);
            //}
            //Debug.Log(ToolKits.EditorIcons.ico_list.Length);


            var themList = ToolKits.EditorIcons.ico_list;
            Dictionary<string, int> array01 = new Dictionary<string, int>(themList.Length);
            Dictionary<int, bool> array02 = new Dictionary<int, bool>(themList.Length);
            for (int i = 0; i < themList.Length; i++)
            {
                try
                {
                    array01.Add(themList[i], i);
                    array02.Add(i, false);
                }
                catch (ArgumentException arg)
                {
                    Debug.LogError("重复 " + themList[i]);
                    Debug.LogError(arg);
                }
            }

            foreach (var texture2D in allTextture)
            {
                if (array01.TryGetValue(texture2D.name, out int index))
                {
                    if (array02.TryGetValue(index, out bool isCheck))
                    {
                        if (isCheck)
                        {
                            Debug.Log(texture2D.name + "  重复已经被检测");
                        }
                        else
                        {
                            array02[index] = true;
                        }
                    }
                    else
                    {
                        // Debug.LogError(texture2D.name + "  未添加 check line 57 & 58");
                    }
                }
                else
                {
                    Debug.Log(texture2D.name + "  未添加arry01");
                }
            }

            foreach (var item in array01)
            {
                if (array02[item.Value] == false)
                {
                    //string key = item.Key;
                    string key = string.Empty;

                    var t2d = EditorGUIUtility.IconContent(item.Key);
                    if (t2d.image != null)
                    {
                        Debug.LogWarning(key + "  不在内置ab包.  可 load");
                    }
                    else
                    {
                        //Debug.LogWarning(key + "  不在内置ab包.  ");
                        t2d = EditorGUIUtility.IconContent("d_" + item.Key);
                        if (t2d.image != null)
                        {
                            Debug.LogWarning(key + "  不在内置ab包. 可 load  需要加d_ ");
                        }
                        else
                        {
                            Debug.LogWarning(key + "  不在内置ab包.  ");
                        }
                    }
                }
            }
            //EditorResources.generatedIconsPath
        }

        private void OnGUI()
        {
            //todo 东西多了卡，不滑轮，改成左右按钮翻页。

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                searchFilter = searchField.ToolbarSearchField(searchFilter);
            }
            EditorGUILayout.EndHorizontal();

            {
                //GUILayout.BeginHorizontal(EditorStyles.toolbar);
                //{
                //    GUILayout.Button("xxxx", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));
                //    GUILayout.Button("xxxx", EditorStyles.radioButton, GUILayout.ExpandWidth(false));
                //    GUILayout.Button("% xxxx", EditorStyles.toolbarButton, GUILayout.ExpandWidth(false));

                //    //searchFilter = EditorGUILayout.TextField(searchFilter, EditorStyles.toolbarSearchField);

                //    var searchFilterRect = GUILayoutUtility.GetRect(GUIContent.none, EditorStyles.toolbarSearchField/*, GUILayout.ExpandHeight(false)*/);
                //    searchFilter = EditorGUI.TextField(searchFilterRect, searchFilter, EditorStyles.toolbarSearchField);

                //    //if (searchFilterRect.width > 150)
                //    //{
                //    //    searchFilter = GUIExpand.DrawSearchField(searchFilterRect, searchFilter);
                //    //    if (GUI.changed)
                //    //    {
                //    //        Debug.Log("Text field has changed.");
                //    //        GUI.changed = false;
                //    //        this.Repaint();
                //    //    }
                //    //}
                //    //GUILayout.FlexibleSpace();
                //    //SearchField();
                //}
                //GUILayout.EndHorizontal();
            }

            scrollViewPosition = EditorGUILayout.BeginScrollView(scrollViewPosition);
            {
                EditorGUILayout.Space(padding.top);

                float count = allTextture.Length;

                float contentWidth = position.width - padding.left - padding.right + spacing.x;

                float itemWeight = 50;
                float itemHeight = 50;

                int horizontalCount = (int)(contentWidth / (itemWeight + spacing.x));

                int index = 0;
                while (index < count)
                {
                    EditorGUILayout.BeginHorizontal();
                    {
                        EditorGUILayout.Space(padding.left, false);

                        for (int i = 0; i < horizontalCount; i++)
                        {
                            var item = allTextture[index];

                            Rect rect = GUILayoutUtility.GetRect(itemWeight, itemHeight, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
                            //bg
                            if (EditorGUIUtility.isProSkin)
                                EditorGUI.DrawRect(rect, new Color(64f / 255f, 64f / 255f, 64f / 255f));
                            else
                                EditorGUI.DrawRect(rect, new Color(207f / 255f, 207f / 255f, 207f / 255f));

                            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
                            {
                                SetSelectTexture(item);/*EditorGUIUtility.IconContent(item).image as Texture2D*/;
                                Event.current.Use();
                                //texture2DPopup.editorWindow2 = this;
                                //texture2DPopup.texture2D = allTextture2D[index];
                                //PopupWindow.Show(new Rect(Event.current.mousePosition, new Vector2(allTextture2D[index].width, allTextture2D[index].height)), texture2DPopup);
                            }

                            //item
                            if (item.width > itemWeight || item.height > itemHeight)
                            {
                                rect.width -= 3f;
                                rect.height -= 3f;
                            }
                            else
                            {
                                rect.x += (rect.width - item.width) * 0.5f;
                                rect.y += (rect.height - item.height) * 0.5f;
                                rect.width = item.width;
                                rect.height = item.height;
                            }
                            GUI.DrawTexture(rect, item, ScaleMode.ScaleToFit);



                            index++;
                            if (index == count)
                            {
                                GUILayout.FlexibleSpace();
                                break;
                            }
                            if (i < horizontalCount - 1)
                            {
                                EditorGUILayout.Space(spacing.x, false);
                            }
                        }

                        EditorGUILayout.Space(padding.right, false);
                    }
                    EditorGUILayout.EndHorizontal();

                    if (index < count)
                    {
                        EditorGUILayout.Space(spacing.y, false);
                    }
                }

                #region MyRegion
                //for (int j = 0; j < count;)
                //{
                //    EditorGUILayout.Space(padding.left);

                //    for (int i = 0; i < horizontalCount; i++, j++)
                //    {
                //        if (j == count) break;

                //        var item = smallTexture2D[j];

                //        Vector2 itemSize = new Vector2(itemWeight, itemHeight);
                //        if (i < horizontalCount - 1)
                //        {
                //            itemSize.x = itemWeight + spacing.x;
                //        }

                //        Rect rect;
                //        if (i == 9)
                //        {
                //            rect = GUILayoutUtility.GetRect(itemSize.x, itemSize.y, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
                //        }
                //        else
                //            rect = GUILayoutUtility.GetRect(itemSize.x, itemSize.y, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));

                //        EditorGUI.DrawRect(new Rect(rect.x + 5, rect.y + 5, rect.width - 10, rect.height - 10), Color.white);

                //        rect.x = rect.x + 5;
                //        rect.y = rect.y + 5;
                //        rect.width = item.width;
                //        rect.height = item.height;

                //        GUI.DrawTexture(rect, item, ScaleMode.ScaleToFit);

                //        //EditorGUILayout.LabelField(new GUIContent(item),, GUILayout.Width(65), GUILayout.Height(65));
                //    }

                //    EditorGUILayout.Space(padding.right);
                //} 
                #endregion

                EditorGUILayout.Space(padding.bottom);
            }
            EditorGUILayout.EndScrollView();

            ShowInfo();

            #region MyRegion
            //Rect bottomRect = GUILayoutUtility.GetRect(GUIContent.none, "ProjectBrowserBottomBarBg", GUILayout.Height(21f));
            //GUI.Label(bottomRect, GUIContent.none, "ProjectBrowserBottomBarBg");

            //Rect sliderRect = new Rect(bottomRect.x + bottomRect.width - k_SliderWidth - 16f, rect.y + rect.height - (m_BottomBarRect.height + EditorGUI.kSingleLineHeight) / 2, k_SliderWidth, m_BottomBarRect.height);
            //IconSizeSlider(sliderRect);

            //void IconSizeSlider(Rect r)
            //{
            //    // Slider
            //    EditorGUI.BeginChangeCheck();
            //    int newGridSize = (int)GUI.HorizontalSlider(r, m_ListArea.gridSize, m_ListArea.minGridSize, m_ListArea.maxGridSize);
            //    if (EditorGUI.EndChangeCheck())
            //    {
            //        AssetStorePreviewManager.AbortSize(m_ListArea.gridSize);
            //        m_ListArea.gridSize = newGridSize;
            //    }
            //}

            //if (Event.current.type == EventType.KeyDown)
            //{
            //    Event.current.Use();
            //    Debug.Log(1);
            //}


            //if (allTextture2D == null)
            //{
            //    return;
            //}
            //Vector2 iconSize = new Vector2(40, 40);

            //float iconSpace = 4f;
            //float minLeftSpace = 8f;

            //int maxHorizontalCount = (int)(position.width - minLeftSpace * 2) / 42;

            //// （总长 - 距左右间距 * 2 - （icon宽度 + icon间距）* icon行最大数量）/ 2(剧中所以最后除以2) + 距左右间距 
            //float startX = (position.width - minLeftSpace * 2 - (iconSize.x + iconSpace) * maxHorizontalCount) / 2 + minLeftSpace;

            //Vector2 startPosition = new Vector2(startX, 5f);

            //Rect rect = new Rect(startPosition, iconSize);

            //float viewHeight = startPosition.y + (iconSize.y + iconSpace) * (int)((allTextture2D.Length / maxHorizontalCount) + 0.99);

            //scrollViewPosition = GUI.BeginScrollView(new Rect(0, 0, position.width, position.height), scrollViewPosition, new Rect(0, 0, position.width, viewHeight), false, true);

            ////scrollViewPosition = EditorGUILayout.BeginScrollView(scrollViewPosition);
            //{
            //    int currentHpriazontalCount = 0;
            //    foreach (var item in allTextture2D)
            //    {

            //        var contentItem = EditorGUIUtility.IconContent(item.name);

            //        GUI.Button(rect, contentItem);


            //        //GUI.Box(rect, (Texture2D)null, (GUIStyle)"box");

            //        ////GUI.DrawTextureWithTexCoords(rect, item, new Rect());

            //        //float width = textureItem.width;
            //        //float height = textureItem.height;
            //        //if (width > rect.width || height > rect.height)
            //        //{
            //        //    //StretchToFill  拉伸纹理以填充传递给 GUI.DrawTexture 的完整矩形。
            //        //    //ScaleAndCrop   缩放纹理，保持纵横比，因此它完全覆盖传递给 GUI.DrawTexture 的位置矩形。 如果纹理被绘制到与原始纵横比不同的矩形，则图像将被裁剪。
            //        //    //ScaleToFit     缩放纹理，保持纵横比，使其完全适合传递给 GUI.DrawTexture 的位置矩形。
            //        //    // todo 居中
            //        //    GUI.DrawTexture(new Rect(rect.x, rect.y, rect.width - 4, rect.height - 4), item, ScaleMode.ScaleToFit);

            //        //    //GUI.Box(rect, item, (GUIStyle)"box");
            //        //}
            //        //else
            //        //{
            //        //    // todo 居中
            //        //    GUI.DrawTexture(new Rect(rect.x, rect.y, width, height), item, ScaleMode.ScaleToFit);
            //        //}


            //        currentHpriazontalCount++;

            //        if (currentHpriazontalCount == maxHorizontalCount)
            //        {
            //            currentHpriazontalCount = 0;

            //            rect = new Rect(new Vector2(startX, rect.y + iconSpace + iconSize.x), iconSize);
            //        }
            //        else
            //        {
            //            rect = new Rect(new Vector2(rect.x + iconSize.x + iconSpace, rect.y), iconSize);
            //        }
            //    }
            //}
            ////EditorGUILayout.EndScrollView();
            //GUI.EndScrollView();

            ////GUILayout.Label("Editor window with Popup example", EditorStyles.boldLabel);

            ////if (GUILayout.Button("Popup Options", GUILayout.Width(200)))
            ////{
            ////    PopupWindow.Show(buttonRect, new PopupExample());
            ////}
            ////if (Event.current.type == EventType.Repaint)
            ////    buttonRect = GUILayoutUtility.GetLastRect(); 
            #endregion
        }

        private void ShowInfo()
        {
            if (selectTexture == null) return;

            // todo 参考 sprite做一个
            var bottomRect = EditorGUILayout.BeginVertical("ProjectBrowserBottomBarBg", GUILayout.ExpandWidth(true), GUILayout.Height(infoHeight));
            {
                var topBar = EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true), GUILayout.Height(21f));
                {
                    //EditorGUILayout.Space(0, true);
                    //sliderValue = GUILayout.HorizontalSlider(sliderValue, minSliderValue, maxSliderValue, GUILayout.Width(55f));
                }
                EditorGUILayout.EndHorizontal();


                Rect textureRect = GUILayoutUtility.GetRect(this.position.width, infoHeight, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

                Vector2 textureSize;
                if (selectTexture.width < textureRect.width && selectTexture.height < textureRect.height)
                {
                    textureSize.x = selectTexture.width;
                    textureSize.y = selectTexture.height;

                    textureRect.position = textureRect.position + new Vector2((textureRect.width - textureSize.x) * 0.5f, (textureRect.height - textureSize.y) * 0.5f);
                }
                else
                {
                    textureSize.x = textureRect.width;
                    textureSize.y = textureRect.height;
                }

                GUI.DrawTexture(new Rect(textureRect.position, textureSize), selectTexture, ScaleMode.ScaleToFit);


                EditorGUILayout.LabelField("Size:" + selectTexture.width + "x" + selectTexture.height);
                EditorGUILayout.TextField(new GUIContent("FileName:"), selectTexture.name);

            }
            EditorGUILayout.EndVertical();


            if (Event.current.type == EventType.MouseDown && bottomRect.Contains(Event.current.mousePosition))
            {
                SetSelectTexture(null);
                Event.current.Use();
            }

            //Rect bottomRect = GUILayoutUtility.GetRect(this.position.width, infoHeight, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));


            //// todo drag
            //#region MyRegion
            ////Rect dragRect = new Rect(bottomRect.position, new Vector2(bottomRect.width, 25f));
            ////if (Event.current.type == EventType.MouseDrag && dragRect.Contains(Event.current.mousePosition))
            ////{
            ////    Debug.Log("drag");
            ////}

            ////if (Event.current.type == EventType.MouseDown && bottomRect.Contains(Event.current.mousePosition))
            ////{
            ////    Event.current.Use();
            ////}
            ////else
            ////{

            ////} 
            //#endregion

            ////bg
            //EditorGUI.LabelField(bottomRect, GUIContent.none, "ProjectBrowserBottomBarBg");

            //GUI.DrawTexture(new Rect(bottomRect.position, new Vector2(bottomRect.width, bottomRect.height - 50f)), selectTexture,ScaleMode.ScaleToFit);
        }
        private void SetSelectTexture(Texture texture)
        {
            if (selectTexture != texture)
            {
                selectTexture = texture;
                sliderValue = minSliderValue;
            }
        }
        private void SortIcons()
        {
            // 按照文件夹排序全部资源

        }
    }

    public class AA : PopupWindowContent
    {
        public Texture2D texture2D;
        public EditorWindow editorWindow2;

        public override void OnGUI(Rect rect)
        {
            GUI.DrawTexture(rect, texture2D, ScaleMode.ScaleToFit);

            if (Event.current.type == EventType.MouseMove)
            {
                Debug.Log(1);
                Event.current.Use();
                editorWindow2.Focus();
            }
        }
    }

    public sealed class BuiltInResourcesWindow : EditorWindow
    {
        //[MenuItem("ToolKit/Built-in styles and icons")]
        public static void OpenWindow()
        {
            EditorWindow.GetWindow<BuiltInResourcesWindow>().Show();
        }

        private struct Drawing
        {
            public Rect Rect;
            public Action Draw;
        }

        private List<Drawing> Drawings;

        private List<UnityEngine.Object> _objects;
        private float _scrollPos;
        private float _maxY;
        private Rect _oldPosition;

        private bool _showingStyles = true;
        private bool _showingIcons = false;

        private string _search = "";

        void OnGUI()
        {
            Debug.Log(EditorGUIUtility.pixelsPerPoint);
        }
        void LastOnGUI()
        {
            if (position.width != _oldPosition.width && Event.current.type == EventType.Layout)
            {
                Drawings = null;
                _oldPosition = position;
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Toggle(_showingStyles, "Styles", EditorStyles.toolbarButton) != _showingStyles)
            {
                _showingStyles = !_showingStyles;
                _showingIcons = !_showingStyles;
                Drawings = null;
            }

            if (GUILayout.Toggle(_showingIcons, "Icons", EditorStyles.toolbarButton) != _showingIcons)
            {
                _showingIcons = !_showingIcons;
                _showingStyles = !_showingIcons;
                Drawings = null;
            }

            GUILayout.EndHorizontal();

            string newSearch = GUILayout.TextField(_search);
            if (newSearch != _search)
            {
                _search = newSearch;
                Drawings = null;
            }

            float top = 36;

            if (Drawings == null)
            {
                string lowerSearch = _search.ToLower();

                Drawings = new List<Drawing>();

                GUIContent inactiveText = new GUIContent("inactive");
                GUIContent activeText = new GUIContent("active");

                float x = 5.0f;
                float y = 5.0f;

                if (_showingStyles)
                {
                    foreach (GUIStyle ss in GUI.skin.customStyles)
                    {
                        if (lowerSearch != "" && !ss.name.ToLower().Contains(lowerSearch))
                            continue;

                        GUIStyle thisStyle = ss;

                        Drawing draw = new Drawing();

                        float width = Mathf.Max(
                            100.0f,
                            GUI.skin.button.CalcSize(new GUIContent(ss.name)).x,
                            ss.CalcSize(inactiveText).x + ss.CalcSize(activeText).x
                                          ) + 16.0f;

                        float height = 60.0f;

                        if (x + width > position.width - 32 && x > 5.0f)
                        {
                            x = 5.0f;
                            y += height + 10.0f;
                        }

                        draw.Rect = new Rect(x, y, width, height);

                        width -= 8.0f;

                        draw.Draw = () =>
                        {
                            if (GUILayout.Button(thisStyle.name, GUILayout.Width(width)))
                                CopyText("(GUIStyle)\"" + thisStyle.name + "\"");

                            GUILayout.BeginHorizontal();
                            GUILayout.Toggle(false, inactiveText, thisStyle, GUILayout.Width(width / 2));
                            GUILayout.Toggle(false, activeText, thisStyle, GUILayout.Width(width / 2));
                            GUILayout.EndHorizontal();
                        };

                        x += width + 18.0f;

                        Drawings.Add(draw);
                    }
                }
                else if (_showingIcons)
                {
                    if (_objects == null)
                    {
                        _objects = new List<UnityEngine.Object>(Resources.FindObjectsOfTypeAll(typeof(Texture)));
                        _objects.Sort((pA, pB) => System.String.Compare(pA.name, pB.name, System.StringComparison.OrdinalIgnoreCase));
                    }

                    float rowHeight = 0.0f;

                    foreach (UnityEngine.Object oo in _objects)
                    {
                        Texture texture = (Texture)oo;

                        if (texture.name == "")
                            continue;

                        if (lowerSearch != "" && !texture.name.ToLower().Contains(lowerSearch))
                            continue;

                        Drawing draw = new Drawing();

                        float width = Mathf.Max(
                            GUI.skin.button.CalcSize(new GUIContent(texture.name)).x,
                            texture.width
                        ) + 8.0f;

                        float height = texture.height + GUI.skin.button.CalcSize(new GUIContent(texture.name)).y + 8.0f;

                        if (x + width > position.width - 32.0f)
                        {
                            x = 5.0f;
                            y += rowHeight + 8.0f;
                            rowHeight = 0.0f;
                        }

                        draw.Rect = new Rect(x, y, width, height);

                        rowHeight = Mathf.Max(rowHeight, height);

                        width -= 8.0f;

                        draw.Draw = () =>
                        {
                            if (GUILayout.Button(texture.name, GUILayout.Width(width)))
                                CopyText("EditorGUIUtility.FindTexture( \"" + texture.name + "\" )");

                            Rect textureRect = GUILayoutUtility.GetRect(texture.width, texture.width, texture.height, texture.height, GUILayout.ExpandHeight(false), GUILayout.ExpandWidth(false));

                            // EditorGUI.DrawTextureTransparent(textureRect, texture);

                            try
                            {
                                EditorGUI.DrawTextureTransparent(textureRect, texture);
                            }
                            catch (Exception e)
                            {
                                throw e;
                            }
                            finally
                            {
                                Debug.LogWarning(texture.name);
                            }

                        };

                        x += width + 8.0f;

                        Drawings.Add(draw);
                    }
                }

                _maxY = y;
            }

            Rect r = position;
            r.y = top;
            r.height -= r.y;
            r.x = r.width - 16;
            r.width = 16;

            float areaHeight = position.height - top;
            _scrollPos = GUI.VerticalScrollbar(r, _scrollPos, areaHeight, 0.0f, _maxY);

            Rect area = new Rect(0, top, position.width - 16.0f, areaHeight);
            GUILayout.BeginArea(area);

            int count = 0;
            foreach (Drawing draw in Drawings)
            {
                Rect newRect = draw.Rect;
                newRect.y -= _scrollPos;

                if (newRect.y + newRect.height > 0 && newRect.y < areaHeight)
                {
                    GUILayout.BeginArea(newRect, GUI.skin.textField);
                    draw.Draw();
                    GUILayout.EndArea();

                    count++;
                }
            }

            GUILayout.EndArea();
        }

        void CopyText(string pText)
        {
            TextEditor editor = new TextEditor();

            //editor.content = new GUIContent(pText); // Unity 4.x code
            editor.text = pText; // Unity 5.x code

            editor.SelectAll();
            editor.Copy();
        }
    }


    public class ExampleWindow : EditorWindow
    {
        float sliderValue = 0;
        string labelText = "-";

        [MenuItem("Window/Example Window")]
        static void Init()
        {
            var example = (ExampleWindow)EditorWindow.GetWindow(typeof(ExampleWindow));
            example.Show();
        }

        void OnGUI()
        {
            EditorGUILayout.LabelField("New value", labelText);

            // Start a code block to check for GUI changes
            EditorGUI.BeginChangeCheck();

            sliderValue = EditorGUILayout.Slider(sliderValue, 0, 1);

            // End the code block and update the label if a change occurred
            if (EditorGUI.EndChangeCheck())
            {
                labelText = sliderValue.ToString();
            }
        }
    }
}