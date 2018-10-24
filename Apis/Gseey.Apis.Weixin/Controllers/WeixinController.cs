﻿using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy;
using Gseey.Middleware.WeixinQy.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.HttpUtility;
using System;
using System.Threading.Tasks;

namespace Gseey.Apis.Weixin.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/Weixin")]
    [ApiController]
    public class WeixinController : ControllerBase
    {
        IChannelConfigService _channelService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configService"></param>
        public WeixinController(IChannelConfigService configService)
        {
            _channelService = configService;
        }
        /// <summary>
        /// 微信企业号后台验证地址（使用Get），微信后台的“接口配置信息”的Url
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> QyIndexAsync(int channelId, string msg_signature, string timestamp, string nonce, string echostr)
        {
            //校验微信签名
            var checkResult = await _channelService.CheckChannelWeixinQySignAsync(channelId, msg_signature, timestamp, nonce, echostr);
            if (checkResult.Success)
                return Content(checkResult.Data.Item2);
            else
                return Content(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// 微信企业号事件处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> QyIndexAsync(int channelId)
        {
            try
            {
                var stream = Request.GetRequestMemoryStream();

                var result = await _channelService.HandleInputWeixinQyMessageAsync(channelId, stream);
                if (result.Success)
                {
                    return Content(result.Data.ResponseDocument.ToString());
                }
            }
            catch (Exception ex)
            {
                ex.WriteExceptionLog("微信企业号事件处理出错");
            }

            return Content(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Test()
        {
            return Content(Guid.NewGuid().ToString());
        }
    }
}