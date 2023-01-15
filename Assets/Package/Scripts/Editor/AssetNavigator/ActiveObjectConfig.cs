//using UnityEngine;
//using UnityEditor;
//using System.Collections.Generic;
//using System;

//    public static class ActiveObjectConfig
//    {
//        public const uint MIN_CAPACITY = 10;
//        public const uint MAX_CAPACITY = 1000;

//        private static List<ActiveObjectType> _allTypes;
//        public static List<ActiveObjectType> allTypes
//        {
//            get
//            {
//                if (_allTypes == null)
//                    Init();
//                return _allTypes;
//            }
//        }

//        private static void Init()
//        {
//            _allTypes = new List<ActiveObjectType>();

//            _allTypes.Add(new ActiveObjectType("prefab", typeof(GameObject), "PrefabNormal Icon"));
//            _allTypes.Add(new ActiveObjectType("script", typeof(MonoScript), "cs Script Icon"));
//            _allTypes.Add(new ActiveObjectType("texture", typeof(Texture2D), "Texture Icon"));
//            _allTypes.Add(new ActiveObjectType("folder", typeof(DefaultAsset), "Folder Icon"));
//            _allTypes.Add(new ActiveObjectType("scene", typeof(SceneAsset), "SceneAsset Icon"));
//            _allTypes.Add(new ActiveObjectType("text", typeof(TextAsset), "TextAsset Icon"));
//            _allTypes.Add(new ActiveObjectType("animationClip", typeof(AnimationClip), "AnimationClip Icon"));
//            _allTypes.Add(new ActiveObjectType("animatorController", typeof(UnityEditor.Animations.AnimatorCondition), "AnimatorController Icon"));
//            _allTypes.Add(new ActiveObjectType("shader", typeof(Shader), "Shader Icon"));
//            _allTypes.Add(new ActiveObjectType("material", typeof(Material), "Material Icon"));
//            _allTypes.Add(new ActiveObjectType("audio", typeof(AudioClip), "AudioClip Icon"));
//        }

//        public static List<string> GetDefaultMenu()
//        {
//            List<string> menu = new List<string>();
//            menu.Add("prefab");
//            menu.Add("script");
//            menu.Add("texture");
//            menu.Add("folder");
//            return menu;
//        }
//    }

//    [Serializable]
//    public class ActiveObjectType
//    {
//        public string typeName;
//        public Type type;
//        public GUIContent gUIContent;
//        public bool isOn;

//        public ActiveObjectType(string pTypeName, Type pType, string pIconName)
//        {
//            typeName = pTypeName;
//            type = pType;
//            gUIContent = EditorGUIUtility.IconContent(pIconName);
//            gUIContent.text = pTypeName;
//        }
//    }