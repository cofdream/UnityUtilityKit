using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public class DevelopmentTools : EditorWindow//, IHasCustomMenu
    {
        //[MenuItem("ToolKit/DevelopmentTool")]
        //private static void OpenWindow()
        //{
        //    GetWindow<DevelopmentTools>(typeof(DevelopmentTools).Name, typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow")).Show();
        //}

        //private Vector2 scrollViewPosition;
        //private BaseToolCell[] cells;

        //private Texture helpIcon;
        //private GUIStyle iconButtonStyle;

        //private void Awake()
        //{
        //    minSize = new Vector2(300, 200);

        //    helpIcon = EditorGUIUtility.FindTexture("d__Help");
        //    iconButtonStyle = "IconButton";//Unable to find style 'IconButton' in skin 'GameSkin' <called outside OnGUI>

        //    LoadTool();
        //}

        //private void OnEnable()
        //{
        //    foreach (var cell in cells)
        //    {
        //        cell.Reload();
        //    }
        //}

        //private void OnDestroy()
        //{
        //    foreach (var cell in cells)
        //    {
        //        if (cell != null)
        //        {
        //            cell.Dispose();
        //        }
        //    }
        //}

        //private void OnGUI()
        //{
        //    scrollViewPosition = EditorGUILayout.BeginScrollView(scrollViewPosition);
        //    {
        //        foreach (var cell in cells)
        //        {

        //            EditorGUILayout.BeginVertical("box");
        //            {
        //                EditorGUILayout.BeginHorizontal();
        //                {
        //                    cell.IsShow = GUILayout.Toggle(cell.IsShow, cell.Tool.ToolName, EditorStyles.foldout);

        //                    EditorGUILayout.Space(0, true);

        //                    if (GUILayout.Button(helpIcon))
        //                    {
        //                        // todo URL
        //                        //System.Diagnostics.Process.Start("");
        //                        // Application.OpenURL
        //                    }
        //                }
        //                EditorGUILayout.EndHorizontal();

        //                EditorGUILayout.BeginHorizontal();
        //                {
        //                    if (cell.IsShow)
        //                    {
        //                        //GUILayoutUtility.GetRect(20, 1, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(false));
        //                        //EditorGUILayout.BeginVertical();
        //                        cell.Tool.OnGUITool(new Rect());
        //                        //EditorGUILayout.EndVertical();
        //                    }
        //                }
        //                EditorGUILayout.EndHorizontal();

        //                EditorGUILayout.Space(5, false);
        //            }
        //            EditorGUILayout.EndVertical();
        //        }

        //        EditorGUILayout.Space(0, true);
        //    }
        //    EditorGUILayout.EndScrollView();
        //}

        //public void AddItemsToMenu(GenericMenu menu)
        //{
        //    menu.AddItem(EditorGUIUtility.TrTextContent("Reload tools"), false, LoadTool);
        //    this.EditScript(menu);
        //}

        //private void LoadTool()
        //{
        //    var toolType = typeof(IDevelopmentTool);
        //    //var assemblys = System.AppDomain.CurrentDomain.GetAssemblies();    //收集全部程序集
        //    var assembly = toolType.Assembly;    //只收集本命名空间下的

        //    // 收集继承 接口的 所有非抽象类
        //    var allType = assembly.GetTypes();
        //    var types = allType.Where(type => toolType.IsAssignableFrom(type) && type.IsAbstract == false);

        //    List<BaseToolCell> cellList = new List<BaseToolCell>(allType.Length);
        //    var scriptableObjectType = typeof(ScriptableObject);
        //    var assemblyString = assembly.FullName;

        //    foreach (var type in types)
        //    {
        //        BaseToolCell cell;
        //        if (scriptableObjectType.IsAssignableFrom(type))
        //        {
        //            var cell2 = ScriptableObject.CreateInstance<ScriptableObjectToolCell>();
        //            cell2.Init(type);
        //            cell2.Load();
        //            cell = cell2;
        //        }
        //        else
        //        {
        //            var cell2 = ScriptableObject.CreateInstance<NormalToolCell>();
        //            cell2.Init(assemblyString, type.Namespace + "." + type.Name);
        //            cell2.Load();
        //            cell = cell2;
        //        }
        //        cell.hideFlags = HideFlags.DontSaveInEditor;
        //        cellList.Add(cell);
        //    }

        //    int length = cellList.Count;
        //    cells = new BaseToolCell[cellList.Count];
        //    for (int i = 0; i < length; i++)
        //    {
        //        cells[i] = cellList[i];
        //    }
        //}



        //// 本该使用接口
        //// 为了使用编辑器的热重载,且支持多态，所以继承SO
        //[System.Serializable]
        //private abstract class BaseToolCell : ScriptableObject
        //{
        //    public bool IsShow { get; set; } = false;
        //    public IDevelopmentTool Tool { get; protected set; }

        //    public virtual void Reload() { }
        //    public virtual void Dispose() { }

        //}

        ////由于 接口无法 热重载，所以只能保存 程序集信息 和 类名 去动态创建对象
        //[System.Serializable]
        //private class NormalToolCell : BaseToolCell
        //{
        //    [SerializeField] private string assembluString;
        //    [SerializeField] private string typeName;

        //    public void Init(string assembluName, string typeName)
        //    {
        //        this.assembluString = assembluName;
        //        this.typeName = typeName;
        //    }
        //    public void Load()
        //    {
        //        Tool = (IDevelopmentTool)System.Reflection.Assembly.Load(assembluString).CreateInstance(typeName);
        //    }
        //    public override void Reload()
        //    {
        //        if (Tool == null) Load();
        //    }
        //}

        //[System.Serializable]
        //private class ScriptableObjectToolCell : BaseToolCell
        //{
        //    [SerializeField] private ScriptableObject ToolSO;

        //    public void Init(System.Type type)
        //    {
        //        ToolSO = ScriptableObject.CreateInstance(type);
        //        ToolSO.hideFlags = HideFlags.DontSaveInEditor;//不加标记会导致：进入播放模式，在退出以后，重新热重载后 本机C++对象被释放
        //    }
        //    public void Load()
        //    {
        //        Tool = (IDevelopmentTool)ToolSO;
        //    }
        //    public override void Reload()
        //    {
        //        if (Tool == null)
        //            Load();
        //    }
        //    public override void Dispose()
        //    {
        //        if (Tool != null)
        //        {
        //            Tool.OnDestroyTool();
        //            Tool = null;
        //        }
        //        ScriptableObject.DestroyImmediate(ToolSO);
        //        ToolSO = null;
        //    }
        //}
    }
}