using UnityEngine;

namespace Cofdream.ToolKitEditor.UPM
{
    [System.Serializable]
    public class UPMCommand
    {
        public bool State;
        public string Name;
        [TextArea] public string Command;
    } 
}