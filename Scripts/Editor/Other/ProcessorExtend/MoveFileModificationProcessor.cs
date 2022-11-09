using System.Collections;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    internal class MoveFileModificationProcessor :
#if UNITY_2020_3_OR_NEWER
        UnityEditor.AssetModificationProcessor
#else
        AssetModificationProcessor
#endif
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            string title = "Move Scirpts Tip";
            string message = $"Are you sure you want to move the file? \n\nSource path: {sourcePath}\nDestination path: {destinationPath}";

            bool isMove = EditorUtility.DisplayDialog(title, message, "Yes Move", "Cancel", DialogOptOutDecisionType.ForThisMachine, EditorPrefsKey.MoveFileTip);

            if (isMove)
            {
                return AssetMoveResult.DidMove;
            }
            else
            {
                return AssetMoveResult.FailedMove;
            }
        }
    }
}