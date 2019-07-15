using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToolBox.Security
{
    public static class Md5Helper
    {
        /// <summary>
        /// MD5函数,需引用：using System.Security.Cryptography;
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>MD5结果</returns>
        public static string MD5(this string str)
        {
            //微软md5方法参考return FormsAuthentication.HashPasswordForStoringInConfigFile(str, "md5");
            byte[] b = Encoding.Default.GetBytes(str);
            b = new MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }



        /// <summary>
        /// MD5加密byte
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>md5加密结果</returns>
        public static string MD5(this byte[] bytes)
        {
            var md5 = new MD5CryptoServiceProvider();
            byte[] hash = md5.ComputeHash(bytes, 0, bytes.Length);
            StringBuilder sb = new StringBuilder();
            foreach (byte value in hash)
            {
                sb.AppendFormat("{0:x2}", value);
            }
            return sb.ToString();
        }


        /// <summary>
        /// 读取文件MD5值
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns>MD5</returns>
        public static string FileMd5(string filePath)
        {
            var cfilePath = filePath + "e";
            if (File.Exists(cfilePath))
                File.Delete(cfilePath);
            File.Copy(filePath, cfilePath);//复制一份，防止占用


            if (File.Exists(cfilePath))
            {
                var buffer = File.ReadAllBytes(cfilePath);
                System.IO.File.Delete(cfilePath);
                return buffer.MD5();
            }
            else
            {
                throw new Exception("读取文件MD5出错!");
            }
        }

    }
}
