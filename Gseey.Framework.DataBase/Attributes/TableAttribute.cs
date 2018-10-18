using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Framework.DataBase.Attributes
{
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Name { get; set; }


    }
}
