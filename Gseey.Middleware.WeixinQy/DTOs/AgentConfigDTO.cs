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
        /// 应用密钥
        /// </summary>
        public string Sercet { get; set; }

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
