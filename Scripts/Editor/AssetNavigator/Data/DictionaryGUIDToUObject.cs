using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor.AssetNavigator
{
    public class DictionaryGUIDToUObject : SerializableDictionary<GUID, AssetNavigatorMenuContet>
    {
        
    }

    //[CustomPropertyDrawer(typeof(SerializableDictionary<GUID, AssetNavigatorMenuContet>))]
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