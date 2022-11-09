using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Cofdream.Editor
{
    public class Temp : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        [MenuItem("Test/12121")]
        static void Func() {
            Debug.Log(Application.productName);
        }
    }

}
