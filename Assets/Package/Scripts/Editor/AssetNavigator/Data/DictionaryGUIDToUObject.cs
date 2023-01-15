using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public class DictionaryGUIDToUObject : SerializableDictionary<GUID, Object>
    {
        
    }

    [CustomPropertyDrawer(typeof(SerializableDictionary<GUID, Object>))]
    public class SSS : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            base.OnGUI(position, property, label);

            //position.y += 20;
            //EditorGUI.LabelField(position, "1213");
        }
    }
}