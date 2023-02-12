using Cofdream.ToolKitEditor.AssetNavigator;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    [Serializable]
    public abstract class SerializableDictionary<TKey, TValue> : ScriptableObject
    {
        [SerializeField]
        private TKey[] _keys;
        [SerializeField]
        private TValue[] _values;
        public Dictionary<TKey, TValue> Dictionary { get; private set; }

        private void OnEnable()
        {
            if (Dictionary == null)
                Dictionary = new Dictionary<TKey, TValue>();
            else
                Dictionary.Clear();

            if (_keys != null && _values != null)
            {
                int length = _keys.Length < _values.Length ? _keys.Length : _values.Length;
                for (int i = 0; i < length; i++)
                {
                    Dictionary.Add(_keys[i], _values[i]);
                }
            }
        }

        private void OnDisable()
        {
            //if (Dictionary == null)
            //    Dictionary = new Dictionary<TKey, TValue>();

            _keys = Dictionary.Keys.ToArray();
            _values = Dictionary.Values.ToArray();
            Dictionary.Clear();

            CustomAssetModificationProcessor.SaveAssetIfDirty(this);
        }
    }
}