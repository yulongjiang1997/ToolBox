using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ToolBox.Files
{
    public partial class FileUtil
    {
        private Log.LogUtil Log { get; set; }
        private FileUtil()
        {
            Log = new Log.LogUtil();
        }

        /// <summary>
        /// 获取应用程序当前可执行文件的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppCurrentDirectory()
        {
            return AppDomain.CurrentDomain.BaseDirectory;
        }

        /// <summary>
        /// 获取文件的md5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }


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
        public static DateTime GetFileLastUpdateTime(string FileUrl)
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
        public static string GetFileExtension(string PathFileName)
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
        public static bool IsHiddenFile(string path)
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
