using Gseey.Framework.BaseDTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Services
{
    public interface IMessageHandlerService
    {
        /// <summary>
        /// 获取相应消息
        /// </summary>
        /// <param name="channelId">渠道消息</param>
        /// <param name="msg_signature">加密串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机串</param>
        /// <param name="inputMsg">加密消息</param>
        /// <returns></returns>
        Task<string> GetResponseAsync(int channelId, string msg_signature, string timestamp, string nonce, string inputMsg);

        /// <summary>
        /// 校验渠道微信签名
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <returns></returns>
        ExecuteResult<Tuple<bool, string>> CheckChannelWeixinSign(int channelId, string msg_signature, string signature, string timestamp, string nonce, string echo);
    }
}
