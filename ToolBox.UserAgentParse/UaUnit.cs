using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using ToolBox.PhoneModelParse;

namespace ToolBox.UserAgentParse
{
    public class UaUnit
    {
        private string UaStr { get; set; }

        public UaUnit(string ua)
        {
            UaStr = ua;
        }

        public UaInfo Parse()
        {
            if (string.IsNullOrEmpty(UaStr)) return null;
            var result = new UaInfo();
            var systemInfo = GetSystemInfo();
            result.SystemName = systemInfo.Item1;
            result.SystemVersion = systemInfo.Item2;
            result.Platform = GetPlatform();
            result.BrowserKernel = GetBrowserKernel();
            if (result.Platform == "Mobile")
            {
                var phoneModelInfo = GetPhoneModel();
                result.PhoneModelCode = phoneModelInfo.Item1;
                result.PhoneModelName = phoneModelInfo.Item2;
            }
            var browserInfo = GetBrowserName();
            result.BrowserName = browserInfo.Item1;
            result.BrowserVersion = browserInfo.Item2;
            return result;
        }

        /// <summary>
        /// 获得手机型号
        /// </summary>
        /// <returns></returns>
        private (string, string) GetPhoneModel()
        {
            var str1 = string.Empty;
            var str2 = string.Empty;
            if (UaStr.Contains("Android"))
            {
                var jiexi1 = Regex.Split(UaStr, "Build", RegexOptions.IgnoreCase);
                var jiexi2 = jiexi1[0].Split(';');
                str1 = jiexi2[jiexi2.Length - 1].Trim();
            }
            if (UaStr.Contains("iPhone"))
            {
                str1 = "iPhone";
            }
            return (str1, new PhoneModelNumberTool().ModelNumberToName(str1));
        }

        /// <summary>
        /// 获得浏览器名称
        /// </summary>
        /// <returns></returns>
        private (string, string) GetBrowserName()
        {
            if (UaStr.Contains("LBBROWSER")) return ("猎豹浏览器", "");
            if (UaStr.Contains("Chromium")) return ("Chromium", GetVersion("Chromium"));
            if (UaStr.Contains("Opera")) return ("欧朋浏览器", GetVersion("Opera"));
            if (UaStr.Contains("OPR")) return ("欧朋浏览器", GetVersion("OPR"));
            if (UaStr.Contains("Firefox")) return ("火狐浏览器", GetVersion("Firefox"));
            if (UaStr.Contains("SeaMonkey")) return ("SeaMonkey", GetVersion("SeaMonkey"));
            if (UaStr.Contains("MicroMessenger")) return ("微信浏览器", GetVersion("MicroMessenger"));
            if (UaStr.Contains("QQBrowser")) return ("QQ浏览器", "QQBrowser");
            if (UaStr.Contains("MiuiBrowser")) return ("小米浏览器", "MiuiBrowser");
            if (UaStr.Contains("baiduboxapp")) return ("手机百度APP浏览器", "baiduboxapp");
            if (UaStr.Contains("360SE")) return ("360浏览器", GetVersion("360SE"));
            if (UaStr.Contains("360EE")) return ("360浏览器", GetVersion("360EE"));
            if (UaStr.Contains("weibo")) return ("新浪微博浏览器", GetVersion("__weibo__"));
            if (UaStr.Contains("NokiaBrowser")) return ("诺基亚浏览器", GetVersion("NokiaBrowser"));
            if (UaStr.Contains("Edge")) return ("Edge浏览器", GetVersion("Edge"));
            if (UaStr.Contains("MSIE")) return ("IE浏览器", GetVersion("MSIE"));
            if (UaStr.Contains("Trident") || UaStr.Contains("WOW64")) return ("IE浏览器", "11");
            if (UaStr.Contains("Chrome")) return ("谷歌浏览器", GetVersion("Chrome"));
            if (UaStr.Contains("CrMo")) return ("谷歌浏览器", GetVersion("CrMo"));
            if (UaStr.Contains("curl")) return ("curl", GetVersion("curl"));
            if (UaStr.Contains("Safari")) return ("Safari浏览器", GetVersion("Safari"));
            return ("", "");
        }

        /// <summary>
        /// 获得系统名称
        /// </summary>
        /// <returns></returns>
        private (string, string) GetSystemInfo()
        {
            if (UaStr.Contains("Android")) return ("Android OS", GetVersion("Android"));
            if (UaStr.Contains("iPad")) return ("iPad OS", GetVersion("OS"));
            if (UaStr.Contains("iPhone")) return ("iPhone OS", GetVersion("OS"));
            if (UaStr.Contains("BB") && UaStr.Contains("Mobile")) return ("Black Berry", GetVersion("BB"));
            if (UaStr.Contains("RIM Tablet")) return ("Black Berry", GetVersion("OS"));
            if (UaStr.Contains("BlackBerry")) return ("Black Berry", GetVersion("BlackBerry"));
            if (UaStr.Contains("Windows Phone")) return ("Windows Phone", GetVersion("Windows Phone"));
            if (UaStr.Contains("Mac OS X")) return ("Mac OS X", GetVersion("Mac OS X"));
            if (UaStr.Contains("MacOS")) return ("Mac OS", "");
            if (UaStr.Contains("Windows NT 5.1") && !UaStr.Contains("Windows NT 5.2")) return ("Windows", "XP");
            if (UaStr.Contains("Windows NT 6.0")) return ("Windows", "Vista");
            if (UaStr.Contains("Windows NT 6.1")) return ("Windows", "7");
            if (UaStr.Contains("Windows NT 6.2")) return ("Windows", "8");
            if (UaStr.Contains("Windows NT 6.3")) return ("Windows", "8.1");
            if (UaStr.Contains("Windows NT 10")) return ("Windows", "10");
            if (UaStr.Contains("CentOS")) return ("Linux CentOS", GetVersion("CentOS"));
            if (UaStr.Contains("Red Hat")) return ("Linux Red Hat", GetVersion("Red Hat"));
            if (UaStr.Contains("SUSE")) return ("Linux SUSE", GetVersion("SUSE"));
            if (UaStr.Contains("Debian")) return ("Linux Debian", GetVersion("Debian"));
            if (UaStr.Contains("Arch Linux")) return ("Arch Linux", GetVersion("Arch Linux"));
            if (UaStr.Contains("Fedora")) return ("Linux Fedora", GetVersion("Fedora"));
            if (UaStr.Contains("Ubuntu")) return ("Linux Ubuntu", GetVersion("Ubuntu"));
            if (UaStr.Contains("Linux")) return ("Linux", GetVersion("Linux"));
            if (UaStr.Contains("curl")) return ("curl", GetVersion("curl"));
            return ("Unknow", "Unknow");
        }

        /// <summary>
        /// 获得版本号
        /// </summary>
        /// <returns></returns>
        private string GetVersion(string value)
        {
            string text = UaStr.Substring(UaStr.IndexOf(value) + value.Length).TrimStart('-').TrimStart()
                .TrimStart('/')
                .TrimStart();
            string text2 = string.Empty;
            string text3 = text;
            for (int i = 0; i < text3.Length; i++)
            {
                char c = text3[i];
                bool flag = false;
                int result = 0;
                if (int.TryParse(c.ToString(), out result))
                {
                    text2 += c.ToString();
                    flag = true;
                }
                if (c == '.' || c == '_')
                {
                    text2 += ".";
                    flag = true;
                }
                if (!flag)
                {
                    break;
                }
            }
            return text2;
        }

        /// <summary>
        /// 获得浏览器内核
        /// </summary>
        /// <returns></returns>
        private string GetBrowserKernel()
        {
            if (UaStr.Contains("Trident")) return "Trident";
            if (UaStr.Contains("Presto")) return "Presto";
            if (UaStr.Contains("WebKit")) return "Webkit";
            if (UaStr.Contains("Blink")) return "Blink"; 
            if (UaStr.Contains("Gecko")) return "Gecko";
            return "";
        }

        /// <summary>
        /// 获得平台
        /// </summary>
        /// <returns></returns>
        private string GetPlatform()
        {
            if (UaStr.Contains("Android") || UaStr.Contains("iPad") || UaStr.Contains("iPhone") || UaStr.Contains("Kindle Fire") || UaStr.Contains("Black Berry") || UaStr.Contains("Windows Phone"))
            {
                return "Mobile";
            }
            if (UaStr.Contains("Mac") || UaStr.Contains("Windows") || UaStr.Contains("Linux"))
            {
                return "Desktop";
            }
            return "Unknown";
        }

    }
    public class UaInfo
    {
        /// <summary>
        /// 系统名称
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 系统版本号
        /// </summary>
        public string SystemVersion { get; set; }

        /// <summary>
        /// 浏览器名称
        /// </summary>
        public string BrowserName { get; set; }

        /// <summary>
        /// 浏览器版本号
        /// </summary>
        public string BrowserVersion { get; set; }

        /// <summary>
        /// 浏览器内核
        /// </summary>
        public string BrowserKernel { get; set; }

        /// <summary>
        /// 手机型号代码
        /// </summary>
        public string PhoneModelCode { get; set; }

        /// <summary>
        /// 手机型号名称
        /// </summary>
        public string PhoneModelName { get; set; }

        /// <summary>
        /// 平台
        /// </summary>
        public string Platform { get; set; }

    }

}
