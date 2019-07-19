using System;
using System.Collections.Generic;
using System.Text;
using ToolBox.Log;

namespace ToolBox.Files
{
    public partial class FileUtil
    {
        /// <summary>
        /// 获取本地驱动器名列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetLocalDrives()
        {
            try
            {
                return System.IO.Directory.GetLogicalDrives();
            }
            catch (Exception ex)
            {
                ("获取本地驱动器名列表 错误" + ex.Message).WriteErrorLog();
                throw;
            }
            
        }
    }
}
