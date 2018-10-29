using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Request
{
    /// <summary>
    /// 图文消息
    /// </summary>
    public class RequestWorkNewsMsgDTO : RequestWorkContextMsgDTO
    {
        public RequestWorkNewsMsgDTO() : base(Enums.ResponseWorkMsgTypeEnum.Mpnews)
        {
            TextCard = new RequestWorkNewsItemMsgDTO();
        }

        [JsonProperty(PropertyName = "news")]
        public RequestWorkNewsItemMsgDTO TextCard { get; set; }
    }

    public class RequestWorkNewsItemMsgDTO
    {
        /// <summary>
        /// 图文消息，一个图文消息支持1到8条图文
        /// </summary>
        [JsonProperty(PropertyName = "articles")]
        public List<RequestWorkNewsItemDetailMsgDTO> Articles { get; set; }
    }

    public class RequestWorkNewsItemDetailMsgDTO
    {
        /// <summary>
        /// 标题，不超过128个字节，超过会自动截断
        /// </summary>
        public string title { get; set; }

        /// <summary>
        /// 描述，不超过512个字节，超过会自动截断
        /// </summary>
        public string description { get; set; }

        /// <summary>
        /// 点击后跳转的链接。
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图 640x320，小图80x80。
        /// </summary>
        public string picurl { get; set; }

        /// <summary>
        /// 按钮文字，仅在图文数为1条时才生效。 默认为“阅读全文”， 不超过4个文字，超过自动截断。该设置只在企业微信上生效，微工作台（原企业号）上不生效。
        /// </summary>
        public string btntxt { get; set; }
    }
}
