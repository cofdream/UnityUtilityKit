using System;
using UnityEngine;

namespace Cofdream.NavigatorMenuItem
{
    public interface IMenuItem
    {
        Type type { get; set; }
        string str { get; set; }
        bool isIcon { get; set; }
        GUIContent GUIContent { get; }
        void Draw();
    }
}