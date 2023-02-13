using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
using UnityEngine.Assertions.Must;
using System.Linq;
using UnityEditor.Compilation;
using NUnit.Framework.Internal;

namespace Cofdream.ToolKitEditor
{
    //[AttributeUsage(AttributeTargets.Class)]
    //public class InitializeOnLoadAttribute : Attribute { }//UnityEditor.InitializeOnLoadAttribute { }

    //[AttributeUsage(AttributeTargets.Method)]
    //public class InitializeOnLoadMethodAttribute : Attribute { }//UnityEditor.InitializeOnLoadMethodAttribute { }

    //[AttributeUsage(AttributeTargets.Method)]
    //public class InitializeOnEnterPlayModeAttribute : Attribute { }//UnityEditor.InitializeOnEnterPlayModeAttribute { }


    [AttributeUsage(AttributeTargets.Method)]
    public class DelayCallAttribute : Attribute { }


    [UnityEditor.InitializeOnLoad]
    internal static class Initialize
    {
        static Initialize()
        {
            //foreach (var type in TypeCache.GetTypesWithAttribute<InitializeOnLoadAttribute>())
            //{
            //    if (type.GetCustomAttribute(typeof(InitializeOnLoadAttribute), false) != null)
            //    {
            //        System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            //    }
            //}


            //foreach (var method in TypeCache.GetMethodsWithAttribute<InitializeOnLoadMethodAttribute>())
            //{
            //    if (method.ContainsGenericParameters)
            //    {
            //        Debug.LogError($"不能对泛型函数添加: {nameof(InitializeOnLoadMethodAttribute)} \n{method.Name}");
            //        continue;
            //    }

            //    if (method.GetParameters().Length > 0)
            //    {
            //        Debug.LogError($"不能对带有参数的函数添加: {nameof(InitializeOnLoadMethodAttribute)} \n{method.Name}");
            //        continue;
            //    }
            //    if (method.IsStatic == false)
            //    {
            //        Debug.LogError($"不能对非静态函数添加: {nameof(InitializeOnLoadMethodAttribute)} \n{method.Name}");
            //        continue;
            //    }
            //    try
            //    {
            //        method.Invoke(null, null);
            //    }
            //    catch (Exception e)
            //    {
            //        Debug.LogError($"调用 {nameof(InitializeOnLoadMethodAttribute)} \n{method.Name} Exception:\n{e}");
            //    }
            //}

            EditorApplication.delayCall += DelayCall;
        }

        private static void DelayCall()
        {
            EditorApplication.delayCall -= DelayCall;

            foreach (var method in TypeCache.GetMethodsWithAttribute<DelayCallAttribute>())
            {
                if (method.ContainsGenericParameters)
                {
                    Debug.LogError($"不能对泛型函数添加: {nameof(DelayCallAttribute)} \n{method.Name}");
                    continue;
                }

                if (method.GetParameters().Length > 0)
                {
                    Debug.LogError($"不能对带有参数的函数添加: {nameof(DelayCallAttribute)} \n{method.Name}");
                    continue;
                }
                if (method.IsStatic == false)
                {
                    Debug.LogError($"不能对非静态函数添加: {nameof(DelayCallAttribute)} \n{method.Name}");
                    continue;
                }
                try
                {
                    method.Invoke(null, null);
                }
                catch (Exception e)
                {
                    Debug.LogError($"调用 {nameof(DelayCallAttribute)} \n{method.Name} Exception:\n{e}");
                }
            }
        }
   

        //[UnityEditor.InitializeOnLoadMethod]
        //private static void InitializeOnLoad()
        //{
        //    foreach (var method in TypeCache.GetMethodsWithAttribute<InitializeOnLoadMethodAttribute>())
        //    {
        //        if (method.ContainsGenericParameters)
        //        {
        //            Debug.LogError($"不能对泛型函数添加: {nameof(InitializeOnLoadMethodAttribute)} \n{method.Name}");
        //            continue;
        //        }

        //        if (method.GetParameters().Length > 0)
        //        {
        //            Debug.LogError($"不能对带有参数的函数添加: {nameof(InitializeOnLoadMethodAttribute)} \n{method.Name}");
        //            continue;
        //        }
        //        if (method.IsStatic == false)
        //        {
        //            Debug.LogError($"不能对非静态函数添加: {nameof(InitializeOnLoadMethodAttribute)} \n{method.Name}");
        //            continue;
        //        }
        //        try
        //        {
        //            method.Invoke(null, null);
        //        }
        //        catch (Exception e)
        //        {
        //            Debug.LogError($"调用 {nameof(InitializeOnLoadMethodAttribute)} \n{method.Name} Exception:\n{e}");
        //        }
        //    }
        //}
    }
}