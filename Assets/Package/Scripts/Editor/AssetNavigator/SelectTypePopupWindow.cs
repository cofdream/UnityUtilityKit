using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor.AssetNavigator
{
    
    public interface IMenuContet
    {
        public GUIContent GUIContent { get; }
        public void OnGUI(Rect rect);
    }

    public struct MenuContet : IMenuContet
    {
        public bool IsOn;
        public string Text;
        public GUIContent GUIContent { get; }

        public MenuContet(GUIContent content)
        {
            IsOn = false;
            Text = content.text;
            GUIContent = content;
        }

        public void OnGUI(Rect rect)
        {
            IsOn = EditorGUILayout.ToggleLeft(GUIContent, IsOn);
        }
    }

    public class SelectTypePopupWindow<T> : PopupWindowContent where T : IMenuContet
    {
        private EditorSearchField _editorSearchField;

        private List<T> _searchResults;

        private List<T> _searchSources;

        public SelectTypePopupWindow(List<T> searchSource)
        {
            if (searchSource == null)
                _searchSources = new List<T>();
            else
                _searchSources = searchSource;

            _searchResults = _searchSources;
        }

        public override Vector2 GetWindowSize()
        {
            return new Vector2(200, 150);
        }

        public override void OnGUI(Rect rect)
        {
            _editorSearchField.ToolbarSearchField();
            if (string.IsNullOrWhiteSpace(_editorSearchField.SearchTerm) == false)
            {

            }

            var length = _searchResults.Count;
            for (int i = 0; i < length; i++)
            {
                var menuContet = _searchResults[i];
                menuContet.OnGUI(rect);
                _searchResults[i] = menuContet;
            }

        }

        public override void OnOpen()
        {
            _editorSearchField = new EditorSearchField();

        }

        public override void OnClose()
        {
            _editorSearchField = null;
        }
    }
}