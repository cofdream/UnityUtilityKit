using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Type = System.Type;
using UnityEditor.Compilation;

namespace Cofdream.ToolKitEditor
{
    public class FindMissComponent : EditorWindowPlus
    {
        Type compentType;
        GameObject selectGO;
        DefaultAsset selectFolder;

        List<GameObject> gameObjects;
        Vector2 scPosition;

        MonoScript monoScript;

        private void OnGUI()
        {
            monoScript = EditorGUILayout.ObjectField(EditorGUIUtility.TrTextContent("MonoCompent:"), monoScript, typeof(MonoScript), true) as MonoScript;
            if ((object)monoScript != null)
            {
                var assemblyName = CompilationPipeline.GetAssemblyNameFromScriptPath(AssetDatabase.GetAssetPath(monoScript));
                assemblyName = System.IO.Path.GetFileNameWithoutExtension(assemblyName);
                GUILayout.Label(assemblyName);


                if (compentType == null && monoScript != null)
                {
                    var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
                    foreach (var assembly in assemblies)
                    {
                        if (assembly.GetName().Name == assemblyName)
                        {
                            compentType = assembly.GetType(monoScript.name);
                            GUILayout.Label(monoScript.name);
                            break;
                        }
                    }
                }
            }



            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.BeginVertical();
                {
                    selectGO = EditorGUILayout.ObjectField(EditorGUIUtility.TrTextContent("Target:"), selectGO, typeof(GameObject), false) as GameObject;

                    bool v = selectGO == null || compentType == null;
                    //Debug.Log(v);
                    EditorGUI.BeginDisabledGroup(v);
                    {
                        if (GUILayout.Button("Find"))
                        {
                            Find(new GameObject[] { selectGO }, compentType);
                        }
                    }
                    EditorGUI.EndDisabledGroup();
                }
                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical();
                {
                    selectFolder = EditorGUILayout.ObjectField(EditorGUIUtility.TrTextContent("Floder Target:"), selectFolder, typeof(DefaultAsset), true) as DefaultAsset;
                    if (AssetDatabase.IsValidFolder(AssetDatabase.GetAssetPath(selectFolder)) == false)
                    {
                        selectFolder = null;
                    }

                    bool v = selectFolder == null || compentType == null;
                    //Debug.Log(v);
                    EditorGUI.BeginDisabledGroup(v);
                    {
                        if (GUILayout.Button("Find"))
                        {
                            var guids = AssetDatabase.FindAssets("t:Prefab", new string[] { AssetDatabase.GetAssetPath(selectFolder) });
                            GameObject[] gos = new GameObject[guids.Length];
                            int index = 0;
                            foreach (var guid in guids)
                            {
                                string path = AssetDatabase.GUIDToAssetPath(guid);
                                gos[index++] = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                            }

                            Find(gos, compentType);
                        }
                    }
                    EditorGUI.EndDisabledGroup();



                }
                EditorGUILayout.EndVertical();
            }
            EditorGUILayout.EndHorizontal();

            scPosition = EditorGUILayout.BeginScrollView(scPosition);
            {
                if (gameObjects != null)
                {
                    int length = gameObjects.Count;
                    var goType = typeof(GameObject);
                    for (int i = 0; i < length; i++)
                    {
                        EditorGUILayout.ObjectField(gameObjects[i], goType, true);
                    }
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void Find(GameObject[] objects, Type type)
        {
            int length = objects.Length;
            gameObjects = new List<GameObject>(length);

            float value = 0;
            for (int i = 0; i < length; i++)
            {
                if (i < value)
                {
                    value += 5;
                    if (EditorUtility.DisplayCancelableProgressBar("Find in project all prefabs", "Finding...  000", i * 1f / length))
                        break;
                    System.Threading.Thread.Sleep((int)(1.0f));
                }

                object component = objects[i].GetComponentInChildren(type, true);
                if (component != null)
                {
                    gameObjects.Add(objects[i]);
                }

            }

            Debug.Log("Find end. Count: " + length.ToString());

            EditorUtility.ClearProgressBar();
        }
    }
}