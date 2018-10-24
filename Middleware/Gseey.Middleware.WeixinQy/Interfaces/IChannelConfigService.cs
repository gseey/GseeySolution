using Gseey.Framework.BaseDTO;
using Gseey.Middleware.WeixinQy.Service.MessageHandler;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Gseey.Middleware.WeixinQy.Interfaces
{
    public interface IChannelConfigService
    {
        /// <summary>
        /// 校验渠道微信签名
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <returns></returns>
        Task<ExecuteResult<Tuple<bool, string>>> CheckChannelWeixinQySignAsync(int channelId, string signature, string timestamp, string nonce, string echo);

        /// <summary>
        /// 处理微信消息
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="inputStream"></param>
        /// <returns></returns>
        Task<ExecuteResult<CustomMessageHandler>> HandleInputWeixinQyMessageAsync(int channelId, Stream inputStream);
    }
}
