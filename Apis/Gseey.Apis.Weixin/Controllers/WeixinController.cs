using Gseey.Middleware.WeixinQy;
using Gseey.Middleware.WeixinQy.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Gseey.Apis.Weixin.Controllers
{
    [Route("api/Weixin")]
    [ApiController]
    public class WeixinController : ControllerBase
    {
        IChannelConfigService _channelService;

        public WeixinController(IChannelConfigService configService)
        {
            _channelService = configService;
        }
        /// <summary>
        /// 微信后台验证地址（使用Get），微信后台的“接口配置信息”的Url
        /// </summary>
        [HttpGet]
        //[ActionName("QyIndex")]
        public async Task<ActionResult<string>> QyIndexAsync(int channelId, string signature, string timestamp, string nonce, string echostr)
        {
            var configDTO = await _channelService.GetAgentConfigDTOByChannelIdAsync(channelId);
            //var sign = new Signature(configDTO);
            //var checkResult = sign.CheckSign(signature, timestamp, nonce);
            //if (checkResult)
            //    return Content(echostr);
            //else
                return Content("");
        }
    }
}