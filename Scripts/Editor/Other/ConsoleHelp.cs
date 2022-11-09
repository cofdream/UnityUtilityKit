using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public static class ConsoleHelp
    {
        public static void ClearConsole()
        {
            var assembly = System.Reflection.Assembly.GetAssembly(typeof(UnityEditor.Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}