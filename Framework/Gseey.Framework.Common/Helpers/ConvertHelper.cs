namespace Gseey.Framework.Common.Helpers
{
    using System;

    /// <summary>
    /// 数值格式转换
    /// </summary>
    public static class ConvertHelper
    {
        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int ToInt(this string input)
        {
            var temp = 0;
            if (!int.TryParse(input, out temp))
            {
                return 0;
            }
            return temp;
        }

        /// <summary>
        /// 转换为长整型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static long ToLong(this string input)
        {
            long temp = 0;
            if (!long.TryParse(input, out temp))
            {
                return 0;
            }
            return temp;
        }

        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double ToDoubule(this string input)
        {
            double temp = 0;
            if (!double.TryParse(input, out temp))
            {
                return 0;
            }
            return temp;
        }

        /// <summary>
        /// 转换为长整型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string input)
        {
            decimal temp = 0;
            if (!decimal.TryParse(input, out temp))
            {
                return 0;
            }
            return temp;
        }

        /// <summary>
        /// 转换为整型
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string input)
        {
            DateTime temp = DateTime.MinValue;
            if (!DateTime.TryParse(input, out temp))
            {
                return DateTime.MinValue;
            }
            return temp;
        }
    }
}
