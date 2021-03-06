﻿namespace Gseey.Framework.Common.Extensions
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.ComponentModel;

    /// <summary>
    /// 枚举扩展类
    /// </summary>
    public static class EnumExtension
    {
        /// <summary>
        /// Defines the enumDescriptionDic
        /// </summary>
        private static ConcurrentDictionary<Type, Dictionary<string, string>>
     enumDescriptionDic = new ConcurrentDictionary<Type, Dictionary<string, string>>();

        /// <summary>
        /// 获取枚举的描述
        /// 枚举必须打上DescriptionAttribute标签
        /// 如果枚举没有DescriptionAttribute特性，那么将返回枚举的ToString()值，如果枚举与类型不匹配，则返回String.Empty
        /// </summary>
        /// <param name="@enum">The enum<see cref="Enum"/></param>
        /// <returns>枚举的描述值</returns>
        public static string GetDescription(this Enum @enum)
        {
            var dic = GetEnumDic(@enum.GetType());
            var enumStr = @enum.ToString();
            return dic.ContainsKey(enumStr) ? dic[enumStr] : string.Empty;
        }

        /// <summary>
        /// The GetEnumDic
        /// </summary>
        /// <param name="enumType">The enumType<see cref="Type"/></param>
        /// <returns>The <see cref="Dictionary{string, string}"/></returns>
        public static Dictionary<string, string> GetEnumDic(Type enumType)
        {
            return enumDescriptionDic.GetOrAdd(enumType, t =>
            {
                var dic = new Dictionary<string, string>();
                var fieldinfos = enumType.GetFields();
                foreach (var field in fieldinfos)
                {
                    if (field.FieldType.IsEnum)
                    {
                        var objs = field.GetCustomAttributes(typeof(DescriptionAttribute), false);
                        var innerID = Convert.ToInt32(field.GetValue(enumType));
                        if (objs.Length > 0)
                            dic.Add(field.Name, ((DescriptionAttribute)objs[0]).Description);
                        else
                            dic.Add(field.Name, field.Name);
                    }
                }
                return dic;
            });
        }
    }
}
