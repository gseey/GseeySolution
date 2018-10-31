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
    /// 验证/被动回复消息
    /// </summary>
    [Produces("application/json")]
    [Route("/")]
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
        /// <param name="channelId">渠道id</param>
        /// <param name="msg_signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="signature">微信加密签名，signature结合了开发者填写的token参数和请求中的timestamp参数、nonce参数。</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>   
        /// <returns>
        /// /index/1?msg_signature=634d15b89eac28a4971bcd0cad3e93c9b812d215&timestamp=1540396364&nonce=1541329218
        /// </returns>
        [HttpGet]
        [Route("index/{channelId}")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public IActionResult Index(int channelId, string msg_signature, string signature, string timestamp, string nonce, string echostr)
        {
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
        [Route("index/{channelId}")]
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

            return Content(result);
        }
        #endregion
    }
}