using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Security
{
   public static class Base64Helper
    {

        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="str">需要加密的字符串</param>
        /// <returns>加密后的数据</returns>
        public static string Base64Encrypt(this string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] encbuff = encoding.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="str">需要解密的字符串</param>
        /// <returns>解密后的数据</returns>
        public static string Base64Decrypt(this string str, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] decbuff = Convert.FromBase64String(str);
            return encoding.GetString(decbuff);
        }
    }
}
