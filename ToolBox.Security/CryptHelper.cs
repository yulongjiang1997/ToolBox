using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ToolBox.Security
{
    /// <summary>
    /// 简单加密类
    /// </summary>
    public static class CryptHelper
    {

     
        /// <summary>
        /// 文本_加密 简单加密
        /// </summary>
        /// <param name="str">待加密的文本</param>
        /// <param name="pass">加密的密码</param>
        /// <returns></returns>
        public static string StringEncrypt(string str, string pass, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            byte[] bin = encoding.GetBytes(str);
            List<byte> list = new List<byte>();
            for (int i = 0; i < bin.Length; i++)
            {
                var ch = (byte)(bin[i] ^ 3600);
                list.Add(ch);
            }

            

            string md5 = pass.MD5().Substring(2, 9);

            string hex =list.ToArray().ByteToHex();


            return hex + md5.ToUpper();
        }

        /// <summary>
        /// 文本解密 (对应易语言模块)
        /// </summary>
        /// <param name="str">待解密的文本</param>
        /// <param name="pass">解密的密文</param>
        /// <returns></returns>
        public static string StringDecrypt(string str, string pass, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            

            string md5 = pass.MD5().Substring(2, 9).ToUpper();
            if (!str.EndsWith(md5))
            {
                return "";
            }

            string item = str.Substring(0, str.Length - 9);

            byte[] bin = item.HexToByte();
            List<byte> list = new List<byte>();
            for (int i = 0; i < bin.Length; i++)
            {
                var ch = (byte)(bin[i] ^ 3600);
                list.Add(ch);
            }

            string html = encoding.GetString(list.ToArray());

            return html;
        }

   


    }
}
