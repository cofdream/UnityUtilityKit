using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Cofdream.ToolKitEditor
{
    // 对 一批 文件进行批量拷贝 批量重命名
    //  Todo 
    // 使用树结构去写
    // 参考 https://blog.csdn.net/b8566679/article/details/112966961

    public class MuiltAssetCopyTool : EditorWindow
    {
        [MenuItem("ToolKit/Develop/AssetCopyTool")]
        private static void OpenAssetCopyToolWindow()
        {
            GetWindow<MuiltAssetCopyTool>().Show();
        }

        private Rect assetFileRootGUIRect;
        private string assetFileRootPath = null;
        private string[] assetFileArray;

        private string[] coptyAssetFileArray;


        private Vector2 scroolViewPosition = default;
        //private Vector2 rightScroolViewPosition = default;


        FileInfo root;

        private void OnGUI()
        {
            if (Directory.Exists(assetFileRootPath) == false)
            {
                EditorGUILayout.LabelField("需要拷贝文件夹目录请拖入下面的输入框");

                assetFileRootGUIRect = EditorGUILayout.GetControlRect(GUILayout.Width(250), GUILayout.Height(50));

                EditorGUI.TextField(assetFileRootGUIRect, assetFileRootPath);

                if (assetFileRootGUIRect != null && assetFileRootGUIRect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.type == EventType.DragUpdated)
                    {
                        DragAndDrop.visualMode = DragAndDropVisualMode.Move;
                    }
                    //如果鼠标拖拽结束时，并且鼠标所在位置在文本输入框内  
                    else if ((Event.current.type == EventType.DragExited))
                    {
                        if (DragAndDrop.paths != null && DragAndDrop.paths.Length > 0)
                        {
                            string selectFilePath = DragAndDrop.paths[0];
                            if (Directory.Exists(selectFilePath))
                            {
                                assetFileRootPath = selectFilePath;
                            }
                            else
                            {
                                assetFileRootPath = Path.GetDirectoryName(selectFilePath);
                            }

                            ResetAssetPathArray();
                        }
                    }
                }
                return;
            }

            GUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Copy"))
                {
                    CopyNewFile();
                }

                if (Directory.Exists(assetFileRootPath) && GUILayout.Button(EditorGUIUtility.FindTexture("RotateTool")))
                {
                    ResetAssetPathArray();
                }

                GUILayout.FlexibleSpace();

                if (GUILayout.Button("Reset select folder"))
                {
                    assetFileRootPath = null;
                }
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            {
                scroolViewPosition = EditorGUILayout.BeginScrollView(scroolViewPosition, "box");
                {
                    root?.StartDraw(false, new Vector2((int)(position.size.x * 0.5f), 16));
                }
                EditorGUILayout.EndScrollView();


                scroolViewPosition = EditorGUILayout.BeginScrollView(scroolViewPosition, "box");
                {
                    root?.StartDraw(true, new Vector2((int)(position.size.x * 0.5f), 16));
                }
                EditorGUILayout.EndScrollView();
            }
            GUILayout.EndHorizontal();

            if (GUI.changed)
            {
                base.Repaint();
            }
        }

        private void ResetAssetPathArray()
        {
            root = new FileInfo();
            root.IsDirectory = true;
            root.RootFileInfo = null;
            root.Path = assetFileRootPath;
            root.ChildInfos = GetChildInfo(assetFileRootPath, root);
        }

        public List<FileInfo> GetChildInfo(string path, FileInfo rootInfo)
        {
            List<FileInfo> childInfo = null;
            var directories = Directory.GetDirectories(path);
            if (directories.Length > 0)
            {
                childInfo = new List<FileInfo>();
            }
            foreach (var directory in directories)
            {
                FileInfo directoryInfo = new FileInfo();
                directoryInfo.IsDirectory = true;
                directoryInfo.RootFileInfo = rootInfo;
                directoryInfo.Path = directory.Replace("\\", "/");
                directoryInfo.ChildInfos = GetChildInfo(directoryInfo.Path, directoryInfo);

                childInfo.Add(directoryInfo);
            }

            var files = Directory.GetFiles(path);
            if (files.Length > 0 && childInfo == null)
            {
                childInfo = new List<FileInfo>();
            }
            foreach (var file in files)
            {
                if (file.EndsWith(".meta") == false)
                {
                    FileInfo fileInfo = new FileInfo();
                    fileInfo.IsDirectory = false;
                    fileInfo.RootFileInfo = rootInfo;
                    fileInfo.Path = file.Replace("\\", "/");
                    fileInfo.ChildInfos = null;

                    childInfo.Add(fileInfo);
                }
            }
            return childInfo;
        }

        public void CopyNewFile()
        {
            if (root != null && root.Path == "Assets/AssetGreen")
            {
                if (AssetDatabase.CopyAsset(root.Path, root.Path))
                {
                    Debug.LogError($"Copty error path:{root.Path} \nnewPath:{root.Path}");
                }
            }
        }
    }

    public class FileInfo
    {
        public bool IsDirectory;
        public FileInfo RootFileInfo;

        public List<FileInfo> ChildInfos;
        public string Path;



        public bool isShow;
        private Rect rect;
        private int offsetX = 0;

        static Texture openTexture = EditorGUIUtility.IconContent("IN foldout act on").image;
        static Texture closeTexture = EditorGUIUtility.IconContent("IN foldout act").image;
        static int offsetY;
        static bool isChange;
        public void StartDraw(bool isChange, Vector2 size)
        {
            offsetY = 0;
            FileInfo.isChange = isChange;
            rect = new Rect(new Vector2(0f, 2f), size);
            Draw();
        }
        private void Draw()
        {
            if (RootFileInfo != null)
            {
                float x = 20 * (RootFileInfo.offsetX + 1);
                float y = 18 * offsetY;

                rect = new Rect(new Vector2(x, y), RootFileInfo.rect.size);
            }
            DrawShowIcon();

            DrawPath();

            if (IsDirectory && Event.current.type == EventType.MouseUp)
            {
                Rect position = new Rect(new Vector2(0, rect.y), rect.size);
                if (position.Contains(Event.current.mousePosition))
                {
                    isShow = !isShow;
                    GUI.changed = true;
                }
            }

            offsetY++;

            DrawChild();
        }
        private void DrawChild(int index)
        {
            offsetX = index;
            Draw();
        }

        private void DrawShowIcon()
        {
            if (IsDirectory)
            {
                Rect iconRect = new Rect(rect.position - new Vector2(0f, 3f), new Vector2(20f, 20f));
                EditorGUI.LabelField(iconRect, new GUIContent(isShow ? openTexture : closeTexture));
            }
        }
        private void DrawPath()
        {
            Rect pathRect = new Rect(rect.position + new Vector2(20, 0), rect.size);
            if (isChange)
            {
                Path = EditorGUI.TextField(pathRect, Path);
            }
            else
            {
                EditorGUI.LabelField(pathRect, Path);
            }
        }
        private void DrawChild()
        {
            if (isShow && ChildInfos != null)
            {
                int index = 1;
                foreach (var childInfo in ChildInfos)
                {
                    childInfo.DrawChild(index++);
                }
            }
        }
    }
}