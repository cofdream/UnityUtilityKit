using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public class ToolData : ScriptableObject
    {
        public List<SceneData> SceneDatas;

        private void OnEnable()
        {
            if (SceneDatas == null)
                SceneDatas = new List<SceneData>();

            Debug.Log("ToolData OnEnable done.");
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

    [System.Serializable]
    public class SceneData
    {
        public string SceneName;
        public SceneAsset SceneAsset;
    }

}