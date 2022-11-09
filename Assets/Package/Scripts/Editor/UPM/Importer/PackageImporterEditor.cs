using System.IO;
using UnityEditor;
using UnityEngine;
#if UNITY_2020_3_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Localization.Editor;
using UnityEditor.Experimental.AssetImporters;
#endif

namespace Cofdream.ToolKitEditor.UPM
{
    [CustomEditor(typeof(PackageImporter))]
    [CanEditMultipleObjects]
    public class PackageImporterEditor : ScriptedImporterEditor, IHasCustomMenu
    {
        public override bool showImportedObject => true;
        protected override bool needsApplyRevert => base.needsApplyRevert;
        protected override System.Type extraDataType => typeof(PackageManifest);

        private PackageManifest packageManifest;

        private SerializedProperty nameSP;
        private SerializedProperty version;
        private SerializedProperty displayName;
        private SerializedProperty description;
        private SerializedProperty unity;

        private Styles styles;


        public void AddItemsToMenu(GenericMenu menu)
        {
            menu.AddItem(new GUIContent("Edit Script"), false, () =>
            {
                SerializedObject serializedObject = new SerializedObject(this);
                serializedObject.Update();
                SerializedProperty m_Script = serializedObject.FindProperty("m_Script");
                AssetDatabase.OpenAsset(m_Script.objectReferenceValue.GetInstanceID());
            });
        }

        protected override void Awake()
        {
            base.Awake();
        }

        protected override void InitializeExtraDataInstance(Object extraData, int targetIndex)
        {
            var assetPath = ((AssetImporter)targets[targetIndex]).assetPath;
            var json = File.ReadAllText(assetPath);
            var package = JsonUtility.FromJson<Package>(json);

            packageManifest = (PackageManifest)extraData;
            packageManifest.Package = package;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            if (styles == null)
                styles = new Styles();

            nameSP = extraDataSerializedObject.FindProperty("Package.name");
            version = extraDataSerializedObject.FindProperty("Package.version");
            displayName = extraDataSerializedObject.FindProperty("Package.displayName");
            description = extraDataSerializedObject.FindProperty("Package.description");
            unity = extraDataSerializedObject.FindProperty("Package.unity");
        }

        public override void OnDisable()
        {
            base.OnDisable();
        }

        protected override void OnHeaderGUI()
        {
            base.OnHeaderGUI();
        }

        public override void OnInspectorGUI()
        {
            if (nameSP == null)
            {
                Debug.LogError("PackageManifest 字段 Package 获取不到!");
                return;
            }

            bool change;
            using (new LocalizationGroup(/*target*/this))
            {
                change = DoDrawDefaultInspector(serializedObject);

                bool DoDrawDefaultInspector(SerializedObject obj)
                {
                    EditorGUI.BeginChangeCheck();
                    obj.UpdateIfRequiredOrScript();
                    SerializedProperty iterator = obj.GetIterator();
                    bool enterChildren = true;
                    while (iterator.NextVisible(enterChildren))
                    {
                        using (new EditorGUI.DisabledScope("m_Script" == iterator.propertyPath))
                        {
                            EditorGUILayout.PropertyField(iterator, true);
                        }
                        enterChildren = false;
                    }

                    obj.ApplyModifiedProperties();
                    return EditorGUI.EndChangeCheck();
                }
            }

            GUILayout.Label(styles.information, EditorStyles.boldLabel);

            extraDataSerializedObject.Update();

            // Package information
            using (new EditorGUILayout.VerticalScope(GUI.skin.box, GUILayout.ExpandWidth(true)))
            {
                EditorGUILayout.DelayedTextField(nameSP, styles.name);

                EditorGUILayout.DelayedTextField(version, styles.version);

                EditorGUILayout.DelayedTextField(displayName, styles.displayName);
                EditorGUILayout.DelayedTextField(description, styles.description);
                EditorGUILayout.DelayedTextField(unity, styles.unity);
            }


            if (GUILayout.Button("Add Small Version", GUILayout.ExpandWidth(false)))
                packageManifest.AddSmallVersion();

            extraDataSerializedObject.ApplyModifiedProperties();

            ApplyRevertGUI();
        }

        protected override void Apply()
        {
            base.Apply();
            // After the Importer is applied, rewrite the file with the custom value.
            for (int i = 0; i < targets.Length; i++)
            {
                var packageManifest = JsonUtility.ToJson(((PackageManifest)extraDataTargets[i]).Package, true);
                string path = ((AssetImporter)targets[i]).assetPath;
                File.WriteAllText(path, packageManifest);
            }
        }



        private class Styles
        {
            public readonly GUIContent information = EditorGUIUtility.TrTextContent("Information");
            public readonly GUIContent name = EditorGUIUtility.TrTextContent("Name", "Package name. Must be lowercase");
            // public readonly GUIContent organizationName = EditorGUIUtility.TrTextContent("Organization name", "Package organization name. Must be lowercase and not include dots '.'");
            public readonly GUIContent displayName = EditorGUIUtility.TrTextContent("Display name", "Display name used in UI.");
            public readonly GUIContent version = EditorGUIUtility.TrTextContent("Version", "Package Version, much follow SemVer (ex: 1.0.0-preview.1).");
            // public readonly GUIContent type = EditorGUIUtility.TrTextContent("Type", "Package Type (optional).");

            // public readonly GUIContent showAdvanced = EditorGUIUtility.TrTextContent("Advanced", "Show advanced settings.");

            // public readonly GUIContent visibility = EditorGUIUtility.TrTextContent("Visibility in Editor", "Package visibility in Editor.");

            public readonly GUIContent unity = EditorGUIUtility.TrTextContent("Minimal Unity Version");
            // public readonly GUIContent unityMajor = EditorGUIUtility.TrTextContent("Major", "Major version of Unity");
            // public readonly GUIContent unityMinor = EditorGUIUtility.TrTextContent("Minor", "Minor version of Unity");
            // public readonly GUIContent unityRelease = EditorGUIUtility.TrTextContent("Release", "Specific release (ex: 0a9)");

            public readonly GUIContent description = EditorGUIUtility.TrTextContent("Brief Description");

            // public readonly GUIContent dependencies = EditorGUIUtility.TrTextContent("Dependencies");
            // public readonly GUIContent package = EditorGUIUtility.TrTextContent("Package name", "Package name. Must be lowercase");

            // public readonly GUIContent viewInPackageManager = EditorGUIUtility.TrTextContent("View in Package Manager");
        }
    }
}