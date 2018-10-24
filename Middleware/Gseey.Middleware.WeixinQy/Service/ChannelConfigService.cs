using Gseey.Framework.BaseDTO;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.Interfaces;
using Gseey.Middleware.WeixinQy.Service.MessageHandler;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Gseey.Middleware.WeixinQy.Service
{
    internal class ChannelConfigService : IChannelConfigService
    {
        #region 校验渠道微信签名

        /// <summary>
        /// 校验渠道微信签名
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <returns></returns>
        public async Task<ExecuteResult<Tuple<bool, string>>> CheckChannelWeixinQySignAsync(int channelId, string signature, string timestamp, string nonce, string echo)
        {
            var result = new ExecuteResult<Tuple<bool, string>>
            {
                ErrorCode = ExecuteResult.ErrorCodeEnum.Fail,
                Success = false,
                ErrorMsg = "校验渠道微信签名失败"
            };
            try
            {
                var configDto = await ChannelHelper.GetAgentConfigDTOByChannelIdAsync(channelId);

                SignatureHelper signatureHelper = new SignatureHelper(configDto);

                var checkResult = signatureHelper.CheckSign(signature, timestamp, nonce, echo, out string replyEcho);


                result.Data = new Tuple<bool, string>(checkResult, replyEcho);
                result.Success = true;
                result.ErrorCode = ExecuteResult.ErrorCodeEnum.Success;
                result.ErrorMsg = "校验渠道微信签名成功";
                return result;
            }
            catch (Exception ex)
            {
                ex.WriteExceptionLog("校验渠道微信签名失败");
            }
            return result;
        }

        /// <summary>
        /// 处理微信消息
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="msg_signature"></param>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public async Task<ExecuteResult<string>> HandleInputWeixinQyMessageAsync(int channelId, string msg_signature, string timestamp, string nonce, string msg)
        {
            var result = new ExecuteResult<string>
            {
                Success = false,
                ErrorMsg = "",
                ErrorCode = ExecuteResult.ErrorCodeEnum.Fail,
            };

            try
            {

                var configDto = await ChannelHelper.GetAgentConfigDTOByChannelIdAsync(channelId);
                Senparc.Weixin.Work.Tencent.WXBizMsgCrypt crypt = new Senparc.Weixin.Work.Tencent.WXBizMsgCrypt(configDto.Token, configDto.EncodingAESKey, configDto.CorpId);
                var info = string.Empty;
                var r1 = crypt.DecryptMsg(msg_signature, timestamp, nonce, msg, ref info);

                var replyMsg = string.Empty;

                var reply = info.Replace("<ToUserName><![CDATA[wx9a80f6e6ed2a89e6]]></ToUserName><FromUserName><![CDATA[Yan-Xia]]></FromUserName>", "<ToUserName><![CDATA[Yan-Xia]]></ToUserName><FromUserName><![CDATA[wx9a80f6e6ed2a89e6]]></FromUserName>");

                crypt.EncryptMsg(reply, timestamp, nonce, ref replyMsg);

                result.Data = replyMsg;
                return result;

                var signHelper = new SignatureHelper(configDto);
                //signHelper.d

                var xmlInfo = XDocument.Parse(msg);
                var postModel = new Senparc.Weixin.Work.Entities.PostModel
                {
                    CorpId = configDto.CorpId,
                    EncodingAESKey = configDto.EncodingAESKey,
                    Msg_Signature = msg_signature,
                    Nonce = nonce,
                    Timestamp = timestamp,
                    Token = configDto.Token,

                    //Msg_Signature = "845997ceb6e4fd73edd9a377be227848ce20d34f",
                    //Timestamp = "1412587525",
                    //Nonce = "1501543730",

                    //Token = "fzBsmSaI8XE1OwBh",
                    //EncodingAESKey = "9J8CQ7iF9mLtQDZrUM1loOVQ6oNDxVtBi1DBU2oaewl",
                    //CorpId = "wx7618c0a6d9358622"
                };
                var handler = new CustomMessageHandler(xmlInfo, postModel, 10);
            }
            catch (Exception ex)
            {
                ex.WriteExceptionLog("");
            }


            return result;
        }

        #endregion


    }
}
