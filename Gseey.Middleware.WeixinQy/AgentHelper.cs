using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.DTOs;
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
        /// <param name="appId"></param>
        /// <returns></returns>
        public static async Task<AgentConfigDTO> GetAgentConfigDTOAsync(int appId)
        {
            //根据appid获取应用信息(从缓存中获取)
            RedisHelper redisHelper = new RedisHelper();
            var redisKey = string.Format("CorpAppId_{0}", appId);
            var configDto = await redisHelper.StringGetAsync<AgentConfigDTO>(redisKey);

            return configDto;
        }
    }
}
