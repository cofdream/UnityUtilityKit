using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    [System.Serializable]
    public class ProjectGroup : ScriptableObject
    {
        public string Name;

        public ProjectInfo[] ProjectInfos;

        private void OnValidate()
        {
            // todo 把 ProjectInfos 改造为 sub asset
        }
    }
}