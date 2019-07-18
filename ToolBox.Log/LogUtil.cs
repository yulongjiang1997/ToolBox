using System;
using System.IO;
using System.Text;

namespace ToolBox.Log
{
    /// <summary>
    /// 一个超级简单的日志帮助类
    /// </summary>
    public static class LogUtil
    {
        private static string LogPath { get; set; }

        /// <summary>
        /// 设置指定的日志目录
        /// </summary>
        /// <param name="directoryPath">日志保存目录</param>
        public static void SetdirectoryPath(string directoryPath)
        {
            LogPath = directoryPath + "\\";
        }

        /// <summary>
        /// 不指定目录默认当前目录
        /// </summary>
        static LogUtil()
        {
            LogPath = "logs\\";
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
        /// 输出调试信息
        /// </summary>
        /// <param name="message"></param>
        public static void WriteDebugLog(this string message)
        {
            WriteMessage(LogType.Debug, message);
        }

        /// <summary>
        /// 输出普通信息
        /// </summary>
        /// <param name="message"></param>
        public static void WriteInfoLog(this string message)
        {
            WriteMessage(LogType.Info, message);
        }

        /// <summary>
        /// 输出错误信息
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

                var tLogPath = LogPath + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";

                if (!File.Exists(tLogPath))
                {
                    using (File.Create(tLogPath))
                    {

                    }
                }

                using (StreamWriter sw = new StreamWriter(tLogPath, true, Encoding.UTF8))
                {
                    sw.Write($"{DateTime.Now}----【{logType.ToString()}】：{message}\r\n");
                    sw.Close();
                    sw.Dispose();
                }
            }
            catch (Exception ex)
            {
                WriteMessage(LogType.Error, ex.Message);
            }
        }
    }
}
