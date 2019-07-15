using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ToolBox.Security
{
   public static partial class AesHelper
    {



        private static  string Default_AES_Key = "@#kim123";

        private static byte[] Keys =
        {
            0x41,
            0x72,
            0x65,
            0x79,
            0x6F,
            0x75,
            0x6D,
            0x79,
            0x53,
            0x6E,
            0x6F,
            0x77,
            0x6D,
            0x61,
            0x6E,
            0x3F
        };

        /// <summary>
        /// 对称加密算法AES RijndaelManaged加密(RijndaelManaged（AES）算法是块式加密算法)
        /// </summary>
        /// <param name="encryptString">待加密字符串</param>
        /// <returns>加密结果字符串</returns>
        public static string AESEncrypt(this string encryptString)
        {
            return AESEncrypt(encryptString, Default_AES_Key);
        }

        /// <summary>
        /// 对称加密算法AES RijndaelManaged加密(RijndaelManaged（AES）算法是块式加密算法)
        /// </summary>
        /// <param name="encryptString">待加密字符串</param>
        /// <param name="encryptKey">加密密钥，须半角字符</param>
        /// <returns>加密结果字符串</returns>
        public static string AESEncrypt(this string encryptString, string encryptKey, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            encryptKey = GetSubString(encryptKey, 32, "");
            encryptKey = encryptKey.PadRight(32, ' ');
            var rijndaelProvider = new RijndaelManaged
            {
                Key = encoding.GetBytes(encryptKey.Substring(0, 32)),
                IV = Keys
            };
            ICryptoTransform rijndaelEncrypt = rijndaelProvider.CreateEncryptor();
            byte[] inputData = encoding.GetBytes(encryptString);
            byte[] encryptedData = rijndaelEncrypt.TransformFinalBlock(inputData, 0, inputData.Length);
            return Convert.ToBase64String(encryptedData);
        }

        /// <summary>
        /// 对称加密算法AES RijndaelManaged解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <returns>解密成功返回解密后的字符串,失败返源串</returns>
        public static string AESDecrypt(this string decryptString)
        {
            return AESDecrypt(decryptString, Default_AES_Key);
        }

        /// <summary>
        /// 对称加密算法AES RijndaelManaged解密字符串
        /// </summary>
        /// <param name="decryptString">待解密的字符串</param>
        /// <param name="decryptKey">解密密钥,和加密密钥相同</param>
        /// <returns>解密成功返回解密后的字符串,失败返回空</returns>
        public static string AESDecrypt(this string decryptString, string decryptKey, Encoding encoding = null)
        {
            try
            {
                if (encoding == null)
                {
                    encoding = Encoding.UTF8;
                }

                decryptKey = GetSubString(decryptKey, 32, "");
                decryptKey = decryptKey.PadRight(32, ' ');
                var rijndaelProvider = new RijndaelManaged()
                {
                    Key = encoding.GetBytes(decryptKey),
                    IV = Keys
                };
                ICryptoTransform rijndaelDecrypt = rijndaelProvider.CreateDecryptor();
                byte[] inputData = Convert.FromBase64String(decryptString);
                byte[] decryptedData = rijndaelDecrypt.TransformFinalBlock(inputData, 0, inputData.Length);
                return encoding.GetString(decryptedData);
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 按字节长度(按字节,一个汉字为2个字节)取得某字符串的一部分
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="length">所取字符串字节长度</param>
        /// <param name="tailString">附加字符串(当字符串不够长时，尾部所添加的字符串，一般为"...")</param>
        /// <returns>某字符串的一部分</returns>
        private static string GetSubString(this string sourceString, int length, string tailString)
        {
            return GetSubString(sourceString, 0, length, tailString);
        }

        /// <summary>
        /// 按字节长度(按字节,一个汉字为2个字节)取得某字符串的一部分
        /// </summary>
        /// <param name="sourceString">源字符串</param>
        /// <param name="startIndex">索引位置，以0开始</param>
        /// <param name="length">所取字符串字节长度</param>
        /// <param name="tailString">附加字符串(当字符串不够长时，尾部所添加的字符串，一般为"...")</param>
        /// <returns>某字符串的一部分</returns>
        private static string GetSubString(this string sourceString, int startIndex, int length, string tailString)
        {
            //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            if (System.Text.RegularExpressions.Regex.IsMatch(sourceString, "[\u0800-\u4e00]+") || System.Text.RegularExpressions.Regex.IsMatch(sourceString, "[\xAC00-\xD7A3]+"))
            {
                //当截取的起始位置超出字段串长度时
                if (startIndex >= sourceString.Length)
                {
                    return string.Empty;
                }

                return sourceString.Substring(startIndex, length + startIndex > sourceString.Length ? (sourceString.Length - startIndex) : length);
            }

            //中文字符，如"中国人民abcd123"
            if (length <= 0)
            {
                return string.Empty;
            }

            byte[] bytesSource = Encoding.Default.GetBytes(sourceString);

            //当字符串长度大于起始位置
            if (bytesSource.Length > startIndex)
            {
                int endIndex = bytesSource.Length;

                //当要截取的长度在字符串的有效长度范围内
                if (bytesSource.Length > (startIndex + length))
                {
                    endIndex = length + startIndex;
                }
                else
                {
                    //当不在有效范围内时,只取到字符串的结尾
                    length = bytesSource.Length - startIndex;
                    tailString = "";
                }

                var anResultFlag = new int[length];
                int nFlag = 0;
                //字节大于127为双字节字符
                for (int i = startIndex; i < endIndex; i++)
                {
                    if (bytesSource[i] > 127)
                    {
                        nFlag++;
                        if (nFlag == 3)
                        {
                            nFlag = 1;
                        }
                    }
                    else
                    {
                        nFlag = 0;
                    }

                    anResultFlag[i] = nFlag;
                }

                //最后一个字节为双字节字符的一半
                if ((bytesSource[endIndex - 1] > 127) && (anResultFlag[length - 1] == 1))
                {
                    length++;
                }

                byte[] bsResult = new byte[length];
                Array.Copy(bytesSource, startIndex, bsResult, 0, length);
                var myResult = Encoding.Default.GetString(bsResult);
                myResult += tailString;
                return myResult;
            }

            return string.Empty;
        }

        /// <summary>
        /// 加密文件流
        /// </summary>
        /// <param name="fs">需要加密的文件流</param>
        /// <param name="decryptKey">加密密钥</param>
        /// <returns>加密流</returns>
        public static CryptoStream AESEncryptStrream(FileStream fs, string decryptKey, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            decryptKey = GetSubString(decryptKey, 32, "");
            decryptKey = decryptKey.PadRight(32, ' ');
            var rijndaelProvider = new RijndaelManaged()
            {
                Key = encoding.GetBytes(decryptKey),
                IV = Keys
            };
            ICryptoTransform encrypto = rijndaelProvider.CreateEncryptor();
            return new CryptoStream(fs, encrypto, CryptoStreamMode.Write);
        }

        /// <summary>
        /// 解密文件流
        /// </summary>
        /// <param name="fs">需要解密的文件流</param>
        /// <param name="decryptKey">解密密钥</param>
        /// <returns>加密流</returns>
        public static CryptoStream AESDecryptStream(FileStream fs, string decryptKey, Encoding encoding = null)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            decryptKey = GetSubString(decryptKey, 32, "");
            decryptKey = decryptKey.PadRight(32, ' ');
            var rijndaelProvider = new RijndaelManaged()
            {
                Key = encoding.GetBytes(decryptKey),
                IV = Keys
            };
            ICryptoTransform decrypto = rijndaelProvider.CreateDecryptor();
            return new CryptoStream(fs, decrypto, CryptoStreamMode.Read);
        }

        /// <summary>
        /// 对指定文件AES加密
        /// </summary>
        /// <param name="input">源文件流</param>
        /// <param name="outputPath">输出文件路径</param>
        public static void AESEncryptFile(FileStream input, string outputPath)
        {
            using (FileStream fren = new FileStream(outputPath, FileMode.Create))
            {
                CryptoStream enfr = AESEncryptStrream(fren, Default_AES_Key);
                byte[] bytearrayinput = new byte[input.Length];
                input.Read(bytearrayinput, 0, bytearrayinput.Length);
                enfr.Write(bytearrayinput, 0, bytearrayinput.Length);
            }
        }

        /// <summary>
        /// 对指定的文件AES解密
        /// </summary>
        /// <param name="input">源文件流</param>
        /// <param name="outputPath">输出文件路径</param>
        public static void AESDecryptFile(FileStream input, string outputPath)
        {
            FileStream frde = new FileStream(outputPath, FileMode.Create);
            CryptoStream defr = AESDecryptStream(input, Default_AES_Key);
            byte[] bytearrayoutput = new byte[1024];
            while (true)
            {
                var count = defr.Read(bytearrayoutput, 0, bytearrayoutput.Length);
                frde.Write(bytearrayoutput, 0, count);
                if (count < bytearrayoutput.Length)
                {
                    break;
                }
            }
        }






    }
}
