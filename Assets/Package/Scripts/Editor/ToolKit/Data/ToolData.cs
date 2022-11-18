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
}