using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using UnityEditor.Compilation;

namespace Cofdream.ToolKitEditor
{
    public class AttributeHelper
    {

        public static string MethodToString(MethodInfo method)
        {
            return string.Format("{0}{1}", method.IsStatic ? "static " : "", method);
        }

        public struct MethodWithAttribute
        {
            public MethodInfo info;
            public Attribute attribute;
        }

        public class MethodInfoSorter
        {
            internal MethodInfoSorter(List<MethodWithAttribute> methodsWithAttributes)
            {
                this.methodsWithAttributes = methodsWithAttributes;
            }

            public IEnumerable<MethodInfo> FilterAndSortOnAttribute<T>(Func<T, bool> filter, Func<T, IComparable> sorter) where T : Attribute
            {
                return methodsWithAttributes.Where(a => filter((T)a.attribute)).OrderBy(c => sorter((T)c.attribute)).Select(o => o.info);
            }

            public IEnumerable<MethodWithAttribute> methodsWithAttributes { get; }
        }

        static Dictionary<Type, MethodInfoSorter> s_DecoratedMethodsByAttrTypeCache = new Dictionary<Type, MethodInfoSorter>();
        private const BindingFlags kAllStatic = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;

        public static MethodInfoSorter GetMethodsWithAttribute<T>(BindingFlags bindingFlags = kAllStatic) where T : Attribute
        {
            MethodInfoSorter result;
            if (!s_DecoratedMethodsByAttrTypeCache.TryGetValue(typeof(T), out result))
            {
                var tmp = new List<MethodWithAttribute>();
                foreach (var method in EditorAssemblies.GetAllMethodsWithAttribute<T>(bindingFlags))
                {
                    if (method.IsGenericMethod)
                    {
                        Debug.LogErrorFormat($"{MethodToString(method)} is a generic method. {typeof(T)} cannot be applied to it.");
                    }
                    else
                    {
                        foreach (var attr in method.GetCustomAttributes(typeof(T), false))
                        {
                            var methodWithAttribute = new MethodWithAttribute { info = method, attribute = (T)attr };
                            tmp.Add(methodWithAttribute);
                        }
                    }
                }

                result = new MethodInfoSorter(tmp);
                s_DecoratedMethodsByAttrTypeCache[typeof(T)] = result;
            }
            return result;
        }
    }

    public class EditorAssemblies
    {
        const BindingFlags k_DefaultMethodBindingFlags =
            BindingFlags.Public
            | BindingFlags.NonPublic
            | BindingFlags.Instance
            | BindingFlags.Static;

        internal static IEnumerable<MethodInfo> GetAllMethodsWithAttribute<T>(BindingFlags bindingFlags = k_DefaultMethodBindingFlags) where T : Attribute
        {
            // h
            var assemblys = CompilationPipeline.GetAssemblies(AssembliesType.Editor);
            foreach (var assembly in assemblys)
            {
                Debug.Log(assembly);
            }


            return null;
        }

    }

    public class Test2234
    {
        [MenuItem("Test/AAA")]
        static void Test2()
        {
            EditorAssemblies.GetAllMethodsWithAttribute<ToolProviderAttribute>();
        }
    }

}