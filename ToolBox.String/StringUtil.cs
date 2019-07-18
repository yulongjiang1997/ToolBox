using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ToolBox.String
{
    /// <summary>
    /// 字符串工具类。
    /// </summary>
    public partial class StringUtil
    {

        private StringUtil() {
        }


        private static Regex RegexBr = new Regex(@"(\r\n)", RegexOptions.IgnoreCase);

        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <returns></returns>
        public static string ToTitleCase(string str)
        {
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str);
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns>返回int</returns>
        public static int GetStringLength(string str)
        {
            return Encoding.Default.GetBytes(str).Length;
        }

        /// <summary>
        /// 判断指定字符串在指定字符串数组中的位置
        /// </summary>
        /// <param name="strSearch">字符串</param>
        /// <param name="stringArray">字符串数组</param>
        /// <param name="caseInsensetive">是否不区分大小写, true为不区分, false为区分</param>
        /// <returns>字符串在指定字符串数组中的位置, 如不存在则返回-1</returns>
        public static int GetInArrayID(string strSearch, string[] stringArray, bool caseInsensetive)
        {
            for (int i = 0; i < stringArray.Length; i++)
            {
                if (caseInsensetive)
                {
                    if (strSearch.ToLower() == stringArray[i].ToLower())
                    {
                        return i;
                    }
                }
                else
                {
                    if (strSearch == stringArray[i])
                    {
                        return i;
                    }
                }

            }
            return -1;
        }

        /// <summary>
        /// 删除字符串尾部的回车/换行/空格
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }

        /// <summary>
        /// 清除给定字符串中的回车及换行符
        /// </summary>
        /// <param name="str">要清除的字符串</param>
        /// <returns>清除后返回的字符串</returns>
        public static string ClearBR(string str)
        {
            Match m = null;
            for (m = RegexBr.Match(str); m.Success; m = m.NextMatch())
            {
                str = str.Replace(m.Groups[0].ToString(), "");
            }
            return str;
        }

        /// <summary>
        /// 字符串如果操过指定长度则将超出的部分用指定字符串代替
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_Length, string p_TailString)
        {
            return GetSubString(p_SrcString, 0, p_Length, p_TailString);
        }

        /// <summary>
        /// 取指定长度的字符串
        /// </summary>
        /// <param name="p_SrcString">要检查的字符串</param>
        /// <param name="p_StartIndex">起始位置</param>
        /// <param name="p_Length">指定长度</param>
        /// <param name="p_TailString">用于替换的字符串</param>
        /// <returns>截取后的字符串</returns>
        public static string GetSubString(string p_SrcString, int p_StartIndex, int p_Length, string p_TailString)
        {
            string myResult = p_SrcString;

            //当是日文或韩文时(注:中文的范围:\u4e00 - \u9fa5, 日文在\u0800 - \u4e00, 韩文为\xAC00-\xD7A3)
            if (System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\u0800-\u4e00]+") ||
                System.Text.RegularExpressions.Regex.IsMatch(p_SrcString, "[\xAC00-\xD7A3]+"))
            {
                //当截取的起始位置超出字段串长度时
                if (p_StartIndex >= p_SrcString.Length)
                {
                    return "";
                }
                else
                {
                    return p_SrcString.Substring(p_StartIndex,
                                                   ((p_Length + p_StartIndex) > p_SrcString.Length) ? (p_SrcString.Length - p_StartIndex) : p_Length);
                }
            }


            if (p_Length >= 0)
            {
                byte[] bsSrcString = Encoding.Default.GetBytes(p_SrcString);

                //当字符串长度大于起始位置
                if (bsSrcString.Length > p_StartIndex)
                {
                    int p_EndIndex = bsSrcString.Length;

                    //当要截取的长度在字符串的有效长度范围内
                    if (bsSrcString.Length > (p_StartIndex + p_Length))
                    {
                        p_EndIndex = p_Length + p_StartIndex;
                    }
                    else
                    {   //当不在有效范围内时,只取到字符串的结尾

                        p_Length = bsSrcString.Length - p_StartIndex;
                        p_TailString = "";
                    }



                    int nRealLength = p_Length;
                    int[] anResultFlag = new int[p_Length];
                    byte[] bsResult = null;

                    int nFlag = 0;
                    for (int i = p_StartIndex; i < p_EndIndex; i++)
                    {

                        if (bsSrcString[i] > 127)
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

                    if ((bsSrcString[p_EndIndex - 1] > 127) && (anResultFlag[p_Length - 1] == 1))
                    {
                        nRealLength = p_Length + 1;
                    }

                    bsResult = new byte[nRealLength];

                    Array.Copy(bsSrcString, p_StartIndex, bsResult, 0, nRealLength);

                    myResult = Encoding.Default.GetString(bsResult);

                    myResult = myResult + p_TailString;
                }
            }

            return myResult;
        }

        /// <summary>
        /// 自定义的替换字符串函数
        /// </summary>
        /// <param name="SourceString">源字符串</param>
        /// <param name="SearchString">待替换字符串</param>
        /// <param name="ReplaceString">目标替换字符串</param>
        /// <param name="IsCaseInsensetive">是否不区分大小写，true=指定不区分大小写的匹配</param>
        /// <returns>返回string</returns>
        public static string ReplaceString(string SourceString, string SearchString, string ReplaceString, bool IsCaseInsensetive)
        {
            return Regex.Replace(SourceString, Regex.Escape(SearchString), ReplaceString, IsCaseInsensetive ? RegexOptions.IgnoreCase : RegexOptions.None);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        public static string[] SplitString(string strContent, string strSplit)
        {
            if (strContent.IndexOf(strSplit) < 0)
            {
                string[] tmp = { strContent };
                return tmp;
            }
            return Regex.Split(strContent, Regex.Escape(strSplit), RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="strContent">待分割字符串</param>
        /// <param name="strSplit">分隔符</param>
        /// <param name="p_3">指定分割数组大小</param>
        /// <returns>string[]</returns>
        public static string[] SplitString(string strContent, string strSplit, int p_3)
        {
            string[] result = new string[p_3];

            string[] splited = SplitString(strContent, strSplit);

            for (int i = 0; i < p_3; i++)
            {
                if (i < splited.Length)
                    result[i] = splited[i];
                else
                    result[i] = string.Empty;
            }

            return result;
        }


        /// <summary>
        /// 把字符串按照分隔符转换成 List
        /// </summary>
        /// <param name="str">源字符串</param>
        /// <param name="speater">分隔符</param>
        /// <param name="toLower">是否转换为小写</param>
        /// <returns></returns>
        public static List<string> GetStrArray(string str, char speater, bool toLower)
        {
            List<string> list = new List<string>();
            string[] ss = str.Split(speater);
            foreach (string s in ss)
            {
                if (!string.IsNullOrEmpty(s) && s != speater.ToString())
                {
                    string strVal = s;
                    if (toLower)
                    {
                        strVal = s.ToLower();
                    }
                    list.Add(strVal);
                }
            }
            return list;
        }


        /// <summary>
        /// 把 List<string> 按照分隔符组装成 string
        /// </summary>
        /// <param name="list"></param>
        /// <param name="speater"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<string> list, string speater)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i]);
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(speater);
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayStr(List<int> list)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < list.Count; i++)
            {
                if (i == list.Count - 1)
                {
                    sb.Append(list[i].ToString());
                }
                else
                {
                    sb.Append(list[i]);
                    sb.Append(",");
                }
            }
            return sb.ToString();
        }


        /// <summary>
        /// 得到数组列表以逗号分隔的字符串
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string GetArrayValueStr(Dictionary<int, int> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (KeyValuePair<int, int> kvp in list)
            {
                sb.Append(kvp.Value + ",");
            }
            if (list.Count > 0)
            {
                return DelLastComma(sb.ToString());
            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 删除最后结尾的一个逗号
        /// </summary>
        public static string DelLastComma(string str)
        {
            return str.Substring(0, str.LastIndexOf(","));
        }


        /// <summary>
        /// 删除最后结尾的指定字符后的字符
        /// </summary>
        public static string DelLastChar(string str, string strchar)
        {
            return str.Substring(0, str.LastIndexOf(strchar));
        }


        /// <summary>
        /// 删除最后结尾的长度
        /// </summary>
        /// <param name="str"></param>
        /// <param name="Length"></param>
        /// <returns></returns>
        public static string DelLastLength(string str, int Length)
        {
            if (string.IsNullOrEmpty(str))
                return "";
            str = str.Substring(0, str.Length - Length);
            return str;
        }



        /// <summary>
        /// 转全角的函数(SBC case)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string ToSBC(string input)
        {
            //半角转全角：
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                    c[i] = (char)(c[i] + 65248);
            }
            return new string(c);
        }


        /// <summary>
        ///  转半角的函数(SBC case)
        /// </summary>
        /// <param name="input">输入</param>
        /// <returns></returns>
        public static string ToDBC(string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                    c[i] = (char)(c[i] - 65248);
            }
            return new string(c);
        }


        /// <summary>
        /// 获取英文字母对应的小键盘码（数字）
        /// </summary>
        /// <param name="spStr">英文字母串</param>
        /// <returns></returns>
        public static string GetKeyPad(string spStr)
        {
            string[] strArray = { "viaoeu", "ym", "dt", "kr", "zcs", "hwf", "ln", "qx", "pb", "gj" };
            if (spStr == null)
            {
                return "";
            }
            string spStrs = null;
            for (int i = 0; i < spStr.Length; i++)
            {
                bool sp = false;
                for (int t = 0; t < strArray.Length; t++)
                {
                    if (strArray[t].Contains(spStr[i].ToString()))
                    {
                        spStrs += t;
                        sp = true;
                    }
                }
                if (!sp)
                {
                    spStrs += spStr[i];
                }
            }
            return spStrs;
        }


        /// <summary>
        ///  将字符串样式转换为纯字符串
        /// </summary>
        /// <param name="StrList"></param>
        /// <param name="SplitString"></param>
        /// <returns></returns>
        public static string GetCleanStyle(string StrList, string SplitString)
        {
            string RetrunValue = "";
            //如果为空，返回空值
            if (StrList == null)
            {
                RetrunValue = "";
            }
            else
            {
                //返回去掉分隔符
                string NewString = "";
                NewString = StrList.Replace(SplitString, "");
                RetrunValue = NewString;
            }
            return RetrunValue;
        }



        /// <summary>
        /// 取中间文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="leftStr"></param>
        /// <param name="rightStr"></param>
        /// <param name="ignoreCase">忽略大小写 则为true 默认区分大小写</param>
        /// <returns></returns>
        public static string Between( string str, string leftStr, string rightStr, bool ignoreCase = false)
        {
            StringComparison comparison = StringComparison.CurrentCulture;

            if (ignoreCase)
            {
                comparison = StringComparison.OrdinalIgnoreCase;
            }


            int index = str.IndexOf(leftStr, comparison);
            if (index == -1) return "";


            int i = index + leftStr.Length;

            int last = str.IndexOf(rightStr, i, comparison);

            if (last == -1) return "";

            return str.Substring(i, last - i);

        }

        /// <summary>
        /// 取左边文本
        /// </summary>
        /// <param name="str"></param>
        /// <param name="left"></param>
        /// <param name="ignoreCase">忽略大小写 则为true 默认区分大小写</param>
        /// <returns></returns>
        public static string GetLeft( string str, string left, bool ignoreCase = false)
        {
            StringComparison comparison = StringComparison.CurrentCulture;

            if (ignoreCase)
            {
                comparison = StringComparison.OrdinalIgnoreCase;
            }

            int index = str.IndexOf(left, comparison);
            if (index == -1) return "";

            return str.Substring(0, index);
        }

        /// <summary>
        /// 取文本右边
        /// </summary>
        /// <param name="str"></param>
        /// <param name="right"></param>
        /// <param name="ignoreCase">忽略大小写 则为true 默认区分大小写</param>
        /// <returns></returns>
        public static string GetRight( string str, string right, bool ignoreCase = false)
        {
            StringComparison comparison = StringComparison.CurrentCulture;

            if (ignoreCase)
            {
                comparison = StringComparison.OrdinalIgnoreCase;
            }

            int index = str.LastIndexOf(right, comparison);
            if (index == -1) return "";

            int index_start = index + right.Length;

            int end_len = str.Length - index_start;
            string temp = str.Substring(index_start, end_len);
            return temp;


        }

        /// <summary>
        /// 匹配文本 
        /// </summary>
        /// <param name="regStr">正则</param>
        /// <param name="text">源方本</param>
        /// <param name="lable">返回的标签</param>
        /// <returns></returns>
        public static string RegMatch(string regStr, string text, string lable)
        {
            Regex r = new Regex(regStr);
            Match m = r.Match(text);
            if (!m.Success) return "";
            return m.Groups[lable].ToString();
        }


        /// <summary>
        /// HTML转行成TEXT
        /// </summary>
        /// <param name="strHtml"></param>
        /// <returns></returns>
        public static string HtmlToTxt(string strHtml)
        {
            string[] aryReg ={
            @"<script[^>]*?>.*?</script>",
            @"<(\/\s*)?!?((\w+:)?\w+)(\w+(\s*=?\s*(([""'])(\\[""'tbnr]|[^\7])*?\7|\w+)|.{0})|\s)*?(\/\s*)?>",
            @"([\r\n])[\s]+",
            @"&(quot|#34);",
            @"&(amp|#38);",
            @"&(lt|#60);",
            @"&(gt|#62);",
            @"&(nbsp|#160);",
            @"&(iexcl|#161);",
            @"&(cent|#162);",
            @"&(pound|#163);",
            @"&(copy|#169);",
            @"&#(\d+);",
            @"-->",
            @"<!--.*\n"
            };

            string newReg = aryReg[0];
            string strOutput = strHtml;
            for (int i = 0; i < aryReg.Length; i++)
            {
                Regex regex = new Regex(aryReg[i], RegexOptions.IgnoreCase);
                strOutput = regex.Replace(strOutput, string.Empty);
            }

            strOutput.Replace("<", "");
            strOutput.Replace(">", "");
            strOutput.Replace("\r\n", "");


            return strOutput;
        }


        #region 随机
        /// <summary>
        /// 返回两个数字之间的随机数
        /// </summary>
        /// <param name="min">最小数</param>
        /// <param name="max">最大数</param>
        public static int GetRandNumber(int min, int max)
        {
            Random random = new Random(GetRandomGuid());

            if (min > max)//交换变量 防止用户写反了
            {
                int temp = min;
                min = max;
                max = temp;
            }
            max++;//max 不会大于或等于 这个值 我们这里 加一下

            return random.Next(min, max);


        }

        /// <summary>
        /// 返回0.8991527960220353 16-18位随机小数
        /// </summary>
        /// <returns></returns>
        public static string GetRandJs()
        {
            Random rand = new Random(GetRandomGuid());
            return rand.NextDouble().ToString();
        }


        /// <summary>
        /// 随机生成英文字母 首字母不是数字
        /// </summary>
        ///<param name="count">长度</param>
        ///<param name="char_type">0=小写 1=大写 2=大小写混合</param>
        /// <returns></returns>
        public static string GetRandstr(int count, int char_type = 0)
        {
            char[] constant =
             {
                '0','1','2','3','4','5','6','7','8','9',
                'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'
              };

            StringBuilder sb = new StringBuilder(62);
            Random rd = new Random(GetRandomGuid());
            for (int i = 0; i < count; i++)
            {
                char ch = ' ';

                if (i == 0)
                {
                    ch = constant[rd.Next(9, constant.Length)];
                }
                else
                {
                    ch = constant[rd.Next(constant.Length)];

                }

                if (char_type == 2)
                {
                    int num = GetRandNumber(1, 2);

                    if (num == 2)
                    {
                        sb.Append(ch.ToString().ToUpper());
                    }
                    else
                    {
                        sb.Append(ch);
                    }



                }
                else
                {
                    sb.Append(ch);
                }



            }

            if (char_type == 1)
            {
                return sb.ToString().ToUpper();
            }


            return sb.ToString();
        }


        /// <summary>
        /// 生成随机中文名字
        /// </summary>
        /// <returns></returns>
        public static string GetChineseName()
        {
            /// <summary>
            /// 生成中文名字的姓
            /// </summary>
            string[] _crabofirstName = new string[] {  "白", "毕", "卞", "蔡", "曹", "岑", "常", "车", "陈", "成" , "程", "池", "邓", "丁", "范", "方", "樊", "费", "冯", "符"
        , "傅", "甘", "高", "葛", "龚", "古", "关", "郭", "韩", "何" , "贺", "洪", "侯", "胡", "华", "黄", "霍", "姬", "简", "江"
        , "姜", "蒋", "金", "康", "柯", "孔", "赖", "郎", "乐", "雷" , "黎", "李", "连", "廉", "梁", "廖", "林", "凌", "刘", "柳"
        , "龙", "卢", "鲁", "陆", "路", "吕", "罗", "骆", "马", "梅" , "孟", "莫", "母", "穆", "倪", "宁", "欧", "区", "潘", "彭"
        , "蒲", "皮", "齐", "戚", "钱", "强", "秦", "丘", "邱", "饶" , "任", "沈", "盛", "施", "石", "时", "史", "司徒", "苏", "孙"
        , "谭", "汤", "唐", "陶", "田", "童", "涂", "王", "危", "韦" , "卫", "魏", "温", "文", "翁", "巫", "邬", "吴", "伍", "武"
        , "席", "夏", "萧", "谢", "辛", "邢", "徐", "许", "薛", "严" , "颜", "杨", "叶", "易", "殷", "尤", "于", "余", "俞", "虞"
        , "元", "袁", "岳", "云", "曾", "詹", "张", "章", "赵", "郑" , "钟", "周", "邹", "朱", "褚", "庄", "卓"
                                        };

            /// <summary>
            /// 用于生成中文名字的后面两位
            /// </summary>
            string _lastName = "匕刁丐歹戈夭仑讥冗邓艾夯凸卢叭叽皿凹囚矢乍尔冯玄邦迂邢芋芍吏夷吁吕吆" +

                               "屹廷迄臼仲伦伊肋旭匈凫妆亥汛讳讶讹讼诀弛阱驮驯纫玖玛韧抠扼汞扳抡坎坞抑拟抒芙芜苇芥芯芭杖杉巫" +

                               "杈甫匣轩卤肖吱吠呕呐吟呛吻吭邑囤吮岖牡佑佃伺囱肛肘甸狈鸠彤灸刨庇吝庐闰兑灼沐沛汰沥沦汹沧沪忱" +

                               "诅诈罕屁坠妓姊妒纬玫卦坷坯拓坪坤拄拧拂拙拇拗茉昔苛苫苟苞茁苔枉枢枚枫杭郁矾奈奄殴歧卓昙哎咕呵" +

                               "咙呻咒咆咖帕账贬贮氛秉岳侠侥侣侈卑刽刹肴觅忿瓮肮肪狞庞疟疙疚卒氓炬沽沮泣泞泌沼怔怯宠宛衩祈诡" +

                               "帚屉弧弥陋陌函姆虱叁绅驹绊绎契贰玷玲珊拭拷拱挟垢垛拯荆茸茬荚茵茴荞荠荤荧荔栈柑栅柠枷勃柬砂泵" +

                               "砚鸥轴韭虐昧盹咧昵昭盅勋哆咪哟幽钙钝钠钦钧钮毡氢秕俏俄俐侯徊衍胚胧胎狰饵峦奕咨飒闺闽籽娄烁炫" +

                               "洼柒涎洛恃恍恬恤宦诫诬祠诲屏屎逊陨姚娜蚤骇耘耙秦匿埂捂捍袁捌挫挚捣捅埃耿聂荸莽莱莉莹莺梆栖桦" +

                               "栓桅桩贾酌砸砰砾殉逞哮唠哺剔蚌蚜畔蚣蚪蚓哩圃鸯唁哼唆峭唧峻赂赃钾铆氨秫笆俺赁倔殷耸舀豺豹颁胯" +

                               "胰脐脓逛卿鸵鸳馁凌凄衷郭斋疹紊瓷羔烙浦涡涣涤涧涕涩悍悯窍诺诽袒谆祟恕娩骏琐麸琉琅措捺捶赦埠捻" +

                               "掐掂掖掷掸掺勘聊娶菱菲萎菩萤乾萧萨菇彬梗梧梭曹酝酗厢硅硕奢盔匾颅彪眶晤曼晦冕啡畦趾啃蛆蚯蛉蛀" +

                               "唬啰唾啤啥啸崎逻崔崩婴赊铐铛铝铡铣铭矫秸秽笙笤偎傀躯兜衅徘徙舶舷舵敛翎脯逸凰猖祭烹庶庵痊阎阐" +

                               "眷焊焕鸿涯淑淌淮淆渊淫淳淤淀涮涵惦悴惋寂窒谍谐裆袱祷谒谓谚尉堕隅婉颇绰绷综绽缀巢琳琢琼揍堰揩" +

                               "揽揖彭揣搀搓壹搔葫募蒋蒂韩棱椰焚椎棺榔椭粟棘酣酥硝硫颊雳翘凿棠晰鼎喳遏晾畴跋跛蛔蜒蛤鹃喻啼喧" +

                               "嵌赋赎赐锉锌甥掰氮氯黍筏牍粤逾腌腋腕猩猬惫敦痘痢痪竣翔奠遂焙滞湘渤渺溃溅湃愕惶寓窖窘雇谤犀隘" +

                               "媒媚婿缅缆缔缕骚瑟鹉瑰搪聘斟靴靶蓖蒿蒲蓉楔椿楷榄楞楣酪碘硼碉辐辑频睹睦瞄嗜嗦暇畸跷跺蜈蜗蜕蛹" +

                               "嗅嗡嗤署蜀幌锚锥锨锭锰稚颓筷魁衙腻腮腺鹏肄猿颖煞雏馍馏禀痹廓痴靖誊漓溢溯溶滓溺寞窥窟寝褂裸谬" +

                               "媳嫉缚缤剿赘熬赫蔫摹蔓蔗蔼熙蔚兢榛榕酵碟碴碱碳辕辖雌墅嘁踊蝉嘀幔镀舔熏箍箕箫舆僧孵瘩瘟彰粹漱" +

                               "漩漾慷寡寥谭褐褪隧嫡缨撵撩撮撬擒墩撰鞍蕊蕴樊樟橄敷豌醇磕磅碾憋嘶嘲嘹蝠蝎蝌蝗蝙嘿幢镊镐稽篓膘" +

                               "鲤鲫褒瘪瘤瘫凛澎潭潦澳潘澈澜澄憔懊憎翩褥谴鹤憨履嬉豫缭撼擂擅蕾薛薇擎翰噩橱橙瓢蟥霍霎辙冀踱蹂" +

                               "蟆螃螟噪鹦黔穆篡篷篙篱儒膳鲸瘾瘸糙燎濒憾懈窿缰壕藐檬檐檩檀礁磷瞭瞬瞳瞪曙蹋蟋蟀嚎赡镣魏簇儡徽" +

                               "爵朦臊鳄糜癌懦豁臀藕藤瞻嚣鳍癞瀑襟璧戳攒孽蘑藻鳖蹭蹬簸簿蟹靡癣羹鬓攘蠕巍鳞糯譬霹躏髓蘸镶瓤矗";


            Random rnd = new Random(GetRandomGuid());
            return string.Format("{0}{1}{2}", _crabofirstName[rnd.Next(_crabofirstName.Length - 1)], _lastName.Substring(rnd.Next(0, _lastName.Length - 1), 1), _lastName.Substring(rnd.Next(0, _lastName.Length - 1), 1));
        }


        /// <summary>
        /// 随机生成英文名字
        /// </summary>
        /// <returns></returns>
        public static string GenerateSurname()
        {
            string name = string.Empty;
            string[] currentConsonant;
            string[] vowels = "a,a,a,a,a,e,e,e,e,e,e,e,e,e,e,e,i,i,i,o,o,o,u,y,ee,ee,ea,ea,ey,eau,eigh,oa,oo,ou,ough,ay".Split(',');
            string[] commonConsonants = "s,s,s,s,t,t,t,t,t,n,n,r,l,d,sm,sl,sh,sh,th,th,th".Split(',');
            string[] averageConsonants = "sh,sh,st,st,b,c,f,g,h,k,l,m,p,p,ph,wh".Split(',');
            string[] middleConsonants = "x,ss,ss,ch,ch,ck,ck,dd,kn,rt,gh,mm,nd,nd,nn,pp,ps,tt,ff,rr,rk,mp,ll".Split(',');
            string[] rareConsonants = "j,j,j,v,v,w,w,w,z,qu,qu".Split(',');
            Random rng = new Random(GetRandomGuid());
            int[] lengthArray = new int[] { 2, 2, 2, 2, 2, 2, 3, 3, 3, 4 };
            int length = lengthArray[rng.Next(lengthArray.Length)];
            for (int i = 0; i < length; i++)
            {
                int letterType = rng.Next(1000);
                if (letterType < 775) currentConsonant = commonConsonants;
                else if (letterType < 875 && i > 0) currentConsonant = middleConsonants;
                else if (letterType < 985) currentConsonant = averageConsonants;
                else currentConsonant = rareConsonants;
                name += currentConsonant[rng.Next(currentConsonant.Length)];
                name += vowels[rng.Next(vowels.Length)];
                if (name.Length > 4 && rng.Next(1000) < 800) break;
                if (name.Length > 6 && rng.Next(1000) < 950) break;
                if (name.Length > 7) break;
            }
            int endingType = rng.Next(1000);
            if (name.Length > 6)
                endingType -= (name.Length * 25);
            else
                endingType += (name.Length * 10);
            if (endingType < 400) { }
            else if (endingType < 775) name += commonConsonants[rng.Next(commonConsonants.Length)];
            else if (endingType < 825) name += averageConsonants[rng.Next(averageConsonants.Length)];
            else if (endingType < 840) name += "ski";
            else if (endingType < 860) name += "son";
            else if (Regex.IsMatch(name, "(.+)(ay|e|ee|ea|oo)$") || name.Length < 5)
            {
                name = "Mc" + name.Substring(0, 1).ToUpper() + name.Substring(1);
                return name;
            }
            else name += "ez";
            name = name.Substring(0, 1).ToUpper() + name.Substring(1);
            return name;
        }

        /// <summary>
        /// Guid 获取随机数种子
        /// </summary>
        /// <returns></returns>
        public static int GetRandomGuid()
        {
            return Guid.NewGuid().GetHashCode();
        }
        #endregion 随机 结束

        #region 转义
        private static string Escape(string arg)
        {
            return arg.Replace(":", "\\:").Replace(";", "\\;");
        }

        private static string UnEscape(string arg)
        {
            return arg.Replace("\\:", ":").Replace("\\;", ";");
        }
        #endregion

    }
}
