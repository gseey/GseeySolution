using Gseey.Framework.BaseDTO;
using Gseey.Middleware.WeixinQy.DTOs;
using Gseey.Middleware.WeixinQy.Entities;
using Senparc.NeuChar.Context;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
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
        Task<ExecuteResult<WorkMessageHandler<MessageContext<IWorkRequestMessageBase, IWorkResponseMessageBase>>>> HandleInputWeixinQyMessageAsync(int channelId, Stream inputStream);
    }
}
