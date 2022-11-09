using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public static class OpenEditorWindow
    {

        #region Other
        const string HEAD_OTHER = MenuItemUtil.ToolKit + "Other/";

        [MenuItem(HEAD_OTHER + "2D快速排序")]
        public static void OpenTransformSort()
        {
            EditorWindow.GetWindow<Transform2DSort>("2D快速排序").Show();
        }

        [MenuItem(HEAD_OTHER + "修改 GameView 分辨率")]
        public static void OpenGameViewSizeSelect()
        {
            var window = EditorWindow.GetWindow<GameViewSizeSelect>();
            window.maxSize = new Vector2(200, 500);
            window.Show();
        }

        [MenuItem(HEAD_OTHER + "Find Missing Tools/Find Missing Object Reference")]
        public static void OpenFindMissingObjectReference()
        {
            var window = EditorWindow.GetWindow<FindMissingObjectReference>();
            window.position = EditorWindowExtension.GetMainWindowCenteredPosition(new Vector2(300, 400));
            window.Show();
        }

        [MenuItem(HEAD_OTHER + "Find Missing Tools/Find Miss Component")]
        public static void OpenFindMissComponent()
        {
            var window = EditorWindow.GetWindow<FindMissComponent>();
            window.position = EditorWindowExtension.GetMainWindowCenteredPosition(new Vector2(300, 400));
            window.Show();
        }

        #endregion


        #region UGUI
        const string HEAD_UGUI = "ToolKit/UGUI/";

        [MenuItem(HEAD_UGUI + "快速排序")]
        public static void OpenUGUISort()
        {
            EditorWindow.GetWindow<RectTransformSort>("快速排序").Show();
        }

        [MenuItem(HEAD_UGUI + "批量选择同结构物体")]
        public static void OpenSelectSameItemStructure()
        {
            EditorWindow.GetWindow<SelectSameItemStructure>("批量选择同结构物体").Show();
        }

        [MenuItem(HEAD_UGUI + "绘制射线检测")]
        public static void OpenDrawRaycast()
        {
            EditorWindow.GetWindow<DrawRaycast>("绘制UI射线检测").Show();
        }

        [MenuItem(HEAD_UGUI + "未分类工具")]
        public static void OpenUnclassified()
        {
            EditorWindow.GetWindow<Unclassified>("未分类工具").Show();
        }
        #endregion


        #region Infrequently

        #endregion
    }
}