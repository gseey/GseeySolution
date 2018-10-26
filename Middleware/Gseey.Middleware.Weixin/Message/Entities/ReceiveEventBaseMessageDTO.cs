using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities
{
    /// <summary>
    /// 事件类型消息基类
    /// </summary>
    public class ReceiveEventBaseMessageDTO : ReceiveBaseMessageDTO
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public string Event { get; set; }
    }
}
