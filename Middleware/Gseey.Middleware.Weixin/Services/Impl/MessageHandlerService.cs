using Gseey.Framework.BaseDTO;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.Helpers;
using Gseey.Middleware.Weixin.Message;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Services.Impl
{
    public class MessageHandlerService : IMessageHandlerService
    {
        /// <summary>
        /// 获取相应消息
        /// </summary>
        /// <param name="channelId">渠道消息</param>
        /// <param name="msg_signature">加密串</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机串</param>
        /// <param name="inputMsg">加密消息</param>
        /// <returns></returns>
        public async Task<string> GetResponseAsync(int channelId, string msg_signature, string timestamp, string nonce, string inputMsg)
        {
            var result = await PassiveMessageApi.ParseInputMsgAsync(channelId, msg_signature, timestamp, nonce, inputMsg);
            return result;
        }



        /// <summary>
        /// 校验渠道微信签名
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <returns></returns>
        public ExecuteResult<Tuple<bool, string>> CheckChannelWeixinSign(int channelId, string msg_signature, string signature, string timestamp, string nonce, string echo)
        {
            var result = new ExecuteResult<Tuple<bool, string>>
            {
                ErrorCode = ExecuteResult.ErrorCodeEnum.Fail,
                Success = false,
                ErrorMsg = "校验渠道微信签名失败"
            };
            try
            {
                var checkResult = false;
                var replyEcho = string.Empty;
                var configDto = WeixinConfigHelper.GetWeixinConfigDTOAsync(channelId).Result;
                switch (configDto.WxType)
                {
                    case Enums.WeixinType.WxMp:
                        {
                            checkResult = SignHelper.CheckSignature(channelId, signature, timestamp, nonce, echo, out replyEcho);
                        }
                        break;
                    case Enums.WeixinType.WxWork:
                    default:
                        {
                            checkResult = SignHelper.ValidateUrl(channelId, msg_signature, timestamp, nonce, echo, out replyEcho);
                        }
                        break;
                }
                result.Data = new Tuple<bool, string>(checkResult, replyEcho);
                result.Success = true;
                result.ErrorCode = ExecuteResult.ErrorCodeEnum.Success;
                result.ErrorMsg = "校验渠道微信签名成功";
                return result;
            }
            catch (Exception ex)
            {
                ex.WriteExceptionLog("校验渠道微信签名失败");
                throw ex;
            }
            return result;
        }
    }
}
