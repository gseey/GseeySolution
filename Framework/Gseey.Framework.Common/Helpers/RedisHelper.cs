using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Gseey.Framework.Common.Helpers
{
    /// <summary>
    /// Redis 助手
    /// http://www.cnblogs.com/liqingwen/p/6672452.html
    /// </summary>
    public static class RedisHelper
    {
        /// <summary>
        /// 连接字符串
        /// </summary>
        private static readonly string ConnectionString;

        /// <summary>
        /// redis 连接对象
        /// </summary>
        private static IConnectionMultiplexer _connMultiplexer;

        /// <summary>
        /// 默认的 Key 值（用来当作 RedisKey 的前缀）
        /// </summary>
        private static readonly string DefaultKey;

        /// <summary>
        /// 锁
        /// </summary>
        private static readonly object Locker = new object();

        /// <summary>
        /// 数据库
        /// </summary>
        private static IDatabase _db;

        /// <summary>
        /// 获取 Redis 连接对象
        /// </summary>
        /// <returns></returns>
        public static IConnectionMultiplexer GetConnectionRedisMultiplexer()
        {
            if ((_connMultiplexer == null) || !_connMultiplexer.IsConnected)
            {
                lock (Locker)
                {
                    if ((_connMultiplexer == null) || !_connMultiplexer.IsConnected)
                        _connMultiplexer = ConnectionMultiplexer.Connect(ConnectionString);
                }
            }

            return _connMultiplexer;
        }

        private static IDatabase GetDatabase(int dbIndex = -1)
        {
            _db = _connMultiplexer.GetDatabase(dbIndex);
            return _db;
        }

        /// <summary>
        /// The GetTransaction
        /// </summary>
        /// <returns>The <see cref="ITransaction"/></returns>
        public static ITransaction GetTransaction(int dbIndex = -1)
        {
            var db = GetDatabase(dbIndex);
            return db.CreateTransaction();
        }

        /// <summary>
        /// Initializes static members of the <see cref="RedisHelper"/> class.
        /// </summary>
        static RedisHelper()
        {
            ConnectionString = ConfigHelper.Get("Gseey:Connections:RedisConnectionString");
            _connMultiplexer = ConnectionMultiplexer.Connect(ConnectionString);
            DefaultKey = ConfigHelper.Get("Redis:DefaultKey");
            AddRegisterEvent();
        }

        /// <summary>
        /// 设置 key 并保存字符串（如果 key 已存在，则覆盖值）
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool StringSet(string redisKey, string redisValue, TimeSpan? expiry = null,int dbIndex=-1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.StringSet(redisKey, redisValue, expiry);
        }

        /// <summary>
        /// 保存多个 Key-value
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public static bool StringSet(IEnumerable<KeyValuePair<RedisKey, RedisValue>> keyValuePairs, int dbIndex = -1)
        {
            keyValuePairs =
                keyValuePairs.Select(x => new KeyValuePair<RedisKey, RedisValue>(AddKeyPrefix(x.Key), x.Value));
            var db = GetDatabase(dbIndex);
            return db.StringSet(keyValuePairs.ToArray());
        }

        /// <summary>
        /// 获取字符串
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static string StringGet(string redisKey, TimeSpan? expiry = null, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.StringGet(redisKey);
        }

        /// <summary>
        /// 存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool StringSet<T>(string redisKey, T redisValue, TimeSpan? expiry = null, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(redisValue);
            var db = GetDatabase(dbIndex);
            return db.StringSet(redisKey, json, expiry);
        }

        /// <summary>
        /// 获取一个对象（会进行反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static T StringGet<T>(string redisKey, TimeSpan? expiry = null, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return Deserialize<T>(db.StringGet(redisKey));
        }

        /// <summary>
        /// 保存一个字符串值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync(string redisKey, string redisValue, TimeSpan? expiry = null, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.StringSetAsync(redisKey, redisValue, expiry);
        }

        /// <summary>
        /// 保存一组字符串值
        /// </summary>
        /// <param name="keyValuePairs"></param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync(IEnumerable<KeyValuePair<RedisKey, RedisValue>> keyValuePairs, int dbIndex = -1)
        {
            keyValuePairs =
                keyValuePairs.Select(x => new KeyValuePair<RedisKey, RedisValue>(AddKeyPrefix(x.Key), x.Value));
            var db = GetDatabase(dbIndex);
            return await db.StringSetAsync(keyValuePairs.ToArray());
        }

        /// <summary>
        /// 获取单个值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<string> StringGetAsync(string redisKey, string redisValue, TimeSpan? expiry = null, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.StringGetAsync(redisKey);
        }

        /// <summary>
        /// 存储一个对象（该对象会被序列化保存）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> StringSetAsync<T>(string redisKey, T redisValue, TimeSpan? expiry = null, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(redisValue);
            var db = GetDatabase(dbIndex);
            return await db.StringSetAsync(redisKey, json, expiry);
        }

        /// <summary>
        /// 获取一个对象（会进行反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<T> StringGetAsync<T>(string redisKey, TimeSpan? expiry = null, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return Deserialize<T>(await db.StringGetAsync(redisKey));
        }

        /// <summary>
        /// 判断该字段是否存在 hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool HashExists(string redisKey, string hashField, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.HashExists(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static bool HashDelete(string redisKey, string hashField, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.HashDelete(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static long HashDelete(string redisKey, IEnumerable<RedisValue> hashField, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.HashDelete(redisKey, hashField.ToArray());
        }

        /// <summary>
        /// 在 hash 设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HashSet(string redisKey, string hashField, string value, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.HashSet(redisKey, hashField, value);
        }

        /// <summary>
        /// 在 hash 中设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        public static void HashSet(string redisKey, IEnumerable<HashEntry> hashFields, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            db.HashSet(redisKey, hashFields.ToArray());
        }

        /// <summary>
        /// 在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static RedisValue HashGet(string redisKey, string hashField, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.HashGet(redisKey, hashField);
        }

        /// <summary>
        /// 在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static RedisValue[] HashGet(string redisKey, RedisValue[] hashField, string value, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.HashGet(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 返回所有的字段值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> HashKeys(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.HashKeys(redisKey);
        }

        /// <summary>
        /// 返回 hash 中的所有值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static RedisValue[] HashValues(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.HashValues(redisKey);
        }

        /// <summary>
        /// 在 hash 设定值（序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool HashSet<T>(string redisKey, string hashField, T value, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(value);
            var db = GetDatabase(dbIndex);
            return db.HashSet(redisKey, hashField, json);
        }

        /// <summary>
        /// 在 hash 中获取值（反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static T HashGet<T>(string redisKey, string hashField, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return Deserialize<T>(db.HashGet(redisKey, hashField));
        }

        /// <summary>
        /// 判断该字段是否存在 hash 中
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static async Task<bool> HashExistsAsync(string redisKey, string hashField,int dbIndex=-1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.HashExistsAsync(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static async Task<bool> HashDeleteAsync(string redisKey, string hashField, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.HashDeleteAsync(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 中移除指定字段
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static async Task<long> HashDeleteAsync(string redisKey, IEnumerable<RedisValue> hashField, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.HashDeleteAsync(redisKey, hashField.ToArray());
        }

        /// <summary>
        /// 在 hash 设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<bool> HashSetAsync(string redisKey, string hashField, string value, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.HashSetAsync(redisKey, hashField, value);
        }

        /// <summary>
        /// 在 hash 中设定值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashFields"></param>
        /// <returns>The <see cref="Task"/></returns>
        public static async Task HashSetAsync(string redisKey, IEnumerable<HashEntry> hashFields, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            await db.HashSetAsync(redisKey, hashFields.ToArray());
        }

        /// <summary>
        /// 在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static async Task<RedisValue> HashGetAsync(string redisKey, string hashField, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.HashGetAsync(redisKey, hashField);
        }

        /// <summary>
        /// 在 hash 中获取值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<RedisValue>> HashGetAsync(string redisKey, RedisValue[] hashField, string value, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.HashGetAsync(redisKey, hashField);
        }

        /// <summary>
        /// 从 hash 返回所有的字段值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<RedisValue>> HashKeysAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.HashKeysAsync(redisKey);
        }

        /// <summary>
        /// 返回 hash 中的所有值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<RedisValue>> HashValuesAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.HashValuesAsync(redisKey);
        }

        /// <summary>
        /// 在 hash 设定值（序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static async Task<bool> HashSetAsync<T>(string redisKey, string hashField, T value, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(value);
            var db = GetDatabase(dbIndex);
            return await db.HashSetAsync(redisKey, hashField, json);
        }

        /// <summary>
        /// 在 hash 中获取值（反序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="hashField"></param>
        /// <returns></returns>
        public static async Task<T> HashGetAsync<T>(string redisKey, string hashField, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return Deserialize<T>(await db.HashGetAsync(redisKey, hashField));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static string ListLeftPop(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.ListLeftPop(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static string ListRightPop(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.ListRightPop(redisKey);
        }

        /// <summary>
        /// 移除列表指定键上与该值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListRemove(string redisKey, string redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.ListRemove(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListRightPush(string redisKey, string redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.ListRightPush(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListLeftPush(string redisKey, string redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.ListLeftPush(redisKey, redisValue);
        }

        /// <summary>
        /// 返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static long ListLength(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.ListLength(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> ListRange(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.ListRange(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static T ListLeftPop<T>(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return Deserialize<T>(db.ListLeftPop(redisKey));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static T ListRightPop<T>(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return Deserialize<T>(db.ListRightPop(redisKey));
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListRightPush<T>(string redisKey, T redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.ListRightPush(redisKey, Serialize(redisValue));
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static long ListLeftPush<T>(string redisKey, T redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.ListLeftPush(redisKey, Serialize(redisValue));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<string> ListLeftPopAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.ListLeftPopAsync(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<string> ListRightPopAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.ListRightPopAsync(redisKey);
        }

        /// <summary>
        /// 移除列表指定键上与该值相同的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListRemoveAsync(string redisKey, string redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.ListRemoveAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListRightPushAsync(string redisKey, string redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.ListRightPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListLeftPushAsync(string redisKey, string redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.ListLeftPushAsync(redisKey, redisValue);
        }

        /// <summary>
        /// 返回列表上该键的长度，如果不存在，返回 0
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<long> ListLengthAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.ListLengthAsync(redisKey);
        }

        /// <summary>
        /// 返回在该列表上键所对应的元素
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<RedisValue>> ListRangeAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.ListRangeAsync(redisKey);
        }

        /// <summary>
        /// 移除并返回存储在该键列表的第一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<T> ListLeftPopAsync<T>(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return Deserialize<T>(await db.ListLeftPopAsync(redisKey));
        }

        /// <summary>
        /// 移除并返回存储在该键列表的最后一个元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<T> ListRightPopAsync<T>(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return Deserialize<T>(await db.ListRightPopAsync(redisKey));
        }

        /// <summary>
        /// 在列表尾部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListRightPushAsync<T>(string redisKey, T redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.ListRightPushAsync(redisKey, Serialize(redisValue));
        }

        /// <summary>
        /// 在列表头部插入值。如果键不存在，先创建再插入值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="redisValue"></param>
        /// <returns></returns>
        public static async Task<long> ListLeftPushAsync<T>(string redisKey, T redisValue, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.ListLeftPushAsync(redisKey, Serialize(redisValue));
        }

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd(string redisKey, string member, double score, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.SortedSetAdd(redisKey, member, score);
        }

        /// <summary>
        /// 在有序集合中返回指定范围的元素，默认情况下从低到高。
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static IEnumerable<RedisValue> SortedSetRangeByRank(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.SortedSetRangeByRank(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static long SortedSetLength(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.SortedSetLength(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="memebr"></param>
        /// <returns></returns>
        public static bool SortedSetLength(string redisKey, string memebr, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.SortedSetRemove(redisKey, memebr);
        }

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static bool SortedSetAdd<T>(string redisKey, T member, double score, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(member);
            var db = GetDatabase(dbIndex);
            return db.SortedSetAdd(redisKey, json, score);
        }

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async Task<bool> SortedSetAddAsync(string redisKey, string member, double score, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.SortedSetAddAsync(redisKey, member, score);
        }

        /// <summary>
        /// 在有序集合中返回指定范围的元素，默认情况下从低到高。
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<IEnumerable<RedisValue>> SortedSetRangeByRankAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.SortedSetRangeByRankAsync(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<long> SortedSetLengthAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.SortedSetLengthAsync(redisKey);
        }

        /// <summary>
        /// 返回有序集合的元素个数
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="memebr"></param>
        /// <returns></returns>
        public static async Task<bool> SortedSetRemoveAsync(string redisKey, string memebr, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.SortedSetRemoveAsync(redisKey, memebr);
        }

        /// <summary>
        /// SortedSet 新增
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="redisKey"></param>
        /// <param name="member"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public static async Task<bool> SortedSetAddAsync<T>(string redisKey, T member, double score, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var json = Serialize(member);
            var db = GetDatabase(dbIndex);
            return await db.SortedSetAddAsync(redisKey, json, score);
        }

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool KeyDelete(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.KeyDelete(redisKey);
        }

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public static long KeyDelete(IEnumerable<string> redisKeys, int dbIndex = -1)
        {
            var keys = redisKeys.Select(x => (RedisKey)AddKeyPrefix(x));
            var db = GetDatabase(dbIndex);
            return db.KeyDelete(keys.ToArray());
        }

        /// <summary>
        /// 校验 Key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static bool KeyExists(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.KeyExists(redisKey);
        }

        /// <summary>
        /// 重命名 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisNewKey"></param>
        /// <returns></returns>
        public static bool KeyRename(string redisKey, string redisNewKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.KeyRename(redisKey, redisNewKey);
        }

        /// <summary>
        /// 设置 Key 的时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static bool KeyExpire(string redisKey, TimeSpan? expiry, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return db.KeyExpire(redisKey, expiry);
        }

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<bool> KeyDeleteAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.KeyDeleteAsync(redisKey);
        }

        /// <summary>
        /// 移除指定 Key
        /// </summary>
        /// <param name="redisKeys"></param>
        /// <returns></returns>
        public static async Task<long> KeyDeleteAsync(IEnumerable<string> redisKeys, int dbIndex = -1)
        {
            var keys = redisKeys.Select(x => (RedisKey)AddKeyPrefix(x));
            var db = GetDatabase(dbIndex);
            return await db.KeyDeleteAsync(keys.ToArray());
        }

        /// <summary>
        /// 校验 Key 是否存在
        /// </summary>
        /// <param name="redisKey"></param>
        /// <returns></returns>
        public static async Task<bool> KeyExistsAsync(string redisKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.KeyExistsAsync(redisKey);
        }

        /// <summary>
        /// 重命名 Key
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="redisNewKey"></param>
        /// <returns></returns>
        public static async Task<bool> KeyRenameAsync(string redisKey, string redisNewKey, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.KeyRenameAsync(redisKey, redisNewKey);
        }

        /// <summary>
        /// 设置 Key 的时间
        /// </summary>
        /// <param name="redisKey"></param>
        /// <param name="expiry"></param>
        /// <returns></returns>
        public static async Task<bool> KeyExpireAsync(string redisKey, TimeSpan? expiry, int dbIndex = -1)
        {
            redisKey = AddKeyPrefix(redisKey);
            var db = GetDatabase(dbIndex);
            return await db.KeyExpireAsync(redisKey, expiry);
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="handle"></param>
        public static void Subscribe(RedisChannel channel, Action<RedisChannel, RedisValue> handle, int dbIndex = -1)
        {
            var sub = _connMultiplexer.GetSubscriber();
            var db = GetDatabase(dbIndex);
            sub.Subscribe(channel, handle);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static long Publish(RedisChannel channel, RedisValue message, int dbIndex = -1)
        {
            var sub = _connMultiplexer.GetSubscriber();
            var db = GetDatabase(dbIndex);
            return sub.Publish(channel, message);
        }

        /// <summary>
        /// 发布（使用序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static long Publish<T>(RedisChannel channel, T message, int dbIndex = -1)
        {
            var sub = _connMultiplexer.GetSubscriber();
            var db = GetDatabase(dbIndex);
            return sub.Publish(channel, Serialize(message));
        }

        /// <summary>
        /// 订阅
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="handle"></param>
        /// <returns>The <see cref="Task"/></returns>
        public static async Task SubscribeAsync(RedisChannel channel, Action<RedisChannel, RedisValue> handle, int dbIndex = -1)
        {
            var sub = _connMultiplexer.GetSubscriber();
            var db = GetDatabase(dbIndex);
            await sub.SubscribeAsync(channel, handle);
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<long> PublishAsync(RedisChannel channel, RedisValue message, int dbIndex = -1)
        {
            var sub = _connMultiplexer.GetSubscriber();
            var db = GetDatabase(dbIndex);
            return await sub.PublishAsync(channel, message);
        }

        /// <summary>
        /// 发布（使用序列化）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="channel"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public static async Task<long> PublishAsync<T>(RedisChannel channel, T message, int dbIndex = -1)
        {
            var sub = _connMultiplexer.GetSubscriber();
            var db = GetDatabase(dbIndex);
            return await sub.PublishAsync(channel, Serialize(message));
        }

        /// <summary>
        /// 添加 Key 的前缀
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string AddKeyPrefix(string key)
        {
            return $"{DefaultKey}:{key}";
        }

        /// <summary>
        /// 添加注册事件
        /// </summary>
        private static void AddRegisterEvent()
        {
            _connMultiplexer.ConnectionRestored += ConnMultiplexer_ConnectionRestored;
            _connMultiplexer.ConnectionFailed += ConnMultiplexer_ConnectionFailed;
            _connMultiplexer.ErrorMessage += ConnMultiplexer_ErrorMessage;
            _connMultiplexer.ConfigurationChanged += ConnMultiplexer_ConfigurationChanged;
            _connMultiplexer.HashSlotMoved += ConnMultiplexer_HashSlotMoved;
            _connMultiplexer.InternalError += ConnMultiplexer_InternalError;
            _connMultiplexer.ConfigurationChangedBroadcast += ConnMultiplexer_ConfigurationChangedBroadcast;
        }

        /// <summary>
        /// 重新配置广播时（通常意味着主从同步更改）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChangedBroadcast(object sender, EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChangedBroadcast)}: {e.EndPoint}");
        }

        /// <summary>
        /// 发生内部错误时（主要用于调试）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_InternalError(object sender, InternalErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_InternalError)}: {e.Exception}");
        }

        /// <summary>
        /// 更改集群时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_HashSlotMoved(object sender, HashSlotMovedEventArgs e)
        {
            Console.WriteLine(
                $"{nameof(ConnMultiplexer_HashSlotMoved)}: {nameof(e.OldEndPoint)}-{e.OldEndPoint} To {nameof(e.NewEndPoint)}-{e.NewEndPoint}, ");
        }

        /// <summary>
        /// 配置更改时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConfigurationChanged(object sender, EndPointEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConfigurationChanged)}: {e.EndPoint}");
        }

        /// <summary>
        /// 发生错误时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ErrorMessage(object sender, RedisErrorEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ErrorMessage)}: {e.Message}");
        }

        /// <summary>
        /// 物理连接失败时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionFailed)}: {e.Exception}");
        }

        /// <summary>
        /// 建立物理连接时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void ConnMultiplexer_ConnectionRestored(object sender, ConnectionFailedEventArgs e)
        {
            Console.WriteLine($"{nameof(ConnMultiplexer_ConnectionRestored)}: {e.Exception}");
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private static byte[] Serialize(object obj)
        {
            if (obj == null)
                return null;

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream())
            {
                binaryFormatter.Serialize(memoryStream, obj);
                var data = memoryStream.ToArray();
                return data;
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        private static T Deserialize<T>(byte[] data)
        {
            if (data == null)
                return default(T);

            var binaryFormatter = new BinaryFormatter();
            using (var memoryStream = new MemoryStream(data))
            {
                var result = (T)binaryFormatter.Deserialize(memoryStream);
                return result;
            }
        }
    }
}
