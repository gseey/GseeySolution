using Gseey.Middleware.WeixinQy.DTOs;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Gseey.Middleware.WeixinQy
{
    public class Signature
    {
        #region 基本配置

        public ChannelConfigDTO Config { get; set; }

        #endregion

        public Signature(ChannelConfigDTO config)
        {
            Config = config;
        }

        /// <summary>
        /// 校验签名
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        public bool CheckSign(string signature, string timestamp, string nonce)
        {
            var customSign = GetSignature(timestamp, nonce);
            return signature.Equals(customSign);
        }

        /// <summary>
        /// 返回正确的签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <returns></returns>
        private string GetSignature(string timestamp, string nonce)
        {
            var arr = new[] { Config.Token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var arrString = string.Join("", arr);
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(arrString));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }
    }
}
