using Gseey.Middleware.Weixin.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Contact.DTOs.Tag
{
    /// <summary>
    /// 增加/删除标签成员返回结果
    /// </summary>
    public class AddOrDeleteTagMemberResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// 非法的成员帐号列表
        /// </summary>
        public string invalidlist { get; set; }

        /// <summary>
        /// 非法的部门id列表
        /// </summary>
        public List<int> invalidparty { get; set; }
    }
}
