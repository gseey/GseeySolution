using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities
{
    /// <summary>
    /// 文本消息
    /// </summary>
    public class ReceiveTextMessageDTO : ReceiveBaseMessageDTO
    {
        /// <summary>
        /// 文本消息内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 消息id，64位整型
        /// </summary>
        public long MsgId { get; set; }
    }
}
