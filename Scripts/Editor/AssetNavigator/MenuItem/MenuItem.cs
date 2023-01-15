using System;
using UnityEditor;
using UnityEngine;

namespace Cofdream.NavigatorMenuItem
{
    public interface IInstance
    {

    }
    public interface IMenuItem
    {
        GUIContent GUIContent { get; }
        void Draw();
    }


    public abstract class MenuItem : ScriptableObject, IMenuItem
    {
        public Type type;
        public string str;
        public bool isIcon;

        public virtual GUIContent GUIContent
        {
            get
            {
                // 临时显示 str 未 toolTips
                if (isIcon)
                {
                    var texture2D = EditorGUIUtility.FindTexture(str);
                    if (texture2D != null)
                        return new GUIContent(texture2D, str);
                }

                return new GUIContent(str, str);
            }
        }

        public virtual void Draw()
        {
            //EditorGUILayout.BeginScrollView();

            {

            }
        }
    }


    
}
//保存的对象是抽象类，继承接口

//本地是变量 类型 抽象数组、抽象类

//提供实例化多态类的选项

//反射获取所有继承当前接口的类

