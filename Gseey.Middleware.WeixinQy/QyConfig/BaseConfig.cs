using Gseey.Framework.Common.Helpers;
using Senparc.CO2NET.Cache;
using Senparc.CO2NET.Cache.Redis;
using Senparc.Weixin.QY.CommonAPIs;
using Senparc.Weixin.QY.Containers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.WeixinQy.QyConfig
{
    /// <summary>
    /// 基本配置信息
    /// </summary>
    public class BaseConfig
    {
        #region 基本配置

        /// <summary>
        /// 企业号appid
        /// </summary>
        public string CorpId
        {
            get
            {
                return ConfigHelper.Get("GseeyWeixinConfig:QY:CorpId");
            }
        }

        /// <summary>
        /// 企业号appsercet
        /// </summary>
        public string CorpSecret { get; set; }

        /// <summary>
        /// redis连接串
        /// </summary>
        public string RedisConnection
        {
            get
            {
                return ConfigHelper.Get("GseeyConnections:RedisConnectionString");
            }
        }
        #endregion

        /// <summary>
        /// 初始化配置
        /// </summary>
        /// <param name="corpSecret">应用appsercet</param>
        /// <param name="useRedis">是否使用Reids</param>
        public BaseConfig(string corpSecret, bool useRedis = true)
        {
            CorpSecret = corpSecret;

            if (useRedis)
            {
                RedisManager.ConfigurationOption = RedisConnection;
                CacheStrategyFactory.RegisterObjectCacheStrategy(() => RedisObjectCacheStrategy.Instance);//Redis
            }
            //全局只需注册一次
            AccessTokenContainer.Register(CorpId, CorpSecret);
        }

        public string GetAccessToken()
        {
            var tokenResult = CommonApi.GetToken(CorpId, CorpSecret);
            return tokenResult.access_token;
        }

        public async Task<string> GetAccessTokenAsync()
        {
            var tokenResult = await CommonApi.GetTokenAsync(CorpId, CorpSecret);
            return tokenResult.access_token;
        }
    }
}
