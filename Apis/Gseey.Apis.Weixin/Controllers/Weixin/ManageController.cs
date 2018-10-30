using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Gseey.Apis.Weixin.Controllers.Weixin
{
    [Route("api/weixin/manage")]
    [ApiController]
    public class ManageController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index(int id)
        {
            return Content("HttpGet");
        }

        [HttpPost]
        public IActionResult Index()
        {
            return Content("HttpPost");
        }
    }
}