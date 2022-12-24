using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace Cofdream.ToolKitEditor
{
    // todo 实现一个自定义的 弹窗提示
    public class DialogWidnow : EditorWindow
    {
        [MenuItem("DialogWidnow Test/Open")]
        static void Open()
        {
            CreateInstance<DialogWidnow>().ShowModal();
            Debug.Log(aa);
        }

        [MenuItem("Test Test/Open")]
        static void Open2()
        {
            var directoryInfo = Directory.CreateDirectory(@"E:\UnityProject\UnityToolKit\Assets\_A_WorkData\asda");
            string defaultGraphFolderPath = directoryInfo.FullName.Remove(0, Application.dataPath.Length - 6); 
            Debug.Log(defaultGraphFolderPath);

            Debug.Log(directoryInfo.Parent);
        }


        [MenuItem("Test asdasdas/Open33333")]
        static void Open3() {

            var path22 = "Assets/BundleResources/ScriptAsset/AnimAsset/" + "asd/asda/asdasd";
            int index = path22.LastIndexOf('/');
            path22 = path22.Substring(0, index);

            Debug.Log(path22);

            //var s = AssetDatabase.GetAssetPath(null);
            //Debug.Log(s);

            //var files = Directory.GetFiles(@"Assets/_A_WorkData/PackageDatas", "*.asset", SearchOption.AllDirectories);
            //foreach (var item in files)
            //{
            //    Debug.Log(item);
            //}
        }

        public static void DisplayDialog()
        {
            //DialogWidnow.CreateInstance<DialogWidnow>().ShowModal();
         
        }

        private static bool aa;
        private void OnGUI()
        {
            GUI.backgroundColor = Color.white;
            GUI.color = Color.white;
            GUILayout.Label("1");


            if (GUILayout.Button("Ok"))
            {
                aa = true;
                Close();
            }
            if (GUILayout.Button("Cancel"))
            {
                aa = false;
                Close();
            }
        }
      
    }
}