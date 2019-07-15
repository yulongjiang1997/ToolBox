using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ToolBox.Files
{
    public partial class FileUtil
    {
        #region 获取文件大小并以B，KB，GB，TB方式表示[+2 重载]
        /// <summary>
        /// 获取文件大小并以B，KB，GB，TB方式表示
        /// </summary>
        /// <param name="File">文件(FileInfo类型)</param>
        /// <returns></returns>
        public static string GetFileSize(FileInfo File)
        {
            string Result = "";
            long FileSize = File.Length;
            if (FileSize >= 1024 * 1024 * 1024)
            {
                if (FileSize / 1024 * 1024 * 1024 * 1024 >= 1024) Result = string.Format("{0:############0.00} TB", (double)FileSize / 1024 * 1024 * 1024 * 1024);
                else Result = string.Format("{0:####0.00} GB", (double)FileSize / 1024 * 1024 * 1024);
            }
            else if (FileSize >= 1024 * 1024) Result = string.Format("{0:####0.00} MB", (double)FileSize / 1024 * 1024);
            else if (FileSize >= 1024) Result = string.Format("{0:####0.00} KB", (double)FileSize / 1024);
            else Result = string.Format("{0:####0.00} Bytes", FileSize);
            return Result;
        }


        /// <summary>
        /// 获取文件大小并以B，KB，GB，TB方式表示
        /// </summary>
        /// <param name="FilePath">文件的具体路径</param>
        /// <returns></returns>
        public static string GetFileSize(string FilePath)
        {
            string Result = "";
            FileInfo File = new FileInfo(FilePath);
            long FileSize = File.Length;
            if (FileSize >= 1024 * 1024 * 1024)
            {
                if (FileSize / 1024 * 1024 * 1024 * 1024 >= 1024) Result = string.Format("{0:########0.00} TB", (double)FileSize / 1024 * 1024 * 1024 * 1024);
                else Result = string.Format("{0:####0.00} GB", (double)FileSize / 1024 * 1024 * 1024);
            }
            else if (FileSize >= 1024 * 1024) Result = string.Format("{0:####0.00} MB", (double)FileSize / 1024 * 1024);
            else if (FileSize >= 1024) Result = string.Format("{0:####0.00} KB", (double)FileSize / 1024);
            else Result = string.Format("{0:####0.00} Bytes", FileSize);
            return Result;
        }
        #endregion
    }
}
