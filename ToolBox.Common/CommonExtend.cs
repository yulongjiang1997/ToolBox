using System;
using System.Collections.Generic;
using System.Text;

namespace ToolBox.Common
{
    public static class CommonExtend
    {

        /// <summary>
        /// 返回对象obj的String值,obj为null时返回空值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>字符串。</returns>
        public static string ToObjectString(this object obj)
        {
            return null == obj ? String.Empty : obj.ToString();
        }


        /// <summary>
        /// 取得Int值,如果为Null 则返回０
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int GetInt(this object obj)
        {
            if (obj != null)
            {
                int i;
                int.TryParse(obj.ToString(), out i);
                return i;
            }
            else
                return 0;
        }


        public static float GetFloat(this object obj)
        {
            float i;
            float.TryParse(obj.ToString(), out i);
            return i;
        }


        /// <summary>
        /// 取得Int值,如果不成功则返回指定exceptionvalue值
        /// </summary>
        /// <param name="obj">要计算的值</param>
        /// <param name="exceptionvalue">异常时的返回值</param>
        /// <returns></returns>
        public static int GetInt(this object obj, int exceptionvalue)
        {
            if (obj == null)
                return exceptionvalue;
            if (string.IsNullOrEmpty(obj.ToString()))
                return exceptionvalue;
            int i = exceptionvalue;
            try { i = Convert.ToInt32(obj); }
            catch { i = exceptionvalue; }
            return i;
        }


        /// <summary>
        /// 取得byte值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte Getbyte(this object obj)
        {
            if (obj != null && obj.ToString() != "")
                return byte.Parse(obj.ToString());
            else
                return 0;
        }


        /// <summary>
        /// 获得Long值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long GetLong(this object obj)
        {
            if (obj != null && obj.ToString() != "")
                return long.Parse(obj.ToString());
            else
                return 0;
        }


        /// <summary>
        /// 取得Long值,如果不成功则返回指定exceptionvalue值
        /// </summary>
        /// <param name="obj">要计算的值</param>
        /// <param name="exceptionvalue">异常时的返回值</param>
        /// <returns></returns>
        public static long GetLong(this object obj, long exceptionvalue)
        {
            if (obj == null)
            {
                return exceptionvalue;
            }
            if (string.IsNullOrEmpty(obj.ToString()))
            {
                return exceptionvalue;
            }
            long i = exceptionvalue;
            try
            {
                i = Convert.ToInt64(obj);
            }
            catch
            {
                i = exceptionvalue;
            }
            return i;
        }


        /// <summary>
        /// 取得Decimal值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static decimal GetDecimal(this object obj)
        {
            if (obj != null && obj.ToString() != "")
                return decimal.Parse(obj.ToString());
            else
                return 0;
        }


        /// <summary>
        /// 取得DateTime值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime GetDateTime(this object obj)
        {
            if (obj != null && obj.ToString() != "")
                return DateTime.Parse(obj.ToString());
            else
                return DateTime.Now;
            //return DateTime.MinValue;
        }


        /// <summary>
        /// 计算耗时 
        /// </summary>
        /// <param name="t">毫秒</param>
        /// <returns></returns>
        public static string CostTime(this long t)
        {
            long hour = t / (1000 * 60 * 24);
            long min = (t - hour * (1000 * 60 * 24)) / (1000 * 60);
            long sec = (t - hour * (1000 * 60 * 24) - min * (1000 * 60)) / 1000;
            long msec = t - hour * (1000 * 60 * 24) - min * (1000 * 60) - sec * 1000;
            string timeString = hour.ToString() + ":" + min.ToString() + ":" + sec.ToString() + "." + msec.ToString();
            string Time = GetDateTime(timeString).ToString("HH:mm:ss");
            if (Time == "00:00:00")
            {
                Time = "00:00:01";
            }
            return Time;
        }


        /// <summary>
        /// 取得DateTime值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this object obj)
        {
            if (obj != null && obj.ToString() != "")
                return DateTime.Parse(obj.ToString());
            else
                return null;
        }


        /// <summary>
        /// 格式化日期 yyyy-MM-dd HH:mm
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetFormatDateTime(this object obj, string Format)
        {
            if (obj != null && obj.ToString() != null && obj.ToString() != "")
                return DateTime.Parse(obj.ToString()).ToString(Format);
            else
                return "";
        }


        /// <summary>
        /// Json 的日期格式与.Net DateTime类型的转换
        /// </summary>
        /// <param name="jsonDate">Date(1242357713797+0800)</param>
        /// <returns></returns>
        public static DateTime JsonToDateTime(string jsonDate)
        {
            string value = jsonDate.Substring(5, jsonDate.Length - 6) + "+0800";
            DateTimeKind kind = DateTimeKind.Utc;
            int index = value.IndexOf('+', 1);
            if (index == -1)
                index = value.IndexOf('-', 1);
            if (index != -1)
            {
                kind = DateTimeKind.Local;
                value = value.Substring(0, index);
            }
            long javaScriptTicks = long.Parse(value, System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture);
            long InitialJavaScriptDateTicks = (new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).Ticks;
            DateTime utcDateTime = new DateTime((javaScriptTicks * 10000) + InitialJavaScriptDateTicks, DateTimeKind.Utc);
            DateTime dateTime;
            switch (kind)
            {
                case DateTimeKind.Unspecified:
                    dateTime = DateTime.SpecifyKind(utcDateTime.ToLocalTime(), DateTimeKind.Unspecified);
                    break;
                case DateTimeKind.Local:
                    dateTime = utcDateTime.ToLocalTime();
                    break;
                default:
                    dateTime = utcDateTime;
                    break;
            }
            return dateTime;
        }


        /// <summary>
        /// 取得bool值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool GetBool(this object obj)
        {
            if (obj != null)
            {
                bool flag;
                bool.TryParse(obj.ToString(), out flag);
                return flag;
            }
            else
                return false;
        }


        /// <summary>
        /// 取得byte[]
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Byte[] GetByte(this object obj)
        {
            if (obj.ToString() != null && obj.ToString() != "")
            {
                return (Byte[])obj;
            }
            else
                return null;
        }


        /// <summary>
        /// 取得string值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string GetString(this object obj)
        {
            if (obj != null && obj != DBNull.Value)
                return obj.ToString();
            else
                return "";
        }


        /// <summary>
        /// 判断文本obj是否为空值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Boolean值。</returns>
        public static bool IsEmpty(this string obj)
        {
            return ToObjectString(obj).Trim() == String.Empty ? true : false;
        }

        /// <summary>
        /// 判断对象是否为正确的日期值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Boolean。</returns>
        public static bool IsDateTime(this object obj)
        {
            try
            {
                DateTime dt = DateTime.Parse(ToObjectString(obj));
                if (dt > DateTime.MinValue && DateTime.MaxValue > dt)
                    return true;
                return false;
            }
            catch
            { return false; }
        }


        /// <summary>
        /// 判断对象是否为正确的Int32值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Int32值。</returns>
        public static bool IsInt(this object obj)
        {
            try
            {
                int.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }


        /// <summary>
        /// 判断对象是否为正确的Long值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Long值。</returns>
        public static bool IsLong(this object obj)
        {
            try
            {
                long.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }


        /// <summary>
        /// 判断对象是否为正确的Float值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Float值。</returns>
        public static bool IsFloat(this object obj)
        {
            try
            {
                float.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }


        /// <summary>
        /// 判断对象是否为正确的Double值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Double值。</returns>
        public static bool IsDouble(this object obj)
        {
            try
            {
                double.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }


        /// <summary>
        /// 判断对象是否为正确的Decimal值。
        /// </summary>
        /// <param name="obj">对象。</param>
        /// <returns>Decimal值。</returns>
        public static bool IsDecimal(this object obj)
        {
            try
            {
                decimal.Parse(ToObjectString(obj));
                return true;
            }
            catch
            { return false; }
        }


    }


}
