﻿using Gseey.Middleware.Weixin.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Message.Entities.Response
{
    /// <summary>
    /// 企业主动回复消息基类
    /// </summary>
    public class ResponseWorkBaseMsgDTO : ResponseBaseMsgDTO
    {
        /// <summary>
        /// 成员ID列表（消息接收者，多个接收者用‘|’分隔，最多支持1000个）。
        /// 特殊情况：指定为@all，则向该企业应用的全部成员发送
        /// </summary>
        [JsonProperty(PropertyName = "touser")]
        public string ToUser { get; set; }

        /// <summary>
        /// 部门ID列表，多个接收者用‘|’分隔，最多支持100个。
        /// 当touser为@all时忽略本参数
        /// </summary>
        [JsonProperty(PropertyName = "toparty")]
        public string ToParty { get; set; }

        /// <summary>
        /// 标签ID列表，多个接收者用‘|’分隔，最多支持100个。
        /// 当touser为@all时忽略本参数
        /// </summary>
        [JsonProperty(PropertyName = "totag")]
        public string ToTag { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        [JsonProperty(PropertyName = "msgtype")]
        public ResponseWorkMsgTypeEnum MsgType { get; }

        /// <summary>
        /// 企业应用的id，整型。
        /// </summary>
        [JsonProperty(PropertyName = "agentid")]
        public int AgentId { get; set; }

        /// <summary>
        /// 表示是否是保密消息，0表示否，1表示是，默认0
        /// </summary>
        [JsonProperty(PropertyName = "safe")]
        public int Safe { get; set; }
    }
}
