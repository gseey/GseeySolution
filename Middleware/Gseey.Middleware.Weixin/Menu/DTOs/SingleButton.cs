using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Menu.DTOs
{
    /// <summary>
    /// 所有单击按钮的基类（view，click等）
    /// </summary>
    public abstract class SingleButton : BaseButton, IBaseButton
    {
        /// <summary>
        /// 按钮类型（click或view）
        /// </summary>
        public string type { get;}

        public SingleButton(string theType)
        {
            type = theType;
        }
    }
}
