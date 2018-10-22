using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.DTOs;
using Gseey.Middleware.WeixinQy.Entities;
using Gseey.Middleware.WeixinQy.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.WeixinQy.Service
{
    internal class ChannelConfigService : IChannelConfigService
    {
        public async Task<ChannelConfigDTO> GetAgentConfigDTOByChannelIdAsync(int channelId)
        {
            //根据appid获取应用信息(从缓存中获取)
            RedisHelper redisHelper = new RedisHelper();
            var redisKey = string.Format("CorpAppId_{0}", channelId);
            var configDto = await redisHelper.StringGetAsync<ChannelConfigDTO>(redisKey);
            return configDto;
        }
    }
}
