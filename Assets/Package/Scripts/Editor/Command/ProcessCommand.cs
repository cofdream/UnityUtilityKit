using System;
using System.Diagnostics;
using System.Text;

namespace Cofdream.ToolKitEditor
{
    [Serializable]
    public class ProcessCommand
    {
        private readonly Process process;
        public string Cmd;

        private string log;
        public string Log => log;
        public ProcessCommand()
        {
            process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.StandardOutputEncoding = Encoding.GetEncoding("GB2312");
        }
        public bool Execute()
        {
            log = string.Empty;
            process.Start();
            try
            {
                process.StandardInput.WriteLine(Cmd);
                process.StandardInput.AutoFlush = true;
                process.StandardInput.WriteLine("exit");

                log = process.StandardOutput.ReadToEnd();

                process.WaitForExit();
                process.Close();
                return true;
            }
            catch (Exception e)
            {
                log += e.Message;
                return false;
            }
        }
    }
}