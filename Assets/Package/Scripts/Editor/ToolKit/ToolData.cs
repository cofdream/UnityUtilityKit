using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    // Single Data

    public class ToolData : ScriptableObject
    {
        public List<SceneData> SceneDatas;

        private void OnEnable()
        {
            if (SceneDatas == null)
                SceneDatas = new List<SceneData>();
        }

        public void OnValidate()
        {
            foreach (var item in SceneDatas)
            {
                if (string.IsNullOrEmpty(item.SceneName) && item.SceneAsset != null)
                {
                    item.SceneName = item.SceneAsset.name;
                }
            }
        }
    }

    // Share Data

    [System.Serializable]
    public class SceneData
    {
        public string SceneName;
        public SceneAsset SceneAsset;
    }


    [System.Serializable]
    public class ProjectGroup
    {
        public string Name;
        public int Count;
    }
    [System.Serializable]
    public class ProjectInfo
    {
        public string Name;
        public string Path;
        public string CommandLine;
        public string UnityEnginePath;
        public int ProcessId;
    }

    [System.Serializable]
    public class ProjectInfoGroup : ScriptableObject
    {
        public ProjectGroup[] ProjectGroups;
        public ProjectInfo[] ProjectInfos;

        [System.NonSerialized]
        private Dictionary<ProjectGroup, ProjectInfo[]> _projectInfoGroups;

        public Dictionary<ProjectGroup, ProjectInfo[]> ProjectInfoGroups
        {
            get
            {
                //if (_projectInfoGroups == null && ProjectGroups != null && ProjectInfos != null)
                //{
                //    _projectInfoGroups = new Dictionary<ProjectGroup, ProjectInfo[]>(ProjectGroups.Length);
                //    int index = 0;
                //    foreach (var item in ProjectGroups)
                //    {
                //        var infos = new ProjectInfo[item.Count];
                //        for (int i = 0; i < item.Count; i++)
                //        {
                //            if (index < ProjectInfos.Length)
                //            {
                //                infos[i] = ProjectInfos[index++];
                //            }
                //            else
                //            {
                //                break;
                //            }
                //        }
                //        if (index <= ProjectInfos.Length)
                //        {
                //            _projectInfoGroups.Add(item, infos);
                //        }
                //        else
                //        {
                //            break;
                //        }
                //    }
                //}

                return _projectInfoGroups;
            }
        }

        private void OnEnable()
        {
            if (ProjectGroups == null)
                ProjectGroups = new ProjectGroup[0];

            if (ProjectInfos == null)
                ProjectInfos = new ProjectInfo[0];

            if (_projectInfoGroups == null)
                _projectInfoGroups = new Dictionary<ProjectGroup, ProjectInfo[]>();
        }

        private void OnValidate()
        {
            if (_projectInfoGroups.Count != ProjectGroups.Length)
            {
                _projectInfoGroups = new Dictionary<ProjectGroup, ProjectInfo[]>(ProjectGroups.Length);
                int index = 0;
                foreach (var item in ProjectGroups)
                {
                    var infos = new ProjectInfo[item.Count];
                    for (int i = 0; i < item.Count; i++)
                    {
                        if (index < ProjectInfos.Length)
                        {
                            infos[i] = ProjectInfos[index++];
                        }
                        else
                        {
                            break;
                        }
                    }
                    if (index <= ProjectInfos.Length)
                    {
                        _projectInfoGroups.Add(item, infos);
                    }
                    else
                    {
                        break;
                    }
                }
            }
        }
    }
}