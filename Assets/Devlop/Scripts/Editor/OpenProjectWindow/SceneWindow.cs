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
        static Logger Logger2;

        [MenuItem("Test222/SceneWindow")]
        static void OpenWindow1()
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
        }


        [MenuItem("Test333/OpenWindow")]
        private static void OpenWindow()
        {
            GetWindow<SceneWindow>().Show();
        }

        public class Logger
        {
            public int Id;
            public string Log;
        }

        private string _projectPath;
        private string _enginePath;
        private string _commonLine;

        private Logger _logger;

        private void OnGUI()
        {
            _enginePath = EditorGUILayout.TextField("引擎路径", _enginePath);
            _projectPath = EditorGUILayout.TextField("工程路径", _projectPath);

            _commonLine = EditorGUILayout.TextField("后面的命令行内容", _commonLine);

            if (GUILayout.Button("Open"))
            {
                ParameterizedThreadStart parameterizedThreadStart = new ParameterizedThreadStart((parameter) =>
                {
                    var logger = parameter as Logger;


                    var process2 = Process.Start(@"E:\App\UnityEngine\2021.2.5f1\Editor\Unity.exe", @"_projectPath E: \UnityProject\OpenGGG");

                    //UnityEngine.Debug.Log(process2.Id);

                    return;
                    Process process;

                    process = new Process();
                    //process.StartInfo.FileName = $"{ _enginePath}

                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.CreateNoWindow = false;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("GB2312");
                    //process.StartInfo.Verb = "";

                    process.Start();

                    logger.Id = process.Id;
                    logger.Log += "Run Process Id: " + process.Id;

                    //process.StandardInput.WriteLine($"{_enginePath} -projectPath {_projectPath} {_commonLine}");

                    //process.StandardInput.AutoFlush = true;

                    //process.StandardInput.WriteLine("exit");

                    logger.Log += process.StandardOutput.ReadToEnd();


                    process.WaitForExit();
                    process.Close();

                    UnityEngine.Debug.Log($"ParameterizedThreadStart--{Thread.CurrentThread.ManagedThreadId.ToString("00")}--{DateTime.Now.ToString(" HH:mm:ss.fff")}");

                    UnityEngine.Debug.Log(logger.Log);
                });

                Thread thread = new Thread(parameterizedThreadStart);
                thread.Start(_logger);
            }
        }

    }
}
