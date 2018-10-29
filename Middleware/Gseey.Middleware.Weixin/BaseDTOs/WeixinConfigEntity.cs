using Gseey.Framework.DataBase.Attributes;
using Gseey.Framework.DataBase.EntityBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.BaseDTOs
{
    [Table(Name = "WeixinConfig")]
    public class WeixinConfigEntity : DapperEntityBase
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
        public int WxType { get; set; }
    }
}
