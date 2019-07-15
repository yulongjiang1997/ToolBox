using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Files
{
    public partial class FileUtil
    {
        #region 获取应用程序当前可执行文件的路径
        /// <summary>
        /// 获取应用程序当前可执行文件的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppCurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }
        #endregion
    }
}
