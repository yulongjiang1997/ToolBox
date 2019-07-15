using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CommomTools
{

    /// <summary>
    /// 得到随机安全码（哈希加密）。
    /// </summary>
    public static class HashHelper
    {


        /// <summary>
        /// 得到随机哈希加密字符串
        /// </summary>
        /// <returns></returns>
        public static string GetSecurity()
        {
            string Security = HashEncoding(GetRandomValue());
            return Security;
        }
        /// <summary>
        /// 得到一个随机数值
        /// </summary>
        /// <returns></returns>
        public static string GetRandomValue()
        {
            Random Seed = new Random();
            string RandomVaule = Seed.Next(1, int.MaxValue).ToString();
            return RandomVaule;
        }

        /// <summary>
        /// 哈希加密一个字符串
        /// </summary>
        /// <param name="security">需要加密的字符串</param>
        /// <returns>加密后的数据</returns>
        public static string HashEncoding(this string security)
        {
            var code = new UnicodeEncoding();
            byte[] message = code.GetBytes(security);
            var arithmetic = new SHA512Managed();
            var value = arithmetic.ComputeHash(message);
            var sb = new StringBuilder();
            foreach (byte o in value)
            {
                sb.Append((int)o + "O");
            }

            return sb.ToString();
        }

    }
}
