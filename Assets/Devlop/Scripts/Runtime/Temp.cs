using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System;
using Cofdream.DotNetLibrary;

namespace Cofdream.Runtime
{
    public class Temp : MonoBehaviour
    {
        public Item[] Item;


        void Start()
        {
            DotNetUtility.Log = UnityEngine.Debug.Log;

            int index = 0;
            Item[index++].StringBuilder.AppendLine("进程名");
            //Item[index++].StringBuilder.AppendLine("Modules");
            Item[index++].StringBuilder.AppendLine("进程ID");
            Item[index++].StringBuilder.AppendLine("命令行参数");

            var processes = Process.GetProcesses();// 获取本地计算机上的进程
            foreach (var item in processes)
            {
                if (item != null)
                {
                    try
                    {
                        if (item.ProcessName.Contains("Unity"))
                        {
                            try
                            {
                                int i = 0;
                                Item[i++].StringBuilder.AppendLine(item.ProcessName);
                                //Item[i++].StringBuilder.AppendLine(item.Modules.Count.ToString());
                                Item[i++].StringBuilder.AppendLine(item.Id.ToString());

                                Item[i++].StringBuilder.AppendLine(DotNetUtility.GetCommandLineArgs(item));

                            }
                            catch (System.Exception e)
                            {

                                UnityEngine.Debug.Log(e);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        UnityEngine.Debug.Log(e);
                    }
                    
                }
            }

            foreach (var item in Item)
            {
                item.RefreshView();
            }

        }
    }
}
