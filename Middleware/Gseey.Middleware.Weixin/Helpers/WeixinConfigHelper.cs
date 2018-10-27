using Gseey.Middleware.Weixin.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Helpers
{
    /// <summary>
    /// 微信配置帮助类
    /// </summary>
    internal class WeixinConfigHelper
    {
        /// <summary>
        /// 获取微信基本配置信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static WeixinConfigDTO GetWeixinConfigDTO(int channelId)
        {
            //这里从数据库中读取当前渠道的配置信息
            var configDto = new WeixinConfigDTO();
            if (channelId == 1)
            {
                configDto.ChannelId = 1;
            }
            else if (channelId == 2)
            {
                configDto.ChannelId = 2;
                configDto.AppId = "wxf469126fc1cbdc63";
                configDto.AppSercet = "2bb6660472c3beff87baa831201fa8ad";
                configDto.WxType = Enums.WeixinType.WxMp;
                configDto.AgentId = 0;
                configDto.EncodingAESKey = "OFhDh1eyfv8GYBluexZUAeLJ5gBx9yoykW45jfGX0Uu";
                configDto.Token = "Gscoy";
            }


            return configDto;
        }

        ///// <summary>
        ///// 获取微信基本配置信息
        ///// </summary>
        ///// <param name="channelId"></param>
        ///// <returns></returns>
        //public static async Task<WeixinConfigDTO> GetWeixinConfigDTOAsync(int channelId)
        //{
        //    var accessToken = await AccessTokenHelper.GetAccessTokenAsync(channelId);

        //    var jsapiTicket = await AccessTokenHelper.GetJsapiTicketAsync(channelId);

        //    var configDto = GetWeixinConfigDTO(channelId);

        //    configDto.AccessToken = accessToken;
        //    configDto.JsapiTicket = jsapiTicket;

        //    return configDto;
        //}
    }
}
