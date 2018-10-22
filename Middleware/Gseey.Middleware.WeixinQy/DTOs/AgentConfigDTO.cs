using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.WeixinQy.DTOs
{
    /// <summary>
    /// 应用配置信息
    /// </summary>
    public class AgentConfigDTO
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

        public AgentConfigDTO()
        {
            Id = 6;
            CorpId = "wx9a80f6e6ed2a89e6";
            CorpSercet = "KVZ_1nE2thZdbu3kcftpgA5Ld-O3TafmS3AlUtWQeHM";
            Token = "qygscoy";
            EncodingAESKey = "FDPibMBM3MExJxBaD9Oe6uaOfsvsjmTQT94f6tt2lJl";
        }
    }
}
