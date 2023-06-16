using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cofdream.ToolKitEditor
{
    public class ToolKitWindow : EditorWindowPlus
    {
        [MenuItem(MenuItemName.ToolKit + "ToolKitWindow", false, 1)]
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

        
        [SerializeField] private ToolData _toolData;

        [SerializeField] public int _sceneAssetIconSize;
        private Texture2D _sceneAssetIcon;

        [SerializeField] private bool _isDisplayOpenProjectTool;
        [SerializeField] private ProjectInfoGroup _projectInfoGroup;

        private void OnEnable()
        {
            //path
            _singleRootPath = "Assets/_A_WorkData";
            if (AssetDatabase.IsValidFolder(_singleRootPath) == false)
            {
                AssetDatabase.CreateFolder("Assets", "_A_WorkData");
                AssetDatabase.ImportAsset(_singleRootPath);
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
            var projectInfoGroupPath = _singleRootPath + "/ProjectInfoGroup.asset";
            _projectInfoGroup = AssetDatabase.LoadAssetAtPath<ProjectInfoGroup>(projectInfoGroupPath);
            if (_projectInfoGroup == null)
            {
                _projectInfoGroup = CreateInstance<ProjectInfoGroup>();
                AssetDatabase.CreateAsset(_projectInfoGroup, projectInfoGroupPath);
                AssetDatabase.ImportAsset(projectInfoGroupPath);
            }
        }

        private void OnGUI()
        {

            if (GUILayout.Button("close"))
            {
                Close();
            }

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

                            EditorGUI.BeginDisabledGroup(!projectInfoGroups.Key.Active);
                            EditorGUILayout.BeginHorizontal();
                            {
                                foreach (var item in projectInfoGroups.Value)
                                {
                                    var active = Directory.Exists(item.Path) && File.Exists(item.UnityEnginePath);

                                    EditorGUI.BeginDisabledGroup(!active);
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
                                            SetProcessId(item, 0);

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

                                                SetProcessId(item, 0);
                                            }
                                            GUI.contentColor = color;
                                        }
                                    }
                                    EditorGUI.EndDisabledGroup();
                                }
                            }
                            EditorGUILayout.EndHorizontal();
                            EditorGUI.EndDisabledGroup();
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

                SetProcessId(projectInfo, process.Id);

                process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close();

                SetProcessId(projectInfo, 0);
            });

            thread.Start();
        }

        #region Visula Code Expand

        [System.Serializable]
        private class Launch
        {
            [System.Serializable]
            public class Configuration
            {
                public string type = "emmylua_attach";
                public string request = "attach";
                public string name = "Attach by process id";
                public int pid = 0;
                public string processName = "";
                public bool captureLog = false;
            }

            public string version = "0.2.0";
            public List<Configuration> configurations = new List<Configuration>() { new Configuration() };
        }


        private static void SetProcessId(ProjectInfo info, int processId)
        {
            info.ProcessId = processId;

            string projectPath = info.Path;
            string visualCodeRelativePath = $"{projectPath}/Assets/BundleResources/Text/.vscode";

            if (Directory.Exists(visualCodeRelativePath) == false)
            {
                return;
            }

            string jsonPath = $"{visualCodeRelativePath}/launch.json";
            Launch launch;
            if (File.Exists(jsonPath) == false)
            {
                launch = new Launch();
            }
            else
            {
                var launchJson = File.ReadAllText(jsonPath);
                launch = JsonUtility.FromJson<Launch>(launchJson);
                if (launchJson == null)
                {
                    UnityEngine.Debug.LogError($"读取 Visual Code json 失败，path: {jsonPath} launchJson:{launchJson}");
                    return;
                }
            }

            launch.configurations[0].pid = processId;

            var json = JsonUtility.ToJson(launch, true);
            if (string.IsNullOrWhiteSpace(json))
            {
                UnityEngine.Debug.LogError($"{nameof(Launch)}转换Json失败");
                return;
            }
            File.WriteAllText(jsonPath, json);

        }

        #endregion
    }
}