using System.IO;
using System.Text;
using UnityEditor;
using UnityEditor.ProjectWindowCallback;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    internal static  class CreateFile
    {
        [MenuItem("Assets/Create/File", false, 101)]
        private static void OnCreateFile()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<CreateFileAction>(), "New File", EditorGUIUtility.IconContent("DefaultAsset Icon").image as Texture2D, string.Empty);
        }

        private class CreateFileAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
                File.WriteAllText(pathName, resourceFile, encoding);
                //todo error Asset import failed
                AssetDatabase.ImportAsset(pathName);

                //var obj = AssetDatabase.LoadAssetAtPath(pathName, typeof(Object));
                //ProjectWindowUtil.ShowCreatedAsset(obj);
            }
        }
    }
}