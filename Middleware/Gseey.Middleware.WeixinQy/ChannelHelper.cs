using Gseey.Framework.Autofac;
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
    public class ChannelHelper
    {
        /// <summary>
        /// 根据appid获取应用配置信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static async Task<ChannelConfigDTO> GetAgentConfigDTOAsync(int channelId)
        {
            AutofacHelper.Register<IChannelConfigService, ChannelConfigService>();

            var service = AutofacHelper.Resolve<IChannelConfigService>();
            var configDto = await service.GetAgentConfigDTOByChannelIdAsync(channelId);



            if (configDto == null)
                configDto = new ChannelConfigDTO();

            return configDto;
        }
    }
}
