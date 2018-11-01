namespace Gseey.Framework.Common.Helpers
{
    using System;
    using System.Text;
    using System.Web;

    /// <summary>
    /// Base64编码解码
    /// </summary>
    public sealed class EncodeHelper
    {
        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Base64Encode(string input, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var inputArray = encoding.GetBytes(input);

            var result = Convert.ToBase64String(inputArray);

            return result;
        }

        /// <summary>
        /// base64编码
        /// </summary>
        /// <param name="inputarr"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Base64Encode(byte[] inputarr, Encoding encoding)
        {
            var result = Convert.ToBase64String(inputarr);

            return result;
        }

        /// <summary>
        /// base64解码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Base64Decode(string input, Encoding encoding)
        {
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var outputArray = Convert.FromBase64String(input);

            var result = encoding.GetString(outputArray);

            return result;
        }

        /// <summary>
        /// base64编码后在进行url编码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Base64UrlEncode(string input, Encoding encoding)
        {
            var output = Base64Encode(input, encoding);
            output = HttpUtility.UrlEncode(output, encoding);
            return output;
        }

        /// <summary>
        /// 先编码成字符串在base64编码后在进行url编码
        /// </summary>
        /// <param name="buffer">The buffer<see cref="byte[]"/></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Base64UrlEncode(byte[] buffer, Encoding encoding)
        {
            var input = encoding.GetString(buffer);
            return Base64UrlEncode(input, encoding);
        }

        /// <summary>
        /// url解码后在进行base64解码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string Base64UrlDecode(string input, Encoding encoding)
        {
            var output = HttpUtility.UrlDecode(input, encoding);
            output = Base64Decode(output, encoding);
            return output;
        }

        /// <summary>
        /// url编码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string UrlEncode(string input, Encoding encoding)
        {
            return HttpUtility.UrlEncode(input, encoding);
        }

        /// <summary>
        /// url解码
        /// </summary>
        /// <param name="input"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string UrlDecode(string input, Encoding encoding)
        {
            return HttpUtility.UrlDecode(input, encoding);
        }

        /// <summary>
        /// html编码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlEncode(string input)
        {
            return HttpUtility.HtmlEncode(input);
        }

        /// <summary>
        /// html解码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string HtmlDecode(string input)
        {
            return HttpUtility.HtmlDecode(input);
        }
    }
}
