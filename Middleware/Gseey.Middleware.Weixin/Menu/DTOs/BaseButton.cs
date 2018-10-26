using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Menu.DTOs
{
    public interface IBaseButton
    {
        string name { get; set; }
    }

    /// <summary>
    /// 所有按钮基类
    /// </summary>
    public class BaseButton : IBaseButton
    {
        /// <summary>
        /// 按钮描述，既按钮名字，不超过16个字节，子菜单不超过40个字节
        /// </summary>
        public string name { get; set; }
    }
}
