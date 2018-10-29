using Gseey.Middleware.Weixin.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Request
{
    /// <summary>
    /// 语音消息
    /// </summary>
    public class RequestWorkVoiceMsgDTO : RequestWorkMediaMsgDTO
    {
        public RequestWorkVoiceMsgDTO() : base(ResponseWorkMsgTypeEnum.Voice)
        {
            VoiceInfo = new RequestWorkMediaItemDTO();
        }


        [JsonProperty(PropertyName = "voice")]
        public RequestWorkMediaItemDTO VoiceInfo { get; set; }
    }
}
