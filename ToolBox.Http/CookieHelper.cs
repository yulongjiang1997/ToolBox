using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolBox.Http
{

    /// <summary>
    /// cookite工具量
    /// </summary>
    public  class CookieHelper
    {

        /// <summary>
        /// 获取简化的cookite
        /// </summary>
        /// <param name="cookies"></param>
        /// <returns></returns>
        public static string GetSmallCookie(IEnumerable<string> cookies) {

            string[] strs;
            StringBuilder sb = new StringBuilder(128);
            HashSet<string> hash = new HashSet<string>();

            foreach (var item in cookies)
            {
                strs = item.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);

                foreach (var sub in strs)
                {
                    if (!Regex.IsMatch(sub, "expires=|max-ag=|path=|domain=|HttpOnly", RegexOptions.IgnoreCase)) {

                        hash.Add(sub.Trim());
                    }
                }

              
            }

            return string.Join(";", hash);

        }


        /// <summary>
        /// 合并更新cookite 
        /// </summary>
        /// <param name="oldCookie">oldCookie</param>
        /// <param name="newCookie">newCookie</param>
        /// <returns></returns>
        public static string UpdateMergeCookie(string oldCookie, string newCookie) {

            if (string.IsNullOrWhiteSpace(oldCookie)) return newCookie;
            if (string.IsNullOrWhiteSpace(newCookie)) return oldCookie;

            List<string> nCookies = newCookie.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            string[] oCookies = newCookie.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
            HashSet<string> hash = new HashSet<string>();

            string[] temps;
            foreach (var item in oCookies)
            {
                temps = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (temps.Length < 2) continue;
                hash.Add(temps[0]);
            }
            foreach (var item in oCookies)
            {
                temps = item.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                if (temps.Length < 2) continue;

                if (hash.Add(temps[0])) nCookies.Add(item);
            }
            return string.Join(";", nCookies);



        }


        /// <summary>
        /// 查询是否有cookname值
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="cook_name"></param>
        /// <returns></returns>
        public static bool HasCookie(string cookie, string cook_name)
        {
            if (string.IsNullOrWhiteSpace(cookie)) return false;
            string[] list_New = cookie.ToString().Split(';');

            foreach (string item in list_New)
            {
                //排除重复项
                string[] tmp = item.Trim().Split('=');

                if (tmp[0] == cook_name)
                {
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 删除指定cookite
        /// </summary>
        /// <param name="cookie"></param>
        /// <param name="cook_name"></param>
        /// <returns></returns>
        public static string CookieDelete(string cookie, string cook_name)
        {
            if (string.IsNullOrWhiteSpace(cookie)) return "";

            string[] list_New = cookie.ToString().Split(';');

            string new_cookie = cookie;

            foreach (string item in list_New)
            {
                //排除重复项
                string[] tmp = item.Trim().Split('=');
                if (tmp.Length == 2)
                {
                    if (tmp[0].Equals(cook_name))
                    {
                        new_cookie = new_cookie.Replace(item + ';', "");
                    }

                }


            }

            return new_cookie;

        }


        /// <summary>
        /// 获取指定cookei值
        /// </summary>
        /// <param name="cook_name"></param>
        /// <returns></returns>
        public static string GetCookValue(string cookie, string cook_name)
        {
            if (string.IsNullOrWhiteSpace(cookie)) return "";

            string[] list_New = cookie.ToString().Split(';');

            string new_cookie = cookie;

            foreach (string item in list_New)
            {
                //排除重复项
                string[] tmp = item.Trim().Split('=');
                if (tmp.Length == 2)
                {
                    if (tmp[0].Equals(cook_name))
                    {
                        return tmp[1];
                    }

                }


            }

            return "";

        }


    }
}
