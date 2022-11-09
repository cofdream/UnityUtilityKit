using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{
	public class EditorWindowPlus : EditorWindow, IHasCustomMenu
	{
        public virtual void AddItemsToMenu(GenericMenu menu)
        {
            this.EditScript(menu);
        }
    }
}