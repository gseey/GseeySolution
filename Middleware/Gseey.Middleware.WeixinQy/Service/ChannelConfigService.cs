using Gseey.Framework.BaseDTO;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.Interfaces;
using Gseey.Middleware.WeixinQy.Service.MessageHandler;
using Senparc.NeuChar.Context;
using Senparc.NeuChar.Helpers;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.MessageHandlers;
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

                var signHelper = new SignatureHelper(configDto);
                //signHelper.d
                string testXml = @"<xml><ToUserName><![CDATA[wx7618c0a6d9358622]]></ToUserName>
<Encrypt><![CDATA[h3z+AK9zKP4dYs8j1FmthAILbJghEmdo2Y1U9Pdghzann6H2KJOpepaDT1zcp09/1/e/6ta48aUXebkHlu0rhzk4GW+cvVUHzbEiQVFlIvD+q4T/NLIm8E8BM+gO+DHslM7aXmYjvgMw6AYiBx80D+nZKNyJD3I8lRT3aHCq/hez0c+HTAnZyuCi5TfUAw0c6jWSfAq61VesRw4lhV925vJUOBXT/zOw760CEsYXSr2IAr/n4aPfDgRs2Ww2h/HPiVOQ2Ms1f/BOtFiKVWMqZCxbmJ7cyPHH7+uOSAS6DtXiQAdwpEZwHz+A5QTsmK6V0C6Ifgr7zrStb7ygM7kmcrAJctPhCfG7WlfrWrFNLdtx9Q2F7d6/soinswdoYF8g56s8UWguOVkM7UFGr8H2QqrUJm5S5iFP/XNcBwvPWYA=]]></Encrypt>
<AgentID><![CDATA[2]]></AgentID>
</xml>";
                var postModel = new Senparc.Weixin.Work.Entities.PostModel()
                {
                    Msg_Signature = "845997ceb6e4fd73edd9a377be227848ce20d34f",
                    Timestamp = "1412587525",
                    Nonce = "1501543730",

                    Token = "fzBsmSaI8XE1OwBh",
                    EncodingAESKey = "9J8CQ7iF9mLtQDZrUM1loOVQ6oNDxVtBi1DBU2oaewl",
                    CorpId = "wx7618c0a6d9358622"
                };
                var messageHandler = new CustomMessageHandlers(XDocument.Parse(testXml), postModel, 10);

                var xmlInfo = XDocument.Parse(msg);
                var postModel1 = new Senparc.Weixin.Work.Entities.PostModel
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
                var handler = new CustomMessageHandler(xmlInfo, postModel1, 10);

                return result;
            }
            catch (Exception ex)
            {
                ex.WriteExceptionLog("");
            }


            return result;
        }

        #endregion


    }

    public class CustomMessageHandlers : WorkMessageHandler<MessageContext<IWorkRequestMessageBase, IWorkResponseMessageBase>>
    {
        public CustomMessageHandlers(XDocument requestDoc, PostModel postModel, int maxRecordCount = 0)
            : base(requestDoc, postModel, maxRecordCount)
        {
        }

        public override IWorkResponseMessageBase OnTextRequest(RequestMessageText requestMessage)
        {
            var responseMessage = RequestMessage.CreateResponseMessage<ResponseMessageText>();

            responseMessage.Content = "文字信息";
            return responseMessage;
        }


        /// <summary>
        /// 默认消息
        /// </summary>
        /// <param name="requestMessage"></param>
        /// <returns></returns>
        public override IWorkResponseMessageBase DefaultResponseMessage(IWorkRequestMessageBase requestMessage)
        {
            var responseMessage = this.CreateResponseMessage<ResponseMessageText>();
            responseMessage.Content = "这是一条默认消息。";
            return responseMessage;
        }
    }
}
