using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    public class EditorWindowPlus : EditorWindow, IHasCustomMenu
    {
        protected bool isLock { get; set; } = false;
        
        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            this.EditScript(menu);
        }

        protected virtual void ShowButton(Rect position)
        {
            ShowLockButton(ref position);
        }

        protected void ShowLockButton(ref Rect position)
        {
            isLock = GUI.Toggle(position, isLock, isLock ? GUIContentExtension.LockOn : GUIContentExtension.LockOff, GUIStyle.none);
            position.x -= 20f;
        }
    }
}