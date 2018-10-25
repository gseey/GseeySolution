using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.BaseEntities
{
    public class AccessTokenResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// 获取到的凭证,最长为512字节
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 凭证的有效时间（秒）
        /// </summary>
        public int expires_in { get; set; }
    }
}
