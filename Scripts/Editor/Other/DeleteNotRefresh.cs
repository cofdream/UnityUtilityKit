using UnityEngine;
using UnityEditor;
using System.IO;
using System.Text;

namespace Cofdream.ToolKitEditor
{
    internal static class DeleteNotRefresh
    {
        [MenuItem("Assets/Delete Not Refresh")]
        private static void Delete()
        {
            var paths = Utility.GetSelectionObjectPaths();

            string content = string.Empty;
            int length = paths.Length;
            for (int i = 0; i < length; i++)
            {
                if (i == 3)
                {
                    content += "...\n";
                    break;
                }

                content += paths[i] + "\n";
            }
            content += "You cannot undo this action.";

            if (!EditorUtility.DisplayDialog("Delete select assets?", content, "Delete", "Cancel"))
            {
                return;
            }

            foreach (var item in paths)
            {
                if (File.Exists(item))
                {
                    File.Delete(item);
                }
                else
                {
                    if (Directory.Exists(item))
                    {
                        Directory.Delete(item, true);
                    }
                }

                string meta = item + ".meta";
                if (File.Exists(meta))
                {
                    File.Delete(meta);
                }
            }
        }

        [MenuItem("Assets/Delete Not Refresh", true)]
        private static bool IsDelete()
        {
            var paths = Utility.GetSelectionObjectPaths();
            if (paths.Length == 0 || paths.Length != Selection.objects.Length)
            {
                return false;
            }
            foreach (var item in paths)
            {
                if (!item.StartsWith("Assets/"))
                {
                    return false;
                }
            }
            return true;
        }
    }
}