﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Menu.DTOs
{
    /// <summary>
    /// 子菜单
    /// </summary>
    public class SubButton : BaseButton, IBaseButton
    {
        /// <summary>
        /// 子按钮数组，按钮个数应为2~5个
        /// </summary>
        public List<SingleButton> sub_button { get; set; }

        public SubButton()
        {
            sub_button = new List<SingleButton>();
        }

        public SubButton(string name) : this()
        {
            base.name = name;
        }
    }
}
