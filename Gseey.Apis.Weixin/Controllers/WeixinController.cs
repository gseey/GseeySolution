using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gseey.Apis.Weixin.Controllers
{
    [Route("api/Weixin")]
    [ApiController]
    public class WeixinController : ControllerBase
    {
        public ActionResult<IEnumerable<string>> Index()
        {
            return Content("fsdfdsfsdfsdfsf");
        }
    }
}