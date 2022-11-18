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
}