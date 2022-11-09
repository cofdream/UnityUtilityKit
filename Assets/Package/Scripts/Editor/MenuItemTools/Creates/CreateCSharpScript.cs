using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEditor.ProjectWindowCallback;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor.Compilation;

namespace Cofdream.ToolKitEditor
{
    public static class CreateCSharpScript
    {
        /// <summary>
        /// 创建一个自定义内容的 C# 脚本
        /// </summary>
        [MenuItem("Assets/Create/C# Scripts/New Class", false, 103)]
        private static void CreatNewClass()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<CreateCSarpScriptAction>(), "NewClass", EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                "using System.Collections;" +
                "\nusing System.Collections.Generic;" +
                "\nusing UnityEngine;" +
                "\n" +
                "\nnamespace #Namespace#" +
                "\n{" +
                "\n\tpublic class #ScriptName#" +
                "\n\t{" +
                "\n\t\t" +
                "\n\t}" +
                "\n}");
        }

        /// <summary>
        /// 创建一个自定义内容的 C# 编辑器脚本
        /// </summary>
        [MenuItem("Assets/Create/C# Scripts/New Editor", false, 103)]
        private static void CreateNewEditorClass()
        {
            ProjectWindowUtil.StartNameEditingIfProjectWindowExists(0, ScriptableObject.CreateInstance<CreateCSarpScriptAction>(), "NewEditor", EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D,
                "using System.Collections;" +
                "\nusing System.Collections.Generic;" +
                "\nusing UnityEngine;" +
                "\nusing UnityEditor;" +
                "\n" +
                "\nnamespace #Namespace#" +
                "\n{" +
                "\n\tpublic class #ScriptName# : EditorWindow" +
                "\n\t{" +
                "\n\t\t" +
                "\n\t}" +
                "\n}");
        }

        private sealed class CreateCSarpScriptAction : EndNameEditAction
        {
            public override void Action(int instanceId, string pathName, string resourceFile)
            {
                string scriptName = Path.GetFileName(pathName);

                var encoding = new UTF8Encoding(encoderShouldEmitUTF8Identifier: true);
                string content = resourceFile.Replace("#ScriptName#", scriptName);


                content = content.Replace("#Namespace#", GetScritpNamespace(pathName));

                content = SetLineEndings(content, LineEndingsMode.OSNative);

                pathName += ".cs";
                File.WriteAllText(pathName, content, encoding);

                AssetDatabase.ImportAsset(pathName);
            }

            private string SetLineEndings(string content, LineEndingsMode lineEndingsMode)
            {
                string replacement;
                switch (lineEndingsMode)
                {
                    case LineEndingsMode.OSNative:
                        replacement = ((Application.platform != RuntimePlatform.WindowsEditor) ? "\n" : "\r\n");
                        break;
                    case LineEndingsMode.Unix:
                        replacement = "\n";
                        break;
                    case LineEndingsMode.Windows:
                        replacement = "\r\n";
                        break;
                    default:
                        replacement = "\n";
                        break;
                }

                content = Regex.Replace(content, "\\r\\n?|\\n", replacement);
                return content;
            }

            private string GetScritpNamespace(string scriptPath)
            {
#if UNITY_2020_3_OR_NEWER
                return CompilationPipeline.GetAssemblyRootNamespaceFromScriptPath(scriptPath);
#else
                return Application.productName;
#endif

            }
        }
    }
}