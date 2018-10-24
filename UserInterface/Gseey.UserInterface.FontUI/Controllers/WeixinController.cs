using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Senparc.CO2NET.HttpUtility;

namespace Gseey.UserInterface.FontUI.Controllers
{
    public class WeixinController : Controller
    {
        #region 构造函数

        IChannelConfigService _channelService;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configService"></param>
        public WeixinController(IChannelConfigService configService)
        {
            _channelService = configService;
        }

        #endregion

        #region 公众号/订阅号

        #endregion

        #region 企业号

        /// <summary>
        /// 微信企业号后台验证地址（使用Get），微信后台的“接口配置信息”的Url
        /// </summary>
        [HttpGet]
        [ActionName("QyIndex")]
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
        [ActionName("QyIndex")]
        public async Task<IActionResult> QyIndexAsync(int channelId, string msg_signature, string timestamp, string nonce)
        {
            //获取企业号推送过来的消息
            var msg = string.Empty;
            using (Stream stream = HttpContext.Request.Body)
            {
                byte[] buffer = new byte[HttpContext.Request.ContentLength.Value];
                stream.Read(buffer, 0, buffer.Length);
                msg = Encoding.UTF8.GetString(buffer);
            }

            var result = await _channelService.HandleInputWeixinQyMessageAsync(channelId, msg_signature, timestamp, nonce, msg);

            return Content(result.Data);
        }
        #endregion

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return Content("fsfsfsd");
        }
    }
}