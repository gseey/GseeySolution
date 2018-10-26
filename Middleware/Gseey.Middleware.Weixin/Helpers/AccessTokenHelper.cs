using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.BaseEntities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Helpers
{
    /// <summary>
    /// token帮助类
    /// </summary>
    internal class AccessTokenHelper
    {
        private static object accessTokenDto;

        /// <summary>
        /// 获取accesstoken
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static async Task<string> GetAccessTokenAsync(int channelId)
        {
            var configDto = WeixinConfigHelper.GetWeixinConfigDTO(channelId);
            var redisHelper = new RedisHelper();
            var redisKey = string.Format("AccessToken_{0}_{1}",configDto.ChannelId,configDto.AppId);
            //从缓存中读取
            var accessToken = await redisHelper.StringGetAsync<string>(redisKey);
            if (string.IsNullOrEmpty(accessToken))
            {
                //从api中获取
                var weixinAccessTokenUrl = string.Empty;
                switch (configDto.WxType)
                {
                    case Enums.WeixinType.WxMp:
                        weixinAccessTokenUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={0}&corpsecret={1}", configDto.AppId, configDto.AppSercet);
                        break;
                    case Enums.WeixinType.WxWork:
                    default:
                        weixinAccessTokenUrl = string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", configDto.AppId, configDto.AppSercet);
                        break;
                    case Enums.WeixinType.WxApp:
                        break;
                }

                var html = await HttpHelper.GetHtmlAsync(weixinAccessTokenUrl);
                var accessTokenDto = html.FromJson<AccessTokenResponseDTO>();
                if (accessTokenDto.errcode == 0)
                {
                    accessToken = accessTokenDto.access_token;

                    var setResult = redisHelper.StringSet<string>(redisKey, accessTokenDto.access_token, TimeSpan.FromSeconds(accessTokenDto.expires_in * 1.0));
                }
            }
            return accessToken;
        }

        /// <summary>
        /// 获取jsapi_ticket
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static async Task<string> GetJsapiTicketAsync(int channelId)
        {
            var configDto = WeixinConfigHelper.GetWeixinConfigDTO(channelId);
            //获取accesstoken
            var accessToken = await GetAccessTokenAsync(channelId);
            var redisHelper = new RedisHelper();
            var redisKey = string.Format("JsapiTicket_{0}_{1}", configDto.ChannelId, configDto.AppId);
            //从缓存中读取
            var jsapiTicket = await redisHelper.StringGetAsync<string>(redisKey);
            if (string.IsNullOrEmpty(jsapiTicket))
            {
                //从api中获取
                var weixinJsapiTicketUrl = string.Empty;
                switch (configDto.WxType)
                {
                    case Enums.WeixinType.WxMp:
                        weixinJsapiTicketUrl = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", accessToken);
                        break;
                    case Enums.WeixinType.WxWork:
                    default:
                        weixinJsapiTicketUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/get_jsapi_ticket?access_token={0}", accessToken);
                        break;
                    case Enums.WeixinType.WxApp:
                        break;
                }

                var html = await HttpHelper.GetHtmlAsync(weixinJsapiTicketUrl);
                var jsapiTicketDto = html.FromJson<JsapiTicketResponseDTO>();
                if (jsapiTicketDto.errcode == 0)
                {
                    jsapiTicket = jsapiTicketDto.ticket;

                    var setResult = redisHelper.StringSet<string>(redisKey, jsapiTicketDto.ticket, TimeSpan.FromSeconds(jsapiTicketDto.expires_in * 1.0));
                }
            }
            return jsapiTicket;
        }
    }
}
