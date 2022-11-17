using System.IO;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    public partial class CustomAssetModificationProcessor
#if UNITY_2019_OR_NEWER
        : UnityEditor.AssetModificationProcessor
#else
        : AssetModificationProcessor
#endif
    {
        // 回调规则
        // 只要返回了 DidMove ，Unity就会执行具体的操作
        // 返回值不存在 DidMove，但是最少有一个 FailedMove。操作为修改名字，Unity 提示警告。 操作为移动文件，Unity没有提示警告

        public delegate CustomAssetModificationProcessor Processor(string sourcePath, string destinationPath);
        public static Processor CreateProcessor;

        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            return (CreateProcessor == null ? new CustomAssetModificationProcessor(sourcePath, destinationPath) : CreateProcessor(sourcePath, destinationPath)).OnWillMoveAsset();
        }


        protected readonly string sourcePath;
        protected readonly string destinationPath;
        protected CustomAssetModificationProcessor(string sourcePath, string destinationPath)
        {
            this.sourcePath = sourcePath;
            this.destinationPath = destinationPath;
        }

        protected virtual AssetMoveResult OnWillMoveAsset()
        {

            UnityEngine.Debug.Log($"{sourcePath}\n{destinationPath}");

            AssetMoveResult result = AssetMoveResult.DidNotMove;

            if (CheckFolderMove(ref result))
                return result;
            else if (CheckFileMove(ref result))
                return result;

            return result;

            #region old code

            //if (ReloadAssembliesSetting.LockReloadAssemblies) return assetMoveResult;

            //bool isTip = false;
            //if (AssetDatabase.IsValidFolder(sourcePath))
            //{
            //    isTip = AssetDatabase.FindAssets("t:Script", new string[] { sourcePath }).Length != 0;
            //}
            //else
            //{
            //    if (sourcePath.EndsWith(".cs"))
            //    {
            //        //只是改了文件名
            //        if (Directory.GetParent(sourcePath).FullName != Directory.GetParent(destinationPath).FullName)
            //        {
            //            isTip = true;
            //        }
            //    }
            //}

            //if (isTip)
            //{
            //    if (EditorUtility.DisplayDialog("Move Folder Tip", $"Source path exist Scripts. Do you want move?\n\nSource path: {sourcePath}\nDestination path: {destinationPath}", "Yes Move", "No"
            //        , DialogOptOutDecisionType.ForThisSession, MoveScriptsTipEdiotrKey) == false)
            //    {
            //        assetMoveResult = AssetMoveResult.FailedMove;
            //    }
            //}

            //return assetMoveResult; 
            #endregion
        }


        protected bool CheckFolderMove(ref AssetMoveResult result)
        {
            if (AssetDatabase.IsValidFolder(sourcePath))
            {
                string title = null;
                string message = null;
                bool isMove = false;
                if (IsReName())
                {
                    title = "Folder Rename Tip!";
                    message = $"Do you want to rename the folder.?\n\n" + $"Source: {sourcePath}\nTarget: {destinationPath}";
                    isMove = EditorUtility.DisplayDialog(title, message, "Yes Rename", "Cancel");
                }
                else
                {
                    title = "Folder Move Tip!";
                    message = $"Do you want to move the folder.?\n\n" + $"Source: {sourcePath}\nTarget: {destinationPath}";
                    isMove = EditorUtility.DisplayDialog(title, message, "Yes Move", "Cancel");
                }

                if (isMove)
                {
                    result = AssetMoveResult.DidNotMove;
                }
                else
                {
                    result = AssetMoveResult.FailedMove;
                }
                return true;
            }
            return false;
        }

        protected bool CheckFileMove(ref AssetMoveResult result)
        {
            if (AssetDatabase.IsValidFolder(sourcePath) == false)
            {
                string title = null;
                string message = null;
                bool isMove = false;
                if (IsReName())
                {
                    title = "File Rename Tip!";
                    message = $"Do you want to rename the file.?\n\n" + $"Source: {sourcePath}\nTarget: {destinationPath}";
                    isMove = EditorUtility.DisplayDialog(title, message, "Yes Rename", "Cancel");
                }
                else
                {
                    title = "File Move Tip!";
                    message = $"Do you want to move the file.?\n\n" + $"Source: {sourcePath}\nTarget: {destinationPath}";
                    isMove = EditorUtility.DisplayDialog(title, message, "Yes Move", "Cancel");
                }

                if (isMove)
                {
                    result = AssetMoveResult.DidNotMove;
                }
                else
                {
                    result = AssetMoveResult.FailedMove;
                }
                return true;
            }

            return false;
        }

        protected bool IsReName()
        {
            return Directory.GetParent(sourcePath).FullName.Equals(Directory.GetParent(destinationPath).FullName);
        }
    }
#if UNITY_2020_3_OR_NEWER
    public partial class CustomAssetModificationProcessor
    {
        [System.Obsolete("Use AssetDatabase.SaveAssetIfDirty(GUID guid)", true)]
        public static void SaveAssetIfDirty(GUID guid) { }

        [System.Obsolete("Use AssetDatabase.SaveAssetIfDirty(UnityEngine.Object obj)", true)]
        public static void SaveAssetIfDirty(UnityEngine.Object obj) { }
    }
#else
    public partial class CustomAssetModificationProcessor
    {
        private static System.Collections.Generic.List<string> _saveAssetPaths = new System.Collections.Generic.List<string>();

        public static void SaveAssetIfDirty(GUID guid)
        {
            try
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                if (EditorUtility.IsDirty(asset))
                {
                    _saveAssetPaths.Add(path);
                    AssetDatabase.SaveAssets();
                }
            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }

        }

        public static void SaveAssetIfDirty(UnityEngine.Object obj)
        {
            try
            {
                if (EditorUtility.IsDirty(obj))
                {
                    var path = AssetDatabase.GetAssetPath(obj);
                    _saveAssetPaths.Add(path);
                    AssetDatabase.SaveAssets();
                }

            }
            catch (System.Exception e)
            {
                UnityEngine.Debug.LogError(e);
            }
        }

        private static string[] OnWillSaveAssets(string[] paths)
        {
            if (_saveAssetPaths.Count > 0)
            {
                paths = _saveAssetPaths.ToArray();
                _saveAssetPaths.Clear();
            }
            return paths;
        }
    }
#endif
}