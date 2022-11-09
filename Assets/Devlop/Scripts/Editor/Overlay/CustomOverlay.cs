using System.Collections;
using System.Collections.Generic;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace Cofdream.Test.Editor
{
    [Overlay(typeof(SceneView), "Selection Count")]
    class SelectionCount : Overlay
    {
        Label m_Label;

        public override VisualElement CreatePanelContent()
        {
            Selection.selectionChanged += () =>
            {
                if (m_Label != null)
                    m_Label.text = $"Selection Count {Selection.count}";
            };

            return m_Label = new Label($"Selection Count {Selection.count}");
        }
    }

    [Overlay(typeof(TestWindow), "10", "Custom Overlay")]
    public class CustomOverlay : Overlay
    {

        Label m_Label;
        public override VisualElement CreatePanelContent()
        {
            Selection.selectionChanged += () =>
            {
                if (m_Label != null)
                    m_Label.text = $"Selection Count {Selection.count}";
            };

            return m_Label = new Label($"Selection Count {Selection.count}");
        }
    }

    public class TestWindow : EditorWindow, ISupportsOverlays
    {
        [MenuItem("Test/wwwww")]
        static void Open()
        {
            GetWindow<TestWindow>().Show();
        }

        private void OnGUI()
        {
            GUILayout.Label("11");
        }
    }
}