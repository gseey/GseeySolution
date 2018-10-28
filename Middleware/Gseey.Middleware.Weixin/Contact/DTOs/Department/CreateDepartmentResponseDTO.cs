using Gseey.Middleware.Weixin.BaseDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Middleware.Weixin.Contact.DTOs.Department
{
    /// <summary>
    /// 创建部门返回结果
    /// </summary>
    public class CreateDepartmentResponseDTO : ResponseBaseDTO
    {
        /// <summary>
        /// 创建的部门id
        /// </summary>
        public int id { get; set; }
    }
}
