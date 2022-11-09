using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Cofdream.ToolKitEditor;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEditor.ProjectWindowCallback;
using System;
using UnityEditorInternal;
using Object = UnityEngine.Object;

[CustomEditor(typeof(CustomizeAssetImporter), true)]
[CanEditMultipleObjects]
public class CustomizeAssetImporterEditor : AssetImporterEditor
{
  
}