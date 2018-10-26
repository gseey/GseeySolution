using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities
{
    /// <summary>
    /// 消息基类
    /// </summary>
    public class ReceiveBaseMessageDTO
    {
        /// <summary>
        /// 企业微信CorpID/开发者微信号
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 成员UserID/发送方帐号（一个OpenID）
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// 消息创建时间（整型）
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string MsgType { get; set; }

        /// <summary>
        /// 企业应用的id，整型。
        /// </summary>
        public int AgentID { get; set; }
    }
}
