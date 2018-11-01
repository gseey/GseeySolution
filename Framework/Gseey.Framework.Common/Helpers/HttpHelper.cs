namespace Gseey.Framework.Common.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// http帮助类
    /// </summary>
    public sealed class HttpHelper
    {
        /// <summary>
        /// 获取httpclient
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 获取网页信息(同步)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetHtml(string url)
        {
            var uri = new Uri(url);
            using (var client = GetHttpClient())
            {
                client.BaseAddress = uri;
                var result = client.GetStringAsync(uri).Result;
                return result;
            }
        }

        /// <summary>
        /// 获取网页信息(异步)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetHtmlAsync(string url)
        {
            var uri = new Uri(url);
            using (var client = GetHttpClient())
            {
                client.BaseAddress = uri;
                var result = await client.GetStringAsync(uri);
                return result;
            }
        }

        /// <summary>
        /// 获取网页信息(同步)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static TResult GetHtml<TResult>(string url) where TResult : class
        {
            var uri = new Uri(url);
            using (var client = GetHttpClient())
            {
                client.BaseAddress = uri;
                var html = client.GetStringAsync(uri).Result;
                var result = html.FromJson<TResult>();
                return result;
            }
        }

        /// <summary>
        /// 获取网页信息(异步)
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<TResult> GetHtmlAsync<TResult>(string url) where TResult : class
        {
            var uri = new Uri(url);
            using (var client = GetHttpClient())
            {
                client.BaseAddress = uri;
                var html = await client.GetStringAsync(uri);
                var result = html.FromJson<TResult>();
                return result;
            }
        }

        /// <summary>
        /// The PostData
        /// </summary>
        /// <param name="url">The url<see cref="string"/></param>
        /// <param name="dataBuffer">The dataBuffer<see cref="Dictionary{string, string}"/></param>
        /// <returns>The <see cref="string"/></returns>
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

        /// <summary>
        /// The PostDataAsync
        /// </summary>
        /// <param name="url">The url<see cref="string"/></param>
        /// <param name="dataBuffer">The dataBuffer<see cref="Dictionary{string, string}"/></param>
        /// <returns>The <see cref="Task{string}"/></returns>
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

        /// <summary>
        /// The PostData
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="url">The url<see cref="string"/></param>
        /// <param name="value">The value<see cref="TValue"/></param>
        /// <returns>The <see cref="TResult"/></returns>
        public static TResult PostData<TResult, TValue>(string url, TValue value) where TResult : class
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var json = value.ToJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = client.PostAsync(uri, content).Result;
            var html = response.Content.ReadAsStringAsync().Result;
            var result = html.FromJson<TResult>();
            return result;
        }

        /// <summary>
        /// The PostDataAsync
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="url">The url<see cref="string"/></param>
        /// <param name="value">The value<see cref="TValue"/></param>
        /// <returns>The <see cref="Task{TResult}"/></returns>
        public static async Task<TResult> PostDataAsync<TResult, TValue>(string url, TValue value) where TResult : class
        {
            var uri = new Uri(url);
            var client = GetHttpClient();
            client.BaseAddress = uri;
            var json = value.ToJson();
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(uri, content);
            var html = await response.Content.ReadAsStringAsync();
            var result = html.FromJson<TResult>();
            return result;
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="fileFullPath"></param>
        /// <returns></returns>
        public static async Task<TResult> UploadFile<TResult>(string url, string fileFullPath) where TResult : class
        {
            var uri = new Uri(url);
            using (var client = GetHttpClient())
            {
                client.BaseAddress = uri;
                var nonceStr = Guid.NewGuid().ToString().Replace("-", "");

                var buffers = await File.ReadAllBytesAsync(fileFullPath);
                Stream paramFileStream = new MemoryStream(buffers);
                var fileName = Path.GetFileName(fileFullPath);

                var stringContentfilename = new StringContent(fileName);
                var fileStreamContent = new StreamContent(paramFileStream);
                var bytesContent = new ByteArrayContent(buffers);

                using (var formdata = new MultipartFormDataContent())
                {
                    formdata.Add(stringContentfilename, "file_name");

                    formdata.Add(fileStreamContent, "file", fileName);
                    formdata.Add(bytesContent, fileName);
                    HttpResponseMessage response = new HttpResponseMessage();
                    response = await client.PostAsync(uri, formdata);

                    var html = await response.Content.ReadAsStringAsync();

                    var result = html.FromJson<TResult>();

                    return result;
                }
            }
        }
    }
}
