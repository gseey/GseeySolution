using System;
using System.Collections.Generic;
using System.Text;

namespace Gseey.Framework.DataBase.DalBase
{
    public class DALBase<T> where T : class
    {
        public abstract class DbSqlBase
        {
            /// <summary>
            /// insert 语句格式
            /// </summary>
            public virtual string InsertSqlFormat { get; set; }
            /// <summary>
            /// update 语句格式
            /// </summary>
            public virtual string UpdateSqlFormat { get; set; }
            /// <summary>
            /// delete 语句格式
            /// </summary>
            public virtual string DeleteSqlFormat { get; set; }


        }

        public class SqlServer : DbSqlBase { }

        public class Mysql : DbSqlBase { }

        public class Sqlite : DbSqlBase { }
    }
}
