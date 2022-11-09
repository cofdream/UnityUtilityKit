using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    //TODO 想实现 打开一个窗口里面都是继承 ScriptableObject 的类脚本文件，然后创建一个对应对象的.asset 文件。
    // 问题
    // .cs文件内可以有多个class  导致难以判断
    //EditorGUIUtility.ShowObjectPicker<MonoScript>(target, false, "", GUIUtility.GetControlID(FocusType.Keyboard));


    /// <summary>
    /// 快速创建一个.asset文件 而不需要写对应的创建脚本
    /// </summary>
    [CreateAssetMenu(menuName = "Asset", fileName = "New Asset", order = 102)]
    internal sealed class CreateAsset : UnityEngine.ScriptableObject { }


    [CustomEditor(typeof(CreateAsset), false)]
    internal sealed class CreateAssetInspector : Editor
    {
        SerializedProperty serializedProperty;
        private void OnEnable()
        {
            serializedProperty = serializedObject.FindProperty("m_Script");
        }

        public override void OnInspectorGUI()
        {
            //通过修改Script的值，来快速创建一个.asset文件 而不需要写对应的创建脚本
            GUILayout.Label("通过修改Script的值，达到快速创建Asset的目的", EditorStyles.boldLabel);

            serializedObject.Update();

            EditorGUILayout.PropertyField(serializedProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}