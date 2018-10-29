using Gseey.Middleware.Weixin.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Request
{
    public class RequestWorkMediaMsgDTO : RequestWorkBaseMsgDTO
    {
        /// <summary>
        /// 文件路径
        /// </summary>
        [JsonIgnore]
        public string FilePath { get; set; }

        public RequestWorkMediaMsgDTO(ResponseWorkMsgTypeEnum msgTypeEnum) : base(msgTypeEnum)
        {
        }

        public override bool Validate()
        {
            if (UserIdList == null
                 && PartyIdList == null
                 && TagIdList == null
                 )//校验用户是否为空
            {
                return false;
            }
            if (UserIdList.Count <= 0
                    && PartyIdList.Count <= 0
                    && TagIdList.Count <= 0
                    )//校验是否有用户
            {
                return false;
            }
            if (!File.Exists(FilePath))//文件路径不存在
            {
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// 图片消息
    /// </summary>
    public class RequestWorkImageMsgDTO : RequestWorkMediaMsgDTO
    {
        public RequestWorkImageMsgDTO() : base(ResponseWorkMsgTypeEnum.Image)
        {
            Image = new RequestWorkMediaItemDTO();
        }

        [JsonProperty(PropertyName = "image")]
        public RequestWorkMediaItemDTO Image { get; set; }
    }

    public class RequestWorkMediaItemDTO
    {
        /// <summary>
        /// 媒体文件id，可以调用上传临时素材接口获取
        /// </summary>
        [JsonProperty(PropertyName = "media_id")]
        public string MeidaId { get; internal set; }
    }
}
