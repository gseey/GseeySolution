using Gseey.Middleware.Weixin.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Contact.DTOs.Tag
{
    /// <summary>
    /// 获取标签列表返回结果
    /// </summary>
    public class GetTagListResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// 标签列表
        /// </summary>
        public List<TagList> taglist { get; set; }
        
    }
    public class TagList
    {
        /// <summary>
        /// 标签id
        /// </summary>
        public int tagid { get; set; }

        /// <summary>
        /// 标签名
        /// </summary>
        public string tagname { get; set; }
    }

}
