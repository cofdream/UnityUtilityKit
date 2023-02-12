using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace Cofdream.ToolKitEditor
{
    public static class EditorGUIUtilityExtensions
    {
        internal static MethodInfo LoadIconRequiredMethod => typeof(EditorGUIUtility).GetMethod("LoadIconRequired", BindingFlags.NonPublic | BindingFlags.Static);


        public static Texture2D LoadIconRequired(string name)
        {
            return (Texture2D)LoadIconRequiredMethod?.Invoke(null, new object[] { name });
        }
    }
}