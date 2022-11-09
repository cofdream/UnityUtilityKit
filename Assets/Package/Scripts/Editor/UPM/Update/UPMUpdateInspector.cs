using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor.UPM
{
    [CustomEditor(typeof(UPMUpdate))]
    [CanEditMultipleObjects]
    public class UPMUpdateInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            bool isExecute = true;
            foreach (var item in targets)
            {
                var upmUpdate = item as UPMUpdate;
                if (upmUpdate.IsExecute() == false)
                {
                    EditorGUILayout.HelpBox("UPMUpdate not execute.", MessageType.Warning);//want 改成字段高亮
                    isExecute = false;
                    continue;
                }
            }

            var enabled = GUI.enabled;
            GUI.enabled = isExecute;
            if (GUILayout.Button(" Execute ", GUILayout.ExpandWidth(false)))
            {
                foreach (var item in targets)
                {
                    var upmUpdate = item as UPMUpdate;
                    upmUpdate?.Execute();
                }
            }
            GUI.enabled = enabled;
        }
    }
}