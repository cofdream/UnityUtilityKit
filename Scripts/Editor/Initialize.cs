using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using UnityEngine.Assertions.Must;

namespace Cofdream.ToolKitEditor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class InitializeOnLoadAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class InitializeOnLoadMethodAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Method)]
    public class InitializeOnEnterPlayModeAttribute : Attribute { }


    [UnityEditor.InitializeOnLoad]
    internal static class Initialize
    {
        static Initialize()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (type.GetCustomAttribute(typeof(InitializeOnLoadAttribute), false) != null)
                    {
                        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
                    }
                }
            }
        }

        [UnityEditor.InitializeOnLoadMethod]
        private static void InitializeOnLoad()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod))
                    {

                        if (method.GetCustomAttribute(typeof(InitializeOnLoadMethodAttribute), false) != null)
                        {
                            if (method.GetParameters().Length == 0)
                            {

                                method.Invoke(null, BindingFlags.InvokeMethod, Type.DefaultBinder, new object[0], null);
                            }
                            else
                            {
                                // todo 可变参数 剔除
                                try
                                {
                                    method.Invoke(null, BindingFlags.InvokeMethod, Type.DefaultBinder, new object[0], null);
                                }
                                catch (Exception e)
                                {
                                    Debug.LogError(e);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}