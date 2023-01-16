using Cofdream.NavigatorMenuItem;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditorInternal;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Cofdream.ToolKitEditor.AssetNavigator
{
    [Serializable]
    public struct AssetNavigatorMenuContet : IMenuContet
    {
        public bool IsOn;

        public Object Object;

        private GUIContent _content;
        public GUIContent GUIContent => _content;

        public AssetNavigatorMenuContet(GUIContent content, Object obj)
        {
            IsOn = false;
            _content = content;
            Object = obj;
        }

        public void OnGUI(Rect rect)
        {
            IsOn = EditorGUILayout.ToggleLeft(GUIContent.none, IsOn);
        }
    }

    public class AssetNavigatorEditorWindow : EditorWindowPlus
    {
        ////icon
        //private const string ICON_LOCK =
        //    "iVBORw0KGgoAAAANSUhEUgAAAA0AAAAQCAYAAADNo/U5AAAAtklEQVQoFb2QsQ0CMQxFY6Cgp+EmYA4WgDVYhtuFLViBCagoKY/wXxRHXJDCQcGXvuxvf599sRhj+BaLaqCT3osr8SaexKs4Bpsyd4p38RVo6u5J0UWnBoazuBGXOaKp03dv8OSgImDAa0Q0oF/qs3zsOsfL+Pjg2vup7UOV94PU2l4cxBbo40snmpJB352y8SHfnBswvw2Y2ZZmheIrSWVoyv8N/fwQR/0AL9gCfXwJbPJ8cnwCewTKXVfaQ3EAAAAASUVORK5CYII=";

        ////data
        ////private ActiveObjectData _data;
        //private bool _debug;
        //private static bool isOpen;//the tool window is opened

        ////saveFile
        //private const string SAVE_FILE = "ActiveObjectNavigator.txt";

        ////gui var
        //private bool _clickInnerItem;
        //private Vector2 _scrollPosition;
        //private int _removeIndex = -1;
        //private string _keyWords = "";
        //private bool _showSetting;
        //private bool _showFullPath;

        ////tool bar
        //private Menu _menu;

        //↑ old

        private int menuIndex;
        private IMenuItem[] menuItems;
        private GUIContent[] menuGUIContents;


        [SerializeField]
        private AssetNavigatorData _assetNavigatorData;

        private void Awake()
        {
            _assetNavigatorData = AssetNavigatorData.LoadAsset("Assets/Cofdream/UtilityKit/AssetNavigator/AssetNavigatorData.asset");

            InitNavigatorObjects();
        }

        private void OnDestroy()
        {
            Resources.UnloadAsset(_assetNavigatorData);
            _assetNavigatorData = null;
        }

        private void OnEnable()
        {
            GenerateGUIContents();
        }

        private void OnDisable()
        {
            CustomAssetModificationProcessor.SaveAssetIfDirty(_assetNavigatorData);
        }

        private int aa;
        private void OnGUI()
        {
            if (EditorApplication.isCompiling)
            {
                ShowNotification(GUIContentExtension.Compiling);
                return;
            }

            // 菜单栏
            menuGUIContents = new GUIContent[]
            {
                new GUIContent("All"),
                new GUIContent(EditorGUIUtility.FindTexture("Settings")),
            };

            menuIndex = GUILayout.Toolbar(menuIndex, menuGUIContents, GUI.skin.button, GUI.ToolbarButtonSize.Fixed);


            //搜索栏
            _assetNavigatorData.SearchField.ToolbarSearchField();

            // content
            if (menuItems != null && menuIndex < menuItems.Length)
                menuItems[menuIndex]?.Draw();

            EditorGUILayout.BeginHorizontal();

            aa = EditorGUILayout.IntPopup(aa, new string[] { "1", "2", "3" }, new int[] { 1, 2, 3 });

            EditorGUILayout.EndHorizontal();


            if (EditorGUILayout.DropdownButton(new GUIContent("Popup Options"), FocusType.Passive))
            {
                PopupWindow.Show(_assetNavigatorData.SelectTypePopupWindowRect, new SelectTypePopupWindow<AssetNavigatorMenuContet>(_assetNavigatorData.SelectedObjectDictionary.Dictionary.Values.ToList()));
            }

            if (Event.current.type == EventType.Repaint)
                _assetNavigatorData.SelectTypePopupWindowRect = GUILayoutUtility.GetLastRect();


            foreach (var item in _assetNavigatorData.SelectedObjectDictionary.Dictionary)
            {
                EditorGUILayout.BeginHorizontal();
                {
                    EditorGUILayout.ObjectField(item.Value.GUIContent, item.Value.Object, item.Value.Object.GetType(), false);
                }
                EditorGUILayout.EndHorizontal();
            }


            // ↓ old

            //CheckAsset();
            //if (_showSetting)
            //    DrawSettingPanel();
            //else
            //    DrawContent(); //draw the list

            //Event e = Event.current;
            //EventType type = e.type;
            //switch (type)
            //{
            //    case EventType.DragUpdated:
            //        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
            //        break;
            //    case EventType.DragPerform:
            //        DragAndDrop.AcceptDrag();
            //        if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
            //        {
            //            foreach (var path in DragAndDrop.paths)
            //            {
            //                Debug.Log(path);
            //            }

            //            AppendItems(DragAndDrop.paths);
            //            _scrollPosition = Vector2.zero;
            //            _menu.index = 0; //back to first menu
            //            _keyWords = "";
            //        }

            //        break;
            //}
        }

        protected override void ShowButton(Rect position)
        {
            base.ShowLockButton(ref position);
        }

        private void OnSelectionChange()
        {
            // 缓存选中对象
            if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(Selection.activeInstanceID, out var guid, out long _) == false)
                return;

            if (GUID.TryParse(guid, out var result) == false)
                return;

            var dic = _assetNavigatorData.SelectedObjectDictionary.Dictionary;
            if (dic.ContainsKey(result) == false)
                _assetNavigatorData.SelectedObjectDictionary.Dictionary.Add(result, new AssetNavigatorMenuContet(new GUIContent(Selection.activeObject.name), Selection.activeObject));
            
            EditorUtility.SetDirty(_assetNavigatorData);
            base.Repaint();
        }


        private void InitNavigatorObjects()
        {
            MenuItemSetting setting = new MenuItemSetting();
            setting.icon = "Settings";
            var mm = new MenuItemSetting[]
            {
                new MenuItemSetting(){ type = typeof(Object)                            , str = "All"               },
                new MenuItemSetting(){ type = typeof(MonoScript)      , isIcon = true   , str = "cs Script Icon"    },
                new MenuItemSetting(){ type = typeof(GameObject)      , isIcon = true   , str = "Prefab Icon"       },

                new MenuItemSetting(){ type = typeof(Object)          , isIcon = true   , str = "Settings"          },

                setting,
            };

            foreach (var item in mm)
            {
                item.Awake();
            }
            menuItems = mm;
        }
        private void GenerateGUIContents()
        {
            int length = menuItems.Length;
            if (menuGUIContents == null || menuGUIContents.Length != length)
            {
                menuGUIContents = new GUIContent[length];
            }
            for (int i = 0; i < length; i++)
            {
                menuGUIContents[i] = menuItems[i].GUIContent;
            }
        }



        //private void DrawContent()
        //{
        //    EditorGUILayout.BeginVertical();

        //    int newIndex = GUILayout.Toolbar(_menu.index, _menu.menuItems, GUILayout.Height(20));
        //    if (newIndex != _menu.index)
        //    {
        //        _menu.index = newIndex;
        //        _keyWords = "";
        //    }

        //    GUILayout.Space(5);

        //    //using (new EditorGUILayout.HorizontalScope())
        //    //{
        //    //    _keyWords = EditorGUILayout.TextField("", _keyWords, "SearchTextField");
        //    //    if (GUILayout.Button(GUIContent.none,
        //    //        string.IsNullOrEmpty(_keyWords) ? "SearchCancelButtonEmpty" : "SearchCancelButton"))
        //    //    {
        //    //        _keyWords = "";
        //    //        Repaint();
        //    //    }
        //    //}

        //    GUILayout.Space(5);

        //    _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUIStyle.none,
        //        GUI.skin.verticalScrollbar, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

        //    try
        //    {
        //        //draw list

        //        for (int i = _data.list.Count - 1; i >= 0; i--)
        //        {
        //            var info = _data.list[i];
        //            if (info == null || info.obj == null)
        //            {
        //                _removeIndex = i;
        //                continue;
        //            }

        //            if (!FilterObject(info))
        //                continue;
        //            if (!string.IsNullOrEmpty(_keyWords) && !info.name.ToLower().Contains(_keyWords))
        //                continue;
        //            GUILayout.BeginHorizontal("box");
        //            DrawObject(info, i);
        //            GUILayout.EndHorizontal();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError(e);
        //    }

        //    if (!string.IsNullOrEmpty(_keyWords))
        //    {
        //        EditorGUILayout.Space();
        //        EditorGUILayout.LabelField("按guid搜索：");
        //        var obj = AssetDatabase.LoadAssetAtPath<Object>(AssetDatabase.GUIDToAssetPath(_keyWords));
        //        if (obj != null)
        //        {
        //            GUILayout.BeginHorizontal("box");
        //            var texture = AssetDatabase.GetCachedIcon(AssetDatabase.GUIDToAssetPath(_keyWords));
        //            GUILayout.Label(texture, GUILayout.Width(20), GUILayout.Height(20));
        //            if (GUILayout.Button(obj.name))
        //            {
        //                Selection.activeObject = obj;
        //            }
        //            GUILayout.EndHorizontal();
        //        }
        //        EditorGUILayout.Space();

        //        ulong ticks = 0;
        //        if (ulong.TryParse(_keyWords, out ticks))
        //        {
        //            EditorGUILayout.LabelField("按DataTime计算：");
        //            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //            dt = dt.AddSeconds(ticks);
        //            EditorGUILayout.LabelField(string.Format("UTC0:{0}", dt));

        //            dt = dt.AddHours(8);
        //            EditorGUILayout.LabelField(string.Format("UTC8:{0}", dt));
        //        }
        //    }

        //    EditorGUILayout.EndScrollView();

        //    GUILayout.FlexibleSpace();

        //    GUILayout.Space(10);
        //    GUILayout.Label(string.Format("capacity:{0}/{1}", _data.list.Count, _data.maxCacheCount));

        //    /*
        //    using (new EditorGUILayout.HorizontalScope("box"))
        //    {
        //        GUI.color = _data.isLock ? Color.yellow : Color.white;
        //        string iconName = _data.isLock ? "LockIcon-On" : "LockIcon";
        //        if (GUILayout.Button(EditorGUIUtility.IconContent(iconName), GUILayout.Width(30), GUILayout.Height(20)))
        //        {
        //            _data.isLock = !_data.isLock;
        //            Save();
        //        }

        //        GUI.color = Color.white;

        //        if (GUILayout.Button("More"))
        //        {
        //            _showFullPath = !_showFullPath;
        //        }

        //        if (GUILayout.Button("打开文件夹"))
        //        {
        //            if (Selection.activeObject != null)
        //            {
        //                string path = AssetDatabase.GetAssetPath(Selection.activeObject);
        //                EditorUtility.RevealInFinder(path);
        //            }
        //        }

        //        if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.Width(40)))
        //        {
        //            Clear();
        //        }

        //        if (GUILayout.Button(EditorGUIUtility.IconContent("SettingsIcon"), GUILayout.Width(25),
        //            GUILayout.Height(20)))
        //        {
        //            _showSetting = true;
        //        }
        //    }
        //    */
        //    //	    _data.highlightColor.r = EditorGUILayout.Slider("r", _data.highlightColor.r, 0, 1);
        //    //	    _data.highlightColor.g = EditorGUILayout.Slider("g", _data.highlightColor.g, 0, 1);
        //    //	    _data.highlightColor.b = EditorGUILayout.Slider("b", _data.highlightColor.b, 0, 1);
        //    //        Debug.Log(_data.highlightColor);

        //    EditorGUILayout.EndVertical();

        //    if (_removeIndex > -1)
        //    {
        //        Remove(_removeIndex);
        //        _removeIndex = -1;
        //    }
        //}

        //private void DrawObject(ActiveObjectInfo activeObj, int index)
        //{
        //    GUILayout.Label(activeObj.texture, GUILayout.Width(20), GUILayout.Height(20));
        //    GUI.color = Selection.activeObject == activeObj.obj ? _data.highlightColor : Color.white;
        //    if (!_showFullPath)
        //    {
        //        if (GUILayout.Button(activeObj.name))
        //        {
        //            ActiveObjectHelper.ClearProjectSearchField();
        //            Selection.activeObject = activeObj.obj;
        //            _clickInnerItem = true;
        //            Debug.Log(activeObj.obj.GetType());
        //        }
        //    }
        //    else
        //    {
        //        using (var h = new EditorGUILayout.VerticalScope("Button"))
        //        {
        //            if (GUI.Button(h.rect, GUIContent.none))
        //            {
        //                ActiveObjectHelper.ClearProjectSearchField();
        //                Selection.activeObject = activeObj.obj;
        //                _clickInnerItem = true;
        //            }

        //            GUILayout.Label(activeObj.name);
        //            GUILayout.Label(activeObj.path.Replace("Assets/", ""), GUILayout.MaxWidth(h.rect.width),
        //                GUILayout.ExpandWidth(true));
        //        }
        //    }

        //    GUI.color = Color.white;

        //    string iconName = activeObj.favorite ? "Favorite Icon" : "Favorite";
        //    if (GUILayout.Button(EditorGUIUtility.IconContent(iconName), GUILayout.Width(25), GUILayout.Height(20)))
        //    {
        //        activeObj.favorite = !activeObj.favorite;
        //        Save();
        //    }

        //    if (GUILayout.Button(EditorGUIUtility.IconContent("TreeEditor.Trash"), GUILayout.Width(25),
        //        GUILayout.Height(20)))
        //    {
        //        _removeIndex = index;
        //    }
        //}


        //private bool FilterObject(ActiveObjectInfo info)
        //{
        //    if (_menu.IsAll())
        //    {
        //        return true;
        //    }
        //    else if (_menu.IsFavorite() && info.favorite)
        //    {
        //        return true;
        //    }
        //    else if (_menu.IsElse())
        //    {
        //        return !_data.menu.Contains(info.typeName);
        //    }
        //    else
        //    {
        //        string typeName = null;
        //        if (!_menu.IsAll() && !_menu.IsElse() && !_menu.IsFavorite())
        //        {
        //            if (_data.menu.Count > 0)
        //                typeName = _data.menu[_menu.index - 1];
        //        }
        //        return typeName == info.typeName;
        //    }
        //}


        #region setting panel

        //private Vector2 _settingScrollPos;
        //private string icon = "";
        //List<ActiveObjectType> _typeList;
        //private ReorderableList reorderableList;


        //private void DrawSettingPanel()
        //{
        //    GUILayout.BeginVertical();

        //    using (new EditorGUILayout.HorizontalScope("Box"))
        //    {
        //        GUILayout.Label("Setting");

        //        if (GUILayout.Button(" X ", GUILayout.Width(50)))
        //        {
        //            _showSetting = false;
        //            _menu = new Menu();
        //            _menu.SetData(_data.menu);
        //        }
        //    }

        //    _settingScrollPos = GUILayout.BeginScrollView(_settingScrollPos);

        //    if (reorderableList == null)
        //    {
        //        var allTypes = ActiveObjectConfig.allTypes;
        //        foreach (var t in allTypes)
        //        {
        //            if (_data.menu.Contains(t.typeName))
        //                t.isOn = true;
        //            else
        //                t.isOn = false;
        //        }

        //        reorderableList = new ReorderableList(allTypes, typeof(ActiveObjectType), true, false, false, false);
        //        reorderableList.drawElementCallback = (rect, index, isActive, isFocused) =>
        //        {
        //            var myType = allTypes[index];
        //            bool state = EditorGUI.Toggle(rect, myType.gUIContent, myType.isOn);
        //            if (state != myType.isOn)
        //            {
        //                if (myType.isOn)
        //                {
        //                    _data.menu.Remove(myType.typeName);
        //                    Save();
        //                }
        //                else
        //                {
        //                    _data.menu.Add(myType.typeName);
        //                    Save();
        //                }
        //                myType.isOn = state;
        //            }
        //        };
        //        reorderableList.onReorderCallback = (list) =>
        //        {
        //            _data.menu.Clear();
        //            foreach (var i in list.list)
        //            {
        //                var t = i as ActiveObjectType;
        //                if (t.isOn)
        //                {
        //                    _data.menu.Add(t.typeName);
        //                }
        //            }
        //            Save();
        //        };
        //    }
        //    using (new EditorGUILayout.VerticalScope("Box"))
        //    {
        //        GUILayout.Label("勾选感兴趣的类型：");
        //        reorderableList.DoLayoutList();
        //    }

        //    using (new EditorGUILayout.VerticalScope("Box"))
        //    {
        //        GUILayout.Label(string.Format("设置缓存容量:{0} ({1}-{2})", _data.maxCacheCount, ActiveObjectConfig.MIN_CAPACITY, ActiveObjectConfig.MAX_CAPACITY));
        //        _data.maxCacheCount = (int)GUILayout.HorizontalSlider(_data.maxCacheCount, ActiveObjectConfig.MIN_CAPACITY, ActiveObjectConfig.MAX_CAPACITY);
        //    }

        //    using (new EditorGUILayout.VerticalScope("Box"))
        //    {
        //        try
        //        {
        //            var guiContent = EditorGUIUtility.IconContent(icon);
        //            if (guiContent != null)
        //                icon = EditorGUILayout.TextField(guiContent, icon, GUILayout.Height(25));
        //            else
        //                icon = EditorGUILayout.TextField(icon, GUILayout.Height(25));
        //        }
        //        catch (Exception e)
        //        {
        //        }
        //    }

        //    using (new EditorGUILayout.VerticalScope("Box"))
        //    {
        //        _debug = GUILayout.Toggle(_debug, "打开调试开关");
        //    }
        //    using (new EditorGUILayout.VerticalScope("Box"))
        //    {
        //        if (GUILayout.Button("Open Save File"))
        //        {
        //            EditorUtility.OpenWithDefaultApp(FilePath);
        //        }
        //    }

        //    GUILayout.EndScrollView();


        //    GUILayout.EndVertical();
        //}

        #endregion


        //private void TrackSelectionChange()
        //{
        //    if (_clickInnerItem)
        //    {
        //        _clickInnerItem = false;
        //        return;
        //    }

        //    if (_data.isLock) return;
        //    var curGo = Selection.activeObject;
        //    if (curGo == null)
        //    {
        //        return;
        //    }

        //    if (!EditorUtility.IsPersistent(curGo))
        //    {
        //        return;
        //    }

        //    if (curGo is GameObject)
        //    {
        //        var prefabRoot = PrefabUtility.FindPrefabRoot(curGo as GameObject);
        //        if (prefabRoot != null && prefabRoot != curGo)
        //            curGo = prefabRoot;
        //    }
        //    else if (curGo is Sprite)
        //    {
        //        Sprite sprite = (Sprite)curGo;
        //        if (sprite != null && sprite.texture != null)
        //        {
        //            curGo = sprite.texture;
        //        }
        //    }

        //    AppendItem(curGo);
        //    _scrollPosition = Vector2.zero;
        //    Repaint();
        //}

        //[DidReloadScripts()]
        //static void OnReloadScripts()
        //{
        //    if (isOpen)
        //    {
        //        var window = EditorWindow.GetWindow<ActiveObjectNavigator>();
        //        if (window != null)
        //            window.Repaint();
        //    }
        //}




        //#region handle list

        //private void AppendItems(string[] paths)
        //{
        //    foreach (var path in paths)
        //    {
        //        Object obj = AssetDatabase.LoadAssetAtPath<Object>(path);
        //        if (obj != null)
        //            AppendItem(obj, false);
        //    }

        //    Save();
        //    Repaint();
        //}

        //private void AppendItem(Object obj, bool autoSave = true)
        //{
        //    int index = _data.list.FindLastIndex(i => i.obj == obj);
        //    if (index > -1)
        //    {
        //        if (index == _data.list.Count - 1)
        //            return;
        //        //exist, switch to top place
        //        var info = _data.list[index];
        //        _data.list.RemoveAt(index);
        //        _data.list.Add(info);
        //    }
        //    else
        //    {
        //        string guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(obj));
        //        if (string.IsNullOrEmpty(guid))
        //        {
        //            return;
        //        }

        //        _data.list.Add(new ActiveObjectInfo()
        //        {
        //            guid = guid,
        //            favorite = false,
        //        });
        //    }

        //    //limit capacity
        //    try
        //    {
        //        while (_data.list.Count > _data.maxCacheCount)
        //        {
        //            int removeIndex = _data.list.FindIndex(item => item.favorite == false);
        //            if (removeIndex > -1)
        //                _data.list.RemoveAt(removeIndex);
        //            else
        //                _data.list.RemoveAt(0);
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError(e);
        //    }

        //    if (autoSave) Save();
        //}

        //private void Remove(int index)
        //{
        //    try
        //    {
        //        var info = _data.list[index];
        //        if (Selection.activeObject == info.obj)
        //            Selection.activeObject = null;
        //        _data.list.RemoveAt(index);
        //        Save();
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.LogError(e);
        //        //                _data.list.Clear();
        //        //                Save();
        //    }
        //}

        //private void Clear()
        //{
        //    if (_menu.IsFavorite())
        //    {
        //        bool apply = EditorUtility.DisplayDialog("Warning", "Are you really want to clear [Favorites]?", "Yes", "Cancel");
        //        if (!apply) return;
        //    }
        //    for (int i = _data.list.Count - 1; i >= 0; i--)
        //    {
        //        var item = _data.list[i];
        //        bool valid = false;
        //        if (item != null)
        //        {
        //            if (_menu.IsAll())
        //            {
        //                valid = !item.favorite;
        //            }
        //            else
        //            {
        //                if (_menu.IsFavorite())
        //                    valid = item.favorite;
        //                else
        //                    valid = !item.favorite && FilterObject(item);
        //            }
        //        }
        //        else
        //        {
        //            valid = true;
        //        }

        //        if (valid)
        //            _data.list.RemoveAt(i);
        //    }

        //    Selection.activeObject = null;
        //    Save();
        //}

        //#endregion

        //#region save&load

        //private void CheckAsset()
        //{
        //    if (_data == null || _data.list == null)
        //    {
        //        Load();
        //    }
        //    if (_menu == null)
        //    {
        //        _menu = new Menu();
        //        _menu.SetData(_data.menu);
        //    }
        //    //PrepareBuildInIcons();
        //}

        //private void Save()
        //{
        //    if (EditorApplication.isCompiling) return;
        //    if (_data == null) return;
        //    if (_data.list.Count == 1)
        //    {
        //        string content = ActiveObjectHelper.ReadFile(FilePath);
        //        if (!string.IsNullOrEmpty(content))
        //        {
        //            Load();
        //            return;
        //        }
        //    }
        //    string saveData = EditorJsonUtility.ToJson(_data);
        //    //EditorPrefs.SetString(KEY_ACTIVEOBJECTNAVIGATOR, saveData);
        //    ActiveObjectHelper.WriteFile(FilePath, saveData);
        //    Log("---- save : " + FilePath);
        //}

        //private void Load()
        //{
        //    Log("--- load");

        //    string saveData = ActiveObjectHelper.ReadFile(FilePath);
        //    if (string.IsNullOrEmpty(saveData))
        //    {
        //        _data = new ActiveObjectData();
        //    }
        //    else
        //    {
        //        try
        //        {
        //            _data = new ActiveObjectData();
        //            EditorJsonUtility.FromJsonOverwrite(saveData, _data);
        //            for (int i = _data.list.Count - 1; i >= 0; i--)
        //            {
        //                var item = _data.list[i];
        //                string path = AssetDatabase.GUIDToAssetPath(item.guid);
        //                if (string.IsNullOrEmpty(path))
        //                    _data.list.RemoveAt(i);
        //            }
        //            Log("--  load count : " + _data.list.Count);

        //            _menu = new Menu();
        //            _menu.SetData(_data.menu);
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.LogError(e);
        //            _data = new ActiveObjectData();
        //        }
        //    }
        //}

        //private string FilePath
        //{
        //    get
        //    {
        //        return string.Format("{0}/UnityToolKit/{1}", Directory.GetParent(Application.dataPath), SAVE_FILE);
        //    }
        //}

        //private void PrepareBuildInIcons()
        //{
        //    //if (lockGUIContent == null)
        //    //    lockGUIContent = StringToTexture(ICON_LOCK);
        //}

        //private Texture2D StringToTexture(string content)
        //{
        //    Texture2D texture = new Texture2D(0, 0, TextureFormat.ARGB32, false, true);
        //    texture.hideFlags = HideFlags.HideAndDontSave;
        //    texture.LoadImage(Convert.FromBase64String(content));
        //    return texture;
        //}


        //#endregion save&load

        //#region log

        //private void Log(string content)
        //{
        //    if (_debug)
        //        Debug.Log(content);
        //}

        //private void LogFormat(string format, params object[] pars)
        //{
        //    if (_debug)
        //        Debug.Log(string.Format(format, pars));
        //}


        //#endregion

        //#region data define

        //[Serializable]
        //public class ActiveObjectData
        //{
        //    public Color highlightColor = Color.green;
        //    public int maxCacheCount = 100;
        //    public bool isLock = false;
        //    public List<string> menu;
        //    public List<ActiveObjectInfo> list = new List<ActiveObjectInfo>();

        //    public ActiveObjectData()
        //    {
        //        menu = ActiveObjectConfig.GetDefaultMenu();
        //    }
        //}

        //[Serializable]
        //public class ActiveObjectInfo
        //{
        //    public string guid;
        //    public bool favorite;

        //    [NonSerialized] private string _name;
        //    [NonSerialized] private UnityEngine.Object _obj;
        //    [NonSerialized] private Texture _texture;
        //    [NonSerialized] private string _path;
        //    [NonSerialized] private string _typeName;

        //    public string name
        //    {
        //        get
        //        {
        //            if (string.IsNullOrEmpty(_name))
        //            {
        //                _name = obj.name;
        //            }

        //            return _name;
        //        }
        //    }

        //    public UnityEngine.Object obj
        //    {
        //        get
        //        {
        //            if (_obj == null)
        //            {
        //                _obj = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetDatabase.GUIDToAssetPath(guid));
        //            }

        //            return _obj;
        //        }
        //    }

        //    public Texture texture
        //    {
        //        //set { _texture = value; }
        //        get
        //        {
        //            if (_texture == null)
        //            {
        //                _texture = AssetDatabase.GetCachedIcon(AssetDatabase.GUIDToAssetPath(guid));
        //            }

        //            return _texture;
        //        }
        //    }

        //    public string path
        //    {
        //        get
        //        {
        //            if (string.IsNullOrEmpty(_path))
        //            {
        //                _path = AssetDatabase.GetAssetPath(obj);
        //            }

        //            return _path;
        //        }
        //    }

        //    public string typeName
        //    {
        //        get
        //        {
        //            if (string.IsNullOrEmpty(_typeName))
        //            {
        //                _typeName = ActiveObjectHelper.GetNameByType(obj.GetType());
        //            }
        //            return _typeName;
        //        }
        //    }
        //}

        //#endregion

        //class Menu
        //{
        //    private const int TOOLBAR_ALL = 0;

        //    public int index;
        //    public GUIContent[] menuItems;


        //    public bool IsAll()
        //    {
        //        return index == TOOLBAR_ALL;
        //    }

        //    public bool IsFavorite()
        //    {
        //        return index == menuItems.Length - 1;
        //    }

        //    public bool IsElse()
        //    {
        //        return index == menuItems.Length - 2;
        //    }

        //    public void SetData(List<string> typeNames)
        //    {
        //        int menuCount = typeNames.Count + 3;
        //        menuItems = new GUIContent[menuCount];
        //        menuItems[0] = new GUIContent("All");
        //        menuItems[menuCount - 2] = new GUIContent("Else");
        //        menuItems[menuCount - 1] = EditorGUIUtility.IconContent("Favorite Icon");
        //        int j = 1;

        //        foreach (var n in typeNames)
        //        {
        //            var type = ActiveObjectHelper.GetTypeByName(n);
        //            var guiContent = new GUIContent(type.gUIContent);
        //            guiContent.text = string.Empty;
        //            guiContent.tooltip = type.typeName;
        //            menuItems[j] = guiContent;
        //            j++;
        //        }
        //    }
        //}

    }
}