using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Request
{
    /// <summary>
    /// 事件类型消息基类
    /// </summary>
    public class RequestEventBaseMessageDTO : RequestBaseMessageDTO
    {
        /// <summary>
        /// 事件类型
        /// </summary>
        public string Event { get; set; }
    }
}
