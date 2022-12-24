using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    [System.Serializable]
    public class ProjectInfoGroup : ScriptableObject
    {
        public ProjectGroup[] ProjectGroups;

        [System.NonSerialized]
        private Dictionary<ProjectGroup, ProjectInfo[]> _projectInfoGroups;

        public Dictionary<ProjectGroup, ProjectInfo[]> ProjectInfoGroups
        {
            get
            {
                if (_projectInfoGroups.Count == 0)
                {
                    foreach (var item in ProjectGroups)
                    {
                        if (item != null)
                        {
                            _projectInfoGroups.Add(item, item.ProjectInfos);
                        }
                    }
                }
                return _projectInfoGroups;
            }
        }

        private void OnEnable()
        {
            if (ProjectGroups == null)
                ProjectGroups = new ProjectGroup[0];

            if (_projectInfoGroups == null)
                _projectInfoGroups = new Dictionary<ProjectGroup, ProjectInfo[]>();
        }

        private void OnValidate()
        {
            if (_projectInfoGroups == null)
                _projectInfoGroups = new Dictionary<ProjectGroup, ProjectInfo[]>();
            else
                _projectInfoGroups.Clear();
        }
    }
    
    [CustomEditor(typeof(ProjectInfoGroup)), CanEditMultipleObjects]
    internal class ProjectInfoGroupInspector : Editor
    {
        [SerializeField] private string _commandLine;

        private GUIContent _setCommandLineValueGUIContent;

        private void OnEnable()
        {
            _setCommandLineValueGUIContent = new GUIContent("Set Command Line Values", "Set Command Line Value To All Project Info");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            EditorGUILayout.BeginHorizontal();
            {
                _commandLine = EditorGUILayout.TextField("Command Line",_commandLine);
                if (GUILayout.Button(_setCommandLineValueGUIContent,GUILayout.ExpandWidth(false)))
                {
                    foreach (var targetObj in targets)
                    {
                        var  projectInfoGroup = (ProjectInfoGroup)targetObj;
                        foreach (var projectGroup in projectInfoGroup.ProjectGroups)
                        {
                            foreach (var projectInfo in projectGroup.ProjectInfos)
                            {
                                projectInfo.CommandLine = _commandLine;
                                EditorUtility.SetDirty(projectInfo);
                                CustomAssetModificationProcessor.SaveAssetIfDirty(projectInfo);
                            }
                            EditorUtility.SetDirty(projectGroup);
                            CustomAssetModificationProcessor.SaveAssetIfDirty(projectGroup);
                        }
                        EditorUtility.SetDirty(projectInfoGroup);
                        CustomAssetModificationProcessor.SaveAssetIfDirty(projectInfoGroup);
                    }
                }
            }
            EditorGUILayout.EndHorizontal();
            
        }
    }
}