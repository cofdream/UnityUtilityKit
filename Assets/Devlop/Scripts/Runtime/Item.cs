using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Cofdream.Runtime
{
    public class Item : MonoBehaviour
    {
        public Text text;
        public StringBuilder StringBuilder;


        private void Awake()
        {
            StringBuilder = new StringBuilder();
        }

        public void RefreshView()
        {
            text.text = StringBuilder.ToString();
        }
    }
}
