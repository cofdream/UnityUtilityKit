using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    //本改使用抽象类
    [System.Serializable]
    public class ToolProvider
    {
        private string toolPath;
        public string ToolPath => toolPath;


        public ToolProvider(string path)
        {
            toolPath = path;
        }

        public virtual void OnActive() { }
        public virtual void OnDeactivate() { }
        public virtual void OnDraw() { }

        public virtual void OnDestroy() { }
    }
}