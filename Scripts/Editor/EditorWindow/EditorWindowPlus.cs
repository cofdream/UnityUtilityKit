using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
    public class EditorWindowPlus : EditorWindow, IHasCustomMenu
    {
        protected bool IsLock { get; set; } = false;
        
        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            this.EditScript(menu);
            this.PinScript(menu);
        }

        protected virtual void ShowButton(Rect position)
        {
            ShowLockButton(ref position);
        }

        protected void ShowLockButton(ref Rect position)
        {
            IsLock = GUI.Toggle(position, IsLock, IsLock ? GUIContentExtension.LockOn : GUIContentExtension.LockOff, GUIStyle.none);
            position.x -= 20f;
        }
    }
}