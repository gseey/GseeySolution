using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Gseey.Framework.Common.Helpers
{
    /// <summary>
    /// JSON帮助类
    /// </summary>
    public static class JsonHelper
    {
        private static JsonSerializerSettings _jsonSettings;

        static JsonHelper()
        {
            IsoDateTimeConverter datetimeConverter = new IsoDateTimeConverter();
            datetimeConverter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";

            _jsonSettings = new JsonSerializerSettings();
            _jsonSettings.MissingMemberHandling = Newtonsoft.Json.MissingMemberHandling.Ignore;
            _jsonSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
            _jsonSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            _jsonSettings.Converters.Add(datetimeConverter);
        }

        /// <summary>
        /// 将指定的对象序列化成 JSON 数据。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            try
            {
                if (null == obj) return null;

                return JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.None, _jsonSettings);
            }
            catch (Exception ex)
            {
                //ex.WriteExceptionLog("转换json出错", bizEnum: LogHelper.LogicBuissnussEnum.Framework);
            }
            return null;
        }

        /// <summary>
        /// 将指定的 JSON 数据反序列化成指定对象。
        /// </summary>
        /// <typeparam name="T">对象类型。</typeparam> 
        /// <param name="json">JSON 数据。</param>
        /// <returns></returns>
        public static T FromJson<T>(this string json) where T : class
        {
            try
            {
                return JsonConvert.DeserializeObject<T>(json, _jsonSettings);
            }
            catch (Exception ex)
            {
                //ex.WriteExceptionLog("转换json出错", bizEnum: LogHelper.LogicBuissnussEnum.Framework);
            }
            return default(T);
        }

        /// <summary>
        /// xml转json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T XmlToJson<T>(this string xml) where T : class
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(xml);
            ReplaceXmlCData(doc.DocumentElement);

            return FromJson<T>(doc);
        }

        /// <summary>
        /// 将xml序列化成json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="doc"></param>
        /// <returns></returns>
        private static T FromJson<T>(this XmlDocument doc) where T : class
        {
            var json = JsonConvert.SerializeXmlNode(doc);
            var obj = FromJson<T>(json);
            return obj;
        }
        private static void ReplaceXmlCData(XmlNode node)
        {
            if (node.FirstChild != null)
            {
                switch (node.FirstChild.NodeType)
                {
                    case XmlNodeType.CDATA:
                        node.InnerText = node.FirstChild.InnerText;
                        break;
                    default:
                        {
                            foreach (XmlNode item in node.ChildNodes)
                            {
                                ReplaceXmlCData(item);
                            }
                            break;
                        }
                }
            }
        }
    }
}
