using Dapper;
using Gseey.Framework.Common.Helpers;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Threading.Tasks;

namespace Gseey.Framework.DataBase
{
    public class DapperDBHelper
    {
        #region 内部枚举
        /// <summary>
        /// 数据库类型
        /// </summary>
        internal enum DBType
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

        private enum DBOptType
        {
            /// <summary>
            /// 可读
            /// </summary>
            Read = 10,

            /// <summary>
            /// 可读可写
            /// </summary>
            Write = 20
        }
        #endregion

        #region 内部属性

        /// <summary>
        /// 读 连接
        /// </summary>
        private static IDbConnection DBReadConnection { get; }

        /// <summary>
        /// 写 连接
        /// </summary>
        private static IDbConnection DBWriteConnection { get; }

        #endregion

        #region 公用属性

        /// <summary>
        /// 数据库类型
        /// </summary>
        internal static DBType DataBaseType { get; }

        #endregion

        #region 构造函数

        static DapperDBHelper()
        {
            //从配置文件中获取数据库类型
            DataBaseType = GetDbType();

            //根据数据库类型,获取指定数据库连接
            DBReadConnection = GetDbConection(DataBaseType);
            DBWriteConnection = GetDbConection(DataBaseType, DBOptType.Write);
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

            var configDbTypeStr = ConfigHelper.Get("Gseey:Connections:DbConnectionType");
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
        private static IDbConnection GetDbConection(DBType dataBaseType, DBOptType optType = DBOptType.Read)
        {
            //根据数据库类型,获取指定数据库连接串
            var connectionString = GetDbConectionString(dataBaseType, optType);

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
            //connection.Open();

            return connection;
        }

        /// <summary>
        /// 根据数据库类型,获取指定数据库连接串
        /// </summary>
        /// <param name="dataBaseType"></param>
        /// <returns></returns>
        private static string GetDbConectionString(DBType dataBaseType, DBOptType optType = DBOptType.Read)
        {
            var connString = string.Empty;

            var configKey = string.Format("Gseey:Connections:{0}Db{1}ConnectionString",
                dataBaseType.ToString().ToUpper(),
                optType.ToString().ToUpper());

            connString = ConfigHelper.Get(configKey);

            return connString;
        }

        /// <summary>
        /// 生成带自增的sql语句
        /// </summary>
        /// <returns></returns>
        private static string GetInsertSqlWithIndentity()
        {
            var indentitySql = string.Empty;
            switch (DataBaseType)
            {
                case DBType.MSSQL:
                default:
                    indentitySql = string.Format("select @@IDENTITY;");
                    break;
                case DBType.MYSQL:
                    indentitySql = string.Format("select LAST_INSERT_ID();");
                    break;
                case DBType.SQLITE:
                    //indentitySql = string.Format("{0};select {1} last_insert_rowid();", sql, useParam ? "@id=" : "");
                    indentitySql = string.Format("select last_insert_rowid();");
                    break;
            }
            return indentitySql;
        }

        /// <summary>
        /// 开启事务
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        private static IDbTransaction OpenTransaction(IDbConnection connection)
        {
            if (connection.State != ConnectionState.Open)
                connection.Open();
            var tran = connection.BeginTransaction();
            return tran;
        }
        #endregion

        #region 公共函数

        #region 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【同步】

        /// <summary>
        /// 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【同步】
        /// </summary>
        /// <param name="sql">SQL语句（本方法会自动在SQL的结尾增加分号，并增加查询ID的语句）</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>自增ID</returns>
        public static int Insert(string sql, object param = null, int? commandTimeout = null)
        {
            var indentitySql = GetInsertSqlWithIndentity();

            using (var tran = OpenTransaction(DBWriteConnection))
            {
                try
                {
                    DBWriteConnection.Execute(sql, param, transaction: tran, commandTimeout: commandTimeout);

                    var id = DBWriteConnection.ExecuteScalar<int>(indentitySql, transaction: tran, commandTimeout: commandTimeout);

                    tran.Commit();

                    return id;
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    ex.WriteExceptionLog("插入数据库失败");
                    return -1;
                }
            }
        }

        #endregion

        #region 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【异步】

        /// <summary>
        /// 执行增（INSERT）语句【执行单条SQL语句，返回自增列】【异步】
        /// </summary>
        /// <param name="sql">SQL语句（本方法会自动在SQL的结尾增加分号，并增加查询ID的语句）</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>自增ID</returns>
        public static async Task<int> InsertAsync(string sql, object param = null, int? commandTimeout = null)
        {
            var indentitySql = GetInsertSqlWithIndentity();

            using (var tran = OpenTransaction(DBWriteConnection))
            {
                try
                {
                    await DBWriteConnection.ExecuteAsync(sql, param, transaction: tran, commandTimeout: commandTimeout);

                    var id = await DBWriteConnection.ExecuteScalarAsync<int>(indentitySql, transaction: tran, commandTimeout: commandTimeout);

                    tran.Commit();

                    return id;
                }
                catch (Exception ex)
                {
                    tran.Rollback();

                    ex.WriteExceptionLog("插入数据库失败");
                    return -1;
                }
            }
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【同步】

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【同步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="outParam">输出参数[参数名(@name)] </param>
        /// <param name="commandType">如何解释命令字符串</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>Tuple(受影响的行数, Dictionary(输出参数名(@name), 输出参数))</returns>
        public static Tuple<int, Dictionary<string, string>> Execute(string sql, DynamicParameters param, List<string> outParam, CommandType? commandType = null, int? commandTimeout = null)
        {
            var rows = DBWriteConnection.Execute(sql, param, commandTimeout: commandTimeout, commandType: commandType);
            var outDic = new Dictionary<string, string>();
            foreach (var item in outParam)
            {
                outDic.Add(item, param.Get<string>(item));
            }
            return new Tuple<int, Dictionary<string, string>>(rows, outDic);
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【异步】

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句或存储过程【单条SQL语句，可以获得输出参数】【异步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="outParam">输出参数[参数名(@name)] </param>
        /// <param name="commandType">如何解释命令字符串</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>Tuple(受影响的行数, Dictionary(输出参数名(@name), 输出参数))</returns>
        public static async Task<Tuple<int, Dictionary<string, string>>> ExecuteAsync(string sql, DynamicParameters param, List<string> outParam, CommandType? commandType = null, int? commandTimeout = null)
        {
            var rows = await DBWriteConnection.ExecuteAsync(sql, param, commandTimeout: commandTimeout, commandType: commandType);
            var outDic = new Dictionary<string, string>();
            foreach (var item in outParam)
            {
                outDic.Add(item, param.Get<string>(item));
            }
            return new Tuple<int, Dictionary<string, string>>(rows, outDic);
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【同步】

        /// <returns></returns>
        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【同步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>受影响的行数</returns>
        public static int Execute(string sql, object param = null, int? commandTimeout = null)
        {
            return DBWriteConnection.Execute(sql, param, commandTimeout: commandTimeout);
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【异步】

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【执行单条SQL语句】【异步】
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns>受影响的行数</returns>
        public static Task<int> ExecuteAsync(string sql, object param = null, int? commandTimeout = null)
        {
            return DBWriteConnection.ExecuteAsync(sql, param, commandTimeout: commandTimeout);
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【同步】

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【同步】
        /// </summary>
        /// <param name="sqlDic">SQL + 参数【key：sql语句 value：参数】</param>ConnectionString
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static void ExecuteToTransaction(Dictionary<string, object> sqlDic, int? commandTimeout = null)
        {
            var trans = DBWriteConnection.BeginTransaction();
            try
            {
                foreach (var item in sqlDic)
                {
                    DBWriteConnection.Execute(item.Key, item.Value, transaction: trans, commandTimeout: commandTimeout);
                }
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        #endregion

        #region 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【异步】【待测试】

        /// <summary>
        /// 执行增（INSERT）删（DELETE）改（UPDATE）语句【带事务，可以同时执行多条SQL】【异步】【待测试】
        /// </summary>
        /// <param name="sqlDic">SQL + 参数【key：sql语句 value：参数】</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static async Task ExecuteToTransactionAsync(Dictionary<string, object> sqlDic, int? commandTimeout = null)
        {
            var trans = DBWriteConnection.BeginTransaction();
            try
            {
                foreach (var item in sqlDic)
                {
                    await DBWriteConnection.ExecuteAsync(item.Key, item.Value, transaction: trans, commandTimeout: commandTimeout);
                }
                trans.Commit();
            }
            catch (Exception)
            {
                trans.Rollback();
                throw;
            }
        }

        #endregion

        #region 执行存储过程（Procdeure）【同步】

        /// <summary>
        /// 执行存储过程（Procdeure）【同步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="porcdeureName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static IEnumerable<T> ExecuteToProcdeure<T>(string porcdeureName, object param = null, int? commandTimeout = null)
        {
            return DBWriteConnection.Query<T>(porcdeureName, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure, param: param);
        }

        #endregion

        #region 执行存储过程（Procdeure）【异步】

        /// <summary>
        /// 执行存储过程（Procdeure）【异步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="porcdeureName">存储过程名称</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> ExecuteToProcdeureAsync<T>(string porcdeureName, object param = null, int? commandTimeout = null)
        {
            return DBWriteConnection.QueryAsync<T>(porcdeureName, commandTimeout: commandTimeout, commandType: CommandType.StoredProcedure, param: param);
        }

        #endregion

        #region 执行查询【同步】

        /// <summary>
        /// 执行查询【同步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static IEnumerable<T> Query<T>(string sql, object param = null, int? commandTimeout = null)
        {
            return DBReadConnection.Query<T>(sql, param: param, commandTimeout: commandTimeout);
        }

        #endregion

        #region 执行查询【异步】

        /// <summary>
        /// 执行查询【异步】
        /// </summary>
        /// <typeparam name="T">返回结果的类型</typeparam>
        /// <param name="sql">SQL语句</param>
        /// <param name="param">参数</param>
        /// <param name="commandTimeout">超时时间</param>
        /// <returns></returns>
        public static Task<IEnumerable<T>> QueryAsync<T>(string sql, object param = null, int? commandTimeout = null)
        {
            return DBReadConnection.QueryAsync<T>(sql, param: param, commandTimeout: commandTimeout);
        }

        #endregion
        #endregion
    }
}
