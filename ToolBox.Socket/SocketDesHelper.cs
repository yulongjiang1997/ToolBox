using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToolBox.Socket
{
    /// <summary>
    /// socket des helper
    /// </summary>
   public static class SocketDesHelper
    {

        /// <summary>
        /// Set Encryption key
        /// </summary>
        private static string encryptKey { get; set; } = "socket123";


        /// <summary>
        /// the key is length must >=8 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static bool SetEncryptKey(string key) {

            if (key.Length >= 8)
            {
                string result=  "test".Encrypt();

                if (result.Equals("")) {
                    return false;
                }

                if (result.Decrypt().Equals("")) {

                    return false;
                }


                return true;
            }
            else {

                return false;
            }

          
        }


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
        /// <param name="key">密钥字符串</param> 
        /// <returns>返回string</returns> 
        public static string Encrypt(string text, string key)
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(key.Substring(0, 8));
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
                return "";
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
        /// <param name="key">密钥字符串</param> 
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
