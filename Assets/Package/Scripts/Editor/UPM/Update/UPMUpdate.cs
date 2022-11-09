using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor.UPM
{
    public class UPMUpdate : ScriptableObject
    {
        [SerializeField] private PackageManifest packageManifest;
        [SerializeField] private UPMCommand[] commands;

        public bool IsExecute()
        {
            return packageManifest != null;
        }
        public void Execute()
        {
            var processCommand = new ProcessCommand();

            var version = packageManifest.Package.version;
            var packagePath = AssetDatabase.GetAssetPath(packageManifest).Replace("/package.json", string.Empty);

            int length = commands.Length;
            for (int i = 0; i < length; i++)
            {
                var gitCommand = commands[i];

                EditorUtility.DisplayProgressBar(/*this.name + */"UPM Update  " + gitCommand.Name, gitCommand.Command, (float)i / commands.Length);

                if (gitCommand.State == false) continue;

                string cmd;
                if (packageManifest != null)
                {
                    cmd = gitCommand.Command.Replace("#version#", version).Replace("#PackagePath#", packagePath);
                }
                else
                    cmd = gitCommand.Command;

                processCommand.Cmd = cmd;
                processCommand.Execute();

                Debug.Log(processCommand.Log);
            }

            EditorUtility.ClearProgressBar();
        }
    }
}