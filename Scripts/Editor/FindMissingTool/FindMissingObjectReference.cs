using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

namespace Cofdream.ToolKitEditor
{
    public class FindMissingObjectReference : EditorWindow
    {
        private Vector2 scroolViewPosition;

        private string needFindPath;
        private Object needFindObject;

        [SerializeField]
        private List<MissingInfo> missingInfos;

        static string[] ignores = new string[]
        {
            ".meta",
            ".cs",
            ".shader",
        };

        private void OnEnable()
        {
            missingInfos = new List<MissingInfo>();
        }

        private void OnGUI()
        {
            // 绘制当前脚本
            GUI.enabled = false;
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty m_Script = serializedObject.FindProperty("m_Script");
            EditorGUILayout.PropertyField(m_Script);
            GUI.enabled = true;

            //寻找指定对象的脚本

            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent("Aseet: "));
                needFindObject = EditorGUILayout.ObjectField(needFindObject, typeof(Object), true);

                GUI.enabled = needFindObject != null;
                if (GUILayout.Button("Find"))
                {
                    missingInfos.Clear();

                    var missingInfo = GetMissingObjectReference__(needFindObject);
                    if (missingInfo != null)
                    {
                        GameObject go = needFindObject as GameObject;
                        if (go != null && go.scene.rootCount > 0)
                        {
                            missingInfo.Path = AssetDatabase.GetAssetPath(go);
                        }
                        else
                        {
                            missingInfo.Path = string.Empty;
                        }

                        missingInfos.Add(missingInfo);
                    }

                }
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            {
                GUILayout.Label(new GUIContent("AssetPath: "));
                needFindPath = EditorGUILayout.TextField(needFindPath);

                GUI.enabled = !string.IsNullOrEmpty(needFindPath);
                if (GUILayout.Button("Find"))
                {
                    missingInfos.Clear();

                    if (Directory.Exists(needFindPath))
                    {
                        var files = Directory.GetFiles(needFindPath);
                        foreach (var file in files)
                        {
                            if (IsIgnore(file) == false)
                            {
                                var missingInfo = GetMissingObjectReference(file);
                                if (missingInfo.MissingDatas.Count > 0)
                                {
                                    missingInfos.Add(
                                        missingInfo
                                    );
                                }
                            }

                        }
                    }
                    else if (File.Exists(needFindPath))
                    {
                        var missingInfo = GetMissingObjectReference(needFindPath);
                        if (missingInfo.MissingDatas.Count > 0)
                        {
                            missingInfos.Add(
                                missingInfo
                            );
                        }
                    }

                }
                GUI.enabled = true;
            }
            GUILayout.EndHorizontal();


            //绘制数据
            GUILayout.Space(3);
            scroolViewPosition = GUILayout.BeginScrollView(scroolViewPosition);
            {
                GUI.enabled = false;

                SerializedProperty serializedProperty = serializedObject.FindProperty("missingInfos");
                if (serializedProperty != null)
                {
                    EditorGUILayout.PropertyField(serializedProperty);
                }

                GUI.enabled = true;
            }
            GUILayout.EndScrollView();

        }

        public static MissingInfo GetMissingObjectReference(string path)
        {
            if (path.EndsWith(".prefab"))
            {
                return GetMissingObjectReferenceInPrefab(path);
            }
            else
            {
                var findOject = AssetDatabase.LoadAssetAtPath(path, typeof(object));
                if (findOject == null)
                {
                    UnityEngine.Debug.Log(path);
                    return null;

                }
                SerializedObject serializedObject = new SerializedObject(findOject);

                MissingInfo missingInfo = new MissingInfo();
                missingInfo.MissingObject = findOject;
                missingInfo.Path = path;
                missingInfo.MissingDatas = new List<MissingData>();


                SerializedProperty serializedProperty = serializedObject.GetIterator();
                while (serializedProperty.NextVisible(true))
                {
                    if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        // sheder 需要特殊处理下
                        if (serializedProperty.displayName == "Shader" && serializedProperty.objectReferenceValue == null)
                        {
                            missingInfo.MissingDatas.Add(new MissingData()
                            {
                                PropertyName = serializedProperty.propertyPath,
                            });

                            Debug.Log($"{findOject.name} 引用丢失\n路径：{AssetDatabase.GetAssetPath(findOject)}\n丢失属性：{serializedProperty.propertyPath}");
                            continue;
                        }
                        //引用对象是null 并且 引用ID不是0 说明丢失了引用
                        if (serializedProperty.objectReferenceValue == null && serializedProperty.objectReferenceInstanceIDValue != 0)
                        {
                            missingInfo.MissingDatas.Add(new MissingData()
                            {
                                PropertyName = serializedProperty.propertyPath,
                                InstanceID = serializedProperty.objectReferenceInstanceIDValue,
                            });

                            Debug.Log($"{findOject.name} 引用丢失\n路径：{AssetDatabase.GetAssetPath(findOject)}\n丢失属性：{serializedProperty.propertyPath}\n引用id：{serializedProperty.objectReferenceInstanceIDValue}");
                        }
                    }
                }

                return missingInfo;
            }
        }

        public static MissingInfo GetMissingObjectReferenceInPrefab(string path)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null)
            {
                Debug.LogError("预制体路径不存在，无法加载对象。");
                return null;
            }
            MissingInfo missingInfo = new MissingInfo();
            missingInfo.MissingObject = prefab;
            missingInfo.Path = path;
            missingInfo.MissingDatas = new List<MissingData>();

            var components = prefab.GetComponentsInChildren<Component>(true);

            foreach (var component in components)
            {
                if (component == null)
                {
                    Debug.LogError($"Prefab has missing scripts,Name:{prefab.name}.");
                    continue;
                }

                SerializedObject serializedObject = new SerializedObject(component);

                SerializedProperty serializedProperty = serializedObject.GetIterator();
                while (serializedProperty.NextVisible(true))
                {
                    if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                    {
                        // sheder 需要特殊处理下
                        if (serializedProperty.displayName == "Shader" && serializedProperty.objectReferenceValue == null)
                        {
                            missingInfo.MissingDatas.Add(new MissingData()
                            {
                                PropertyName = serializedProperty.propertyPath,
                            });

                            Debug.Log($"{component.name} 引用丢失\n路径：{AssetDatabase.GetAssetPath(component)}\n丢失属性：{serializedProperty.propertyPath}");
                            continue;
                        }
                        //引用对象是null 并且 引用ID不是0 说明丢失了引用
                        if (serializedProperty.objectReferenceValue == null && serializedProperty.objectReferenceInstanceIDValue != 0)
                        {
                            missingInfo.MissingDatas.Add(new MissingData()
                            {
                                PropertyName = serializedProperty.propertyPath,
                                InstanceID = serializedProperty.objectReferenceInstanceIDValue,
                            });

                            Debug.Log($"{component.name} 引用丢失\n路径：{AssetDatabase.GetAssetPath(component)}\n丢失属性：{serializedProperty.propertyPath}\n引用id：{serializedProperty.objectReferenceInstanceIDValue}");
                        }
                    }
                }
            }
            return missingInfo;
        }

        public static MissingInfo GetMissingObjectReference__(Object obj)
        {
            GameObject gameObject = obj as GameObject;
            if (gameObject != null)
            {
                return FindGameObject(gameObject);
            }

            Texture2D texture2D = obj as Texture2D;
            if (texture2D != null)
            {

                return null;
            }

            Sprite sprite = obj as Sprite;
            if (sprite != null)
            {

                return null;
            }

            return null;
        }

        public static MissingInfo FindGameObject(GameObject gameObject)
        {

            MissingInfo missingInfo = new MissingInfo();
            missingInfo.MissingObject = gameObject;
            missingInfo.MissingDatas = new List<MissingData>();

            var components = gameObject.GetComponentsInChildren<Component>(true);

            foreach (var component in components)
            {
                if (component == null)
                {

                    missingInfo.MissingDatas.Add(new MissingData()
                    {
                        PropertyName = "Component missing"
                    });

                    Debug.LogWarningFormat("Prefab has missing scripts,Name:{0}.", gameObject.name);
                }
                else
                {
                    SerializedObject serializedObject = new SerializedObject(component);

                    SerializedProperty serializedProperty = serializedObject.GetIterator();
                    while (serializedProperty.NextVisible(true))
                    {
                        if (serializedProperty.propertyType == SerializedPropertyType.ObjectReference)
                        {
                            //引用对象是null 并且 引用ID不是0 说明丢失了引用
                            if (serializedProperty.objectReferenceValue == null && serializedProperty.objectReferenceInstanceIDValue != 0)
                            {
                                missingInfo.MissingDatas.Add(new MissingData()
                                {
                                    PropertyName = serializedProperty.propertyPath,
                                    InstanceID = serializedProperty.objectReferenceInstanceIDValue,
                                });

                                Debug.LogWarningFormat("{0} 引用丢失\n\n丢失属性：{1}\n引用id：{2}", component.name, serializedProperty.propertyPath, serializedProperty.objectReferenceInstanceIDValue);
                            }
                        }
                    }
                }
            }
            return missingInfo;
        }

        private static bool IsIgnore(string path)
        {
            foreach (var ignor in ignores)
            {
                if (path.EndsWith(ignor))
                {
                    return true;
                }
            }
            return false;
        }

        #region Data Class

        [System.Serializable]
        public class MissingData
        {
            public string PropertyName;
            public int InstanceID;
        }

        [System.Serializable]
        public class MissingInfo
        {
            public Object MissingObject;
            public string Path;
            public List<MissingData> MissingDatas;
        }
        #endregion    
    }
}