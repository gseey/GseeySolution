using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Menu.DTOs
{
    /// <summary>
    /// 单个按键
    /// </summary>
    public class SingleLocationSelectButton : SingleButton
    {
        /// <summary>
        /// 类型为location_select时必须。
        /// 用户点击按钮后，微信客户端将调起地理位置选择工具，完成选择操作后，将选择的地理位置发送给开发者的服务器，同时收起位置选择工具，随后可能会收到开发者下发的消息。
        /// </summary>
        public string key { get; set; }

        public SingleLocationSelectButton()
            : base(MenuButtonType.location_select.ToString())
        {
        }
    }
}
