using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Files
{
    public partial class FileUtil
    {
        #region 获取本地驱动器名列表
        /// <summary>
        /// 获取本地驱动器名列表
        /// </summary>
        /// <returns></returns>
        public static string[] GetLocalDrives()
        {
            return System.IO.Directory.GetLogicalDrives();
        }
        #endregion
    }
}
