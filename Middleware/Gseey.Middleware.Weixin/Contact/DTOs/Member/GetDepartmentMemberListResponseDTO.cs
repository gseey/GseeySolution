using Gseey.Middleware.Weixin.BaseDTOs;
using Gseey.Middleware.Weixin.Contact.DTOs.Tag;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Contact.DTOs.Member
{
    public class GetDepartmentMemberListResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// 成员列表
        /// </summary>
        public List<MemberList> userlist { get; set; }
    }

    public class MemberList: MemberBaseList
    {
        /// <summary>
        /// 成员所属部门
        /// </summary>
        public int[] department { get; set; }
    }

}
