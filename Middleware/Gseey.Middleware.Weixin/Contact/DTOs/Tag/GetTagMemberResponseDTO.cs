using Gseey.Middleware.Weixin.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Contact.DTOs.Tag
{
    /// <summary>
    /// 获取标签成员返回结果
    /// </summary>
    public class GetTagMemberResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// 标签名
        /// </summary>
        public string tagname { get; set; }

        /// <summary>
        /// 标签中包含的成员列表
        /// </summary>
        public List<MemberBaseList> userlist { get; set; }

        /// <summary>
        /// 标签中包含的部门id列表
        /// </summary>
        public List<string> partylist { get; set; }
    }

    public class MemberBaseList
    {
        /// <summary>
        /// 成员UserID。对应管理端的帐号
        /// </summary>
        public string userid { get; set; }

        /// <summary>
        /// 成员名称
        /// </summary>
        public string name { get; set; }
    }

}
