namespace Gseey.Framework.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// 随机数帮助类
    /// </summary>
    public static class RandomHelper
    {
        /// <summary>
        /// 获取指定位数随机数
        /// </summary>
        /// <param name="length">位数</param>
        /// <returns></returns>
        public static int GetRandomNum(int length = 6)
        {
            Random random = new Random(Guid.NewGuid().GetHashCode());//修正多次循环，随机数一致的问题，随机种子；
            return random.Next(length);
        }

        /// <summary>
        /// 从集合中随机取出一条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static T GetRandomListItem<T>(this IEnumerable<T> list)
        {
            if (list == null || list.Count() <= 0)
            {
                return default(T);
            }
            var index = GetRandomNum(list.Count());
            var item = list.ElementAt(index);
            return item;
        }

        /// <summary>
        /// 获取guid
        /// </summary>
        /// <param name="isReplace">是否替换-</param>
        /// <returns></returns>
        public static string GetGUID(bool isReplace = false)
        {
            var value = Guid.NewGuid().ToString();
            value = isReplace ? value.Replace("-", "") : value;
            return value;
        }
    }
}
