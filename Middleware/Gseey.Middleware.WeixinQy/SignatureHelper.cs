using Gseey.Middleware.WeixinQy.DTOs;
using Senparc.Weixin.Work;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Gseey.Middleware.WeixinQy
{
    internal class SignatureHelper
    {
        #region 基本配置

        public ChannelConfigDTO Config { get; set; }

        #endregion

        public SignatureHelper(ChannelConfigDTO config)
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
        public bool CheckSign(string signature, string timestamp, string nonce, string echo, out string replyEcho)
        {
            replyEcho = Signature.VerifyURL(Config.Token, Config.EncodingAESKey, Config.CorpId, signature, timestamp, nonce, echo);

            return echo.Equals(replyEcho);
        }

        public string DecryptMsg(string signature, string timestamp, string nonce)
        {
            return string.Empty;
        }
    }
}
