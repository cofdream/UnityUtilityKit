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

        #region XXXX后播放
        private const string IS_PLAY_EDITOR_AFTER_COMPILATION = "ISPlayEditorAfterCompilation";

        [MenuItem(MenuItemName.Other + "触发脚本编译,结束后自动播放")]
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

        [MenuItem(MenuItemName.Other + "刷新资源并播放")]
        private static void RefreshAndPlay()
        {
            AssetDatabase.Refresh();
            EditorApplication.EnterPlaymode();
        }

        [MenuItem(MenuItemName.Other + "Resources.UnloadUnusedAssets")]
        private static void UnloadUnusedAssets()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}