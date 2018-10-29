using Gseey.Framework.BaseDTO;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.Enums;
using Gseey.Middleware.Weixin.Helpers;
using Gseey.Middleware.Weixin.Keywords;
using Gseey.Middleware.Weixin.Message.Entities.Request;
using Gseey.Middleware.Weixin.Message.Entities.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gseey.Middleware.Weixin.Message
{
    /// <summary>
    /// 被动消息回复api
    /// </summary>
    public class PassiveMessageApi
    {
        /// <summary>
        /// 解析输入消息
        /// </summary>
        /// <param name="channelId">渠道消息</param>
        /// <param name="msg_signature">加密串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机串</param>
        /// <param name="inputMsg">加密消息</param>
        /// <returns></returns>
        public static async Task<string> ParseInputMsgAsync(int channelId, string msg_signature, string timestamp, string nonce, string inputMsg)
        {
            var encryptMsg = SignHelper.DecryptMsg(channelId, msg_signature, timestamp, nonce, inputMsg);

            var baseMessageDTO = ParseMessage(channelId, encryptMsg);

            var result = await KeywordHelper.GetCustomKeywordsReplyAsync<ExecuteResult<string>>(channelId, baseMessageDTO.ToUserName, baseMessageDTO.FromUserName, "");

            return result.Data;
        }

        //public static async Task<ExecuteResult<ResponseWorkBaseMsgDTO>> SendMsgAsync(int channelId, List<string> userIdList, List<string> partyIdList, List<string> tagIdList, ResponseWorkMsgTypeEnum msgType = ResponseWorkMsgTypeEnum.Text)
        //{
        //    var result = new ExecuteResult<ResponseWorkBaseMsgDTO> { };

        //    return result;
        //}

        private static RequestBaseMessageDTO ParseBaseMessage(int channelId, string encryptMsg, out XElement encryptXml)
        {
            RequestBaseMessageDTO baseMessageDTO = new RequestBaseMessageDTO();
            encryptXml = XElement.Parse(encryptMsg);
            var toUserName = encryptXml.Element("ToUserName").Value;
            var fromUserName = encryptXml.Element("FromUserName").Value;
            var createTime = DateTimeHelper.FromUnixTime(encryptXml.Element("CreateTime").Value.ToLong());
            var msgType = encryptXml.Element("MsgType").Value;
            var agentId = encryptXml.Element("AgentID") == null ?
                "-1" :
                encryptXml.Element("AgentID").Value;

            baseMessageDTO.ToUserName = toUserName;
            baseMessageDTO.FromUserName = fromUserName;
            baseMessageDTO.CreateTime = createTime;
            baseMessageDTO.AgentID = agentId.ToInt();
            baseMessageDTO.MsgType = msgType;
            if (msgType == "event")
            {
                var eventBaseMessageDTO = new RequestEventBaseMessageDTO
                {
                    ToUserName = baseMessageDTO.ToUserName,
                    FromUserName = baseMessageDTO.FromUserName,
                    CreateTime = baseMessageDTO.CreateTime,
                    AgentID = baseMessageDTO.AgentID,
                    MsgType = baseMessageDTO.MsgType,
                    Event = encryptXml.Element("Event").Value
                };
                return eventBaseMessageDTO;
            }

            return baseMessageDTO;
        }

        /// <summary>
        /// 解析消息
        /// </summary>
        /// <param name="encryptMsg"></param>
        /// <returns></returns>
        private static RequestBaseMessageDTO ParseMessage(int channelId, string encryptMsg)
        {
            var baseMessageDTO = ParseBaseMessage(channelId, encryptMsg, out XElement encryptXml);

            switch (baseMessageDTO.MsgType)
            {
                case "text"://文本消息
                    {
                        var textMessageDTO = new RequestTextMessageDTO
                        {
                            ToUserName = baseMessageDTO.ToUserName,
                            FromUserName = baseMessageDTO.FromUserName,
                            CreateTime = baseMessageDTO.CreateTime,
                            AgentID = baseMessageDTO.AgentID,
                            MsgType = baseMessageDTO.MsgType,
                        };
                        textMessageDTO.Content = encryptXml.Element("Content").Value;
                        textMessageDTO.MsgId = encryptXml.Element("MsgId").Value.ToLong();
                    }
                    break;
                case "image"://图片消息

                case "voice"://语音消息

                case "video"://视频消息

                case "location"://位置消息

                case "link"://链接消息

                default:
                    break;

                case "event"://事件消息
                    {
                        var eventType = ((RequestEventBaseMessageDTO)baseMessageDTO).Event;

                        switch (eventType)
                        {
                            #region 通用事件
                            case "subscribe"://成员关注事件
                                #region 公众号事件

                                #endregion
                                break;
                            case "unsubscribe"://成员取消关注事件
                                break;
                            case "click"://点击菜单拉取消息的事件推送
                                break;
                            case "view"://点击菜单跳转链接的事件推送
                                break;
                            case "scancode_push"://扫码推事件的事件推送
                                break;
                            case "scancode_waitmsg"://扫码推事件且弹出“消息接收中”提示框的事件推送
                                break;
                            case "pic_sysphoto"://弹出系统拍照发图的事件推送
                                break;
                            case "pic_photo_or_album"://弹出拍照或者相册发图的事件推送
                                break;
                            case "pic_weixin"://弹出微信相册发图器的事件推送
                                break;
                            case "location_select"://弹出地理位置选择器的事件推送 
                                break;
                            #endregion

                            #region 企业号事件
                            case "enter_agent"://本事件在成员进入企业微信的应用时触发
                                break;
                            case "LOCATION"://成员同意上报地理位置后，每次在进入应用会话时都会上报一次地理位置。企业可以在管理端修改应用是否需要获取地理位置权限。
                                break;
                                #endregion
                        }
                    }
                    break;
            }

            return baseMessageDTO;
        }
    }
}
