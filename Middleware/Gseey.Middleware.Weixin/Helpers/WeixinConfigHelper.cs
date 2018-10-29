using Gseey.Framework.DataBase.DalBase;
using Gseey.Middleware.Weixin.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Helpers
{
    /// <summary>
    /// 微信配置帮助类
    /// </summary>
    internal class WeixinConfigHelper
    {
        #region 获取微信基本配置信息

        /// <summary>
        /// 获取微信基本配置信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static async Task<WeixinConfigDTO> GetWeixinConfigDTOAsync(int channelId)
        {
            var configDto = new WeixinConfigDTO();
            var weixinDal = new DapperDALBase<WeixinConfigEntity>();
            var weixinConfigEntities = await weixinDal.QueryListAsync(new { ChannelId = channelId });
            if (weixinConfigEntities != null
                && weixinConfigEntities.Count() > 0
                && weixinConfigEntities.SingleOrDefault().ChannelId > 0)
            {
                configDto.AgentId = weixinConfigEntities.SingleOrDefault().AgentId;
                configDto.AppId = weixinConfigEntities.SingleOrDefault().AppId;
                configDto.AppSercet = weixinConfigEntities.SingleOrDefault().AppSercet;
                configDto.ChannelId = weixinConfigEntities.SingleOrDefault().ChannelId;
                configDto.EncodingAESKey = weixinConfigEntities.SingleOrDefault().EncodingAESKey;
                configDto.Token = weixinConfigEntities.SingleOrDefault().Token;

                return configDto;
            }

            //这里从数据库中读取当前渠道的配置信息
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
            else if (channelId == 3)
            {
                configDto.ChannelId = 2;
                configDto.AppId = "wx9a80f6e6ed2a89e6";
                configDto.AppSercet = "KOSGE5-MGw2aLlc5X7jFClkHYoV77umBMi1LyHnt8tw";
                configDto.WxType = Enums.WeixinType.WxWork;
                configDto.AgentId = 15;
                configDto.EncodingAESKey = "OFhDh1eyfv8GYBluexZUAeLJ5gBx9yoykW45jfGX0Uu";
                configDto.Token = "Gscoy";
            }


            return configDto;
        }

        #endregion

        #region 验证渠道信息

        /// <summary>
        /// 验证渠道信息
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="configDto"></param>
        /// <returns></returns>
        internal static bool ValidateWorkChannel(int channelId, out WeixinConfigDTO configDto)
        {
            configDto = GetWeixinConfigDTOAsync(channelId).Result;
            var result = configDto.WxType == Enums.WeixinType.WxWork;
            return result;
        }

        #endregion
    }
}
