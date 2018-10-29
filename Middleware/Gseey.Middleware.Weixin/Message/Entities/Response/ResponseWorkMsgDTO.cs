using Gseey.Middleware.Weixin.BaseDTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Response
{
    /// <summary>
    /// 企业号主动发送消息返回内容
    /// </summary>
    public class ResponseWorkMsgDTO : ResponseBaseDTO
    {
        [JsonIgnore]
        public List<string> InvalidUserList { get; set; }

        [JsonIgnore]
        public List<string> InvalidPartyList { get; set; }

        [JsonIgnore]
        public List<string> InvalidTagList { get; set; }

        /// <summary>
        /// 非法用户
        /// </summary>
        [JsonProperty(PropertyName = "invaliduser")]
        public string InvalidUser
        {
            get
            {
                var result = string.Join("|", InvalidUserList.ToArray());
                return result;
            }
        }

        /// <summary>
        /// 非法部门
        /// </summary>
        [JsonProperty(PropertyName = "invalidparty")]
        public string InvalidParty
        {
            get
            {
                var result = string.Join("|", InvalidPartyList.ToArray());
                return result;
            }
        }

        /// <summary>
        /// 非法标签
        /// </summary>
        [JsonProperty(PropertyName = "invalidtag")]
        public string InvalidTag
        {
            get
            {
                var result = string.Join("|", InvalidTagList.ToArray());
                return result;
            }
        }
    }
}
