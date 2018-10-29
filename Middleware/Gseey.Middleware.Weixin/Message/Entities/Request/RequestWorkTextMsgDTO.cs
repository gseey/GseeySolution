using Gseey.Middleware.Weixin.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Request
{

    public class RequestWorkContextMsgDTO : RequestWorkBaseMsgDTO
    {
        public RequestWorkContextMsgDTO(ResponseWorkMsgTypeEnum msgTypeEnum) : base(msgTypeEnum)
        {
        }
    }

    /// <summary>
    /// 文本消息
    /// </summary>
    public class RequestWorkTextMsgDTO : RequestWorkContextMsgDTO
    {
        [JsonProperty(PropertyName = "text")]
        public RequestWorkTextContentDTO TextConent { get; set; }


        public RequestWorkTextMsgDTO() : base(ResponseWorkMsgTypeEnum.Text)
        {
            TextConent = new RequestWorkTextContentDTO();
        }
    }

    public class RequestWorkTextContentDTO
    {
        /// <summary>
        /// 消息内容，最长不超过2048个字节，超过将截断
        /// </summary>
        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }
}
