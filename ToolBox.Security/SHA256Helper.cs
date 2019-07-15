using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace ToolBox.Security
{
   public static partial  class SHA256Helper
    {
        /// <summary>
        /// SHA256函数
        /// </summary>
        /// /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果</returns>
        public static string SHA256(string str)
        {
            byte[] SHA256Data = Encoding.UTF8.GetBytes(str);
            SHA256Managed Sha256 = new SHA256Managed();
            byte[] Result = Sha256.ComputeHash(SHA256Data);
            return Convert.ToBase64String(Result);  //返回长度为44字节的字符串
        }

        /// <summary>
        /// SHA256函数
        /// </summary>
        /// <param name="str">原始字符串</param>
        /// <returns>SHA256结果(16进制字节)</returns>
        public static string SHA256(this string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] sha256Data = encoding.GetBytes(str);
            var sha256 = new SHA256Managed();
            byte[] result = sha256.ComputeHash(sha256Data);

            return result.ByteToHex().ToLower();

            //return Convert.ToBase64String(result); //返回base64
        }

    }
}
