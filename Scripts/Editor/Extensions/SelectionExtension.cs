using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Cofdream.ToolKitEditor
{
    public static class SelectionExtension
    {
        /// <summary>
        /// 获取选中游戏对象，顺序为选中时的顺序，仅限场景内
        /// </summary>
        /// <returns></returns>
        public static GameObject[] GameObjects()
        {
            return (from obj in Selection.objects
                    where obj is GameObject
                    select obj as GameObject).ToArray();
        }

        /// <summary>
        /// 获取选中游戏对象的组件，顺序为选中时的顺序，仅限场景内
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] Components<T>() where T : Component
        {
            var objs = Selection.objects;
            var ts = new List<T>(objs.Length);
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i] is GameObject go)
                {
                    if (go.TryGetComponent(out T t))
                    {
                        ts.Add(t);
                    }
                }
            }
            return ts.ToArray();
        }
    }
}