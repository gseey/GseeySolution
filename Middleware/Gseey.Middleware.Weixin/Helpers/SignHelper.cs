using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.TencentSdk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Gseey.Middleware.Weixin.Helpers
{
    /// <summary>
    /// 签名帮助类
    /// </summary>
    internal class SignHelper
    {
        #region 私有函数

        /// <summary>
        /// 获取微信消息加密工具
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        private static WXBizMsgCrypt GetWXBizMsgCrypt(int channelId)
        {
            var configDto = WeixinConfigHelper.GetWeixinConfigDTOAsync(channelId).Result;

            var crypt = new WXBizMsgCrypt(configDto.Token, configDto.EncodingAESKey, configDto.AppId);
            return crypt;
        }

        /// <summary>
        /// 返回正确的签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        private static string GetSignature(string timestamp, string nonce, string token)
        {
            var args = new[] { token, timestamp, nonce }.OrderBy(z => z).ToArray();
            var argsString = string.Join("", args);
            var sha1 = SHA1.Create();
            var sha1Arr = sha1.ComputeHash(Encoding.UTF8.GetBytes(argsString
));
            StringBuilder enText = new StringBuilder();
            foreach (var b in sha1Arr)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }
        #endregion

        #region 校验签名

        /// <summary>
        /// 校验微信公众号签名
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="msg_signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns></returns>
        public static bool CheckSignature(int channelId, string msg_signature, string timestamp, string nonce, string echo, out string replyEcho)
        {
            replyEcho = string.Empty;
            var configDto = WeixinConfigHelper.GetWeixinConfigDTOAsync(channelId).Result;
            var result = msg_signature == GetSignature(timestamp, nonce, configDto.Token);
            if (result)
                replyEcho = echo;
            return result;
        }

        #endregion

        #region 验证url有效性

        /// <summary>
        /// 验证企业号url有效性
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="signature">从接收消息的URL中获取的msg_signature参数</param>
        /// <param name="timestamp">从接收消息的URL中获取的timestamp参数</param>
        /// <param name="nonce">从接收消息的URL中获取的nonce参数</param>
        /// <param name="echo">从接收消息的URL中获取的echostr参数。注意，此参数必须是urldecode后的值</param>
        /// <param name="replyEcho">解密后的明文消息内容，用于回包。注意，必须原样返回，不要做加引号或其它处理</param>
        /// <returns></returns>
        public static bool ValidateUrl(int channelId, string signature, string timestamp, string nonce, string echo, out string replyEcho)
        {
            replyEcho = string.Empty;
            WXBizMsgCrypt crypt = GetWXBizMsgCrypt(channelId);
            var result = crypt.VerifyURL(signature, timestamp, nonce, echo, ref replyEcho);

            return echo.Equals(replyEcho);
        }

        #endregion

        #region 消息解密

        /// <summary>
        /// 消息解密
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="signature">从接收消息的URL中获取的msg_signature参数</param>
        /// <param name="timestamp">从接收消息的URL中获取的timestamp参数</param>
        /// <param name="nonce">从接收消息的URL中获取的nonce参数</param>
        /// <param name="postMsg">从接收消息的URL中获取的整个post数据</param>
        /// <returns>返回解密后的msg，以xml组织，参见普通消息格式和事件消息格式</returns>
        public static string DecryptMsg(int channelId, string msg_signature, string timestamp, string nonce, string postMsg)
        {
            var crypt = GetWXBizMsgCrypt(channelId);
            var msg = string.Empty;
            var result = crypt.DecryptMsg(msg_signature, timestamp, nonce, postMsg, ref msg);
            return msg;
        }

        #endregion

        #region 消息加密

        /// <summary>
        /// 消息加密
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="replyMsg">返回的消息体原文</param>
        /// <returns>返回的密文，以xml组织，参见被动回复消息格式</returns>
        public static string EncryptMsg(int channelId, string replyMsg)
        {
            var crypt = GetWXBizMsgCrypt(channelId);
            var msg = string.Empty;
            var timestamp = DateTime.Now.ToUnixTime().ToString();
            var nonce = Guid.NewGuid().ToString().Replace("-", "");
            var result = crypt.EncryptMsg(replyMsg, timestamp, nonce, ref msg);
            return msg;
        }

        #endregion
    }
}
