using Dapper;
using Gseey.Framework.Common.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;

namespace Gseey.Framework.DataBase
{
    public class DBHelper
    {

        #region 内部枚举

        /// <summary>
        /// 数据库类型
        /// </summary>
        public enum DBType
        {
            /// <summary>
            /// ms sql server 数据库
            /// </summary>
            MSSQL = 10,

            /// <summary>
            /// mysql 数据库
            /// </summary>
            MYSQL = 20,

            /// <summary>
            /// sqlite 数据库
            /// </summary>
            SQLITE = 30,
        }

        #endregion

        #region 内部属性

        /// <summary>
        /// 数据库类型
        /// </summary>
        private static DBType DataBaseType { get; }

        private static IDbConnection DBConnection { get; }

        #endregion

        #region 构造函数

        static DBHelper()
        {
            //从配置文件中获取数据库类型
            DataBaseType = GetDbType();

            //根据数据库类型,获取指定数据库连接
            DBConnection = GetDbConection(DataBaseType);
        }
        #endregion

        #region 私有函数

        /// <summary>
        /// 从配置文件中获取数据库类型
        /// </summary>
        /// <returns></returns>
        private static DBType GetDbType()
        {
            var dbType = DBType.MSSQL;

            var configDbTypeStr = ConfigHelper.Get("GseeyConnections:DbConnectionType");
            if (!string.IsNullOrEmpty(configDbTypeStr))
            {
                if (Enum.TryParse(configDbTypeStr, true, out dbType))
                {
                    return dbType;
                }
            }

            return dbType;
        }

        /// <summary>
        /// 根据数据库类型,获取指定数据库连接
        /// </summary>
        /// <param name="dataBaseType"></param>
        /// <returns></returns>
        private static IDbConnection GetDbConection(DBType dataBaseType)
        {
            //根据数据库类型,获取指定数据库连接串
            var connectionString = GetDbConectionString(dataBaseType);

            IDbConnection connection;
            switch (dataBaseType)
            {
                case DBType.MSSQL:
                default:
                    connection = new SqlConnection(connectionString);
                    //SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLServer);
                    break;
                case DBType.MYSQL:
                    connection = new MySqlConnection(connectionString);
                    //SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
                    break;
                case DBType.SQLITE:
                    connection = new SQLiteConnection(connectionString);
                    //SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
                    break;
            }
            connection.Open();

            return connection;
        }

        /// <summary>
        /// 根据数据库类型,获取指定数据库连接串
        /// </summary>
        /// <param name="dataBaseType"></param>
        /// <returns></returns>
        private static string GetDbConectionString(DBType dataBaseType)
        {
            var connString = string.Empty;

            var configKey = string.Format("GseeyConnections:{0}DbConnectionString", dataBaseType.ToString().ToUpper());

            connString = ConfigHelper.Get(configKey);

            return connString;
        }


        #endregion

        #region 公共函数

        public static T Get<T>(int id) where T : class
        {
            try
            {
                var result = DBConnection.Get<T>(id);
                return result;
            }
            catch (Exception ex)
            {

            }
            return default(T);
        }


        #endregion
    }
}
