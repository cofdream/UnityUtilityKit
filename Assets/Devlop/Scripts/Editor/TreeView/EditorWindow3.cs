using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Cofdream.ToolKitEditor
{

	// 把编辑器工具窗口 以另一种方式绘制

	public class EditorWindow3 : EditorWindow
	{
		[MenuItem("ToolKit/Edito Window 3.0")]
		static void Open()
        {
            GetWindow<EditorWindow3>().Show();
        }


	}
}