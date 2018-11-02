using Gseey.Framework.Common.Attributes;
using Gseey.Middleware.Weixin.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Apis.Weixin.Controllers
{

    /// <summary>
    /// 验证/被动回复消息
    /// </summary>
    [Produces("application/json")]
    [Route("/")]
    [ApiController]
    public class IndexController : ControllerBase
    {
        /// <summary>
        /// Defines the _messageHandlerService
        /// </summary>
        private readonly IMessageHandlerService _messageHandlerService;

        /// <summary>
        /// Defines the _logger
        /// </summary>
        private readonly ILogger<IndexController> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="IndexController"/> class.
        /// </summary>
        /// <param name="messageHandlerService"></param>
        /// <param name="logger"></param>
        public IndexController(IMessageHandlerService messageHandlerService, ILogger<IndexController> logger)
        {
            _messageHandlerService = messageHandlerService;
            _logger = logger;
        }

        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url
        /// </summary>
        /// <param name="channelId">渠道id</param>
        /// <param name="msg_signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <returns>The <see cref="IActionResult"/></returns>
        [HttpGet]
        [Route("index/{channelId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Index(int channelId, string msg_signature, string signature, string timestamp, string nonce, string echostr)
        {

            _logger.LogError("fdsfsfsdfs");

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
        /// <param name="channelId">渠道id</param>
        /// <param name="msg_signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <returns></returns>
        [HttpPost]
        [Route("index/{channelId}")]
        public async Task<IActionResult> IndexAsync(int channelId, string msg_signature, string signature, string timestamp, string nonce)
        {
            //获取推送过来的消息
            var msg = string.Empty;
            using (Stream stream = HttpContext.Request.Body)
            {
                byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);
                msg = Encoding.UTF8.GetString(buffer);
            }

            var signStr = string.IsNullOrEmpty(msg_signature) ? signature : msg_signature;

            var result = await _messageHandlerService.GetResponseAsync(channelId, signStr, timestamp, nonce, msg);

            return Content(result);
        }
    }
}
