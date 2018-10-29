using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Request
{
    /// <summary>
    /// 文本卡片消息
    /// </summary>
    public class RequestWorkTextCardMsgDTO : RequestWorkContextMsgDTO
    {
        public RequestWorkTextCardMsgDTO() : base(Enums.ResponseWorkMsgTypeEnum.Textcard)
        {
            TextCard = new RequestWorkTextCardItemMsgDTO();
        }

        [JsonProperty(PropertyName = "textcard")]
        public RequestWorkTextCardItemMsgDTO TextCard { get; set; }
    }
    public class RequestWorkTextCardItemMsgDTO
    {
        /// <summary>
        /// 标题，不超过128个字节，超过会自动截断
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 描述，不超过512个字节，超过会自动截断
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        /// <summary>
        /// 点击后跳转的链接。
        /// </summary>
        [JsonProperty(PropertyName = "url")]
        public string Url { get; set; }

        /// <summary>
        /// 按钮文字。 默认为“详情”， 不超过4个文字，超过自动截断。
        /// </summary>
        [JsonProperty(PropertyName = "btntxt")]
        public string Btntxt { get; set; }
    }
}
