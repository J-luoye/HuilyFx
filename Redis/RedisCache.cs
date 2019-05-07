using Common.Utility;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Redis
{
    /// <summary>
    /// 表示Redis缓存
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RedisCache<T>
    {
        /// <summary>
        /// multiplexer
        /// </summary>
        private readonly Multiplexer multiplexer;

        /// <summary>
        /// 有效时间
        /// </summary>
        public TimeSpan? expiry { get; set; }

        /// <summary>
        /// 键前缀
        /// </summary>
        public string keyPrefix { get; set; } = "CACHE_";

        /// <summary>
        /// 指定数据集
        /// </summary>
        private readonly int dataBase;

        #region 构造函数
        /// <summary>
        /// 表示会话
        /// </summary>
        /// <param name="multiplexer">Multiplexer</param>
        public RedisCache(Multiplexer multiplexer)
        {
            this.multiplexer = multiplexer;
        }

        /// <summary>
        /// 表示会话
        /// </summary>
        /// <param name="multiplexer">Multiplexer</param>
        /// <param name="database">指定数据库</param>
        public RedisCache(Multiplexer multiplexer, Database database) :
            this(multiplexer)
        {
            this.dataBase = (int)database;
        }

        /// <summary>
        /// 表示会话
        /// </summary>
        /// <param name="multiplexer">Multiplexer</param>
        /// <param name="database">指定数据库</param>
        /// <param name="expiry">有效时间</param>
        public RedisCache(Multiplexer multiplexer, Database database, TimeSpan? expiry)
            : this(multiplexer, database)
        {
            this.expiry = expiry;
        }

        /// <summary>
        /// 表示会话
        /// </summary>
        /// <param name="multiplexer">Multiplexer</param>
        /// <param name="database">指定数据库</param>
        /// <param name="expiry">有效时间</param>
        /// <param name="keyPrefix">键的前缀</param>
        public RedisCache(Multiplexer multiplexer, Database database, TimeSpan? expiry, string keyPrefix)
            : this(multiplexer, database, expiry)
        {
            this.keyPrefix = keyPrefix;
        }
        #endregion

        /// <summary>
        /// 获取db
        /// </summary>
        /// <returns></returns>
        private IDatabase GetDatabase()
        {
            return this.multiplexer.GetMultiplexer().GetDatabase(this.dataBase);
        }

        /// <summary>
        /// 获取key
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        private string GetKey(string key)
        {
            if (string.IsNullOrEmpty(this.keyPrefix))
            {
                return key;
            }
            return this.keyPrefix + ":" + key;
        }


        public T Get(string key)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            var value = db.StringGet(key);

            if (value.IsNullOrEmpty == true)
            {
                return default(T);
            }

            if (this.expiry.HasValue == true)
            {
                db.KeyExpire(key, this.expiry);
            }
            return JsonSerializer.TryDeserialize<T>(value);
        }



        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string key, T value)
        {
            return await SetAsync(key, value, this.expiry);
        }

        /// <summary>
        /// 设置值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        /// <param name="expiry">过期时间</param>
        /// <returns></returns>
        public async Task<bool> SetAsync(string key, T value, TimeSpan? expiry)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            var redisValue = JsonSerializer.Serialize(value);
            return await db.StringSetAsync(key, redisValue, expiry);
        }

        public async Task<bool> ListPushAsync(string key, T value)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            var redisValue = JsonSerializer.Serialize(value);
            var result = await db.ListLeftPushAsync(key, redisValue) > 0;
            await db.KeyExpireAsync(key, this.expiry);
            return result;
        }

        /// <summary>
        /// 获取多个值
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="count">获取总条数</param>
        /// <returns></returns>
        public async Task<List<RedisSyncData>> GetListRangeAsync(string key, int count)
        {
            //key = this.GetKey(key);
            var pattern = key + "*";
            var db = this.GetDatabase();

            var redisResult = await db.ScriptEvaluateAsync(LuaScript.Prepare(
                //Redis的keys模糊查询：
                " local res = redis.call('KEYS',@keypattern) " +
                " return res "), new { @keypattern = pattern });

            var d = db.ListRightPop(key);
            var c = await db.ListRangeAsync(key, 0, count);
            var a = await db.ListRangeAsync(pattern);
            var list = db.HashScan(key, pattern);
            var valuse = await db.ListRightPopAsync(key);
            await Task.Delay(1);
            return default(List<RedisSyncData>);
        }

        /// <summary>
        /// 获取值,过期时间重新计算
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<T> GetAsync(string key)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            var value = await db.StringGetAsync(key);

            if (value.IsNullOrEmpty == true)
            {
                return default(T);
            }

            if (this.expiry.HasValue == true)
            {
                db.KeyExpire(key, this.expiry);
            }
            return JsonSerializer.TryDeserialize<T>(value);
        }


        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<bool> ExistsAsync(string key)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            return await db.KeyExistsAsync(key);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="key">键</param>
        /// <returns></returns>
        public async Task<bool> RemoveAsync(string key)
        {
            key = this.GetKey(key);
            var db = this.GetDatabase();
            return await db.KeyDeleteAsync(key);
        }

    }
}
