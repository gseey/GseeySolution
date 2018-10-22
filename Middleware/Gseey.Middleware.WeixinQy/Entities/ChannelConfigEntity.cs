using Gseey.Framework.DataBase.Attributes;
using Gseey.Framework.DataBase.EntityBase;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.WeixinQy.Entities
{
    [Table(Name = "ChannelConfig")]
    public class ChannelConfigEntity: DapperEntityBase
    {
        /// <summary>
        /// 应用id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 企业号appid
        /// </summary>
        public string CorpId { get; set; }

        /// <summary>
        /// 应用appsercet
        /// </summary>
        public string CorpSercet { get; set; }

        /// <summary>
        /// 应用token
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// 应用消息加密串
        /// </summary>
        public string EncodingAESKey { get; set; }
    }
}
