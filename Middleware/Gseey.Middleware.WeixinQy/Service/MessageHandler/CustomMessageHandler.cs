using Senparc.NeuChar.Context;
using Senparc.NeuChar.Helpers;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.MessageHandlers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

namespace Gseey.Middleware.WeixinQy.Service.MessageHandler
{
    public class CustomMessageHandler : WorkMessageHandler<MessageContext<IWorkRequestMessageBase, IWorkResponseMessageBase>>
    {
        public CustomMessageHandler(XDocument requestDoc, PostModel postModel, int maxRecordCount = 0)
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
