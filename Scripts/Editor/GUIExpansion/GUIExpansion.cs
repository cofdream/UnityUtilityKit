using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public static class GUIExpansion
    {
        // 绘制一个搜索框
        public static string DrawSearchField(Rect position, string text)
        {
            const float cancelButtonWidth = 14f;

            Rect cancelButtonRect = position;
            cancelButtonRect.x += position.width - cancelButtonWidth;
            cancelButtonRect.width = cancelButtonWidth;
            //再绘制文本框之前，就要判断是否点击了删除区域
            if (Event.current.type == EventType.MouseUp && cancelButtonRect.Contains(Event.current.mousePosition))
            {
                text = string.Empty;
                GUI.changed = true;
            }
            //bug？使用GUI去绘制 会导致聚焦window就会自动聚焦到当前控件
            text = GUI.TextField(position, text, EditorStyles.toolbarSearchField);
            GUI.Label(cancelButtonRect, GUIContent.none, !string.IsNullOrEmpty(text) ? "ToolbarSeachCancelButton" : "ToolbarSeachCancelButtonEmpty");

            return text;
        }
    }
}