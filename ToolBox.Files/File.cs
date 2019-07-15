using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Files
{
    public partial class FileUtil
    {
        #region 返回文件是否存在
        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            return System.IO.File.Exists(filename);
        }
        #endregion

        #region 获取文件最后修改时间
        /// <summary>
        /// 获取文件最后修改时间
        /// </summary>
        /// <param name="FileUrl">文件真实路径</param>
        /// <returns></returns>
        public DateTime GetFileWriteTime(string FileUrl)
        {
            return System.IO.File.GetLastWriteTime(FileUrl);
        }
        #endregion

        #region 返回指定文件的扩展名
        /// <summary>
        /// 返回指定路径的文件的扩展名
        /// </summary>
        /// <param name="PathFileName">完整路径的文件</param>
        /// <returns></returns>
        public string GetFileExtension(string PathFileName)
        {
            return System.IO.Path.GetExtension(PathFileName);
        }
        #endregion

        #region 判断是否是隐藏文件
        /// <summary>
        /// 判断是否是隐藏文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public bool IsHiddenFile(string path)
        {
            System.IO.FileAttributes MyAttributes = System.IO.File.GetAttributes(path);
            string MyFileType = MyAttributes.ToString();
            if (MyFileType.LastIndexOf("Hidden") != -1) //是否隐藏文件
            {
                return true;
            }
            else
                return false;
        }
        #endregion
    }
}
