using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor.AssetNavigator
{
    public class AssetNavigatorData : ScriptableObject
    {
        public EditorSearchField SearchField;

        public Rect SelectTypePopupWindowRect;

        public SerializableDictionary<GUID, AssetNavigatorMenuContent> SelectedObjectDictionary;

        public static AssetNavigatorData LoadAsset(string assetPath)
        {
            var data = AssetDatabase.LoadAssetAtPath<AssetNavigatorData>(assetPath);
            if (data == null)
            {
                data = CreateInstance<AssetNavigatorData>();
                AssetDatabase.CreateAsset(data, assetPath);

                data.SearchField = new EditorSearchField();

                data.SelectedObjectDictionary = CreateInstance<DictionaryGUIDToUObject>();
                data.SelectedObjectDictionary.name = "GUID_To_Object_Datas";
                AssetDatabase.AddObjectToAsset(data.SelectedObjectDictionary, data);

                AssetDatabase.SetMainObject(data, assetPath);
                AssetDatabase.ImportAsset(assetPath);

                EditorUtility.SetDirty(data);
                CustomAssetModificationProcessor.SaveAssetIfDirty(data);
            }
            return data;
        }
    }
}