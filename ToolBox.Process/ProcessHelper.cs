using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Process
{
    public static class ProcessHelper
    {
        /// <summary>
        /// 根据进程名结束进程 不需要后缀 多个 相同进程名 会被一起结束
        /// </summary>
        /// <param name="ProssName"></param>
        public static void KillProcessByName(string ProssName)
        {
            string newName = ProssName.Replace(".exe", "");
            try
            {

                Process[] pp = Process.GetProcessesByName(newName);

                for (int i = 0; i < pp.Length; i++)

                {
                    if (pp[i].ProcessName == newName)
                    {
                        pp[i].Kill();

                    }

                }

            }

            catch (Exception ex)

            {
                Console.WriteLine(ex.Message);

            }

        }

        /// <summary>
        /// 根据进程名和进程PID，关闭指定进程 - 进程名不需要带后缀
        /// </summary>
        /// <param name="ProssName">进程名</param>
        /// <param name="ClosePid">需要关闭的进程PID</param>
        public static void CloseProcessByProcessId(string ProssName, int ClosePid)
        {
            Process[] pp = Process.GetProcessesByName(ProssName);
            for (int i = 0; i < pp.Length; i++)
            {
                if (pp[i].Id == ClosePid)
                {
                    pp[i].Kill();
                }
            }
        }

        /// <summary>
        /// 根据进程名获得进程Process对象的集合 - 不需要带上进程后缀
        /// </summary>
        /// <param name="ProssName">进程名</param>
        /// <param name="Pro">进程Process 对象集合</param>
        /// <returns>找不到返回 false</returns>
        public static bool GetProcessPorEx_ByProssName(string ProssName, ref List<Process> Pro)
        {
            bool finded = false;
            Process[] pp = Process.GetProcessesByName(ProssName);
            for (int i = 0; i < pp.Length; i++)
            {
                if (pp[i].ProcessName == ProssName)
                {
                    finded = true;
                    Pro.Add(pp[i]);
                }
            }
            if (finded)
            {
                return true;
            }
            return false;
        }




        [DllImport("user32", EntryPoint = "GetWindowThreadProcessId")]
        private static extern int GetWindowThreadProcessId(IntPtr hwnd, out int pid);

        /// <summary>
        /// 根据窗口句柄获得进程PID和线程PID
        /// </summary>
        /// <param name="hwnd">句柄</param>
        /// <param name="pid">返回 进程PID</param>
        /// <returns>方法的返回值，线程PID，进程PID和线程PID这两个概念不同</returns>
        public static int GetPidByHwnd(int hwnd, out int pid)
        {
            pid = 0;
            return GetWindowThreadProcessId((IntPtr)hwnd, out pid);
        }

        /// <summary>
        /// 根据窗口标题获得进程Process对象
        /// </summary>
        /// <param name="Title">窗口标题</param>
        /// <param name="Pro">进程Process 对象</param>
        /// <returns>找不到返回 false</returns>
        public static bool GetProcessPor_ByTitle(string Title, out Process Pro)
        {
            Pro = null;
            Process[] arrayProcess = Process.GetProcesses();
            foreach (Process p in arrayProcess)
            {
                if (p.MainWindowTitle.IndexOf(Title) != -1)
                {
                    Pro = p;
                    return true;
                }
            }
            return false;
        }


        /// <summary>
        /// 根据窗口标题查找窗口进程PID-返回List
        /// </summary>
        /// <param name="windowTitle">窗口标题</param>
        /// <returns>List</returns>
        public static List<int> FindPidEx_ByTitle(string Title)
        {
            List<int> list = new List<int>();
            Process[] arrayProcess = Process.GetProcesses();
            foreach (Process p in arrayProcess)
            {
                if (p.MainWindowTitle.IndexOf(Title) != -1)
                {
                    list.Add(p.Id);

                }
            }
            return list;
        }



        /// <summary>
        /// 根据进程名获得进程PID - 不需要带上进程后缀
        /// </summary>
        /// <param name="ProssName">进城名</param>
        /// <returns>进城PID 找不到返回 0</returns>
        public static int GetProcessPid(string ProssName)
        {
            Process[] pp = Process.GetProcessesByName(ProssName);
            for (int i = 0; i < pp.Length; i++)
            {
                if (pp[i].ProcessName == ProssName)
                {
                    return pp[i].Id;
                }
            }
            return 0;
        }


        /// <summary>
        /// 运行一个指定文件或者程序
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <returns>失败返回false</returns>
        public static bool Runapp(string Path)
        {
            try
            {
                Process pro = new Process();
                pro.StartInfo.FileName = @Path;
                pro.Start();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }


        /// <summary>
        /// 运行一个指定文件或者程序可以带上参数
        /// </summary>
        /// <param name="Path">文件路径</param>
        /// <param name="Flag">附带参数</param>
        /// <returns>失败返回false</returns>
        private static bool Runapp_ByFlag(string Path, string Flag)
        {
            try
            {
                Process pro = new Process();
                pro.StartInfo.FileName = @Path;
                pro.StartInfo.Arguments = Flag;
                pro.Start();
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }



        /// <summary>
        /// 通过句柄获得进程路径
        /// </summary>
        /// <param name="hwnd">句柄</param>
        /// <returns>返回 进程路径 找不到返回""</returns>
        public static string GetAppRunPath_ByHandle(int hwnd)
        {
            string path = "";
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                if ((int)p.MainWindowHandle == hwnd)
                {
                    path = p.MainModule.FileName.ToString();
                }
            }
            return path;
        }



        /// <summary>
        /// 通过进程名获得进程路径 不需要后缀
        /// </summary>
        /// <param name="hwnd">句柄</param>
        /// <returns>返回 进程路径 找不到返回""</returns>
        public static string GetAppRunPath_ByName(string prossName)
        {
            string path = "";
            Process[] ps = Process.GetProcesses();
            foreach (Process p in ps)
            {
                if (p.ProcessName == prossName)
                {
                    path = p.MainModule.FileName.ToString();
                    return path;
                }
            }


            return "";
        }


    }
}
