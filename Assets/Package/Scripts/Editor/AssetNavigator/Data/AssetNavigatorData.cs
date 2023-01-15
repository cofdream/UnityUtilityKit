using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public class AssetNavigatorData : ScriptableObject
    {
        
        [HideInInspector]
        public EditorSearchField _searchField;

        public DictionaryGUIDToUObject _selectedObjects;


        private void Awake()
        {
            _searchField = new EditorSearchField();
            if (_selectedObjects == null)
            {
                _selectedObjects = CreateInstance<DictionaryGUIDToUObject>();
            }
        }

        private void OnDestroy()
        {
            _searchField = null;
        }
    }
}