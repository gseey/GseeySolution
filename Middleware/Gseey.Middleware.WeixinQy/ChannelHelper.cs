using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.DTOs;
using Gseey.Middleware.WeixinQy.Interfaces;
using Gseey.Middleware.WeixinQy.Service;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.WeixinQy
{
    internal class ChannelHelper
    {
        /// <summary>
        /// 根据appid获取应用配置信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static async Task<ChannelConfigDTO> GetAgentConfigDTOAsync(int channelId)
        {
            ChannelConfigDTO configDto = null;
            try
            {
                var service = new ChannelConfigService();
                configDto = await service.GetAgentConfigDTOByChannelIdAsync(channelId);
            }
            catch (Exception ex)
            {
                ex.WriteExceptionLog("");
            }

            if (configDto == null)
                configDto = new ChannelConfigDTO();

            return configDto;
        }
    }
}
