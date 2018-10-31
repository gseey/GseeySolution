using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gseey.Framework.Common.Helpers;
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
            var dbType = string.Empty;
            try
            {
                dbType = ConfigHelper.Get("Gseey:Connections:DbConnectionType");
            }
            catch (Exception ex)
            {
                ex.WriteExceptionLog("读取配置文件出错");
            }

            return Content("HttpGet" + "------" + dbType);
        }

        [HttpPost]
        public IActionResult Index()
        {
            return Content("HttpPost");
        }
    }
}