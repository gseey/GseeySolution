using Gseey.Middleware.WeixinQy.DTOs;
using Gseey.Middleware.WeixinQy.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.WeixinQy.Interfaces
{
    public interface IChannelConfigService
    {
        /// <summary>
        /// 根据渠道号获取渠道配置信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        Task<ChannelConfigDTO> GetAgentConfigDTOByChannelIdAsync(int channelId);
    }
}
