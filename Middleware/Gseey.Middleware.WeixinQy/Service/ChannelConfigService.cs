using Gseey.Framework.BaseDTO;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.DTOs;
using Gseey.Middleware.WeixinQy.Entities;
using Gseey.Middleware.WeixinQy.Interfaces;
using Senparc.NeuChar.Context;
using Senparc.Weixin.Work.Entities;
using Senparc.Weixin.Work.MessageHandlers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

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
        /// <param name="inputStream"></param>
        /// <returns></returns>
        public Task<ExecuteResult<WorkMessageHandler<MessageContext<IWorkRequestMessageBase, IWorkResponseMessageBase>>>> HandleInputWeixinQyMessageAsync(int channelId, Stream inputStream)
        {
            throw new NotImplementedException();
        }

        #endregion


    }
}
