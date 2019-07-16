using System;

namespace ToolBox.DateTimeTool
{
    public static class DateTimeExtend
    {


      

        /// <summary>
        /// 获取本日开始时间（0点0分0秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetDayStart(this DateTime dateTime)
        {
            return dateTime.Date;
        }

        /// <summary>
        /// 获取本日结束时间（23点59分59秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetDayEnd(this DateTime dateTime)
        {
            return dateTime.Date.AddDays(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// 获取本周开始时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetWeekStart(this DateTime dateTime)
        {
            return dateTime.AddDays(-(int)dateTime.DayOfWeek + 1).GetDayStart();
        }

        /// <summary>
        /// 获取本周结束时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetWeekEnd(this DateTime dateTime)
        {
            return dateTime.AddDays(7 - (int)dateTime.DayOfWeek).GetDayEnd();
        }

        /// <summary>
        /// 获取本月开始时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetMonthStart(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1, 0, 0, 0, 0);
        }

        /// <summary>
        /// 获取本月结束时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetMonthEnd(this DateTime dateTime)
        {
            return GetMonthStart(dateTime).AddMonths(1).AddMilliseconds(-1);
        }

        /// <summary>
        /// 获取本季度开始时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetSeasonStart(this DateTime dateTime)
        {
            var time = dateTime.AddMonths(0 - ((dateTime.Month - 1) % 3));
            return DateTime.Parse(time.AddDays(-time.Day + 1).ToString("yyyy/MM/dd 00:00:00"));
        }

        /// <summary>
        /// 获取本季度结束时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetSeasonEnd(this DateTime dateTime)
        {
            var time = dateTime.AddMonths((3 - ((dateTime.Month - 1) % 3) - 1));
            return DateTime.Parse(time.AddMonths(1).AddDays(-time.AddMonths(1).Day + 1).AddDays(-1).ToString("yyyy/MM/dd 23:59:59"));
        }

        /// <summary>
        /// 获取本年开始时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetYearStart(this DateTime dateTime)
        {
            return DateTime.Parse(dateTime.AddDays(-dateTime.DayOfYear + 1).ToString("yyyy/MM/dd 00:00:00"));
        }

        /// <summary>
        /// 获取本年结束时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetYearEnd(this DateTime dateTime)
        {
            var time2 = dateTime.AddYears(1);
            return DateTime.Parse(time2.AddDays(-time2.DayOfYear).ToString("yyyy/MM/dd 23:59:59"));
        }

        /// <summary>
        /// 北京时间转换成unix时间戳(10位/秒)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long BeijingTimeToUnixTimeStamp10(this DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1, 8, 0, 0)).TotalSeconds;
        }

        /// <summary>
        /// 格林威治时间转换成unix时间戳(10位/秒)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long UtcTimeToUnixTimeStamp10(this DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;
        }

        /// <summary>
        /// 北京时间转换成unix时间戳(13位/毫秒)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long BeijingTimeToUnixTimeStamp13(this DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1, 8, 0, 0)).TotalMilliseconds;
        }

        /// <summary>
        /// 格林威治时间转换成unix时间戳(13位/毫秒)
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long UtcTimeToUnixTimeStamp13(this DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1, 0, 0, 0)).TotalMilliseconds;
        }

        /// <summary>
        /// 10位unix时间戳转换成北京时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStamp10ToBeijingTime(this long unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 8, 0, 0).AddSeconds(unixTimeStamp);
        }

        /// <summary>
        /// 10位unix时间戳转换成格林威治
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStamp10ToUtcTime(this long unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(unixTimeStamp);
        }

        /// <summary>
        /// 13位unix时间戳转换成北京时间
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStamp13ToBeijingTime(this long unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 8, 0, 0).AddMilliseconds(unixTimeStamp);
        }

        /// <summary>
        /// 13位unix时间戳转换成格林威治
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStamp13ToUtcTime(this long unixTimeStamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0).AddMilliseconds(unixTimeStamp);
        }

        /// <summary>
        /// 当前日期所在月份第一个指定星期几的日期
        /// </summary>
        /// <param name="date">给定日期</param>
        /// <param name="dayOfWeek">星期几</param>
        /// <returns>所对应的日期</returns>
        public static DateTime GetFirstWeekDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetMonthStart();
            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(1);

            return dt;
        }

        /// <summary>
        /// 当前日期所在月份最后1个指定星期几的日期
        /// </summary>
        /// <param name="date">给定日期</param>
        /// <param name="dayOfWeek">星期几</param>
        /// <returns>所对应的日期</returns>
        public static DateTime GetLastWeekDayOfMonth(this DateTime date, DayOfWeek dayOfWeek)
        {
            var dt = date.GetMonthEnd();
            while (dt.DayOfWeek != dayOfWeek)
                dt = dt.AddDays(-1);

            return dt;
        }

        /// <summary>
        /// 判断是否比指定之间早
        /// </summary>
        /// <param name="date"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsBefore(this DateTime date, DateTime other)
        {
            return date < other;
        }

        /// <summary>
        /// 判断是否比指定时间晚
        /// </summary>
        /// <param name="date"></param>
        /// <param name="other"></param>
        /// <returns></returns>
        public static bool IsAfter(this DateTime date, DateTime other)
        {
            return date > other;
        }

        /// <summary>
        /// 给定日期所在月份共有多少天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static int GetCountDaysOfMonth(this DateTime date)
        {
            return date.GetMonthEnd().Day;
        }

        /// <summary>
        /// 当前日期与给定日期是否是同一天
        /// </summary>
        /// <param name="date">当前日期</param>
        /// <param name="dateToCompare">给定日期</param>
        /// <returns></returns>
        public static bool IsDateEqual(this DateTime date, DateTime dateToCompare)
        {
            return date.Date == dateToCompare.Date;
        }

        /// <summary>
        /// 是否是周未
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsWeekend(this DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        /// <summary>
        /// 是否是工作日
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsWeekDay(this DateTime date)
        {
            return !date.IsWeekend();
        }

        /// <summary>
        /// 判断是否为今天
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static bool IsToday(this DateTime date)
        {
            return date.Date == DateTime.Now.Date;
        }

        /// <summary>
        /// 判定公历闰年遵循的一般规律为：四年一闰，百年不闰，四百年再闰。
        /// 公历闰年的精确计算方法：（按一回归年365天5小时48分45.5秒）
        /// 普通年能被4整除而不能被100整除的为闰年。 （如2004年就是闰年，1900年不是闰年）
        /// 世纪年能被400整除而不能被3200整除的为闰年。 (如2000年是闰年，3200年不是闰年)
        /// 对于数值很大的年份能整除3200,但同时又能整除172800则又是闰年。(如172800年是闰年，86400年不是闰年）
        /// 公元前闰年规则如下：
        /// 非整百年：年数除4余数为1是闰年，即公元前1、5、9……年；
        /// 整百年：年数除400余数为1是闰年，年数除3200余数为1，不是闰年,年数除172800余1又为闰年，即公元前401、801……年。
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsLeap(this DateTime dateTime)
        {
            var year = dateTime.Year;
            if ((year % 400 == 0 && year % 3200 != 0)
               || (year % 4 == 0 && year % 100 != 0)
               || (year % 3200 == 0 && year % 172800 == 0))
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取当前年天数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetDaysByYear(this DateTime dateTime)
        {
            return (new DateTime(dateTime.Year + 1, 1, 1) - new DateTime(dateTime.Year, 1, 1)).Days;
        }

        /// <summary>
        /// 获取当前年天数
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int GetWeekCountByYear(this DateTime dateTime)
        {
            //找到今年的第一天是周几
            int firstWeekend = Convert.ToInt32(DateTime.Parse(dateTime.Year + "-1-1").DayOfWeek);

            //获取第一周的差额,如果是周日，则firstWeekend为0，第一周也就是从周天开始的。
            int weekDay = firstWeekend == 0 ? 1 : (7 - firstWeekend + 1);

            //获取今天是一年当中的第几天
            int currentDay = dateTime.DayOfYear;

            //（今天 减去 第一周周末）/7 等于 距第一周有多少周 再加上第一周的1 就是今天是今年的第几周了
            //    刚好考虑了惟一的特殊情况就是，今天刚好在第一周内，那么距第一周就是0 再加上第一周的1 最后还是1
            int current_week = Convert.ToInt32(Math.Ceiling((currentDay - weekDay) / 7.0)) + 1;
            return current_week;
        }

    }
}
