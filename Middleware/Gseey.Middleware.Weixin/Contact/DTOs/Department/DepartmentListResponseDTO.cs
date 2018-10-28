using Gseey.Middleware.Weixin.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Contact.DTOs.Department
{
    public class DepartmentListResponseDTO : ResponseBaseDTO
    {
        public List<DepartmentList> department { get; set; }
    }
    public class DepartmentList
    {
        /// <summary>
        /// 部门id
        /// </summary>
        public long id { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string name { get; set; }
        /// <summary>
        /// 上级部门id
        /// </summary>
        public long parentid { get; set; }
        /// <summary>
        /// 在父部门中的次序值。order值小的排序靠前。
        /// </summary>
        public long order { get; set; }
    }
}
