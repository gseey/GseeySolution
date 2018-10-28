using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.Services;
using Microsoft.AspNetCore.Mvc;

namespace Gseey.UserInterface.FontUI.Controllers
{
    public class WeixinController : Controller
    {
        #region 构造函数

        private readonly IMessageHandlerService _messageHandlerService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configService"></param>
        public WeixinController(IMessageHandlerService messageHandlerService)
        {
            _messageHandlerService = messageHandlerService;
        }

        #endregion

        #region 公众号/订阅号

        #endregion

        #region 

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url
        /// </summary>
        [HttpGet]
        [ActionName("Index")]
        public IActionResult Index(int channelId, string msg_signature, string signature, string timestamp, string nonce, string echostr)
        {
            //校验微信签名
            var checkResult = _messageHandlerService.CheckChannelWeixinSign(channelId, msg_signature, signature, timestamp, nonce, echostr);
            if (checkResult.Success)
                return Content(checkResult.Data.Item2);
            else
                return Content(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// 微信事件处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ActionName("Index")]
        public async Task<IActionResult> IndexAsync(int channelId, string msg_signature, string timestamp, string nonce)
        {
            //获取推送过来的消息
            var msg = string.Empty;
            using (Stream stream = HttpContext.Request.Body)
            {
                byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);
                msg = Encoding.UTF8.GetString(buffer);
            }

            var result = await _messageHandlerService.GetResponseAsync(channelId, msg_signature, timestamp, nonce, msg);
            //var result = await _channelService.HandleInputWeixinQyMessageAsync(channelId, msg_signature, timestamp, nonce, msg);

            return Content(result);
        }
        #endregion

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return Content("fsfsfsd");
        }
    }
}