using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    /// <summary>
    /// 一些没法归类的工具合集
    /// </summary>
    public class SundriesKit
    {
        private const string HEAD = MenuItemName.ToolKit + "杂物/";

        #region XXXX后播放
        private const string IS_PLAY_EDITOR_AFTER_COMPILATION = "ISPlayEditorAfterCompilation";

        [MenuItem(HEAD + "触发脚本编译,结束后自动播放")]
        private static void RequestScriptCompilation()
        {
            EditorPrefs.SetBool(IS_PLAY_EDITOR_AFTER_COMPILATION, true);
            CompilationPipeline.RequestScriptCompilation();
        }

        [InitializeOnLoadMethod]
        private static void PlayEditorAfterCompilation()
        {
            if (EditorPrefs.GetBool(IS_PLAY_EDITOR_AFTER_COMPILATION, false))
            {
                EditorPrefs.DeleteKey(IS_PLAY_EDITOR_AFTER_COMPILATION);
                EditorApplication.EnterPlaymode();
            }
        }
        #endregion

        [MenuItem(HEAD + "刷新资源并播放")]
        private static void RefreshAndPlay()
        {
            AssetDatabase.Refresh();
            EditorApplication.EnterPlaymode();
        }

        [MenuItem(HEAD + "Resources.UnloadUnusedAssets")]
        private static void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }

        private const string HEAD2 = "杂物/";
        [MenuItem(HEAD2 + "Resources.Test")]
        private static void Test()
        {
            var guids = AssetDatabase.FindAssets(".asset", new string[] { "" });
            List<string> allAsset = new List<string>(guids.Length);
            foreach (var item in guids)
            {
                allAsset.Add(AssetDatabase.GUIDToAssetPath(item));
            }

            var stopwatch = new System.Diagnostics.Stopwatch();


            stopwatch.Start();
            foreach (var item in allAsset)
            {
                var go = AssetDatabase.LoadAssetAtPath<Object>(item);
                Resources.UnloadAsset(go);
            }
            stopwatch.Stop();
            Debug.LogWarning($"UnloadAsset Use Time : {stopwatch.ElapsedMilliseconds}ms,  Check Time {stopwatch.ElapsedMilliseconds}ms");


            stopwatch.Start();
            foreach (var item in allAsset)
            {
                var go = AssetDatabase.LoadAssetAtPath<Object>(item);
            }
            Resources.UnloadUnusedAssets();

            stopwatch.Stop();
            Debug.LogWarning($"UnloadUnusedAssets Use Time : {stopwatch.ElapsedMilliseconds}ms,  Check Time {stopwatch.ElapsedMilliseconds}ms");
        }

        [MenuItem(HEAD2 + "2222")]
        private static void Test22()
        {
            //var s = AssetDatabase.GetTextMetaFilePathFromAssetPath("Assets/_A_WorkData/ToolData.asset");
            // var s2= AssetDatabase.GetTextMetaFilePathFromAssetPath("Assests/_A_WorkData/ToolDasdata2.sset");

            // Debug.Log(s);

            // Debug.Log(s2);

          var obj =  AssetDatabase.LoadAssetAtPath<GameObject>("asds");
            Debug.Log(obj);

            var path = "Assets/_A_WorkData";
            if (AssetDatabase.IsValidFolder(path))
            {
                var s = AssetDatabase.FindAssets("", new string[] { path });
                foreach (var item in s)
                {
                    Debug.Log(item);
                }
            }
            Debug.Log("asda\ndasd");
            
        }
    }
}