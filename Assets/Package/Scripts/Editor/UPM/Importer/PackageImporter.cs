using UnityEditor;
using UnityEngine;
using UnityEditor.AssetImporters;
using System.IO;

namespace Cofdream.ToolKitEditor.UPM
{
#if COFDREAM_PACKAGE
    [ScriptedImporter(1, new string[] { }, new string[] { "json" })]
#endif
    [ExcludeFromPreset]
    public class PackageImporter : ScriptedImporter
    {
        public override void OnImportAsset(AssetImportContext assetImportContext)
        {
            var aa = AssetDatabase.LoadAssetAtPath<PackageManifest>(assetImportContext.assetPath);

            var packageManifest = ObjectFactory.CreateInstance<PackageManifest>();
            assetImportContext.AddObjectToAsset("main", packageManifest);
            assetImportContext.SetMainObject(packageManifest);

            // 需要重新设置值，和 PackageImporterEditor 内的对象 PackageManifest 不是同一个
            var jsonData = File.ReadAllText(assetImportContext.assetPath);
            var package = JsonUtility.FromJson<Package>(jsonData);
            packageManifest.Package = package;

#if UNITY_2021_2_OR_NEWER
            var texture2D = AssetDatabase.LoadAssetAtPath<Texture2D>(Utils.PackageIconPath);
            EditorGUIUtility.SetIconForObject(packageManifest, texture2D);
#endif
        }
    }

}