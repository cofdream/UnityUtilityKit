using System;
using System.Reflection;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    public static class Utils
    {
        public static string PackageIconPath = "Assets/Resources/GirlHead.png";


        /// <summary>
        /// 获取Project目录下选中对象的 文件夹 的路径
        /// </summary>
        /// <returns>返回 文件夹 的路径数组</returns>
        public static string[] GetSelectionFoldePaths()
        {
            var guids = UnityEditor.Selection.assetGUIDs;
            if (guids == null) return null;

            for (int i = 0; i < guids.Length; i++)
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
                if (System.IO.Directory.Exists(path) == false)
                {
                    int index = path.LastIndexOf('/');
                    path = path.Substring(0, index);
                }
                guids[i] = path;
            }
            return guids;
        }
        /// <summary>
        /// 获取Project目录下选中的路径
        /// </summary>
        /// <returns>返回 文件夹 或 文件 的路径数组</returns>
        public static string[] GetSelectionObjectPaths()
        {
            var guids = UnityEditor.Selection.assetGUIDs;
            if (guids == null) return null;

            for (int i = 0; i < guids.Length; i++)
            {
                guids[i] = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[i]);
            }

            return guids;
        }

        public static string GetSelectedPath(string pathName)
        {
            var adType = typeof(AssetDatabase);
            var editorAssmbly = adType.Assembly;

            var pbType = editorAssmbly.GetType("UnityEditor.ProjectBrowser");
            var fileInfo = pbType.GetField("s_LastInteractedProjectBrowser", BindingFlags.Static | BindingFlags.Public);
            var pbInstance = fileInfo.GetValue(null);
            if (pbInstance != null)
            {
                var validateCreateNewAssetPath_methodInfo = pbType.GetMethod("ValidateCreateNewAssetPath", BindingFlags.NonPublic | BindingFlags.Instance);

                var objsArg = new object[] { pathName };

                // 获取默认路径
                string path = validateCreateNewAssetPath_methodInfo.Invoke(pbInstance, objsArg) as string;
                // 在获取常规路径
                if (!path.StartsWith("assets/", System.StringComparison.CurrentCultureIgnoreCase))
                {
                    var methodInfo = adType.GetMethod("GetUniquePathNameAtSelectedPath", BindingFlags.Static | BindingFlags.NonPublic);
                    objsArg[0] = path;
                    path = methodInfo.Invoke(null, objsArg) as string;
                }
                //else
                //{
                //    //不一定需要
                //    path = AssetDatabase.GenerateUniqueAssetPath(path);
                //}
                return path;
            }
            else
            {
                string path = pathName;
                if (!pathName.StartsWith("assets/", StringComparison.CurrentCultureIgnoreCase))
                    path = "Assets/" + pathName;
                return path;
            }
        }

        /// <summary>
        /// 获取Project目录下选中对象的 文件夹 的路径
        /// </summary>
        /// <returns>返回 文件夹 的路径</returns>
        public static string GetSelectionFoldePath()
        {
            var guids = UnityEditor.Selection.assetGUIDs;
            if (guids == null) return null;

            string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            if (System.IO.Directory.Exists(path) == false)
            {
                int index = path.LastIndexOf('/');
                path = path.Substring(0, index);
            }
            return path;
        }
        /// <summary>
        /// 获取Project目录下选中的路径
        /// </summary>
        /// <returns>返回 文件夹 或 文件 的路径</returns>
        public static string GetSelectionObjectPath()
        {
            var guids = UnityEditor.Selection.assetGUIDs;
            if (guids == null) return null;

            return UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
        }

        public static void OpenDirectory(string path, bool useCMD = true)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                UnityEngine.Debug.Log($"{path} 是Null or WhiteSpace");
            }
            else
            {
                path = path.Replace("/", "\\");//反转 防止找不到文件
                if (System.IO.Directory.Exists(path))
                {
                    path = $"{path}";//路径存在空格会打开错误,加双引号可以解决这个问题
                    if (useCMD)
                        OpenDirectoryByCMD(path);
                    else
                        OpenDirectoryByEXPER(path); //Window10 无法打开不清楚问题
                }
                else
                {
                    UnityEngine.Debug.LogWarning($"{path} 不是文件夹夹路径");
                }
            }
        }
        private static void OpenDirectoryByEXPER(string path)
        {
            System.Diagnostics.Process.Start("exper.exe", path);
        }
        private static void OpenDirectoryByCMD(string path)
        {
            // 新开线程防止锁死
            var newThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(CmdOpenDirectory));
            newThread.Start(path);
        }
        private static void CmdOpenDirectory(object obj)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.Arguments = "/c start " + obj.ToString();
            process.StartInfo.StandardOutputEncoding = System.Text.Encoding.GetEncoding("GB2312");

            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();

            process.WaitForExit();

            UnityEngine.Debug.Log(process.StandardOutput.ReadToEnd());

            process.Close();
        }



        public static string GetPackageRootPath()
        {
            if (System.IO.Directory.Exists("Packages/com.cofdream.toolkit"))
            {
                return "Packages/com.cofdream.toolkit";
            }
            else
            {
                return "Assets/Package";
            }
        }

    }
}