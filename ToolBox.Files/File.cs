using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ToolBox.Log;

namespace ToolBox.Files
{
    public partial class FileUtil
    {

        /// <summary>
        /// 获取应用程序当前可执行文件的路径
        /// </summary>
        /// <returns></returns>
        public static string GetAppCurrentDirectory()
        {
            try
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
            catch (Exception ex)
            {
                ("获取应用程序当前可执行文件的路径 出错" + ex.Message).WriteErrorLog();
                return "获取应用程序当前可执行文件的路径 出错";
            }

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
                var result = "获取文件的md5值出错:" + ex.Message;
                result.WriteErrorLog();
                return result;
            }
        }

        /// <summary>
        /// 删除指定文件
        /// </summary>
        /// <param name="TargetFileDir">指定文件的目录</param>
        public static void DeleteFiles(string TargetFileDir)
        {
            try
            {
                File.Delete(TargetFileDir);
            }
            catch (Exception ex)
            {
                Log.LogUtil.Error("删除指定文件出错：" + ex.Message);
            }
        }

        /// <summary>
        /// 返回文件是否存在
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否存在</returns>
        public static bool FileExists(string filename)
        {
            try
            {
                return File.Exists(filename);
            }
            catch (Exception ex)
            {
                Log.LogUtil.Error("返回文件是否存在出错：" + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 获取文件最后修改时间
        /// </summary>
        /// <param name="FileUrl">文件真实路径</param>
        /// <returns></returns>
        public static DateTime GetFileLastUpdateTime(string FileUrl)
        {
            try
            {
                return File.GetLastWriteTime(FileUrl);
            }
            catch (Exception ex)
            {
                Log.LogUtil.Error("获取文件最后修改时间" + ex.Message);
                return new DateTime(1970, 1, 1);
            }
        }

        /// <summary>
        /// 获得文件的拓展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileExtensionName(string filePath)
        {
            try
            {
                //获取文件的名称
                FileInfo fi = new FileInfo(filePath);
                return fi.Extension;
            }
            catch (Exception ex)
            {
                ("获得文件的拓展名出错" + ex.Message).WriteErrorLog();
                return ex.Message;
            }
        }

        /// <summary>
        /// 判断是否是隐藏文件
        /// </summary>
        /// <param name="path">文件路径</param>
        /// <returns></returns>
        public static bool IsHiddenFile(string path)
        {
            try
            {
                FileAttributes MyAttributes = System.IO.File.GetAttributes(path);
                string MyFileType = MyAttributes.ToString();
                if (MyFileType.LastIndexOf("Hidden") != -1) //是否隐藏文件
                {
                    return true;
                }
                else
                    return false;
            }
            catch (Exception ex)
            {
                ("判断是否是隐藏文件出错" + ex.Message).WriteErrorLog();
                return false;

            }
        }

        /// <summary>
        /// 获取文本文件的行数
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static int GetFileLineCount(string filePath)
        {
            try
            {
                //将文本文件的各行读到一个字符串数组中
                string[] rows = File.ReadAllLines(filePath);

                //返回行数
                return rows.Length;
            }
            catch (Exception ex)
            {
                ("获取文本文件的行数出错了" + ex.Message).WriteErrorLog();
                return 0;
            }
        }

        /// <summary>
        /// 以UTF8编码格式读取文本文件
        /// </summary>
        /// <param name="FilePath">文件路径及文件名</param>
        /// <returns></returns>
        public static string ReadTxtFileByUTF8(string FilePath)
        {
            try
            {
                string content = "";//返回的字符串
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader reader = new StreamReader(fs, Encoding.UTF8))
                    {
                        string text = string.Empty;
                        while (!reader.EndOfStream)
                        {
                            text += reader.ReadLine() + "\r\n";
                            content = text;
                        }
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                ("以UTF8编码格式读取文本文件出错" + ex.Message).WriteErrorLog();
                return ex.Message;
            }

        }

        /// <summary>
        /// 以GB2312编码格式读取文本文件
        /// </summary>
        /// <param name="FilePath">文件路径及文件名</param>
        /// <returns></returns>
        public static string ReadTxtFileByGb2312(string FilePath)
        {
            try
            {
                string content = "";//返回的字符串
                using (FileStream fs = new FileStream(FilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader reader = new StreamReader(fs, Encoding.GetEncoding("gb2312")))
                    {
                        string text = string.Empty;
                        while (!reader.EndOfStream)
                        {
                            text += reader.ReadLine() + "\r\n";
                            content = text;
                        }
                    }
                }
                return content;
            }
            catch (Exception ex)
            {
                ("以gb2312编码格式读取文本文件出错" + ex.Message).WriteErrorLog();
                return ex.Message;
            }

        }

        /// <summary>
        /// 获得文件名，不包含扩展名
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>        
        public static string GetFileNameNoExtension(string filePath)
        {
            try
            {
                //获取文件的名称
                FileInfo fi = new FileInfo(filePath);
                return fi.Name.Split('.')[0];
            }
            catch (Exception ex)
            {
                ("获得文件名，不包含扩展名 出错" + ex.Message).WriteErrorLog();
                return ex.Message;
            }

        }

        /// <summary>
        /// 将内容写入文本文件(如果文件path存在就打开，不存在就新建)
        /// </summary>
        /// <param name="FilePath">文件路径</param>
        /// <param name="WriteStr">要写入的内容</param>
        /// <param name="isAppend">是否追加写入，true追加，flase覆盖  默认追加</param>
        public static void WriteStrToTxtFile(string FilePath, string WriteStr, bool isAppend = true)
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    using (File.Create(FilePath))
                    {

                    }
                }
                using (StreamWriter swt = new StreamWriter(FilePath, isAppend, Encoding.UTF8))
                {
                    swt.WriteLine(WriteStr);
                }
            }
            catch (Exception ex)
            {
                $"将内容写入文本文件出错 写入模式{(isAppend ? "追加" : "覆盖")}{ex.Message}".WriteErrorLog();
            }
        }

        /// <summary>
        /// 将源文件的内容复制到目标文件中
        /// </summary>
        /// <param name="sourceFilePath">源文件的绝对路径</param>
        /// <param name="destFilePath">目标文件的绝对路径</param>
        /// <param name="isOverwrite">是否覆盖，true 覆盖，flase 不覆盖，默认true</param>
        public static void Copy(string sourceFilePath, string destFilePath, bool isOverwrite = true)
        {
            try
            {
                File.Copy(sourceFilePath, destFilePath, isOverwrite);
            }
            catch (Exception ex)
            {
                $"复制文件出错{ex.Message}".WriteErrorLog();
            }

        }

        /// <summary>
        /// 清空文件内容
        /// </summary>
        /// <param name="filePath">文件的绝对路径</param>
        public static void ClearFile(string filePath)
        {
            try
            {
                //删除文件
                File.Delete(filePath);

                //重新创建该文件
                File.Create(filePath);
            }
            catch (Exception ex)
            {
                $"清空文件出错：{ex.Message}".WriteErrorLog();
            }
        }

        /// <summary>
        /// 写入数据流到文件
        /// </summary>
        /// <param name="FromStream">数据</param>
        /// <param name="TargetFile">目标文件</param>
        public static void SaveStreamToFile(Stream FromStream, string TargetFile)
        {
            try
            {
                //创建要保存到的文件
                Stream ToStream = null;
                if (!File.Exists(TargetFile))
                {
                    using (ToStream = File.Create(TargetFile))
                    {

                    }
                }
                else
                {
                    using (ToStream = File.Open(TargetFile, FileMode.Open))
                    {

                    }
                }

                //因为使用二进制 读或写
                //所以它们可以使用所有格式
                //可以是图片。文本。文件。MP3等等

                using (BinaryReader br = new BinaryReader(FromStream))
                {
                    using (BinaryWriter bw = new BinaryWriter(ToStream))
                    {
                        //将数据从fromstream复制到outstream
                        //从long转换为int
                        bw.Write(br.ReadBytes((int)FromStream.Length));
                        //保存
                        bw.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                $"写入数据流失败 {ex.Message}".WriteErrorLog();
            }
        }

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
