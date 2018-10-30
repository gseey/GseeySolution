using Exceptionless;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Apis.Weixin.Controllers.Weixin
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/weixin/Index")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        #region 构造函数

        private readonly IMessageHandlerService _messageHandlerService;
        private readonly ILogger<IndexController> _logger;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="messageHandlerService"></param>
        /// <param name="logger"></param>
        public IndexController(IMessageHandlerService messageHandlerService, ILogger<IndexController> logger)
        {
            _messageHandlerService = messageHandlerService;
            _logger = logger;
        }

        #endregion

        #region 验证微信接口有效性
        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url
        /// </summary>
        [HttpGet]
        public IActionResult Index(int channelId, string msg_signature, string signature, string timestamp, string nonce, string echostr)
        {
            _logger.LogError("fsdfsdfsfsfsd");
            var ex = new Exception("fsfsfsd");
            ex.ToExceptionless().Submit();

            //校验微信签名
            var checkResult = _messageHandlerService.CheckChannelWeixinSign(channelId, msg_signature, signature, timestamp, nonce, echostr);
            if (checkResult.Success)
                return Content(checkResult.Data.Item2);
            else
                return Content(Guid.NewGuid().ToString());
        }
        #endregion

        #region 接收微信消息

        /// <summary>
        /// 微信事件处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Index(int channelId, string msg_signature, string timestamp, string nonce)
        {
            //获取推送过来的消息
            var msg = string.Empty;
            using (Stream stream = HttpContext.Request.Body)
            {
                byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);
                msg = Encoding.UTF8.GetString(buffer);
            }

            var result = _messageHandlerService.GetResponseAsync(channelId, msg_signature, timestamp, nonce, msg).Result;
            //var result = await _channelService.HandleInputWeixinQyMessageAsync(channelId, msg_signature, timestamp, nonce, msg);

            return Content(result);
        }
        #endregion

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