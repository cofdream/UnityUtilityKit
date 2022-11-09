//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//
//public class ActiveObjectAsset : ScriptableObject
//{
    //@[MenuItem("Assets/ScriptableObject/ActiveObjectAsset")]
//    public static void CreateAsset()
//    {
//        if (AssetDatabase.LoadAssetAtPath<ActiveObjectAsset>(path) != null)
//        {
//            Debug.Log("asset already exist");
//            return;
//        }
//        ActiveObjectAsset scriptableObj = ScriptableObject.CreateInstance<ActiveObjectAsset>();
//        AssetDatabase.CreateAsset(scriptableObj, path);
//        AssetDatabase.ImportAsset(path);
//        AssetDatabase.Refresh();
//    }
//
//    public static ActiveObjectAsset CreateAndLoadAsset()
//    {
//        CreateAsset();
//        var asset = AssetDatabase.LoadAssetAtPath<ActiveObjectAsset>(ActiveObjectAsset.path);
//        return asset;
//    }
//
//    public static void ForceSave()
//    {
//        AssetDatabase.ImportAsset(path);
//    }
//    
//    public static string path = "Assets/NextTools/Editor/ActiveObjectNavigator/ActiveObjectAsset.asset";
//    public List<ActiveObjectInfo> list;
//    
//
//    public void Switch(string guid, uint place)
//    {
//        if (guid == null)
//        {
//            Debug.LogError("exception : guid is null");
//            return;
//        }
//        if (list == null) return;
//
//        int index = list.FindIndex(i => i.guid == guid);
//        if (index >= 0)
//        {
//            var info = list[index];
//            list.RemoveAt(index);
//            list.Add(info);
//        }
//        else
//        {
//            Debug.LogError("not found");
//        }
//    }
//
//    public void Clear()
//    {
//        if(list!=null)
//            list.Clear();
//        AssetDatabase.ImportAsset(path);
//    }
//}
