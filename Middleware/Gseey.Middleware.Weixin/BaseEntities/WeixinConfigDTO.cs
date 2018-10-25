using Gseey.Middleware.Weixin.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.BaseEntities
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
        /// 微信类型
        /// </summary>
        public WeixinType WxType { get; set; }

    }
}
