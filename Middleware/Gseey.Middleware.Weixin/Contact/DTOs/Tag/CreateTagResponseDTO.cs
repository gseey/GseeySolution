using Gseey.Middleware.Weixin.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Contact.DTOs.Tag
{
    /// <summary>
    /// 创建标签返回结果
    /// </summary>
    public class CreateTagResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// 创建的标签id
        /// </summary>
        public int tagid { get; set; }
    }
}
