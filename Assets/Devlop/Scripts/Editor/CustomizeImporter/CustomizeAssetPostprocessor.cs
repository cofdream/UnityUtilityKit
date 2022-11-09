using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental;
using UnityEngine;

public class CustomizeAssetPostprocessor : AssetPostprocessor
{
    private void OnPreprocessAsset()
    {
//        Debug.Log(assetImporter.GetType());
//        if (assetImporter.GetType() == typeof(AssetImporter))
//        {
//            AssetDatabase.SetImporterOverride<CustomizeImporter>(assetPath);
//            Debug.Log(assetImporter);
//            Debug.Log(AssetDatabase.GetImporterOverride(assetPath));
//        }
        //Debug.Log(assetImporter);
        //Debug.Log(AssetDatabase.GetImporterOverride(assetPath));
      
//        Debug.Log(AssetImporter.GetAtPath(context.assetPath));

//        var obj = ObjectFactory.CreateInstance<CustomizeImporter>();
//        obj.GenerateAssetData(context);
    }

    private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets,
        string[] movedFromAssetPaths)
    {
        //foreach (string str in importedAssets)
        //{
        //    Debug.Log("Reimported Asset: " + str);
        //}
        //foreach (string str in deletedAssets)
        //{
        //    Debug.Log("Deleted Asset: " + str);
        //}

        //for (int i = 0; i < movedAssets.Length; i++)
        //{
        //    Debug.Log("Moved Asset: " + movedAssets[i] + " from: " + movedFromAssetPaths[i]);
        //}
    }
}