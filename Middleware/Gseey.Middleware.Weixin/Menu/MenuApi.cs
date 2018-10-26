using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.BaseDTOs;
using Gseey.Middleware.Weixin.Helpers;
using Gseey.Middleware.Weixin.Menu.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Menu
{
    public class MenuApi
    {
        #region 创建菜单

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="buttonData">菜单内容</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> CreateMenuAsync(int channelId, ButtonGroup buttonData)
        {
            var configDto = WeixinConfigHelper.GetWeixinConfigDTO(channelId);
            var weixinCreateMenuUrl = string.Empty;
            switch (configDto.WxType)
            {
                case Enums.WeixinType.WxMp:
                    weixinCreateMenuUrl = string.Format("https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}", configDto.AccessToken);
                    break;
                case Enums.WeixinType.WxWork:
                default:
                    weixinCreateMenuUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/create?access_token={0}&agentid={1}", configDto.AccessToken, configDto.AgentId);
                    break;
            }

            var result = await HttpHelper.PostDataAsync<ResponseBaseDTO, ButtonGroup>(weixinCreateMenuUrl, buttonData);
            return result;
        }

        #endregion

        #region 获取菜单

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="buttonData">菜单内容</param>
        /// <returns></returns>
        public static async Task<ButtonGroup> GetMenuAsync(int channelId)
        {
            var configDto = WeixinConfigHelper.GetWeixinConfigDTO(channelId);
            var weixinGetMenuUrl = string.Empty;
            switch (configDto.WxType)
            {
                case Enums.WeixinType.WxMp:
                    weixinGetMenuUrl = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", configDto.AccessToken);
                    break;
                case Enums.WeixinType.WxWork:
                default:
                    weixinGetMenuUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/get?access_token={0}&agentid={1}", configDto.AccessToken, configDto.AgentId);
                    break;
            }

            var result = await HttpHelper.GetHtmlAsync<ButtonGroup>(weixinGetMenuUrl);
            return result;
        }

        #endregion


        #region 删除菜单


        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="buttonData">菜单内容</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> DeleteMenuAsync(int channelId)
        {
            var configDto = WeixinConfigHelper.GetWeixinConfigDTO(channelId);
            var weixinDeleteMenuUrl = string.Empty;
            switch (configDto.WxType)
            {
                case Enums.WeixinType.WxMp:
                    weixinDeleteMenuUrl = string.Format("https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}", configDto.AccessToken);
                    break;
                case Enums.WeixinType.WxWork:
                default:
                    weixinDeleteMenuUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/menu/delete?access_token={0}&agentid={1}", configDto.AccessToken, configDto.AgentId);
                    break;
            }

            var result = await HttpHelper.GetHtmlAsync<ResponseBaseDTO>(weixinDeleteMenuUrl);
            return result;
        }

        #endregion
    }
}
