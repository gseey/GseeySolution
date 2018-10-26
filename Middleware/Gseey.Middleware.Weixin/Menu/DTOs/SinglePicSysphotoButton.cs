using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Menu.DTOs
{
    /// <summary>
    /// 单个按键
    /// </summary>
    public class SinglePicSysphotoButton : SingleButton
    {
        /// <summary>
        /// 类型为pic_sysphoto时必须。
        /// 用户点击按钮后，微信客户端将调起系统相机，完成拍照操作后，会将拍摄的相片发送给开发者，并推送事件给开发者，同时收起系统相机，随后可能会收到开发者下发的消息。
        /// </summary>
        public string key { get; set; }

        public SinglePicSysphotoButton()
            : base(MenuButtonType.pic_sysphoto.ToString())
        {
        }
    }
}
