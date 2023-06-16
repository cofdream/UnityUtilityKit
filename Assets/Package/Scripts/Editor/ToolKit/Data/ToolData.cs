using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    // Single Data

    public class ToolData : ScriptableObject
    {
        public List<SceneData> SceneDatas;

        public List<GroupData> GroupDatas;

        private void OnEnable()
        {
            if (SceneDatas == null)
                SceneDatas = new List<SceneData>();

            if (GroupDatas == null)
                GroupDatas = new List<GroupData>();
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

            foreach (var item in GroupDatas)
            {
                foreach (var sceneData in item.SceneDatas)
                {
                    if (string.IsNullOrEmpty(sceneData.SceneName) && sceneData.SceneAsset != null)
                    {
                        sceneData.SceneName = sceneData.SceneAsset.name;
                    }
                }
            }

            CustomAssetModificationProcessor.SaveAssetIfDirty(this);
        }
    }

    [System.Serializable]
    public class GroupData
    {
        [HideInInspector]
        public bool IsFold;
        public string GroupName;
        public List<SceneData> SceneDatas;
    }

    // Share Data

    [System.Serializable]
    public class SceneData
    {
        public string SceneName;
        public SceneAsset SceneAsset;
    }
}