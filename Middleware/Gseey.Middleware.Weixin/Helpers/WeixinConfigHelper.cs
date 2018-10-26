using Gseey.Middleware.Weixin.BaseEntities;
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
                configDto.AppId = "wx5f06bae632204d55";
                configDto.AppId = "90482a2898b82b64a045c0a08e43c58f";
                configDto.WxType = Enums.WeixinType.WxMp;
                configDto.AgentId = 0;
                configDto.EncodingAESKey = "";
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
