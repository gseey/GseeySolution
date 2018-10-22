using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.DTOs;
using Gseey.Middleware.WeixinQy.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.WeixinQy
{
    public class AgentHelper
    {
        /// <summary>
        /// 根据appid获取应用配置信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static async Task<AgentConfigDTO> GetAgentConfigDTOAsync(int channelId)
        {
            var service = new AgentConfigService();
            //var entity = await service.QueryListAsync(new { Id = appId });

            //根据appid获取应用信息(从缓存中获取)
            RedisHelper redisHelper = new RedisHelper();
            var redisKey = string.Format("CorpAppId_{0}", channelId);
            var configDto = await redisHelper.StringGetAsync<AgentConfigDTO>(redisKey);

            if (configDto == null)
                configDto = new AgentConfigDTO();

            return configDto;
        }
    }
}
