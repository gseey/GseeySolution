﻿namespace Gseey.Framework.DataBase.DalBase
{
    using Dapper;
    using Gseey.Framework.Common.Helpers;
    using Gseey.Framework.DataBase.Attributes;
    using Gseey.Framework.DataBase.EntityBase;
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines the <see cref="DapperDALBase{T}" />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DapperDALBase<T> where T : DapperEntityBase
    {
        /// <summary>
        /// The GetDbSelectSql
        /// </summary>
        /// <param name="selectColumnStr">The selectColumnStr<see cref="string"/></param>
        /// <param name="whereSql">The whereSql<see cref="string"/></param>
        /// <param name="orderbyStr">The orderbyStr<see cref="string"/></param>
        /// <param name="groupbyStr">The groupbyStr<see cref="string"/></param>
        /// <returns>The <see cref="string"/></returns>
        private string GetDbSelectSql(string selectColumnStr = "", string whereSql = "", string orderbyStr = "", string groupbyStr = "")
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

        /// <summary>
        /// 获取对象属性
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private List<string> GetProperties(object obj)
        {
            if (obj == null)
            {
                return new List<string>();
            }
            if (obj is DynamicParameters)
            {
                return (obj as DynamicParameters).ParameterNames.ToList();
            }

            var propList = ReflectionHelper.GetPropertyInfos(obj);
            return propList.Select(m => m.Name).ToList();
        }

        /// <summary>
        /// 获取表名
        /// </summary>
        /// <returns></returns>
        private string GetTableName()
        {
            var customAttrs = ReflectionHelper.GetCustomAttributes<T>();

            var tableAttr = (TableAttribute)customAttrs.Where(m => m is TableAttribute).SingleOrDefault();
            if (tableAttr != null)
                return tableAttr.Name;
            return typeof(T).Name;
        }

        /// <summary>
        /// 获取sql参数化名称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private string BuildSqlParamName(string value)
        {
            var formatValue = string.Empty;

            switch (DapperDBHelper.DataBaseType)
            {
                case DapperDBHelper.DBType.MSSQL:
                default:
                    formatValue = string.Format("@{0}", value);
                    break;
                case DapperDBHelper.DBType.MYSQL:
                    formatValue = string.Format("?{0}", value);
                    break;
                case DapperDBHelper.DBType.SQLITE:
                    formatValue = string.Format("${0}", value);
                    break;
            }

            return formatValue;
        }

        /// <summary>
        /// 生成insert 语句
        /// </summary>
        /// <param name="data"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        private string BuildInsertSql(dynamic data, out object obj)
        {
            obj = data as object;
            var properties = GetProperties(obj);
            var tableName = GetTableName();
            var columns = string.Join(",", properties);
            var values = string.Join(",", properties.Select(p => BuildSqlParamName(p)));
            var sql = string.Format("insert into {0} ({1}) values ({2})", tableName, columns, values);
            return sql;
        }

        /// <summary>
        /// 生成select sql语句
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="conditionObj">The conditionObj<see cref="object"/></param>
        /// <param name="selectPart"></param>
        /// <param name="isOr"></param>
        /// <returns></returns>
        private string BuildQuerySql(dynamic condition, out object conditionObj, string selectPart = "*", bool isOr = false)
        {
            conditionObj = condition as object;
            var properties = GetProperties(conditionObj);
            var tableName = GetTableName();
            if (properties.Count == 0)
            {
                return string.Format("SELECT {1} FROM {0}", tableName, selectPart);
            }

            var separator = isOr ? " OR " : " AND ";
            var wherePart = string.Join(separator, properties.Select(p => p + " = " + BuildSqlParamName(p)));

            return string.Format("SELECT {2} FROM {0} WHERE {1}", tableName, wherePart, selectPart);
        }

        /// <summary>
        /// 创建update sql语句
        /// </summary>
        /// <param name="data">带更新字段</param>
        /// <param name="condition">更新条件</param>
        /// <param name="parameters"></param>
        /// <returns>The <see cref="string"/></returns>
        private string BuildUpdateSql(dynamic data, dynamic condition, out DynamicParameters parameters)
        {
            var obj = data as object;
            var conditionObj = condition as object;

            var updatePropertyInfos = ReflectionHelper.GetPropertyInfos(obj);
            var wherePropertyInfos = ReflectionHelper.GetPropertyInfos(conditionObj);

            var updateProperties = updatePropertyInfos.Select(p => p.Name);
            var whereProperties = wherePropertyInfos.Select(p => p.Name);

            var updateFields = string.Join(",", updateProperties.Select(p => p + " = " + BuildSqlParamName(p)));
            var whereFields = string.Empty;

            if (whereProperties.Any())
            {
                whereFields = " where " + string.Join(" and ", whereProperties.Select(p => p + " = @w_" + p));
            }

            var tableName = GetTableName();

            var sql = string.Format("update {0} set {1}{2}", tableName, updateFields, whereFields);
            parameters = new DynamicParameters(data);
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;
            wherePropertyInfos.ToList().ForEach(p => expandoObject.Add("w_" + p.Name, p.GetValue(conditionObj, null)));
            parameters.AddDynamicParams(expandoObject);

            return sql;
        }

        /// <summary>
        /// Insert操作
        /// </summary>
        /// <param name="data">要插入的字段</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Insert(dynamic data, int? commandTimeout = null)
        {
            var sql = BuildInsertSql(data, out object obj);

            return DapperDBHelper.Insert(sql, param: obj, commandTimeout: commandTimeout);
        }

        /// <summary>
        /// Insert操作
        /// </summary>
        /// <param name="data">要插入的字段</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public async Task<long> InsertAsync(dynamic data, int? commandTimeout = null)
        {
            var sql = BuildInsertSql(data, out object obj);

            return await DapperDBHelper.InsertAsync(sql, param: obj, commandTimeout: commandTimeout);
        }

        /// <summary>
        /// Update操作
        /// </summary>
        /// <param name="data">带更新字段</param>
        /// <param name="condition">更新条件</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public int Update(dynamic data, dynamic condition, int? commandTimeout = null)
        {
            var sql = BuildUpdateSql(data, condition, out DynamicParameters parameters);

            var result = DapperDBHelper.Execute(sql, param: parameters, commandTimeout: commandTimeout);
            return result;
        }

        /// <summary>
        /// Update操作
        /// </summary>
        /// <param name="data">带更新字段</param>
        /// <param name="condition">更新条件</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(dynamic data, dynamic condition, int? commandTimeout = null)
        {
            var sql = BuildUpdateSql(data, condition, out DynamicParameters parameters);

            var result = await DapperDBHelper.ExecuteAsync(sql, param: parameters, commandTimeout: commandTimeout);
            return result;
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="columns"></param>
        /// <param name="isOr"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public IEnumerable<T> QueryList(dynamic condition, string columns = "*", bool isOr = false, int? commandTimeout = null)
        {
            object obj;
            var sql = BuildQuerySql(condition, out obj, columns, isOr);

            var result = DapperDBHelper.Query<T>(sql, param: obj, commandTimeout: commandTimeout);
            return result;
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="columns"></param>
        /// <param name="isOr"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> QueryListAsync(dynamic condition, string columns = "*", bool isOr = false, int? commandTimeout = null)
        {
            object obj;
            var sql = BuildQuerySql(condition, out obj, columns, isOr);

            var result = await DapperDBHelper.QueryAsync<T>(sql, obj, commandTimeout);
            return result;
        }

        /// <summary>
        /// The Execute
        /// </summary>
        /// <param name="sql">The sql<see cref="string"/></param>
        /// <param name="param">The param<see cref="object"/></param>
        /// <param name="timeout">The timeout<see cref="int?"/></param>
        /// <returns>The <see cref="int"/></returns>
        public int Execute(string sql, object param, int? timeout = null)
        {
            return DapperDBHelper.Execute(sql, param: param, commandTimeout: timeout);
        }
    }
}
