namespace Gseey.Framework.Common.Extensions
{
    using Gseey.Framework.Common.Helpers;
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="StringExtension" />
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 分割字符串
        /// </summary>
        /// <param name="input"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string SubEx(this string input, int count)
        {
            if (input.Length <= count)
                return input;
            var result = input.Substring(0, count) + "...";
            return result;
        }

        /// <summary>
        /// The ToStringList
        /// </summary>
        /// <param name="list">The list<see cref="IEnumerable"/></param>
        /// <returns>The <see cref="string"/></returns>
        public static string ToStringList(this IEnumerable list)
        {
            var result = new List<string>();

            foreach (var item in list)
            {
                if (item.GetType().IsClass)
                {
                    result.Add(item.ToJson());
                }
                else
                {
                    result.Add(item.ToString());
                }
            }
            return string.Join(",", result.ToArray());
        }
    }
}
