using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public class ToolKitWindow : EditorWindowPlus
    {
        [MenuItem(MenuItemUtil.ToolKit + "ToolKitWindow", false, 1)]
        private static void OpenWindow()
        {
            GetWindow<ToolKitWindow>("工具").Show();
        }

        private class Style
        {
            public static GUIStyle _categoryBox;
            static Style()
            {
                _categoryBox = new GUIStyle(EditorStyles.helpBox);
                _categoryBox.padding.left = 4;
            }
        }


        private string _singleRootPath;
        private string _shareRootPath;


        [SerializeField] private ToolData _toolData;

        [SerializeField] public int _sceneAssetIconSize;
        private Texture2D _sceneAssetIcon;

        [SerializeField] private bool _isDisplayOpenProjectTool;
        [SerializeField] private ProjectInfoGroup _projectInfoGroup;

        private void OnEnable()
        {
            UnityEngine.Debug.Log();

            //path
            _singleRootPath = "Assets/_A_WorkData";
            if (AssetDatabase.IsValidFolder(_singleRootPath) == false)
            {
                AssetDatabase.CreateFolder("Assets", "_A_WorkData");
                AssetDatabase.ImportAsset(_singleRootPath);
            }
            _shareRootPath = Utils.GetPackageRootPath() + "/PackageDatas";
            if (AssetDatabase.IsValidFolder(_shareRootPath) == false)
            {
              var folder =  AssetDatabase.CreateFolder(Utils.GetPackageRootPath(), "PackageDatas");
                UnityEngine.Debug.Log(folder);
                AssetDatabase.ImportAsset(_shareRootPath);
            }


            // ToolData
            var toolDataPath = _singleRootPath + "/ToolData.asset";
            _toolData = AssetDatabase.LoadAssetAtPath<ToolData>(toolDataPath);
            if (_toolData == null)
            {
                _toolData = CreateInstance<ToolData>();
                AssetDatabase.CreateAsset(_toolData, toolDataPath);
                AssetDatabase.ImportAsset(toolDataPath);
            }


            // SceneAsset
            _sceneAssetIcon = EditorGUIUtility.Load("SceneAsset Icon") as Texture2D;//EditorGUIUtility.FindTexture("SceneAsset Icon");
            _sceneAssetIconSize = _sceneAssetIconSize == 0 ? 15 : _sceneAssetIconSize;


            // Project
            var projectInfoGroupPath = _shareRootPath + "/ProjectInfoGroup.asset";
            _projectInfoGroup = AssetDatabase.LoadAssetAtPath<ProjectInfoGroup>(projectInfoGroupPath);
            if (_projectInfoGroup == null)
            {
                _projectInfoGroup = CreateInstance<ProjectInfoGroup>();
                AssetDatabase.CreateAsset(_projectInfoGroup, projectInfoGroupPath);
                AssetDatabase.ImportAsset(projectInfoGroupPath);
            }


            AssetDatabase.ImportAsset(projectInfoGroupPath,ImportAssetOptions.ForceUpdate);
        }

        private void OnGUI()
        {

            _sceneAssetIconSize = EditorGUILayout.IntField("Scene Asset Icon Size:", _sceneAssetIconSize);

            EditorGUILayout.BeginHorizontal();
            {
                EditorGUIUtility.SetIconSize(Vector2.one * _sceneAssetIconSize);
                for (int i = 0; i < _toolData.SceneDatas.Count; i++)
                {
                    var sceneData = _toolData.SceneDatas[i];
                    if (GUILayout.Button(new GUIContent($" {sceneData.SceneName}", _sceneAssetIcon, $"Open Scene:{AssetDatabase.GetAssetPath(sceneData.SceneAsset)}"),
                         GUILayout.ExpandWidth(false)))
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
                EditorGUIUtility.SetIconSize(Vector2.zero);

                EditorGUILayout.Space(0, true);
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginVertical(Style._categoryBox);
            {
                _isDisplayOpenProjectTool = EditorGUILayout.BeginFoldoutHeaderGroup(_isDisplayOpenProjectTool, "Open Project Tool");
                {
                    if (_isDisplayOpenProjectTool)
                    {

                        foreach (var projectInfoGroups in _projectInfoGroup.ProjectInfoGroups)
                        {
                            EditorGUILayout.LabelField(projectInfoGroups.Key.Name);

                            EditorGUILayout.BeginHorizontal();
                            {
                                foreach (var item in projectInfoGroups.Value)
                                {
                                    Process process = null;
                                    try
                                    {
                                        if (item.ProcessId != 0)
                                        {
                                            process = Process.GetProcessById(item.ProcessId);
                                        }

                                    }
                                    catch (System.Exception e)
                                    {
                                        item.ProcessId = 0;
                                        UnityEngine.Debug.LogError(e);
                                    }

                                    if (process == null)
                                    {
                                        if (GUILayout.Button($"{item.Name}", GUILayout.Width(120)))
                                        {
                                            OpenProject(item);
                                        }
                                    }
                                    else
                                    {
                                        var color = GUI.contentColor;
                                        GUI.contentColor = Color.cyan;
                                        if (GUILayout.Button($"Close: {item.Name} PId: {item.ProcessId}", GUILayout.Width(200)))
                                        {
                                            if (process.HasExited == false)
                                            {
                                                try
                                                {
                                                    process.Kill();
                                                }
                                                catch (System.Exception e)
                                                {
                                                    UnityEngine.Debug.LogError(e);
                                                }

                                            }
                                            else
                                            {
                                                EditorUtility.DisplayDialog("Tips!", $"进程已经被其他方式给退出了！！\n分组: {projectInfoGroups.Key.Name}\n工程: {item.Name} 进程Id: {item.ProcessId}", "OK");
                                            }

                                            item.ProcessId = 0;
                                        }
                                        GUI.contentColor = color;
                                    }
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                        }
                    }
                }
                EditorGUILayout.EndFoldoutHeaderGroup();
            }
            EditorGUILayout.EndVertical();

        }

        private void OpenProject(ProjectInfo projectInfo)
        {
            Thread thread = new Thread((obj) =>
            {
                Process process = new Process();
                process.StartInfo.FileName = projectInfo.UnityEnginePath;
                process.StartInfo.Arguments = $"-projectPath {projectInfo.Path}";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.RedirectStandardInput = true;
                process.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding("GB2312");

                process.Start();

                projectInfo.ProcessId = process.Id;

                process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close();

                projectInfo.ProcessId = 0;
            });

            thread.Start();
        }


    }
}