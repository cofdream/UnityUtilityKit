using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public sealed class CopyComponentTool 
    {
        public string ToolPath => "CopyComponentTool";

        public GameObject copyTarget;
        public GameObject pasteTarget;

        public GUIContent NullGUICOntent;

        private bool isResetComponent;
        private bool isPasteComponent;
        private bool isChange;

        private bool[] isCopy;
        private Component[] copyComponents;
        private Component[] pasteComponents;


        private void Awake()
        {
            NullGUICOntent = new GUIContent();
        }

        public void AwakeTool() { }
        public void OnDestroyTool() { }

        public void OnGUITool(Rect rect)
        {
            // TODO 
            // 目前只实现简单的值 拷贝
            // 拷贝对象的引用类型必须不受影响

            isResetComponent = GUILayout.Button("Rest Get Component Info");

            isPasteComponent = GUILayout.Button("Paste Select Component Value");

            isChange = GUILayout.Button("Change");

            GUILayout.BeginHorizontal();
            {
                GUILayout.BeginVertical();
                {
                    ShowComponent(copyTarget, ref copyComponents);
                }
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                {
                    if (isResetComponent)
                    {
                        if (copyComponents != null)
                        {
                            isCopy = new bool[copyComponents.Length];
                            for (int i = 0; i < isCopy.Length; i++)
                            {
                                isCopy[i] = true;
                            }
                        }
                    }
                    if (isCopy != null)
                    {
                        GUILayout.Label("拷贝");

                        for (int i = 0; i < isCopy.Length; i++)
                        {
                            isCopy[i] = GUILayout.Toggle(isCopy[i], string.Empty);
                        }
                    }
                }
                GUILayout.EndVertical();

                GUILayout.FlexibleSpace();

                GUILayout.BeginVertical();
                {
                    ShowComponent(pasteTarget, ref pasteComponents);
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            if (isChange)
            {
                var tempComponents = copyComponents;
                copyComponents = pasteComponents;
                pasteComponents = tempComponents;
            }

            if (isPasteComponent)
            {
                int length = copyComponents.Length;

                for (int i = 0; i < length; i++)
                {
                    if (isCopy[i])
                    {
                        UnityEditorInternal.ComponentUtility.CopyComponent(copyComponents[i]);

                        UnityEditorInternal.ComponentUtility.PasteComponentValues(pasteComponents[i]);
                    }
                }
            }

            GetCopyTarget();
        }

        private void ShowComponent(GameObject target, ref Component[] components)
        {
            target = (GameObject)EditorGUILayout.ObjectField("1", target, typeof(GameObject), true);

            if (isResetComponent)
            {
                if (target != null)
                {
                    components = target.GetComponents<Component>();
                }
            }

            if (components != null)
            {
                foreach (var component in components)
                {
                    GUILayout.Label(component.ToString());
                }
            }
        }

        private void GetCopyTarget()
        {
            if (Selection.activeGameObject != null)
            {
                //Debug.Log("One:" + Selection.activeGameObject.name);
            }
            if (Selection.gameObjects != null)
            {
                foreach (var item in Selection.gameObjects)
                {
                    //Debug.Log("Two" + item.name);
                }
            }
        }

        public void OnEnableTool()
        {
            throw new System.NotImplementedException();
        }

        public void OnDisableTool()
        {
            throw new System.NotImplementedException();
        }
    }
}