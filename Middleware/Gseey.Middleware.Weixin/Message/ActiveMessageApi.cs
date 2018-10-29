using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.BaseDTOs;
using Gseey.Middleware.Weixin.Helpers;
using Gseey.Middleware.Weixin.Media;
using Gseey.Middleware.Weixin.Message.Entities.Request;
using Gseey.Middleware.Weixin.Message.Entities.Response;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Message
{
    /// <summary>
    /// 主动发送消息api
    /// </summary>
    public class ActiveMessageApi
    {
        /// <summary>
        /// 企业号发送应用内容消息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static async Task<ResponseWorkMsgDTO> SendWorkAgentContentMsgAsync<TMessage>(int channelId, TMessage msgDto) where TMessage : RequestWorkContextMsgDTO
        {
            var validateResult = WeixinConfigHelper.ValidateWorkChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                if (msgDto.Validate())
                {
                    msgDto.AgentId = configDto.AgentId;

                    var sendMsgUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", configDto.AccessToken);

                    var result = await HttpHelper.PostDataAsync<ResponseWorkMsgDTO, TMessage>(sendMsgUrl, msgDto);
                    return result;
                }
                else
                {
                    return new ResponseWorkMsgDTO
                    {
                        errcode = -9998,
                        errmsg = "请选择接收消息的用户范围"
                    };
                }
            }
            else
            {
                return new ResponseWorkMsgDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行主动发送消息"
                };
            }
        }


        /// <summary>
        /// 企业号发送应用媒体消息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        public static async Task<ResponseWorkMsgDTO> SendWorkAgentMediaMsgAsync<TMessage>(int channelId, TMessage msgDto) where TMessage : RequestWorkMediaMsgDTO
        {
            var validateResult = WeixinConfigHelper.ValidateWorkChannel(channelId, out WeixinConfigDTO configDto);
            if (validateResult)
            {
                if (msgDto.Validate())
                {
                    msgDto.AgentId = configDto.AgentId;

                    var sendMsgUrl = string.Format("https://qyapi.weixin.qq.com/cgi-bin/message/send?access_token={0}", configDto.AccessToken);

                    if (msgDto is RequestWorkFileMsgDTO)//文件消息
                    {
                        var mediaUploadResult = MediaApi.UploadFile(channelId, UploadMediaFileType.file, msgDto.FilePath);

                        var mediaDto = msgDto as RequestWorkFileMsgDTO;
                        mediaDto.FileInfo.MeidaId = mediaUploadResult.media_id;
                        var result = await HttpHelper.PostDataAsync<ResponseWorkMsgDTO, RequestWorkFileMsgDTO>(sendMsgUrl, mediaDto);
                        return result;
                    }
                    if (msgDto is RequestWorkImageMsgDTO)//图片消息
                    {
                        var mediaUploadResult = MediaApi.UploadFile(channelId, UploadMediaFileType.image, msgDto.FilePath);

                        var mediaDto = msgDto as RequestWorkImageMsgDTO;
                        mediaDto.Image.MeidaId= mediaUploadResult.media_id;
                        var result = await HttpHelper.PostDataAsync<ResponseWorkMsgDTO, RequestWorkImageMsgDTO>(sendMsgUrl, mediaDto);
                        return result;
                    }
                    if (msgDto is RequestWorkVideoMsgDTO)//视频消息
                    {
                        var mediaUploadResult = MediaApi.UploadFile(channelId, UploadMediaFileType.image, msgDto.FilePath);

                        var mediaDto = msgDto as RequestWorkVideoMsgDTO;
                        mediaDto.VideoInfo.MeidaId = mediaUploadResult.media_id;
                        var result = await HttpHelper.PostDataAsync<ResponseWorkMsgDTO, RequestWorkVideoMsgDTO>(sendMsgUrl, mediaDto);
                        return result;
                    }
                    if (msgDto is RequestWorkVoiceMsgDTO)//语音消息
                    {
                        var mediaUploadResult = MediaApi.UploadFile(channelId, UploadMediaFileType.image, msgDto.FilePath);

                        var mediaDto = msgDto as RequestWorkVoiceMsgDTO;
                        mediaDto.VoiceInfo.MeidaId = mediaUploadResult.media_id;
                        var result = await HttpHelper.PostDataAsync<ResponseWorkMsgDTO, RequestWorkVoiceMsgDTO>(sendMsgUrl, mediaDto);
                        return result;
                    }
                    return new ResponseWorkMsgDTO
                    {
                        errcode = -9997,
                        errmsg = "请选择接收消息类型"
                    };
                }
                else
                {
                    return new ResponseWorkMsgDTO
                    {
                        errcode = -9998,
                        errmsg = "请选择接收消息的用户范围"
                    };
                }
            }
            else
            {
                return new ResponseWorkMsgDTO
                {
                    errcode = -9999,
                    errmsg = "仅企业号才可进行主动发送消息"
                };
            }
        }
    }
}
