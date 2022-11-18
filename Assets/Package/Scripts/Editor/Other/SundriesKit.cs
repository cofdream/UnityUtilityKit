using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Compilation;

namespace Cofdream.ToolKitEditor
{
    /// <summary>
    /// 一些没法归类的工具合集
    /// </summary>
    public class SundriesKit
    {
        private const string HEAD = MenuItemUtil.ToolKit + "杂物/";

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

    }
}