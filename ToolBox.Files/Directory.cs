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
        /// 删除指定目录的所有文件
        /// </summary>
        /// <param name="TargetDir">操作目录</param>
        /// <param name="delSubDir">默认为false 如果为true,包含对子目录的操作</param>
        public static void DeleteDirectoryFiles(string TargetDir, bool delSubDir = false)
        {
            try
            {
                foreach (string fileName in Directory.GetFiles(TargetDir))
                {
                    File.SetAttributes(fileName, FileAttributes.Normal);
                    File.Delete(fileName);
                }
                if (delSubDir)
                {
                    DirectoryInfo dir = new DirectoryInfo(TargetDir);
                    foreach (DirectoryInfo subDi in dir.GetDirectories())
                    {
                        DeleteDirectoryFiles(subDi.FullName, true);
                        subDi.Delete();
                    }
                }
            }
            catch (Exception ex)
            {
                LogUtil.Error("删除指定目录的所有文件和子目录失败：" + ex.Message);
                throw;
            }

        }

        /// <summary>
        /// 创建指定目录
        /// </summary>
        /// <param name="targetDir"></param>
        public static void CreateDirectory(string targetDir)
        {
            try
            {
                if (!Directory.Exists(targetDir))
                    Directory.CreateDirectory(targetDir);
            }
            catch (Exception ex)
            {
                $"创建目录发生错误{ex.Message}".WriteErrorLog();
            }
        }

        /// <summary>
        /// 建立子目录
        /// </summary>
        /// <param name="parentDir">目录路径</param>
        /// <param name="subDirName">子目录名称</param>
        public static void CreateDirectory(string parentDir, string subDirName)
        {
            CreateDirectory($@"{parentDir}/{subDirName}");
        }

        /// <summary>
        /// 重命名文件夹
        /// </summary>
        /// <param name="directoryPath">文件夹路径</param>
        /// <param name="OldFloderName">原文件夹名称</param>
        /// <param name="NewFloderName">新文件夹名称</param>
        /// <returns></returns>
        public static bool ReNameFloder(string directoryPath, string OldFloderName, string NewFloderName)
        {
            try
            {
                if (Directory.Exists($@"{directoryPath}/{OldFloderName}"))
                {
                    Directory.Move($@"{directoryPath}/{OldFloderName}", $@"{directoryPath}/{NewFloderName}");
                }
                return true;
            }
            catch (Exception ex)
            {
                $"重命名文件夹出错：{ex.Message}".WriteErrorLog();
                return false;
            }
        }

        /// <summary>
        /// 删除指定目录
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteDirectory(string targetDir)
        {
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(targetDir);
                if (dirInfo.Exists)
                {
                    DeleteDirectoryFiles(targetDir, true);
                    dirInfo.Delete(true);
                }
            }
            catch (Exception ex)
            {
                $"删除指定目录失败：{ex.Message}".WriteErrorLog();
            }

        }

        /// <summary>
        /// 检测目录是否存在
        /// </summary>
        /// <param name="StrPath">路径</param>
        /// <returns></returns>
        public static bool CheckIsExistsDirectory(string StrPath)
        {
            return Directory.Exists(StrPath);
        }

        /// <summary>
        /// 检测目录是否存在，不存在则创建
        /// </summary>
        /// <param name="StrPath">路径</param>
        /// <param name="Create">如果不存在，是否创建</param>
        public static void CreateDirectoryIfNoExists(string StrPath)
        {
            try
            {
                if (!CheckIsExistsDirectory(StrPath))
                {
                    Directory.CreateDirectory(StrPath);
                }
            }
            catch (Exception ex)
            {
                $"检测目录是否存在，不存在则创建  方法出错：{ex.Message}".WriteErrorLog();
            }

        }

        /// <summary>
        /// 删除指定目录下的所有文件夹
        /// </summary>
        /// <param name="targetDir">目录路径</param>
        public static void DeleteSubDirectory(string targetDir)
        {
            try
            {
                foreach (string subDir in Directory.GetDirectories(targetDir))
                {
                    DeleteDirectory(subDir);
                }
            }
            catch (Exception ex)
            {
                $"删除指定目录下的所有文件夹 出错：{ex.Message}".WriteErrorLog();
            }

        }

        /// <summary>
        /// 复制指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="copySubDir">如果为true,包含目录,否则不包含</param>
        public static void CopyFiles(string sourceDir, string targetDir, bool overWrite, bool copySubDir)
        {
            try
            {
                //复制当前目录文件
                foreach (string sourceFileName in Directory.GetFiles(sourceDir))
                {
                    string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf("\\") + 1));

                    if (File.Exists(targetFileName))
                    {
                        if (overWrite == true)
                        {
                            File.SetAttributes(targetFileName, FileAttributes.Normal);
                            File.Copy(sourceFileName, targetFileName, overWrite);
                        }
                    }
                    else
                    {
                        File.Copy(sourceFileName, targetFileName, overWrite);
                    }
                }
            }
            catch (Exception ex)
            {
                $"复制指定目录的所有文件 出错：{ex.Message}".WriteErrorLog();
            }

        }

        /// <summary>
        /// 移动指定目录的所有文件
        /// </summary>
        /// <param name="sourceDir">原始目录</param>
        /// <param name="targetDir">目标目录</param>
        /// <param name="overWrite">如果为true,覆盖同名文件,否则不覆盖</param>
        /// <param name="moveSubDir">如果为true,包含目录,否则不包含</param>
        public static void MoveFiles(string sourceDir, string targetDir, bool overWrite, bool moveSubDir)
        {
            //移动当前目录文件
            foreach (string sourceFileName in Directory.GetFiles(sourceDir))
            {
                string targetFileName = Path.Combine(targetDir, sourceFileName.Substring(sourceFileName.LastIndexOf("\\") + 1));
                if (File.Exists(targetFileName))
                {
                    if (overWrite == true)
                    {
                        File.SetAttributes(targetFileName, FileAttributes.Normal);
                        File.Delete(targetFileName);
                        File.Move(sourceFileName, targetFileName);
                    }
                }
                else
                {
                    File.Move(sourceFileName, targetFileName);
                }
            }
            if (moveSubDir)
            {
                foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
                {
                    string targetSubDir = Path.Combine(targetDir, sourceSubDir.Substring(sourceSubDir.LastIndexOf("\\") + 1));
                    if (!Directory.Exists(targetSubDir))
                        Directory.CreateDirectory(targetSubDir);
                    MoveFiles(sourceSubDir, targetSubDir, overWrite, true);
                    Directory.Delete(sourceSubDir);
                }
            }
        }

        /// <summary>
        /// 获取指定目录及子目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>
        /// <param name="isSearchChild">是否搜索子目录</param>
        public static string[] GetFileNames(string directoryPath, string searchPattern, bool isSearchChild)
        {
            //如果目录不存在，则抛出异常
            if (!CheckIsExistsDirectory(directoryPath))
            {
                return new string[] { };
            }

            try
            {
                if (isSearchChild)
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.AllDirectories);
                }
                else
                {
                    return Directory.GetFiles(directoryPath, searchPattern, SearchOption.TopDirectoryOnly);
                }
            }
            catch (IOException ex)
            {
                $"获取指定目录及子目录中所有文件列表 出错:{ex.Message}".WriteErrorLog();
                return new string[] { };
            }
        }

        /// <summary>
        /// 检测指定目录中是否存在指定的文件,若要搜索子目录请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>
        /// <param name="searchPattern">模式字符串，"*"代表0或N个字符，"?"代表1个字符。
        /// 范例："Log*.xml"表示搜索所有以Log开头的Xml文件。</param>        
        public static bool Contains(string directoryPath, string searchPattern)
        {
            try
            {
                //获取指定的文件列表
                string[] fileNames = GetFileNames(directoryPath, searchPattern, false);

                //判断指定文件是否存在
                if (fileNames.Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                $"检测指定目录中是否存在指定的文件. 出错:{ex.Message}".WriteErrorLog();
                return false;
            }
        }

        /// <summary>
        /// 获取指定目录中所有子目录列表,若要搜索嵌套的子目录列表,请使用重载方法.
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetDirectories(string directoryPath)
        {
            try
            {
                return Directory.GetDirectories(directoryPath);
            }
            catch (IOException ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取指定目录中所有文件列表
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static string[] GetFileNames(string directoryPath)
        {
            try
            {
                //如果目录不存在，则抛出异常
                if (!CheckIsExistsDirectory(directoryPath))
                {
                    return new string[] { };
                }

                //获取文件列表
                return Directory.GetFiles(directoryPath);
            }
            catch (Exception ex)
            {
                $"获取指定目录中所有文件列表 错误：{ex.Message}".WriteErrorLog();
                return new string[] { };
            }

        }

        /// <summary>
        /// 检测指定目录是否为空
        /// </summary>
        /// <param name="directoryPath">指定目录的绝对路径</param>        
        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                //判断是否存在文件
                string[] fileNames = GetFileNames(directoryPath);
                if (fileNames.Length > 0)
                {
                    return false;
                }

                //判断是否存在文件夹
                string[] directoryNames = GetDirectories(directoryPath);
                if (directoryNames.Length > 0)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                $"检测指定目录是否为空 错误：{ex.Message}".WriteErrorLog();
                return false;
            }
        }

    }
}
