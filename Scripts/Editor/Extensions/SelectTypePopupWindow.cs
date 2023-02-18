using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor.AssetNavigator
{

    public interface IMenuContent
    {
        GUIContent GUIContent { get; }
        void OnGUI(Rect rect);
    }

    public class SelectTypePopupWindow : PopupWindowContent
    {
        private EditorSearchField _editorSearchField;

        private List<IMenuContent> _searchResults;

        private IMenuContent[] _searchSources;

        public SelectTypePopupWindow(IEnumerable<IMenuContent> searchSource)
        {
            _editorSearchField = new EditorSearchField();

            _searchResults = new List<IMenuContent>();
            foreach (var item in searchSource)
            {
                _searchResults.Add(item);
            }
            _searchSources = _searchResults.ToArray();
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
            
        }

        public override void OnClose()
        {
            
        }

        public static void DisplayPropWindow<T>(Rect rect, IEnumerable<T> data) where T : class, IMenuContent
        {
            PopupWindow.Show(rect, new SelectTypePopupWindow(data));
        }
    }
}