namespace Gseey.Framework.DataBase.Attributes
{
    using System;

    /// <summary>
    /// Defines the <see cref="TableAttribute" />
    /// </summary>
    public class TableAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets the Name
        /// 表名
        /// </summary>
        public string Name { get; set; }
    }
}
