namespace Gseey.Framework.Common.Helpers
{
    using Microsoft.Extensions.Configuration;
    using System.IO;

    /// <summary>
    /// Defines the <see cref="ConfigHelper" />
    /// </summary>
    public sealed class ConfigHelper
    {
        /// <summary>
        /// Defines the config
        /// </summary>
        private readonly static IConfiguration config = null;

        /// <summary>
        /// Initializes static members of the <see cref="ConfigHelper"/> class.
        /// </summary>
        static ConfigHelper()
        {
            var binder = new ConfigurationBuilder();
            binder.SetBasePath(Directory.GetCurrentDirectory());
            binder.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            config = binder.Build();
        }

        /// <summary>
        /// 获取配置文件值
        /// </summary>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static string Get(string key, string defaultValue = "")
        {
            var value = config.GetValue(key, defaultValue);
            return string.IsNullOrEmpty(value) ? config[key] : value;
        }

        /// <summary>
        /// 获取配置文件值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">配置键</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns></returns>
        public static T Get<T>(string key, T defaultValue = default(T))
        {
            return config.GetValue(key, defaultValue);
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="name">配置键</param>
        /// <returns></returns>
        public static string GetConnectionString(string name)
        {
            return config.GetConnectionString(name);
        }
    }
}
