using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Response
{
    /// <summary>
    /// 消息回复通用基类
    /// </summary>
    public class ResponseCommonBaseMsgDTO : ResponseBaseMsgDTO
    {
        /// <summary>
        /// 成员UserID
        /// 接收方帐号（收到的OpenID）
        /// </summary>
        public string ToUserName { get; set; }

        /// <summary>
        /// 企业微信CorpID
        /// 开发者微信号
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
    }
}
