using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Cofdream.ToolKitEditor;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System;

namespace Cofdream.Editor
{
    public class SceneWindow : EditorWindowPlus
    {
        public class Logger
        {
            public int Id;
            public string Log;
        }
        static Logger Logger2;

        [MenuItem("Test222/SceneWindow")]
        static void OpenWindow()
        {
            Logger2 = new Logger() { Id = 0, Log = string.Empty };

            ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart((obj) =>
         {
             Func(obj as Logger);
             UnityEngine.Debug.Log($"ParameterizedThreadStart--{Thread.CurrentThread.ManagedThreadId.ToString("00")}--{DateTime.Now.ToString(" HH:mm:ss.fff")}");
         });
            Thread thread = new Thread(parameterizedThreadStart);
            thread.Start(Logger2);
        }

        [MenuItem("Test222/Get id")]
        static void LOgAA()
        {
            UnityEngine.Debug.Log($"进程ID：{Logger2.Id}  log：{Logger2.Log}");
        }

        [MenuItem("Test222/Close")]
        static void Close()
        {
            var processes = Process.GetProcesses();
            foreach (Process process in processes)
            {
                if (Logger2.Id != 0 && process.Id == Logger2.Id)
                {
                    try
                    {
                        process.Kill();
                        process.WaitForExit(); // possibly with a timeout
                        UnityEngine.Debug.Log($"已杀掉 进程:{process.ProcessName}  ID:{process.Id} ！！！");
                    }
                    //catch (Win32Exception e)
                    //{
                    //    Console.WriteLine(e.Message.ToString());
                    //}
                    catch (InvalidOperationException e)
                    {
                        UnityEngine.Debug.Log(e.Message.ToString());
                    }
                }

            }
        }


        static void Func(Logger logger)
        {
            string unityEnginePath = @"E:\App\UnityEngine\2023.1.0a13\Editor\Unity.exe";
            string projectPath = @"E:\UnityProject\WorkTip";

            string str1 = "cmd.exe";
            string str2 = @"E:\UnityProject\WorkTip\Build\WorkTip.exe";

            string fileName = unityEnginePath;
            string writeLine = /*$"-projectPath { projectPath}"; //*/ $"{unityEnginePath} -projectPath {projectPath}";

            Process process;

            process = new Process();
            process.StartInfo.FileName = fileName;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("GB2312");
            process.StartInfo.Verb = "";


            process.Start();
            logger.Id = process.Id;

            //UnityEngine.Debug.Log("Run Process Id: " + process.Id);

            process.StandardInput.WriteLine(writeLine);
            //process.StandardInput.AutoFlush = true;
            //process.StandardInput.WriteLine("exit");

            logger.Log = process.StandardOutput.ReadToEnd();


            process.WaitForExit();
            process.Close();


            //var proc = new Process
            //{
            //    StartInfo = new ProcessStartInfo
            //    {
            //        FileName = "program.exe",

            //        Arguments = "command line arguments to your executable",
            //        UseShellExecute = false,
            //        RedirectStandardOutput = true,
            //        CreateNoWindow = true
            //    }
            //};

            //proc.Start();
            //while (!proc.StandardOutput.EndOfStream)
            //{
            //    string line = proc.StandardOutput.ReadLine();
            //    // do something with line

            //    UnityEngine.Debug.Log(line);
            //}


            //Process[] processes = Process.GetProcesses();
            //foreach (Process process in processes)
            //{
            //    if (process != null && process.ProcessName == "进程名")
            //    {
            //        UnityEngine.Debug.Log("进程ID" + process.Id);
            //    }
            //}


            //Process processes = Process.GetCurrentProcess();


            //GetWindow<SceneWindow>().Show();
        }



        private void OnGUI()
        {

        }
    }
}
