using Cofdream.ToolKit;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Cofdream.ToolKitEditor
{
    [CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyDrawer : PropertyDrawer
    {
        //public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        //{
        //    return EditorGUI.GetPropertyHeight(property, label, true);
        //}
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enabled = GUI.enabled;
            GUI.enabled = false;
            base.OnGUI(position, property, label);
            //EditorGUI.PropertyField(position, property, label);
            GUI.enabled = enabled;
        }
    }
}