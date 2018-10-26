using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Framework.Common.Helpers
{
    /// <summary>
    /// http帮助类
    /// </summary>
    public sealed class HttpHelper
    {
        private static HttpClient GetHttpClient()
        {
            HttpClientHandler handler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate
            };
            var httpClient = new HttpClient(handler);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
            return httpClient;
        }

        #region 获取网页信息

        /// <summary>
        /// 获取网页信息(同步)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var result = client.GetStringAsync(uri).Result;
            return result;
        }
        /// <summary>
        /// 获取网页信息(异步)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetHtmlAsync(string url)
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var result = await client.GetStringAsync(uri);
            return result;
        }


        /// <summary>
        /// 获取网页信息(同步)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static TResult GetHtml<TResult>(string url) where TResult : class
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var html = client.GetStringAsync(uri).Result;
            var result = html.FromJson<TResult>();
            return result;
        }
        /// <summary>
        /// 获取网页信息(异步)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<TResult> GetHtmlAsync<TResult>(string url) where TResult : class
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var html = await client.GetStringAsync(uri);
            var result = html.FromJson<TResult>();
            return result;
        }

        #endregion

        #region 向网页提交数据

        public static string PostData(string url, Dictionary<string, string> dataBuffer)
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var content = new FormUrlEncodedContent(dataBuffer);
            var response = client.PostAsync(uri, content).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            return result;
        }

        public static async Task<string> PostDataAsync(string url, Dictionary<string, string> dataBuffer)
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var content = new FormUrlEncodedContent(dataBuffer);
            var response = await client.PostAsync(uri, content);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }


        public static TResult PostData<TResult, TValue>(string url, TValue value) where TResult : class
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var content = new StringContent(value.ToJson(), Encoding.UTF8, "application/json");
            var response = client.PostAsync(uri, content).Result;
            var html = response.Content.ReadAsStringAsync().Result;
            var result = html.FromJson<TResult>();
            return result;
        }

        public static async Task<TResult> PostDataAsync<TResult, TValue>(string url, TValue value) where TResult : class
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var content = new StringContent(value.ToJson(), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, content);
            var html = await response.Content.ReadAsStringAsync();
            var result = html.FromJson<TResult>();
            return result;
        }
        #endregion
    }
}
