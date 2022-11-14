using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public class ToolKitWindow : EditorWindowPlus
    {
        [MenuItem(MenuItemUtil.ToolKit + "UtilityWindow", false, 1)]
        private static void OpenWindow()
        {
            GetWindow<ToolKitWindow>("工具").Show();
        }

        private ToolData _toolData;

        private Texture2D _scemeAssetIcon;

        private void OnEnable()
        {
            var rootPath = "Assets/_A_WorkData";
            if (AssetDatabase.IsValidFolder(rootPath) == false)
            {
                AssetDatabase.CreateFolder("Assets", "_A_WorkData");
                AssetDatabase.ImportAsset(rootPath);
            }

            var toolDataPath = rootPath + "/ToolData.asset";
            _toolData = AssetDatabase.LoadAssetAtPath<ToolData>(toolDataPath);
            if (_toolData == null)
            {
                _toolData = CreateInstance<ToolData>();
                AssetDatabase.CreateAsset(_toolData, toolDataPath);
                AssetDatabase.ImportAsset(toolDataPath);
            }

            _scemeAssetIcon = EditorGUIUtility.Load("SceneAsset Icon") as Texture2D;//EditorGUIUtility.FindTexture("SceneAsset Icon");
        }

        private void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(false));
            {
                for (int i = 0; i < _toolData.SceneDatas.Count; i++)
                {
                    var sceneData = _toolData.SceneDatas[i];
                    if (GUILayout.Button(new GUIContent($"Open {sceneData.SceneName}", _scemeAssetIcon, AssetDatabase.GetAssetPath(sceneData.SceneAsset)), GUILayout.Height(22), GUILayout.Width((sceneData.SceneName.Length + 6) * 8 + 10)))
                    {
                        // 播放状态检查
                        if (EditorApplication.isPlayingOrWillChangePlaymode)
                        {
                            if (EditorUtility.DisplayDialog("无法打开新场景", "编辑器还在播放模式或是即将切换到播放模式", "退出播放模式", "取消"))
                            {
                                EditorApplication.isPlaying = false;
                            }
                            else
                                return;
                        }

                        //EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();

                        EditorSceneManager.OpenScene(AssetDatabase.GetAssetPath(sceneData.SceneAsset), OpenSceneMode.Single);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
        }
    }
}