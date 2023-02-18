using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

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

    [AttributeUsage(AttributeTargets.Method)]
    public class UpdateAttribute : Attribute { }


    [UnityEditor.InitializeOnLoad]
    internal static class Initialize
    {
        private static MethodInfo[] _updateMethodInfoSet;

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
        }

        [InitializeOnLoadMethod]
        private static void BindDelayCall()
        {
            EditorApplication.delayCall += DelayCall;
            EditorApplication.update += Update;

            _updateMethodInfoSet = GetUpdateMethodInfo();
        }

        private static MethodInfo[] GetUpdateMethodInfo()
        {
            List<MethodInfo> methodInfos = new List<MethodInfo>();
            foreach (var method in TypeCache.GetMethodsWithAttribute<UpdateAttribute>())
            {
                if (method.ContainsGenericParameters)
                {
                    Debug.LogError($"不能对泛型函数添加: {nameof(UpdateAttribute)} \n{method.Name}");
                    continue;
                }

                if (method.GetParameters().Length > 0)
                {
                    Debug.LogError($"不能对带有参数的函数添加: {nameof(UpdateAttribute)} \n{method.Name}");
                    continue;
                }
                if (method.IsStatic == false)
                {
                    Debug.LogError($"不能对非静态函数添加: {nameof(UpdateAttribute)} \n{method.Name}");
                    continue;
                }
                methodInfos.Add(method);
            }
            return methodInfos.ToArray();
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

        private static void Update()
        {
            foreach (var item in _updateMethodInfoSet)
            {
                try
                {
                    item.Invoke(null, null);
                }
                catch (Exception e)
                {

                    Debug.LogError($"调用 {nameof(UpdateAttribute)} \n{item.Name} Exception:\n{e}");
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