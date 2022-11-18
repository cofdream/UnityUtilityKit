using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    [System.Serializable]
    public class ProjectInfo : ScriptableObject
    {
        public string Name;
        public string Path;
        public string CommandLine;
        public string UnityEnginePath;
        public int ProcessId;

    }

}