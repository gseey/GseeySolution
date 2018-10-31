using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gseey.Framework.Common.Helpers;
using Gseey.Middleware.Weixin.Menu.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace Gseey.Apis.Weixin.Controllers.Weixin
{
    /// <summary>
    /// 后台管理模块
    /// </summary>
    [Route("manage")]
    [ApiController]
    public class ManageController : ControllerBase
    {
        #region 菜单

        /// <summary>
        /// 创建微信菜单
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="buttonGroup"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("menu/create/{channelId}")]
        public IActionResult MenuCreate(int channelId, [FromBody]ButtonGroup buttonGroup)
        {
            return Content("MenuCreate");
        }

        /// <summary>
        /// 获取微信菜单
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("menu/get/{channelId}")]
        public IActionResult MenuGet(int channelId)
        {
            return Content("MenuGet");
        }

        /// <summary>
        /// 删除微信菜单
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("menu/delete/{channelId}")]
        public IActionResult MenuDelete(int channelId)
        {
            return Content("MenuDelete");
        }

        #endregion

        #region 部门/成员/标签管理

        #region 部门

        #endregion

        #endregion

        #region 发送主动消息

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("send/text/{channelId}")]
        public IActionResult SendTextMsg(int channelId)
        {
            return Content("SendTextMsg");
        }

        /// <summary>
        /// 发送媒体消息
        /// </summary>
        /// <param name="channelId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("send/media/{channelId}")]
        public IActionResult SendMediaMsg(int channelId)
        {
            return Content("SendMediaMsg");
        }

        #endregion
    }
}