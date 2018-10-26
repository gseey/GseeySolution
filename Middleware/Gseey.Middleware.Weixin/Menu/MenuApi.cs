using Gseey.Middleware.Weixin.BaseEntities;
using Gseey.Middleware.Weixin.Helpers;
using Gseey.Middleware.Weixin.Menu.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gseey.Middleware.Weixin.Menu
{
    public class MenuApi
    {
        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="buttonData">菜单内容</param>
        /// <returns></returns>
        public static async Task<ResponseBaseDTO> CreateMenu(int channelId, ButtonGroup buttonData)
        {
            //AccessTokenHelper
            return null;
        }
    }
}
