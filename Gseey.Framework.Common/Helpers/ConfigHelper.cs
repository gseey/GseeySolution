using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gseey.Framework.Common.Helpers
{
    public sealed class ConfigHelper
    {
        private readonly static IConfiguration config = null;

        #region 静态类,配置对应文件

        static ConfigHelper()
        {
            var binder = new ConfigurationBuilder();
            binder.SetBasePath(Directory.GetCurrentDirectory());
            binder.AddJsonFile("appsetting.json", optional: true, reloadOnChange: true);
            config = binder.Build();
        }

        #endregion

        #region 获取配置文件值

        /// <summary>
        /// 获取配置文件值
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string Get(string key, string defaultValue = "")
        {
            return config.GetValue<string>(key, defaultValue);
        }


        /// <summary>
        /// 获取配置文件值
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T Get<T>(string key, T defaultValue = default(T))
        {
            return config.GetValue<T>(key, defaultValue);
        }

        #endregion

        #region 获取连接字符串

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="key">配置键</param>
        /// <returns></returns>
        public static string GetConnectionString(string key)
        {
            return config.GetConnectionString(key);
        }

        #endregion
    }
}
