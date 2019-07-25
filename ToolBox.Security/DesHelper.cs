using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToolBox.Security
{

    /// <summary>
    /// DES加密/解密工具类。
    /// </summary>
    public static class DesHelper
    {

        public static string encryptKey = "km4250km";

        #region ========加密========

        /// <summary>
        /// DES【加密】字符串，使用缺省密钥。
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>返回string</returns>
        public static string Encrypt(this string text)
        {
            return Encrypt(text, encryptKey);
        }

        /// <summary> 
        /// DES【加密】字符串，使用给定密钥。
        /// </summary> 
        /// <param name="text">字符串</param> 
        /// <param name="sKey">密钥字符串</param> 
        /// <returns>返回string</returns> 
        public static string Encrypt(string text, string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                //rgbIV与rgbKey可以不一样，这里只是为了简便，读者可以自行修改
                byte[] rgbIV = rgbKey;// Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] inputByteArray = Encoding.UTF8.GetBytes(text);
                DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch
            {
                return text;
            }

        }

        #endregion

        #region ========解密========


        /// <summary>
        /// DES【解密】字符串，使用缺省密钥。 
        /// </summary>
        /// <param name="text">字符串</param> 
        /// <returns>返回string</returns>
        public static string Decrypt(this string text)
        {   
             return Decrypt(text, encryptKey);
                
        }

        /// <summary> 
        /// DES【解密】字符串，使用给定密钥。
        /// </summary> 
        /// <param name="text">字符串</param> 
        /// <param name="sKey">密钥字符串</param> 
        /// <returns>返回string</returns> 
        public static string Decrypt(string text, string key)
        {
            try
            {
                byte[] keyBytes = Encoding.UTF8.GetBytes(key.Substring(0, 8));
                byte[] keyIV = keyBytes;
                byte[] inputByteArray = Convert.FromBase64String(text);
                DESCryptoServiceProvider provider = new DESCryptoServiceProvider();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, provider.CreateDecryptor(keyBytes, keyIV), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Encoding.UTF8.GetString(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return "";
             
            }


        }

        #endregion

    }
}
