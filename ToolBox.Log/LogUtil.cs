using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ToolBox.Log
{
    /// <summary>
    /// 一个超级简单的日志帮助类
    /// </summary>
    public static class LogUtil
    {

        private static string LogPath { get; set; }
        private static List<QueueModel> LogQueue { get; set; }

        /// <summary>
        /// 设置日志文件夹路径，true是成功，flase是失败（默认软件运行目录下log文件夹中写入）
        /// </summary>
        /// <param name="directoryPath"></param>
        /// <returns></returns>
        public static bool SetDirectoryPath(string directoryPath)
        {
            try
            {
                if (Directory.Exists(directoryPath) && !File.Exists(directoryPath))
                {

                    Console.WriteLine(Path.GetFullPath(directoryPath));

                    LogPath = Path.GetFullPath(directoryPath);

                    return true;

                }
                else if (!File.Exists(directoryPath))
                {

                    string path = Path.Combine(directoryPath);

                    Directory.CreateDirectory(path);

                    LogPath = path;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                ex.Message.WriteErrorLog();
                return false;
            }
        }

        /// <summary>
        /// 不指定目录默认当前目录
        /// </summary>
        static LogUtil()
        {
            LogQueue = new List<QueueModel>();
            LogPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"logs\");
            Start();
        }

        /// <summary>
        /// 循环检测队列
        /// </summary>
        private static void Start()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    if (LogQueue.Count > 0)
                    {
                        try
                        {
                            using (StreamWriter sw = new StreamWriter(LogQueue[0].Path, true, Encoding.UTF8))
                            {
                                sw.Write(LogQueue[0].Msg);
                                LogQueue.Remove(LogQueue[0]);
                                sw.Close();
                                sw.Dispose();
                            }
                        }
                        catch (Exception ex)
                        {
                            WriteMessage(LogType.Error, ex.Message + "----WriteMessage：" + LogQueue[0].Msg);
                            LogQueue.Remove(LogQueue[0]);
                        }
                        Thread.Sleep(2);
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            });
        }

        /// <summary>
        /// 输出调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            WriteMessage(LogType.Debug, message);
        }

        /// <summary>
        /// 输出普通信息
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            WriteMessage(LogType.Info, message);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string message)
        {
            WriteMessage(LogType.Error, message);
        }

        /// <summary>
        /// 扩展方法 输出调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void WriteDebugLog(this string message)
        {
            WriteMessage(LogType.Debug, message);
        }

        /// <summary>
        /// 扩展方法 输出普通信息
        /// </summary>
        /// <param name="message"></param>
        public static void WriteInfoLog(this string message)
        {
            WriteMessage(LogType.Info, message);
        }

        /// <summary>
        /// 扩展方法 输出错误信息
        /// </summary>
        /// <param name="message"></param>
        public static void WriteErrorLog(this string message)
        {
            WriteMessage(LogType.Error, message);
        }

        private static void WriteMessage(LogType logType, string message)
        {
            try
            {
                if (!Directory.Exists(LogPath))
                {
                    Directory.CreateDirectory(LogPath);
                }

                var tLogPath = Path.Combine(LogPath, DateTime.Now.ToString("yyyy-MM-dd") + ".txt");


                // Console.WriteLine(tLogPath);

                if (!File.Exists(tLogPath))
                {
                    using (File.Create(tLogPath))
                    {

                    }
                }

                //添加队列
                LogQueue.Add(new QueueModel { Msg = $"{DateTime.Now}----【{logType.ToString()}】：{message}\r\n", Path = tLogPath });
            }
            catch (Exception ex)
            {
                WriteMessage(LogType.Error, ex.Message);
            }
        }
    }
}
