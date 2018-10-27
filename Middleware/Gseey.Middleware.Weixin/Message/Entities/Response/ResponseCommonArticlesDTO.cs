using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Response
{
    public class ResponseCommonArticlesDTO
    {
        /// <summary>
        /// 标题，不超过128个字节，超过会自动截断
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 描述，不超过512个字节，超过会自动截断
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 图文消息的图片链接，支持JPG、PNG格式，较好的效果为大图640x320，小图80x80。
        /// </summary>
        public string PicUrl { get; set; }

        /// <summary>
        /// 点击后跳转的链接。
        /// </summary>
        public string Url { get; set; }
    }
}
