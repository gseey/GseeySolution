using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.WeixinQy;
using Microsoft.AspNetCore.Mvc;

namespace Gseey.Apis.Weixin.Controllers
{
    [Route("api/Weixin")]
    [ApiController]
    public class WeixinController : ControllerBase
    {
        public async Task<ActionResult<IEnumerable<string>>> IndexAsync()
        {
            var result = await AgentHelper.GetAgentConfigDTOAsync(1);

            return Content(result.ToJson());
        }
    }
}