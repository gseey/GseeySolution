using Gseey.Middleware.Weixin.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Request
{
    /// <summary>
    /// 文件消息
    /// </summary>
    public class RequestWorkFileMsgDTO : RequestWorkMediaMsgDTO
    {
        public RequestWorkFileMsgDTO() : base(ResponseWorkMsgTypeEnum.File)
        {
            FileInfo = new RequestWorkMediaItemDTO();
        }


        [JsonProperty(PropertyName = "file")]
        public RequestWorkMediaItemDTO FileInfo { get; set; }
    }
}
