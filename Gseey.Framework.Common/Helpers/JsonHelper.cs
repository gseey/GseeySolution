using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gseey.Framework.Common.Helpers
{
    public sealed class JsonHelper
    {
        private static IsoDateTimeConverter timeConverter = null;
       
        #region 静态构造函数

        static JsonHelper()
        {
            timeConverter = new IsoDateTimeConverter
            {
                DateTimeFormat = "yyyy-MM-dd HH:mm:ss"
            };
        }

        #endregion

        #region 序列化json

        /// <summary>
        /// 序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToJson<T>(T value)
        {
            return JsonConvert.SerializeObject(value, Formatting.Indented, timeConverter);
        }

        #endregion


        #region 反序列化json

        /// <summary>
        /// 反序列化json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonValue"></param>
        /// <returns></returns>
        public static T FromJson<T>(string jsonValue)
        {
            return JsonConvert.DeserializeObject<T>(jsonValue, timeConverter);
        }

        #endregion
    }
}
