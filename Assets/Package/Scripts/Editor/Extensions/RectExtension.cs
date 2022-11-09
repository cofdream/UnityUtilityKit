using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public static class RectExtension
    {
        //Split
        public static void SplitVertical(this Rect self, float height, out Rect top, out Rect bottom)
        {
            top = self;
            top.height = height;

            bottom = self;
            bottom.y += height;
            bottom.height -= height;
        }
        public static void SplitHorizontal(this Rect self, float width, out Rect left, out Rect right)
        {
            left = self;
            left.width = width;

            right = self;
            right.x += width;
            right.width -= width;
        }


        // Padding
        public static Rect GetPadding(this Rect self, float left, float right, float top, float bottom)
        {
            Rect rect = self;
            rect.x += left;
            rect.y += top;

            rect.width -= left + right;
            rect.height -= top + bottom;

            return rect;
        }
        public static Rect GetPadding(this Rect self, Vector4 padding)
        {
            // left-x bottom-y right-z top-w
            return GetPadding(self, padding.x, padding.z, padding.w, padding.y);
        }

        // Move
        public static void MoveX(this ref Rect self, float width)
        {
            self.x += width;
            self.width += width;
        }
    }
}