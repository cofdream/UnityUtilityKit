using UnityEngine;
using UnityEditor;
using System.IO;

namespace Cofdream.ToolKitEditor
{
    public class AssetFind : EditorWindow, IHasCustomMenu
    {
        [MenuItem(MenuItemUtil.ToolKit + "Wait tidy/AssetFindTool")]
        private static void Open()
        {
            EditorWindowExtension.GetWindowInCenter<AssetFind>(new Vector2(550, 500)).Show();
        }


        private string path;

        private string assetPath;
        private string assetGuiid;
        private string[] allGUIID;
        private void OnGUI()
        {
            GUILayout.BeginVertical();
            {
                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Path：", GUILayout.Width(40));
                    assetPath = EditorGUILayout.TextField(assetPath);
                }
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("GUIID：" + assetGuiid, GUILayout.Width(280));
                    GUILayout.FlexibleSpace();

                    if (GUILayout.Button(EditorGUIUtility.FindTexture("Refresh")))
                    {
                        assetGuiid = AssetDatabase.AssetPathToGUID(assetPath);
                    }
                }
                GUILayout.EndHorizontal();


                GUILayout.Space(5);

                GUILayout.BeginHorizontal();
                {
                    GUILayout.Label("Path：", GUILayout.Width(40));

                    path = EditorGUILayout.TextField(path);
                }
                GUILayout.EndHorizontal();

            }
            GUILayout.EndVertical();

            if (GUILayout.Button("Find"))
            {
                if (Directory.Exists(path))
                {
                    var files = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
                    allGUIID = new string[files.Length];
                    for (int i = 0; i < files.Length; i++)
                    {
                        allGUIID[i] = AssetDatabase.AssetPathToGUID(files[i]);
                    }
                }
            }
            if (allGUIID != null)
            {
                foreach (var item in allGUIID)
                {
                    GUILayout.Label(item);
                }
            }
        }

        public void AddItemsToMenu(GenericMenu menu)
        {

        }
    }
}