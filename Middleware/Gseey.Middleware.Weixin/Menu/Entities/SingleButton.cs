﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Menu.Entities
{
    /// <summary>
    /// 所有单击按钮的基类（view，click等）
    /// </summary>
    public abstract class SingleButton : BaseButton, IBaseButton
    {
        /// <summary>
        /// 按钮类型（click或view）
        /// </summary>
        public string type { get; set; }

        public SingleButton(string theType)
        {
            type = theType;
        }
    }
}