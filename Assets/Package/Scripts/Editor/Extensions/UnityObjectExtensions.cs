using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
	public static class UnityObjectExtensions
	{
		public static Object GetScript(this Object obj)
		{
			var script = new SerializedObject(obj)?.FindProperty("m_Script")?.objectReferenceValue;
			return script == null ? obj : script;
        }
	}
}