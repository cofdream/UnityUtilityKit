using System.IO;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    public class MoveFolderAssetModificationProcessor :
#if UNITY_2020_3_OR_NEWER
        UnityEditor.AssetModificationProcessor
#else
        AssetModificationProcessor
#endif
    {
        private const string MoveScriptsTipEdiotrKey = "Cofdream_Key_Move_Scirpts_Tip";
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            AssetMoveResult assetMoveResult = AssetMoveResult.DidNotMove;

            if (ReloadAssembliesSetting.LockReloadAssemblies) return assetMoveResult;

            bool isTip = false;
            if (AssetDatabase.IsValidFolder(sourcePath))
            {
                isTip = AssetDatabase.FindAssets("t:Script", new string[] { sourcePath }).Length != 0;
            }
            else
            {
                if (sourcePath.EndsWith(".cs"))
                {
                    //只是改了文件名
                    if (Directory.GetParent(sourcePath).FullName != Directory.GetParent(destinationPath).FullName)
                    {
                        isTip = true;
                    }
                }
            }

            if (isTip)
            {
                if (EditorUtility.DisplayDialog("Move Scirpts Tip", $"Source path exist Scripts. Do you want move?\n\nSource path: {sourcePath}\nDestination path: {destinationPath}", "Yes Move", "No"
                    , DialogOptOutDecisionType.ForThisSession, MoveScriptsTipEdiotrKey) == false)
                {
                    assetMoveResult = AssetMoveResult.FailedMove;
                }
            }

            return assetMoveResult;
        }
    }
    // todo 尝试对不同文件的移动做 监听处理

    //// 防止错误移动文件夹
    //public class MoveFolderTipAssetModificationProcessor : AssetModificationProcessor
    //{
    //    private const string MoveFolderTipEdiotrKey = "Cofdream_Key_Move_Folder_Tip";
    //    private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
    //    {
    //        AssetMoveResult assetMoveResult = AssetMoveResult.DidNotMove;

    //        if (ReloadAssembliesSetting.LockReloadAssemblies) return assetMoveResult;

    //        bool isTip = false;
    //        if (AssetDatabase.IsValidFolder(sourcePath))
    //        {
    //            // 只处理文件夹情况
    //            isTip = AssetDatabase.FindAssets("", new string[] { sourcePath }).Length > 0;
    //        }

    //        if (isTip)
    //        {
    //            if (EditorUtility.DisplayDialog("Move File Tip", $"Do you want move folder?\n\nSource path: {sourcePath}\nDestination path: {destinationPath}", "Yes Move", "No"
    //                , DialogOptOutDecisionType.ForThisSession, MoveFolderTipEdiotrKey) == false)
    //            {
    //                assetMoveResult = AssetMoveResult.FailedMove;
    //            }
    //        }

    //        return assetMoveResult;
    //    }
    //}

    //// 防止错误移动文件
    //public class MoveFileTipAssetModificationProcessor : AssetModificationProcessor
    //{
    //    private const string MoveFileTipEdiotrKey = "Cofdream_Key_Move_File_Tip";
    //    private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
    //    {
    //        AssetMoveResult assetMoveResult = AssetMoveResult.DidNotMove;

    //        if (ReloadAssembliesSetting.LockReloadAssemblies) return assetMoveResult;

    //        if (AssetDatabase.IsValidFolder(sourcePath) == false)
    //        {
    //            // 过滤下cs脚本，其他地方处理
    //            if (sourcePath.EndsWith(".cs") == false)
    //            {
    //                if (EditorUtility.DisplayDialog("Move File Tip", $"Do you want move file?\n\nSource path: {sourcePath}\nDestination path: {destinationPath}", "Yes Move", "No"
    //                , DialogOptOutDecisionType.ForThisSession, MoveFileTipEdiotrKey) == false)
    //                {
    //                    assetMoveResult = AssetMoveResult.FailedMove;
    //                }
    //            }
    //        }

    //        return assetMoveResult;
    //    }
    //}
}