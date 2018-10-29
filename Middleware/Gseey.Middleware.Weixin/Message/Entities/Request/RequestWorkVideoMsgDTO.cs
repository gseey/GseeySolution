using Gseey.Middleware.Weixin.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Request
{
    /// <summary>
    /// 视频消息
    /// </summary>
    public class RequestWorkVideoMsgDTO : RequestWorkMediaMsgDTO
    {
        public RequestWorkVideoMsgDTO() : base(ResponseWorkMsgTypeEnum.Video)
        {
            VideoInfo = new RequestWorkVideoItemDTO();
        }


        [JsonProperty(PropertyName = "video")]
        public RequestWorkVideoItemDTO VideoInfo { get; set; }
    }

    public class RequestWorkVideoItemDTO : RequestWorkMediaItemDTO
    {
        /// <summary>
        /// 视频消息的标题，不超过128个字节，超过会自动截断
        /// </summary>
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        /// <summary>
        /// 视频消息的描述，不超过512个字节，超过会自动截断
        /// </summary>
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
    }
}
