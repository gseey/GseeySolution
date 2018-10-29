using Gseey.Middleware.Weixin.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Request
{
    /// <summary>
    /// 企业主动发送消息基类
    /// </summary>
    public class RequestWorkBaseMsgDTO
    {
        public RequestWorkBaseMsgDTO(ResponseWorkMsgTypeEnum msgTypeEnum, int safe = 1)
        {
            MsgTypeEnum = msgTypeEnum;
            Safe = safe;
        }

        /// <summary>
        /// 成员ID列表（消息接收者，多个接收者用‘|’分隔，最多支持1000个）。
        /// 特殊情况：指定为@all，则向该企业应用的全部成员发送
        /// </summary>
        [JsonProperty(PropertyName = "touser")]
        public string ToUser
        {
            get
            {
                if (UserIdList != null)
                {
                    var result = string.Join("|", UserIdList.ToArray());
                    return result;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 部门ID列表，多个接收者用‘|’分隔，最多支持100个。
        /// 当touser为@all时忽略本参数
        /// </summary>
        [JsonProperty(PropertyName = "toparty")]
        public string ToParty
        {
            get
            {
                if (PartyIdList != null)
                {
                    var result = string.Join("|", PartyIdList.ToArray());
                    return result;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 标签ID列表，多个接收者用‘|’分隔，最多支持100个。
        /// 当touser为@all时忽略本参数
        /// </summary>
        [JsonProperty(PropertyName = "totag")]
        public string ToTag
        {
            get
            {
                if (TagIdList != null)
                {
                    var result = string.Join("|", TagIdList.ToArray());
                    return result;
                }
                return string.Empty;
            }
        }

        /// <summary>
        /// 消息类型
        /// </summary>
        [JsonProperty(PropertyName = "msgtype")]
        public string MsgType
        {
            get
            {
                return MsgTypeEnum.ToString().ToLower();
            }
        }

        /// <summary>
        /// 企业应用的id，整型。
        /// </summary>
        [JsonProperty(PropertyName = "agentid")]
        public int AgentId { get; internal set; }

        /// <summary>
        /// 表示是否是保密消息，0表示否，1表示是，默认0
        /// </summary>
        [JsonProperty(PropertyName = "safe")]
        public int Safe { get; }

        /// <summary>
        /// 用户id集合
        /// </summary>
        [JsonIgnore]
        public List<string> UserIdList { get; set; }

        /// <summary>
        /// 部门id集合
        /// </summary>
        [JsonIgnore]
        public List<string> PartyIdList { get; set; }

        /// <summary>
        /// 标签id集合
        /// </summary>
        [JsonIgnore]
        public List<string> TagIdList { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [JsonIgnore]
        public ResponseWorkMsgTypeEnum MsgTypeEnum { get; internal set; }

        /// <summary>
        /// 校验接收人
        /// </summary>
        /// <returns></returns>
        public virtual bool Validate()
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
            return true;
        }
    }
}
