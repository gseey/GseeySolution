namespace Gseey.Framework.Common.Helpers
{
    using System;

    /// <summary>
    /// Defines the <see cref="DateTimeHelper" />
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 星座
        /// </summary>
        private static string[] _Constellations = new string[] { "水瓶座", "双鱼座", "白羊座", "金牛座", "双子座", "巨蟹座", "狮子座", "处女座", "天秤座", "天蝎座", "射手座", "魔羯座" };

        /// <summary>
        /// 星座分割日期
        /// </summary>
        private static int[] _ConstellationDays = new int[] { 21, 21, 21, 21, 22, 23, 24, 24, 24, 24, 23, 22 };

        /// <summary>
        /// Gets the MinTime
        /// 最小时间
        /// </summary>
        public static DateTime MinTime
        {
            get
            {
                return new DateTime(1970, 1, 1);
            }
        }

        /// <summary>
        /// Gets the MinTimeUnix
        /// 最小时间戳
        /// </summary>
        public static long MinTimeUnix
        {
            get
            {
                return MinTime.ToUnixTime();
            }
        }

        /// <summary>
        /// 日期转换成8位整数格式，如: 2011-8-1 => 20110801
        /// </summary>
        /// <param name="date">The date<see cref="DateTime"/></param>
        /// <returns></returns>
        public static int DateToInt(this DateTime date)
        {
            return int.Parse(date.ToString("yyyyMMdd"));
        }

        /// <summary>
        /// 8位整数转换成日期，如: 20110801 => 2011-8-1
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime IntToDate(int date)
        {
            return DateTime.ParseExact(date.ToString(), "yyyyMMdd", null);
        }

        /// <summary>
        /// 日期转换成10位字符串，如: 2011-8-1 => 2011-08-01
        /// </summary>
        /// <param name="date">The date<see cref="DateTime"/></param>
        /// <returns></returns>
        public static string DateToString(this DateTime date)
        {
            return date.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// 10位字符串转换成日期，如: 2011-08-01 => 2011-8-1
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime StringToDate(string date)
        {
            return DateTime.ParseExact(date, "yyyy-MM-dd", null);
        }

        /// <summary>
        /// 返回时间描述字符串
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static string GetTimeDesc(this DateTime time)
        {
            DateTime now = DateTime.Now;
            TimeSpan ts = now > time ? now - time : TimeSpan.Zero;
            if (ts.Days > 0)
            {
                if (ts.Days == 1)
                {
                    return "昨天";
                }
                else if (ts.Days == 2)
                {
                    return "前天";
                }
                return time.ToString("MM月dd日");
            }
            if (ts.Hours > 0)
            {
                return ts.Hours + "小时前";
            }
            if (ts.Minutes > 0)
            {
                return ts.Minutes + "分钟前";
            }
            return ts.Seconds + "秒前";
        }

        /// <summary>
        /// 返回日期描述字符串
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static string GetDateDesc(this DateTime date)
        {
            DateTime now = DateTime.Now;
            int days = (date - now).Days;
            if (days == 0) return "今天";
            else if (days == 1)
            {
                return "昨天";
            }
            else if (days == 2)
            {
                return "前天";
            }
            else return string.Format("{0}月{1}日", date.Month, date.Day);
        }

        /// <summary>
        /// 根据年月日返回字符串，格式如: 2009-7
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetShortDateString(int year, int month, int day)
        {
            if (year > 0)
            {
                if (month > 0)
                {
                    string[] array;
                    if (day > 0) array = new string[] { year.ToString(), month.ToString(), day.ToString() };
                    else array = new string[] { year.ToString(), month.ToString() };

                    return string.Join("-", array);
                }

                return year.ToString();
            }

            return string.Empty;
        }

        /// <summary>
        /// 根据年月日返回字符串，格式如: 2009年7月
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static string GetLongDateString(int year, int month, int day)
        {
            if (year > 0)
            {
                if (month > 0)
                {
                    if (day > 0) return string.Format("{0}年{1}月{2}日", year, month, day);
                    else return string.Format("{0}年{1}月", year, month);
                }

                return year.ToString() + "年";
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取日期对应的星座
        /// </summary>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public static string GetConstellation(DateTime date)
        {
            return GetConstellation(date.Month, date.Day);
        }

        /// <summary>
        /// 获取日期对应的星座
        /// </summary>
        /// <param name="month">The month<see cref="int"/></param>
        /// <param name="day">The day<see cref="int"/></param>
        /// <returns></returns>
        public static string GetConstellation(int month, int day)
        {
            int index = month - 1;
            if (day < _ConstellationDays[index]) return month == 1 ? _Constellations[11] : _Constellations[index - 1];
            else return _Constellations[index];
        }

        /// <summary>
        /// 将指定的本地时间转换为相应的 Unix 时间戳
        /// </summary>
        /// <param name="localTime"></param>
        /// <param name="IsMilliSecends">是否毫秒级</param>
        /// <returns></returns>
        public static long ToUnixTime(this DateTime localTime, bool IsMilliSecends = false)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, localTime.Kind);
            var result = 0L;
            if (IsMilliSecends)
                result = Convert.ToInt64((localTime - start).TotalMilliseconds);
            else
                result = Convert.ToInt64((localTime - start).TotalSeconds);
            return result;
        }

        /// <summary>
        /// 将指定 Unix 时间戳转换为相应的本地时间
        /// </summary>
        /// <param name="unixTime"></param>
        /// <param name="IsMilliSecends">是否毫秒级</param>
        /// <returns></returns>
        public static DateTime FromUnixTime(this long unixTime, bool IsMilliSecends = false)
        {
            if (IsMilliSecends)
            {
                var start = new DateTime(1970, 1, 1, 0, 0, 0, 000);
                return start.AddMilliseconds(unixTime);
            }
            else
            {
                var start = new DateTime(1970, 1, 1, 0, 0, 0);
                return start.AddSeconds(unixTime);
            }
        }
    }
}
