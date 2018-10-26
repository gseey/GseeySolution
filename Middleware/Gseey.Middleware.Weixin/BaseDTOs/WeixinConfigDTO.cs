using Gseey.Middleware.Weixin.Enums;
using Gseey.Middleware.Weixin.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.BaseDTOs
{
    /// <summary>
    /// 微信配置信息
    /// </summary>
    public class WeixinConfigDTO
    {
        /// <summary>
        /// 渠道id
        /// </summary>
        public int ChannelId { get; set; }

        /// <summary>
        /// 应用id
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// 应用sercet
        /// </summary>
        public string AppSercet { get; set; }

        /// <summary>
        /// 应用token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 应用消息加密串
        /// </summary>
        public string EncodingAESKey { get; set; }

        /// <summary>
        /// 企业号应用id
        /// </summary>
        public int AgentId { get; set; }

        /// <summary>
        /// 微信类型
        /// </summary>
        public WeixinType WxType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string AccessToken
        {
            get
            {
                var accessTokenResult = AccessTokenHelper.GetAccessTokenAsync(ChannelId, AppId, AppSercet, WxType);
                accessTokenResult.Wait();
                var accessToken = accessTokenResult.Result;
                return accessToken;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public string JsapiTicket
        {
            get
            {
                var jsapiTicketResult = AccessTokenHelper.GetJsapiTicketAsync(ChannelId, AppId, AppSercet, WxType);
                jsapiTicketResult.Wait();
                var jsapiTicket = jsapiTicketResult.Result;
                return jsapiTicket;
            }
        }

        public WeixinConfigDTO()
        {
            ChannelId = 6;
            AppId = "wx9a80f6e6ed2a89e6";
            AppSercet = "KVZ_1nE2thZdbu3kcftpgA5Ld-O3TafmS3AlUtWQeHM";
            Token = "qygscoy";
            EncodingAESKey = "FDPibMBM3MExJxBaD9Oe6uaOfsvsjmTQT94f6tt2lJl";
            AgentId = 6;
            WxType = WeixinType.WxWork;
        }

    }
}
