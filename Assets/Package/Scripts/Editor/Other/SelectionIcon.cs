using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    /// <summary>
    /// 工具：为游戏对象的Selcetion Icon 动态设置Icon
    /// </summary>
    public static class SelectionIcon
    {
        // Label类型icon 显示文字的 
        public enum LabelIcon : short
        {
            //圆角矩形
            Gray = 0,
            Blue,
            Teal,
            Green,
            Yellow,
            Orange,
            Red,
            Purple,

            Length,//不做显示，只提供枚举数量
        }

        // 其他icon不显示文字
        public enum NoLabelIcon : short
        {
            // 圆形
            CircleGray = 0,
            CircleBlue,
            CircleTeal,
            CircleGreen,
            CircleYellow,
            CircleOrange,
            CircleRed,
            CirclePurple,
            // 菱形
            DiamondGray,
            DiamondBlue,
            DiamondTeal,
            DiamondGreen,
            DiamondYellow,
            DiamondOrange,
            DiamondRed,
            DiamondPurple,

            Length,//不做显示，只提供枚举数量
        }

        /// <summary>
        /// 设置游戏对象的 Selection Icon
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="labelIcon">Selection Icon 类型：带文字</param>
        public static void SetSelectionIcon(UnityEngine.Object obj, LabelIcon labelIcon)
        {
            SetSelectionIcon(obj, GetGUIContent(labelIcon).image as Texture2D);
        }

        /// <summary>
        /// 设置游戏对象的 Selection Icon
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="labelIcon">Selection Icon 类型：不带文件</param>
        public static void SetSelectionIcon(UnityEngine.Object obj, NoLabelIcon noLabelIcon)
        {
            SetSelectionIcon(obj, GetGUIContent(noLabelIcon).image as Texture2D);
        }

        public static void SetSelectionIcon(UnityEngine.Object obj, Texture2D texture)
        {
            var ty = typeof(EditorGUIUtility);
            var methodInfo = ty.GetMethod("SetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            methodInfo.Invoke(null, new object[] { obj, texture });
        }


        //public static void GetIconForObject
        public static Texture2D GetIconForObject(UnityEngine.Object obj)
        {
            var ty = typeof(EditorGUIUtility);
            var methodInfo = ty.GetMethod("GetIconForObject", BindingFlags.NonPublic | BindingFlags.Static);
            return methodInfo.Invoke(null, new object[] { obj }) as Texture2D;
        }

        public static MethodInfo GetMethodInfo_IconContent()
        {
            return typeof(EditorGUIUtility).GetMethod("IconContent", BindingFlags.Public | BindingFlags.Static, null, new Type[] { typeof(string) }, null);
        }

        public static GUIContent GetGUIContent(LabelIcon labelIcon)
        {
            var methodInfo = GetMethodInfo_IconContent();

            return methodInfo.Invoke(null, new object[] { "sv_label_" + labelIcon.ToString() }) as GUIContent;
        }
        public static GUIContent[] GetGUIContents(LabelIcon labelIcon)
        {
            var methodInfo = GetMethodInfo_IconContent();

            object[] parameters = new object[] { "sv_label_" + labelIcon.ToString() };

            int length = (int)LabelIcon.Length;

            GUIContent[] contents = new GUIContent[length];

            for (int i = 0; i < length; i++)
            {
                contents[i] = methodInfo.Invoke(null, parameters) as GUIContent;
            }

            return contents;
        }
        public static GUIContent GetGUIContent(NoLabelIcon noLabelIcon)
        {
            var methodInfo = GetMethodInfo_IconContent();

            return methodInfo.Invoke(null, new object[] { "sv_icon_dot" + noLabelIcon.ToString() + "_pix16_gizmo" }) as GUIContent;
        }
        public static GUIContent[] GetGUIContents(NoLabelIcon noLabelIcon)
        {
            var methodInfo = GetMethodInfo_IconContent();

            object[] parameters = new object[] { "sv_icon_dot" + noLabelIcon.ToString() + "_pix16_gizmo" };

            int length = (int)LabelIcon.Length;

            GUIContent[] contents = new GUIContent[length];

            for (int i = 0; i < length; i++)
            {
                contents[i] = methodInfo.Invoke(null, parameters) as GUIContent;
            }

            return contents;
        }
    }
}