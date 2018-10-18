using Gseey.Framework.DataBase.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gseey.Framework.DataBase.DalBase
{
    public class DapperDALBase<T> where T : DapperEntityBase
    {
        public abstract class DbSqlBase
        {
            /// <summary>
            /// insert 语句格式
            /// </summary>
            public abstract string InsertSqlFormat { get; set; }
            /// <summary>
            /// update 语句格式
            /// </summary>
            public abstract string UpdateSqlFormat { get; set; }
            /// <summary>
            /// delete 语句格式
            /// </summary>
            public abstract string DeleteSqlFormat { get; set; }

            /// <summary>
            /// select 语句
            /// </summary>
            public abstract string SelectSqlFormat { get; set; }

            /// <summary>
            /// 表名
            /// </summary>
            public abstract string TableName { get; set; }

            /// <summary>
            /// select 字段
            /// </summary>
            public abstract string SelectColumnsFormat { get; set; }

            /// <summary>
            /// select orderby 字段
            /// </summary>
            public abstract string SelectOrderByFormat { get; set; }

            /// <summary>
            /// select groupby 字段
            /// </summary>
            public abstract string SelectGroupByFormat { get; set; }
        }

        public abstract class SqlServer : DbSqlBase { }

        public abstract class Mysql : DbSqlBase { }

        public abstract class Sqlite : DbSqlBase { }

        #region 内部方法

        private string GetDbSql(string selectColumnStr = "", string whereSql = "", string orderbyStr = "", string groupbyStr = "")
        {
            //获取表名
            var tableName = GetTableName();

            var selectFormat = string.Format("select {0} from {1} {2} {3} {4}",
                selectColumnStr,
                tableName,
                string.IsNullOrEmpty(whereSql) ?
                    "" :
                    string.Format(" where {0}", whereSql),
                string.IsNullOrEmpty(orderbyStr) ?
                    "" :
                    string.Format(" order by {0}", orderbyStr),
                string.IsNullOrEmpty(groupbyStr) ?
                    "" :
                    string.Format(" group by {0}", groupbyStr)
                );

            switch (DapperDBHelper.DataBaseType)
            {
                case DapperDBHelper.DBType.MSSQL:
                    break;
                case DapperDBHelper.DBType.MYSQL:
                    break;
                case DapperDBHelper.DBType.SQLITE:
                    break;
                default:
                    break;
            }

            return string.Empty;
        }

        private string GetTableName()
        {
            return string.Empty;
        }

        #endregion

        public async Task<T> GetEntityAsync(int id)
        {
            var sql = GetDbSql("");
            var queryResult = await DapperDBHelper.QueryAsync<T>(sql);
            return queryResult.SingleOrDefault();
        }


    }
}
