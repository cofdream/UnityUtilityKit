using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public class ScriptTemplatesTool
    {
        const int priority = 2000;//不传参数为1000

        /// <summary>
        /// 删除脚本模板末尾的空格
        /// </summary>
        [MenuItem(MenuItemName.ToolKit + "Remove Script Templates End Wrap", false, priority)]
        public static void RemoveScriptTemplatesEndWrap()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                string path = Directory.GetParent(Process.GetCurrentProcess().MainModule.FileName).FullName + @"\Data\Resources\ScriptTemplates";
                var files = Directory.GetFiles(path, "*.txt");

                var sourceBackupPath = path + "/SourceBackup";

                var crlf = "\n";
                foreach (var file in files)
                {
                    var content = File.ReadAllText(file);
                    if (content.EndsWith(crlf))
                    {
                        if (Directory.Exists(sourceBackupPath) == false)
                        {
                            UnityEngine.Debug.Log("创建源备份文件夹");
                            Directory.CreateDirectory(sourceBackupPath);
                        }

                        File.Move(file, sourceBackupPath + "/" + Path.GetFileName(file));
                        content = content.Remove(content.Length - 1, 1);
                        File.WriteAllText(file, content);

                        UnityEngine.Debug.Log("已移除脚本模板末尾的换行符，路径：" + file);
                    }
                }
            }
        }

        /// <summary>
        /// 还原 删除脚本模板末尾的空格
        /// </summary>
        [MenuItem(MenuItemName.ToolKit + "Reduction Remove Script Templates End Wrap", false, priority + 1)]
        public static void ReductionRemoveScriptTemplatesEndWrap()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                string path = Directory.GetParent(Process.GetCurrentProcess().MainModule.FileName).FullName + @"\Data\Resources\ScriptTemplates";
                var sourceBackupPath = path + "/SourceBackup";

                if (Directory.Exists(sourceBackupPath))
                {
                    var files = Directory.GetFiles(sourceBackupPath, "*.txt");

                    foreach (var file in files)
                    {
                        var lastPath = path + "/" + Path.GetFileName(file);
                        File.Delete(lastPath);
                        File.Move(file, lastPath);
                        UnityEngine.Debug.Log("已还原脚本模板末尾的换行符，路径：" + file);
                    }

                    if (Directory.GetFiles(sourceBackupPath).Length == 0)
                    {
                        Directory.Delete(sourceBackupPath);
                        UnityEngine.Debug.Log("删除源备份文件夹");
                    }
                }
            }
        }
    }
}