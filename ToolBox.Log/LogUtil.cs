using System;
using System.IO;
using System.Text;

namespace ToolBox.Log
{
    public class LogUtil
    {
        private string LogPath { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="directoryPath">日志保存目录</param>
        public LogUtil(string directoryPath)
        {
            LogPath = directoryPath + "\\";
        }



        /// <summary>
        /// 不指定目录默认当前目录
        /// </summary>
        public LogUtil()
        {
            LogPath = "logs\\";
        }

        /// <summary>
        /// 输出调试信息
        /// </summary>
        /// <param name="message"></param>
        public void Debug(string message)
        {
            WriteMessage(LogType.Debug, message);
        }

        /// <summary>
        /// 输出普通信息
        /// </summary>
        /// <param name="message"></param>
        public void Info(string message)
        {
            WriteMessage(LogType.Info, message);
        }

        /// <summary>
        /// 输出错误信息
        /// </summary>
        /// <param name="message"></param>
        public void Error(string message)
        {
            WriteMessage(LogType.Error, message);
        }

        private void WriteMessage(LogType logType, string message)
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
